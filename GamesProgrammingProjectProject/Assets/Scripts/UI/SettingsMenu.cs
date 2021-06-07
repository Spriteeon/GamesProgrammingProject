using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{

	//[SerializeField]
	public AudioMixer audioMixer;
    
    public void SetVolume(float volume)
	{
		audioMixer.SetFloat("masterVolume", volume);
	}

}
