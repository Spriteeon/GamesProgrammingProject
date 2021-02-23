using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{

	public float[,] GenerateNoiseMap(int mapDepth, int mapWidth, float scale)
	{
		// create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[mapDepth, mapWidth];

		for (int z = 0; z < mapDepth; z++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				// calculate sample indices based on the coordinates and the scale
				float sampleX = x / scale;
				float sampleZ = z / scale;

				// generate noise value using PerlinNoise
				float noise = Mathf.PerlinNoise(sampleX, sampleZ);

				noiseMap[z, x] = noise;
			}
		}

		return noiseMap;
	}
}
