using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{

    public NavMeshSurface[] surfaces;

    public void Bake()
	{
        for (int i = 0; i < surfaces.Length; i++)
        {
            // Builds new NavMesh for any surface I add to surfaces array
            surfaces[i].BuildNavMesh();
        }
    }
}
