using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is attached to an object that is not destroyed between scenes, used to set level parameters from the options menu
public class GlobalControl : MonoBehaviour
{
    public static GlobalControl instance;

    public bool customGame = false;
    public bool isTrees = true;
    public bool isFoliage = true;
    public bool isBuildings = true;
    public bool isItems = true;
    public bool isEnemies = true;

    public float terrainFreqAmp = 1f;
    public float treesFreqAmp = 1f;
    public float foliageFreqAmp = 1f;
    public float buildingsFreqAmp = 0.5f;
    public float itemsFreqAmp = 1f;
    public int numEnemies = 10;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void ResetValues()
	{
        customGame = false;
        isTrees = true;
        isFoliage = true;
        isBuildings = true;
        isItems = true;
        isEnemies = true;

        terrainFreqAmp = 1f;
        treesFreqAmp = 1f;
        foliageFreqAmp = 1f;
        buildingsFreqAmp = 0.5f;
        itemsFreqAmp = 1f;
        numEnemies = 10;
    }
}
