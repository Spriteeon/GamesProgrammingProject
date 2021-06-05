using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LevelGeneration : MonoBehaviour
{
	[SerializeField]
	private int mapWidthInTiles, mapDepthInTiles;

	[SerializeField]
	private GameObject tilePrefab;

	[SerializeField]
	private float centerVertexZ, maxDistanceZ;

	[SerializeField]
	private TreeGeneration treeGeneration;

	[SerializeField]
	private FoliageGeneration foliageGeneration;

	[SerializeField]
	private BuildingGeneration buildingGeneration;

	[SerializeField]
	private Wave[] terrainWaves;

	[SerializeField]
	private Wave[] genericWaves;

	float buildingMin = 0.5f;
	float buildingMax = 2f;

	float treeMin = 0.5f;
	float treeMax = 2f;

	float foliageMin = 0.5f;
	float foliageMax = 2f;

	[SerializeField]
	private Player player;

	void Start()
	{
		GenerateTerrainWaves();
		GenerateMap();
	}

	private void GenerateTerrainWaves()
	{
		// Create random waves
		terrainWaves[0].seed = GenerateWaveSeed();
		terrainWaves[0].frequency = 1f;
		terrainWaves[0].amplitude = 2f;

		terrainWaves[1].seed = GenerateWaveSeed();
		terrainWaves[1].frequency = 0.5f;
		terrainWaves[1].amplitude = 2f;

		terrainWaves[2].seed = GenerateWaveSeed();
		terrainWaves[2].frequency = 0.5f;
		terrainWaves[2].amplitude = 4f;
	}

	private Wave[] GenerateGenericWaves(float min, float max)
	{
		// Create random waves for Trees and Terrain
		//genericWaves = new Wave[3];
		foreach (Wave wave in genericWaves)
		{
			wave.seed = GenerateWaveSeed();
			wave.frequency = GenerateFreqAmp(min, max);
			wave.amplitude = GenerateFreqAmp(min, max);
		}

		return genericWaves;
	}

	void GenerateMap()
	{
		// get the tile dimensions from the tile Prefab
		Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
		int tileWidth = (int)tileSize.x;
		int tileDepth = (int)tileSize.z;

		// calculate the number of vertices of the tile in each axis using its mesh
		Vector3[] tileMeshVertices = tilePrefab.GetComponent<MeshFilter>().sharedMesh.vertices;
		int tileDepthVert = (int)Mathf.Sqrt(tileMeshVertices.Length);
		int tileWidthVert = tileDepthVert;

		float distanceBetweenVertices = (float)tileDepth / (float)tileDepthVert;

		// build an empty LevelData object, to be filled with the tiles to be generated
		LevelData levelData = new LevelData(tileDepthVert, tileWidthVert, this.mapDepthInTiles, this.mapWidthInTiles);

		// for each Tile, instantiate a Tile in the correct position
		for (int xTile = 0; xTile < mapWidthInTiles; xTile++)
		{
			for (int zTile = 0; zTile < mapDepthInTiles; zTile++)
			{
				// calculate the tile position based on the X and Z indices
				Vector3 tilePosition = new Vector3(this.gameObject.transform.position.x + xTile * tileWidth,
					this.gameObject.transform.position.y,
					this.gameObject.transform.position.z + zTile* tileDepth);
				// instantiate a new Tile
				GameObject tile = Instantiate(tilePrefab, tilePosition, Quaternion.identity) as GameObject;

				// generate the Tile texture and save it in the levelData
				TileData tileData = tile.GetComponent<TileGeneration>().GenerateTile(centerVertexZ, maxDistanceZ, terrainWaves);
				levelData.AddTileData(tileData, zTile, xTile);
			}
		}

		// Update Player Position
		player.UpdatePlayerPosition();

		// generate trees for the level
		buildingGeneration.GenerateBuildings(this.mapDepthInTiles * tileDepthVert, this.mapWidthInTiles * tileWidthVert, distanceBetweenVertices, levelData, GenerateGenericWaves(buildingMin, buildingMax));

		// generate trees for the level
		treeGeneration.GenerateTrees(this.mapDepthInTiles * tileDepthVert, this.mapWidthInTiles * tileWidthVert, distanceBetweenVertices, levelData, GenerateGenericWaves(treeMin, treeMax));

		// generate foliage for the level
		foliageGeneration.GenerateFoliage(this.mapDepthInTiles * tileDepthVert, this.mapWidthInTiles * tileWidthVert, distanceBetweenVertices, levelData, GenerateGenericWaves(foliageMin, foliageMax));

	}

	private float GenerateWaveSeed()
	{
		float seed = Random.Range(0f, 9999f);
		return seed;
	}

	private float GenerateFreqAmp(float min, float max)
	{
		float freqAmp = Random.Range(min, max);
		return freqAmp;
	}

}

public class LevelData
{
	private int tileDepthVert, tileWidthVert;

	public TileData[,] tilesData;

	public LevelData(int tileDepthVert, int tileWidthVert, int mapDepthInTiles, int mapWidthInTiles)
	{
		tilesData = new TileData[tileDepthVert * mapDepthInTiles, tileWidthVert * mapWidthInTiles];

		this.tileDepthVert = tileDepthVert;
		this.tileWidthVert = tileWidthVert;
	}

	public void AddTileData(TileData tileData, int tileZ, int tileX)
	{
		// Save the TileData in the corresponding coordinate
		tilesData[tileZ, tileX] = tileData;
	}

	public TileCoordinate ConvertToTileCoordinate(int z, int x)
	{
		// the tile index is calculated by dividing the index by the number of tiles in that axis
		int tileZIndex = (int)Mathf.Floor((float)z / (float)this.tileDepthVert);
		int tileXIndex = (int)Mathf.Floor((float)x / (float)this.tileWidthVert);
		// the coordinate index is calculated by getting the remainder of the division above
		// we also need to translate the origin to the bottom left corner
		int coordinateZIndex = this.tileDepthVert - (z % this.tileDepthVert) - 1;
		int coordinateXIndex = this.tileWidthVert - (x % this.tileDepthVert) - 1;

		TileCoordinate tileCoordinate = new TileCoordinate(tileZIndex, tileXIndex, coordinateZIndex, coordinateXIndex);
		return tileCoordinate;
	}
}

// Class to represent a coordinate in the Tile Coordinate System
public class TileCoordinate
{
	public int tileZ;
	public int tileX;
	public int coordinateZ;
	public int coordinateX;

	public TileCoordinate(int tileZ, int tileX, int coordinateZ, int coordinateX)
	{
		this.tileZ = tileZ;
		this.tileX = tileX;
		this.coordinateZ = coordinateZ;
		this.coordinateX = coordinateX;
	}
}
