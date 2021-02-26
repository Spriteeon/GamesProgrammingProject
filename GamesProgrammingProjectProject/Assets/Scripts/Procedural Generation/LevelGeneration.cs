using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	[SerializeField]
	private int mapWidthInTiles, mapDepthInTiles;

	[SerializeField]
	private GameObject tilePrefab;

	[SerializeField]
	private float centerVertexZ, maxDistanceZ;

	void Start()
	{
		GenerateMap();
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
				TileData tileData = tile.GetComponent<TileGeneration>().GenerateTile(centerVertexZ, maxDistanceZ);
				levelData.AddTileData(tileData, zTile, xTile);
			}
		}
	}
}

public class LevelData
{
	private int tileDepthVert, tileWidthVert;

	public TileData[,] tilesData;

	public LevelData(int tileDepthVert, int tileWidthVert, int levelDepthTiles, int levelWidthTiles)
	{
		tilesData = new TileData[tileDepthVert * levelDepthTiles, tileWidthVert * levelWidthTiles];

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
