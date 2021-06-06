using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{

	private float healthValue = 10f;

	public override void Interact()
	{
		if (player.currentHealth < 100f)
		{
			player.TakeDamage(-healthValue);
			if (player.currentHealth > 100f)
			{
				player.currentHealth = 100f;
			}
		}
		this.gameObject.SetActive(false);
		UpdatePosition();
		this.gameObject.SetActive(true);
	}
}
