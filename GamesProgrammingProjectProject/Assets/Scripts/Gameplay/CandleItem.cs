using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleItem : Item
{

	private float candleValue = 10f;

	// Gives Player Candle fuel when picked up
	public override void Interact()
	{
		if(player.currentCandle < 100f)
		{
			player.IncreaseCandle(candleValue);
			if(player.currentCandle > 100f)
			{
				player.currentCandle = 100f;
			}
		}
		this.gameObject.SetActive(false);
		UpdatePosition();
		this.gameObject.SetActive(true);
	}
}
