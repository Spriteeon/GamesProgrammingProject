using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public Vector3 position;

    [SerializeField]
    private PauseMenu pauseMenu;

    float health;
    float candleLevel;
    float stamina;

    // Start is called before the first frame update
    void Start()
    {
        health = 100f;
        candleLevel = 100f;
        stamina = 100f;

        position = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = this.gameObject.transform.position;

        if (pauseMenu.isPaused)
		{
            this.GetComponent<FirstPersonController>().enabled = false;
        }
		else
		{
            this.GetComponent<FirstPersonController>().enabled = true;
        }

    }
}
