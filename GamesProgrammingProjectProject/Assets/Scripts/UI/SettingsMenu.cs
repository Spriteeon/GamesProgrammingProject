using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

	[SerializeField]
	private AudioMixer audioMixer;
	[SerializeField]
	private GameObject customGameValuesUI;

	public void SetVolume(float volume)
	{
		audioMixer.SetFloat("masterVolume", volume);
	}

	public void CustomGameToggle(bool toggleValue)
	{
		// Set Global Bool Value
		GlobalControl.instance.customGame = toggleValue;

		// Display Custom Game UI
		if(toggleValue)
		{
			customGameValuesUI.SetActive(true);
		}
		else
		{
			customGameValuesUI.SetActive(false);
		}
	}

	public void IsTrees(bool isTrees)
	{
		// Set Global Bool Value
		GlobalControl.instance.isTrees = isTrees;
	}

	public void IsFoliage(bool isFoliage)
	{
		// Set Global Bool Value
		GlobalControl.instance.isFoliage = isFoliage;
	}

	public void IsBuildings(bool isBuildings)
	{
		// Set Global Bool Value
		GlobalControl.instance.isBuildings = isBuildings;
	}

	public void IsItems(bool isItems)
	{
		// Set Global Bool Value
		GlobalControl.instance.isItems = isItems;
	}

	public void IsEnemies(bool isEnemies)
	{
		// Set Global Bool Value
		GlobalControl.instance.isEnemies = isEnemies;
	}
}
