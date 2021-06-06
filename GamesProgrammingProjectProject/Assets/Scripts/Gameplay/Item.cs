using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	public float radius = 1f;
	public Player player;

	private Vector3 mapCentre = new Vector3(190f, 0f, 190f);
	private float playableMapRadius = 170f;

	RaycastHit hit;
	float maxHeight = 20f;
	Ray ray;

	void Start()
	{
		player = PlayerManager.instance.player.GetComponent<Player>();
	}

	public virtual void Interact()
	{
		// This method is Overwritten
	}

	public void UpdatePosition()
	{
		Vector3 randomPoint = mapCentre;
		randomPoint.x += Random.Range(-playableMapRadius, playableMapRadius);
		randomPoint.z += Random.Range(-playableMapRadius, playableMapRadius);

		ray.origin = new Vector3(randomPoint.x, maxHeight, randomPoint.z);
		ray.direction = Vector3.down;
		hit = new RaycastHit();

		if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Floor")
		{
			float yPos = hit.point.y + 0.5f;

			Vector3 newPosition = new Vector3(randomPoint.x, yPos, randomPoint.z);
			this.gameObject.transform.position = newPosition;
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
