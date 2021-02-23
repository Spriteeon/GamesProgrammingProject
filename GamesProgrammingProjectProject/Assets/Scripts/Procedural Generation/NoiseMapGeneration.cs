using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{

	public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ)
	{
		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[mapDepth, mapWidth];

		for (int z = 0; z < mapDepth; z++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				// calculate sample indices based on the coordinates and the scale
				float sampleX = (x + offsetX) / scale;
				float sampleZ = (z + offsetZ) / scale;

				// generate noise value using PerlinNoise
				float noise = Mathf.PerlinNoise(sampleX, sampleZ);

				noiseMap[z, x] = noise;
			}
		}

		return noiseMap;
	}
}
