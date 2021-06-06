using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGeneration : MonoBehaviour
{
	[SerializeField]
	private NoiseMapGeneration noiseMapGeneration;

	[SerializeField]
	private float levelScale;
	[SerializeField]
	private float neighborRadius;

	[SerializeField]
	private GameObject healthItem;
	[SerializeField]
	private GameObject candleItem;

	private GameObject itemPrefab;

	RaycastHit hit;
	float maxHeight = 20f;
	Ray ray;

	public void GenerateItems(int levelDepth, int levelWidth, float distanceBetweenVertices, LevelData levelData, Wave[] waves)
	{
		// Generate Items here
		float[,] itemMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(levelDepth, levelWidth, levelScale, 0, 0, waves);

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
				TerrainType terrainType = tileData.heightTerrainTypes[tileCoordinate.coordinateZ, tileCoordinate.coordinateX];

				if (terrainType.name == "medium")
				{
					float itemValue = itemMap[z, x];

					// Compares the current item noise value to the neighbor ones
					int neighborZBegin = (int)Mathf.Max(0, z - this.neighborRadius);
					int neighborZEnd = (int)Mathf.Min(levelDepth - 1, z + this.neighborRadius);
					int neighborXBegin = (int)Mathf.Max(0, x - this.neighborRadius);
					int neighborXEnd = (int)Mathf.Min(levelWidth - 1, x + this.neighborRadius);
					float maxValue = 0f;
					for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++)
					{
						for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++)
						{
							float neighborValue = itemMap[neighborZ, neighborX];
							if (neighborValue >= maxValue)
							{
								maxValue = neighborValue;
							}
						}
					}

					// If the current Item noise value is the maximum one, place an Item in this location
					if (itemValue == maxValue)
					{

						float xPos = x * distanceBetweenVertices;
						float zPos = z * distanceBetweenVertices;
						float yPos = 0f;
						
						ray.origin = new Vector3(xPos, maxHeight, zPos);
						ray.direction = Vector3.down;
						hit = new RaycastHit();

						if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Floor")
						{
							yPos = hit.point.y - 0.1f;

							Vector3 itemPosition = new Vector3(xPos, yPos, zPos);

							// Pick a random item
							int randNum = Random.Range(0, 2);
							switch (randNum)
							{
								case 0:
									itemPrefab = candleItem;
									break;
								case 1:
									itemPrefab = healthItem;
									break;
								default:
									break;
							}

							GameObject item = Instantiate(this.itemPrefab, itemPosition, Quaternion.identity) as GameObject;
						}
					}
				}
			}
		}
	}
}
