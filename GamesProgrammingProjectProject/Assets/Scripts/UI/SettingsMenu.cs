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
	[SerializeField]
	private GameObject terrainSlider;
	[SerializeField]
	private GameObject treesSlider;
	[SerializeField]
	private GameObject foliageSlider;
	[SerializeField]
	private GameObject buildingsSlider;
	[SerializeField]
	private GameObject itemsSlider;
	[SerializeField]
	private GameObject enemiesSlider;

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

		// Display Slider UI
		if (isTrees)
		{
			treesSlider.SetActive(true);
		}
		else
		{
			treesSlider.SetActive(false);
		}
	}

	public void IsFoliage(bool isFoliage)
	{
		// Set Global Bool Value
		GlobalControl.instance.isFoliage = isFoliage;

		// Display Slider UI
		if (isFoliage)
		{
			foliageSlider.SetActive(true);
		}
		else
		{
			foliageSlider.SetActive(false);
		}
	}

	public void IsBuildings(bool isBuildings)
	{
		// Set Global Bool Value
		GlobalControl.instance.isBuildings = isBuildings;

		// Display Slider UI
		if (isBuildings)
		{
			buildingsSlider.SetActive(true);
		}
		else
		{
			buildingsSlider.SetActive(false);
		}
	}

	public void IsItems(bool isItems)
	{
		// Set Global Bool Value
		GlobalControl.instance.isItems = isItems;

		// Display Slider UI
		if (isItems)
		{
			itemsSlider.SetActive(true);
		}
		else
		{
			itemsSlider.SetActive(false);
		}
	}

	public void IsEnemies(bool isEnemies)
	{
		// Set Global Bool Value
		GlobalControl.instance.isEnemies = isEnemies;

		// Display Slider UI
		if (isEnemies)
		{
			enemiesSlider.SetActive(true);
		}
		else
		{
			enemiesSlider.SetActive(false);
		}
	}

	public void SetTerrain(float value)
	{
		// Set the Freq and Amp for the Noise waves, bigger the number the more will spawn*
		GlobalControl.instance.terrainFreqAmp = value;
	}

	public void SetTrees(float value)
	{
		// Set the Freq and Amp for the Noise waves, bigger the number the more will spawn*
		GlobalControl.instance.treesFreqAmp = value;
	}

	public void SetFoliage(float value)
	{
		// Set the Freq and Amp for the Noise waves, bigger the number the more will spawn*
		GlobalControl.instance.foliageFreqAmp = value;
	}

	public void SetBuildings(float value)
	{
		// Set the Freq and Amp for the Noise waves, bigger the number the more will spawn*
		GlobalControl.instance.buildingsFreqAmp = value;
	}

	public void SetItems(float value)
	{
		// Set the Freq and Amp for the Noise waves, bigger the number the more will spawn*
		GlobalControl.instance.itemsFreqAmp = value;
	}

	public void SetEnemies(float value)
	{
		// Set the Freq and Amp for the Noise waves, bigger the number the more will spawn*
		GlobalControl.instance.numEnemies = (int)value;
	}
}
