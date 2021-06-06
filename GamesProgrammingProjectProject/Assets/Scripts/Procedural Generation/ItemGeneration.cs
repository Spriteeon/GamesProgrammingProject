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

	public void GenerateItems(int levelDepth, int levelWidth, float distanceBetweenVertices, LevelData levelData, Wave[] waves)
	{
		// Generate Items here
	}
}
