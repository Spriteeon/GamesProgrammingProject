using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMapGeneration : MonoBehaviour
{
	public float[,] GeneratePerlinNoiseMap(int mapDepth, int mapWidth, float scale, float offsetX, float offsetZ, Wave[] waves)
	{
		// Create an empty noise map with the mapDepth and mapWidth coordinates
		float[,] noiseMap = new float[mapDepth, mapWidth];

		for (int z = 0; z < mapDepth; z++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				// Calculate sample indices based on the coordinates and the scale
				float sampleX = (x + offsetX) / scale;
				float sampleZ = (z + offsetZ) / scale;

				float noise = 0f;
				float normalization = 0f;
				foreach (Wave wave in waves)
				{
					// Generate noise value using PerlinNoise for a given Wave
					noise += wave.amplitude * Mathf.PerlinNoise(sampleX * wave.frequency + wave.seed, sampleZ * wave.frequency + wave.seed);
					normalization += wave.amplitude;
				}
				// Normalize the noise value so that it is within 0 and 1
				noise /= normalization;

				noiseMap[z, x] = noise;
			}
		}

		return noiseMap;
	}
}

[System.Serializable]
public class Wave
{
	public float seed;
	public float frequency;
	public float amplitude;
}
