using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.FirstPerson;

public class InsideBuilding : MonoBehaviour
{
	private Player player;

	void Start()
	{
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

    void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			player.isSafe = true;
			Debug.Log("Player entered Building");
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.gameObject.tag == "Player")
		{
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
			player.isSafe = false;
			Debug.Log("Player left Building");
		}
	}

}
