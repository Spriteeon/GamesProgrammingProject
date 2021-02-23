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

	void Start()
	{
		GenerateTile();
	}

	void GenerateTile()
	{
		// calculate tile depth and width based on the mesh vertices
		Vector3[] meshVertices = this.meshFilter.mesh.vertices;
		int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
		int tileWidth = tileDepth;

		// calculate the offsets based on the tile position
		float[,] heightMap = this.noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, this.mapScale);

		// generate a heightMap using noise
		Texture2D tileTexture = BuildTexture(heightMap);
		this.tileRenderer.material.mainTexture = tileTexture;
	}

	private Texture2D BuildTexture(float[,] heightMap)
	{
		int tileDepth = heightMap.GetLength(0);
		int tileWidth = heightMap.GetLength(1);

		Color[] colourMap = new Color[tileDepth * tileWidth];
		for (int z = 0; z < tileDepth; z++)
		{
			for (int x = 0; x < tileWidth; x++)
			{
				// transform the 2D map index is an Array index
				int colourIndex = z * tileWidth + x;
				float height = heightMap[z, x];
				// assign as color a shade of grey proportional to the height value
				colourMap[colourIndex] = Color.Lerp(Color.black, Color.white, height);
			}
		}

		// create a new texture and set its pixel colors
		Texture2D tileTexture = new Texture2D(tileWidth, tileDepth);
		tileTexture.wrapMode = TextureWrapMode.Clamp;
		tileTexture.SetPixels(colourMap);
		tileTexture.Apply();

		return tileTexture;
	}
}
