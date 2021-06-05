using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public Vector3 position;

    [SerializeField]
    private PauseMenu pauseMenu;
    public GameObject candle;

    public float maxHealth = 100f;
    public float currentHealth;
    public float maxCandle = 100f;
    public float currentCandle;

    float stamina;

    public UIBar healthBar;
    public UIBar candleBar;

    Timer candleTimer;

    // Start is called before the first frame update
    void Start()
    {
        candle.SetActive(true);
        candleTimer = new Timer(1f);

        currentHealth = maxHealth;
        healthBar.SetMaxValue(maxHealth);

        currentCandle = maxCandle;
        candleBar.SetMaxValue(maxCandle);

        stamina = 100f;

        position = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = this.gameObject.transform.position;

        if(candleTimer.ExpireReset())
		{
            DecreaseCandle(1f);
		}

        if (pauseMenu.isPaused)
		{
            this.GetComponent<FirstPersonController>().enabled = false;
        }
		else
		{
            this.GetComponent<FirstPersonController>().enabled = true;
        }

        if(Input.GetKeyDown(KeyCode.F))
		{
            SwitchCandle();
		}

  //      if(Input.GetKeyDown(KeyCode.Space))
		//{
  //          TakeDamage(20f);
		//}
    }

    void SwitchCandle()
	{
        candle.SetActive(!candle.activeSelf);
    }

    void TakeDamage(float damage)
	{
        currentHealth -= damage;
        healthBar.SetValue(currentHealth);
	}

    void DecreaseCandle(float value)
	{
        if (candle.activeSelf)
		{
            currentCandle -= value;
            candleBar.SetValue(currentCandle);
        }
	}
}
