using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGeneration : MonoBehaviour
{

	[SerializeField]
	NoiseMapGeneration noiseMapGeneration;

	[SerializeField]
	private MeshRenderer tileRenderer;

	[SerializeField]
	private MeshFilter meshFilter;
	[SerializeField]
	private MeshCollider meshCollider;

	[SerializeField]
	private float mapScale;

	[SerializeField]
	private TerrainType[] terrainTypes;

	[SerializeField]
	private float heightMultiplier;
	[SerializeField]
	private AnimationCurve heightCurve;

	private Vector2[] uvs;
	[SerializeField]
	private Texture lowTexture, mediumTexture, highTexture;
	[SerializeField]
	private Texture lowNormal, mediumNormal, highNormal;
	private Renderer renderer;

	public TileData GenerateTile(float centerVertexZ, float maxDistanceZ, Wave[] waves)
	{
		renderer = this.gameObject.GetComponent<Renderer>();

		// Calculate tile depth and width based on the mesh vertices
		Vector3[] meshVertices = this.meshFilter.mesh.vertices;
		int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
		int tileWidth = tileDepth;

		// Calculate the offsets based on the tile position
		float offsetX = -this.gameObject.transform.position.x;
		float offsetZ = -this.gameObject.transform.position.z;

		// Calculate the offsets based on the tile position
		float[,] heightMap = this.noiseMapGeneration.GeneratePerlinNoiseMap(tileDepth, tileWidth, this.mapScale, offsetX, offsetZ, waves);

		TerrainType[,] heightTerrainTypes = new TerrainType[tileDepth, tileWidth];

		// Terrain Texture
		for (int z = 0; z < tileDepth; z++)
		{
			for (int x = 0; x < tileWidth; x++)
			{

				float height = heightMap[z, x];
				TerrainType terrainType = ChooseTerrainType(height, this.terrainTypes);
				heightTerrainTypes[z, x] = terrainType;

				switch (terrainType.name)
				{
					case "low":
						renderer.material.SetTexture("_MainTex", lowTexture);
						renderer.material.SetTexture("_BumbMap", lowNormal);
						break;
					case "medium":
						renderer.material.SetTexture("_MainTex", mediumTexture);
						renderer.material.SetTexture("_BumbMap", mediumNormal);
						break;
					case "high":
						renderer.material.SetTexture("_MainTex", highTexture);
						renderer.material.SetTexture("_BumbMap", highNormal);
						break;
					default:
						break;
				}
			}
		}

		uvs = new Vector2[meshVertices.Length];
		for (int i = 0, z = 0; z < tileDepth; z++)
		{
			for (int x = 0; x < tileWidth; x++)
			{
				uvs[i] = new Vector2((float)x / tileWidth * 2, (float)z / tileDepth * 2);
				i++;
			}
		}

		// Update the tile mesh vertices according to the height map
		UpdateMeshVertices(heightMap);

		TileData tileData = new TileData(heightMap, heightTerrainTypes, this.meshFilter.mesh, this.gameObject);
		return tileData;
	}
	
	TerrainType ChooseTerrainType(float height, TerrainType[] terrainTypes)
	{
		// For each terrain type, check if the height is lower than the one for the terrain type
		foreach (TerrainType terrainType in terrainTypes)
		{
			// Return the first terrain type whose height is higher than the generated one
			if (height < terrainType.height)
			{
				return terrainType;
			}
		}
		return terrainTypes[terrainTypes.Length - 1];
	}

	private void UpdateMeshVertices(float[,] heightMap)
	{
		int tileDepth = heightMap.GetLength(0);
		int tileWidth = heightMap.GetLength(1);

		Vector3[] meshVertices = this.meshFilter.mesh.vertices;

		// Iterate through all the heightMap coordinates, updating the vertex index
		int vertexIndex = 0;
		for (int zIndex = 0; zIndex < tileDepth; zIndex++)
		{
			for (int xIndex = 0; xIndex < tileWidth; xIndex++)
			{
				float height = heightMap[zIndex, xIndex];

				Vector3 vertex = meshVertices[vertexIndex];
				// Change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
				meshVertices[vertexIndex] = new Vector3(vertex.x, this.heightCurve.Evaluate(height) * this.heightMultiplier, vertex.z);

				vertexIndex++;
			}
		}

		// Update the vertices in the mesh and update its properties
		this.meshFilter.mesh.vertices = meshVertices;
		this.meshFilter.mesh.uv = uvs;
		this.meshFilter.mesh.RecalculateBounds();
		this.meshFilter.mesh.RecalculateNormals();
		// Update the mesh collider
		this.meshCollider.sharedMesh = this.meshFilter.mesh;
	}

}

[System.Serializable]
public class TerrainType
{
	public string name;
	public float height;
	public Color colour;
}

// Class to store all the Data of a Tile
public class TileData
{
	public float[,] heightMap;
	public TerrainType[,] heightTerrainTypes;
	public Mesh mesh;
	public GameObject plane;

	public TileData(float[,] heightMap, TerrainType[,] heightTerrainTypes, Mesh mesh, GameObject plane)
	{
		this.heightMap = heightMap;
		this.heightTerrainTypes = heightTerrainTypes;
		this.mesh = mesh;
		this.plane = plane;
	}
}
