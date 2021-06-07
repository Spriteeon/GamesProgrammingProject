using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.AI;

public class LevelGeneration : MonoBehaviour
{
	[SerializeField]
	private int mapWidthInTiles, mapDepthInTiles;

	[SerializeField]
	private GameObject tilePrefab;
	[SerializeField]
	private GameObject enemyObject;
	[SerializeField]
	private GameObject wellPrefab;

	[SerializeField]
	private float centerVertexZ, maxDistanceZ;

	[SerializeField]
	private TreeGeneration treeGeneration;
	[SerializeField]
	private FoliageGeneration foliageGeneration;
	[SerializeField]
	private BuildingGeneration buildingGeneration;
	[SerializeField]
	private ItemGeneration itemGeneration;

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

	float itemMin = 0.5f;
	float itemMax = 2f;

	private Player player;
	private bool customGame;

	private int maxEnemies = 10;

	private Vector3[] patrolPoints;
	private int numPatrolPoints = 10;
	private Vector3 mapCentre = new Vector3(190f, 0f, 190f);
	private float playableMapRadius = 170f;

	RaycastHit hit;
	float maxHeight = 20f;
	Ray ray;

	[SerializeField]
	private GameObject debugObject;

	[SerializeField]
	private NavigationBaker baker;

	void Start()
	{
		player = PlayerManager.instance.player.GetComponent<Player>();
		customGame = GlobalControl.instance.customGame;
		patrolPoints = new Vector3[numPatrolPoints];

		GenerateTerrainWaves();
		GenerateMap();
	}

	private void GenerateTerrainWaves()
	{
		if(!customGame)
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
		else
		{
			// Seeding stuff
		}
	}

	private Wave[] GenerateGenericWaves(float min, float max)
	{
		if (!customGame)
		{
			// Create random waves for Trees and Terrain
			foreach (Wave wave in genericWaves)
			{
				wave.seed = GenerateWaveSeed();
				wave.frequency = GenerateFreqAmp(min, max);
				wave.amplitude = GenerateFreqAmp(min, max);
			}
		}
		else
		{
			// Seeding stuff
		}

		return genericWaves;
	}

	void GenerateMap()
	{
		// Get the tile dimensions from the tile Prefab
		Vector3 tileSize = tilePrefab.GetComponent<MeshRenderer>().bounds.size;
		int tileWidth = (int)tileSize.x;
		int tileDepth = (int)tileSize.z;

		// Calculate the number of vertices of the tile in each axis using its mesh
		Vector3[] tileMeshVertices = tilePrefab.GetComponent<MeshFilter>().sharedMesh.vertices;
		int tileDepthVert = (int)Mathf.Sqrt(tileMeshVertices.Length);
		int tileWidthVert = tileDepthVert;

		float distanceBetweenVertices = (float)tileDepth / (float)tileDepthVert;

		// Build an empty LevelData object, to be filled with the tiles to be generated
		LevelData levelData = new LevelData(tileDepthVert, tileWidthVert, this.mapDepthInTiles, this.mapWidthInTiles);

		// For each Tile, instantiate a Tile in the correct position
		int surfaceIndex = 0;
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

				//baker.surfaces[surfaceIndex] = tile.GetComponent<NavMeshSurface>();
				//surfaceIndex++;

				// generate the Tile texture and save it in the levelData
				TileData tileData = tile.GetComponent<TileGeneration>().GenerateTile(centerVertexZ, maxDistanceZ, terrainWaves);
				levelData.AddTileData(tileData, zTile, xTile);
			}
		}

		// Doing first bake for Terrain, needs to be baked before spawning Enemies
		baker.Bake();

		// Update Player Position
		player.UpdatePlayerPosition();

		GeneratePatrolPoints();
		for (int i = 0; i < maxEnemies; i++)
		{
			// Spawn Enemies
			// Spawn each enemy on a different patrol point
			Vector3 enemyPosition = patrolPoints[i];
			GameObject enemyObject = Instantiate(this.enemyObject, enemyPosition, Quaternion.identity) as GameObject;
			Enemy enemy = enemyObject.GetComponent<Enemy>();
			enemy.GetPatrolPoints(patrolPoints);
		}

		// Spawn 'Well'
		SpawnWell();

		// Generate trees for the level
		buildingGeneration.GenerateBuildings(this.mapDepthInTiles * tileDepthVert, this.mapWidthInTiles * tileWidthVert, distanceBetweenVertices, levelData, GenerateGenericWaves(buildingMin, buildingMax));
		// Generate trees for the level
		treeGeneration.GenerateTrees(this.mapDepthInTiles * tileDepthVert, this.mapWidthInTiles * tileWidthVert, distanceBetweenVertices, levelData, GenerateGenericWaves(treeMin, treeMax));
		// Generate foliage for the level
		foliageGeneration.GenerateFoliage(this.mapDepthInTiles * tileDepthVert, this.mapWidthInTiles * tileWidthVert, distanceBetweenVertices, levelData, GenerateGenericWaves(foliageMin, foliageMax));
		// Generate items for the level
		itemGeneration.GenerateItems(this.mapDepthInTiles * tileDepthVert, this.mapWidthInTiles * tileWidthVert, distanceBetweenVertices, levelData, GenerateGenericWaves(itemMin, itemMax));

		// Final NavMesh bake
		baker.Bake();

	}

	private void SpawnWell()
	{
		float distanceFromPoint = 5f;
		int randomIndex = Random.Range(0, patrolPoints.Length);
		Vector3 patrolPosition = patrolPoints[randomIndex];
		Vector3 directionToCentre = (mapCentre - patrolPosition).normalized;

		Vector3 wellPosition = patrolPosition + (directionToCentre * distanceFromPoint);

		// Check if on ground
		ray.origin = new Vector3(wellPosition.x, maxHeight, wellPosition.z);
		ray.direction = Vector3.down;
		hit = new RaycastHit();

		while(Physics.Raycast(ray, out hit) && hit.transform.tag != "Floor")
		{
			distanceFromPoint += 5f;
			wellPosition = patrolPosition + (directionToCentre * distanceFromPoint);

			ray.origin = new Vector3(wellPosition.x, maxHeight, wellPosition.z);
			ray.direction = Vector3.down;
			hit = new RaycastHit();
		}

		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Floor")
		{
			float yPos = hit.point.y - 0.5f;

			Vector3 newWellPosition = new Vector3(wellPosition.x, yPos, wellPosition.z);

			float yRotation = Random.Range(-180.0f, 180.0f);

			GameObject well = Instantiate(this.wellPrefab, newWellPosition, Quaternion.identity) as GameObject;
			well.transform.Rotate(0.0f, yRotation, 0.0f);
		}

	}

	private void GeneratePatrolPoints()
	{
		for(int i = 0; i < numPatrolPoints; i++)
		{

			Vector3 randomPoint = mapCentre;
			randomPoint.x += Random.Range(-playableMapRadius, playableMapRadius);
			randomPoint.z += Random.Range(-playableMapRadius, playableMapRadius);

			ray.origin = new Vector3(randomPoint.x, maxHeight, randomPoint.z);
			ray.direction = Vector3.down;
			hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Floor")
			{
				float yPos = hit.point.y + 0.5f;

				Vector3 patrolPointPosition = new Vector3(randomPoint.x, yPos, randomPoint.z);

				patrolPoints[i] = patrolPointPosition;

				GameObject patrolPointDebug = Instantiate(this.debugObject, patrolPointPosition, Quaternion.identity) as GameObject;
			}
		}
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
		// Converting the Level Coordinate to the tile coordinate
		int tileZIndex = (int)Mathf.Floor((float)z / (float)this.tileDepthVert);
		int tileXIndex = (int)Mathf.Floor((float)x / (float)this.tileWidthVert);
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
