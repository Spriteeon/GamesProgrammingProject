using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour
{
    public static GlobalControl instance;

    public bool customGame = false;
    public bool isTrees = true;
    public bool isFoliage = true;
    public bool isBuildings = true;
    public bool isItems = true;
    public bool isEnemies = true;

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
}
