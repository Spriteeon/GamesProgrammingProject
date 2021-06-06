using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingGeneration : MonoBehaviour
{
	[SerializeField]
	private NoiseMapGeneration noiseMapGeneration;

	[SerializeField]
	private float levelScale;
	[SerializeField]
	private float neighborRadius;

	[SerializeField]
	private GameObject buildingPrefab;

	RaycastHit hit;
	float maxHeight = 20f;
	Ray ray;

	public void GenerateBuildings(int levelDepth, int levelWidth, float distanceBetweenVertices, LevelData levelData, Wave[] waves)
	{
		// generate a tree noise map using Perlin Noise
		float[,] buildingMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(levelDepth, levelWidth, levelScale, 0, 0, waves);

		float levelSizeX = levelWidth * distanceBetweenVertices;
		float levelSizeZ = levelDepth * distanceBetweenVertices;

		for (int z = 0; z < levelDepth; z++)
		{
			for (int x = 0; x < levelWidth; x++)
			{
				// Convert from Level Coordinate System to Tile Coordinate System and retrieve the corresponding TileData
				TileCoordinate tileCoordinate = levelData.ConvertToTileCoordinate(z, x);
				TileData tileData = levelData.tilesData[tileCoordinate.tileZ, tileCoordinate.tileX];
				int tileWidth = tileData.heightMap.GetLength(1);

				// Get the terrain type of this coordinate
				TerrainType terrainType = tileData.heightTerrainTypes[tileCoordinate.coordinateZ, tileCoordinate.coordinateX];

				if (terrainType.name == "low")
				{
					float buildingValue = buildingMap[z, x];

					// Compares the current tree noise value to the neighbor ones
					int neighborZBegin = (int)Mathf.Max(0, z - this.neighborRadius);
					int neighborZEnd = (int)Mathf.Min(levelDepth - 1, z + this.neighborRadius);
					int neighborXBegin = (int)Mathf.Max(0, x - this.neighborRadius);
					int neighborXEnd = (int)Mathf.Min(levelWidth - 1, x + this.neighborRadius);
					float maxValue = 0f;
					for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++)
					{
						for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++)
						{
							float neighborValue = buildingMap[neighborZ, neighborX];
							if (neighborValue >= maxValue)
							{
								maxValue = neighborValue;
							}
						}
					}

					// If the current tree noise value is the maximum one, place a tree in this location
					if (buildingValue == maxValue)
					{

						float xPos = x * distanceBetweenVertices;
						float zPos = z * distanceBetweenVertices;
						float yPos = 0f;

						ray.origin = new Vector3(xPos, maxHeight, zPos);
						ray.direction = Vector3.down;
						hit = new RaycastHit();

						if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Floor")
						{
							yPos = hit.point.y + 1.8f;
						}

						Vector3 buildingPosition = new Vector3(xPos, yPos, zPos);

						float buildingScale = 2.5f;

						float yRotation = Random.Range(-180.0f, 180.0f);

						GameObject building = Instantiate(this.buildingPrefab, buildingPosition, Quaternion.identity) as GameObject;
						building.transform.localScale = new Vector3(buildingScale, buildingScale, buildingScale);
						building.transform.Rotate(0.0f, yRotation, 0.0f);
					}
				}
			}
		}
	}
}
