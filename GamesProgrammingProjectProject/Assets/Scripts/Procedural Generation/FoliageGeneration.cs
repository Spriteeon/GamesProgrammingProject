using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageGeneration : MonoBehaviour
{
	[SerializeField]
	private NoiseMapGeneration noiseMapGeneration;

	[SerializeField]
	private Wave[] waves;

	[SerializeField]
	private float levelScale;

	[SerializeField]
	private float neighborRadius;

	[SerializeField]
	private GameObject foliagePrefab1;

	[SerializeField]
	private GameObject foliagePrefab2;

	[SerializeField]
	private GameObject foliagePrefab3;

	private GameObject foliagePrefab;

	public void GenerateFoliage(int levelDepth, int levelWidth, float distanceBetweenVertices, LevelData levelData)
	{
		// generate a tree noise map using Perlin Noise
		float[,] treeMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(levelDepth, levelWidth, levelScale, 0, 0, this.waves);

		float levelSizeX = levelWidth * distanceBetweenVertices;
		float levelSizeZ = levelDepth * distanceBetweenVertices;

		for (int z = 0; z < levelDepth; z++)
		{
			for (int x = 0; x < levelWidth; x++)
			{
				// convert from Level Coordinate System to Tile Coordinate System and retrieve the corresponding TileData
				TileCoordinate tileCoordinate = levelData.ConvertToTileCoordinate(z, x);
				TileData tileData = levelData.tilesData[tileCoordinate.tileZ, tileCoordinate.tileX];
				int tileWidth = tileData.heightMap.GetLength(1);

				// calculate the mesh vertex index
				Vector3[] meshVertices = tileData.mesh.vertices;
				int vertexIndex = tileCoordinate.coordinateZ * tileWidth + tileCoordinate.coordinateX;

				// get the terrain type of this coordinate
				TerrainType terrainType = tileData.heightTerrainTypes[tileCoordinate.coordinateZ, tileCoordinate.coordinateX];
				// check if it is a water terrain. Trees cannot be placed over the water
				if (terrainType.name != "low")
				{
					float treeValue = treeMap[z, x];

					// int terrainTypeIndex = terrainType.index;

					// compares the current tree noise value to the neighbor ones
					int neighborZBegin = (int)Mathf.Max(0, z - this.neighborRadius);
					int neighborZEnd = (int)Mathf.Min(levelDepth - 1, z + this.neighborRadius);
					int neighborXBegin = (int)Mathf.Max(0, x - this.neighborRadius);
					int neighborXEnd = (int)Mathf.Min(levelWidth - 1, x + this.neighborRadius);
					float maxValue = 0f;
					for (int neighborZ = neighborZBegin; neighborZ <= neighborZEnd; neighborZ++)
					{
						for (int neighborX = neighborXBegin; neighborX <= neighborXEnd; neighborX++)
						{
							float neighborValue = treeMap[neighborZ, neighborX];
							// saves the maximum tree noise value in the radius
							if (neighborValue >= maxValue)
							{
								maxValue = neighborValue;
							}
						}
					}

					// if the current tree noise value is the maximum one, place a tree in this location
					if (treeValue == maxValue)
					{
						//Vector3 treePosition = new Vector3(x * distanceBetweenVertices, meshVertices[vertexIndex].y, z * distanceBetweenVertices);
						Vector3 foliagePosition = new Vector3(x * distanceBetweenVertices, meshVertices[vertexIndex].y, z * distanceBetweenVertices);

						// Pick a random tree Prefab
						int randNum = Random.Range(1, 4);
						switch (randNum)
						{
							case 1:
								foliagePrefab = foliagePrefab1;
								break;
							case 2:
								foliagePrefab = foliagePrefab2;
								break;
							case 3:
								foliagePrefab = foliagePrefab3;
								break;
							default:
								break;
						}

						// Pick a random tree scale
						float foliageScale = Random.Range(0.3f, 0.5f);

						GameObject foliage = Instantiate(this.foliagePrefab, foliagePosition, Quaternion.identity) as GameObject;
						foliage.transform.localScale = new Vector3(foliageScale, foliageScale, foliageScale);
						//foliage.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
					}
				}
			}
		}
	}
}
