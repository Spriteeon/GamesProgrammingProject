using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour
{
    public Vector3 position;

    [SerializeField]
    private PauseMenu pauseMenu;
    [SerializeField]
    private GameObject candle;

    public float maxHealth = 100f;
    public float currentHealth;
    public float maxCandle = 100f;
    public float currentCandle;

    float stamina;

    public UIBar healthBar;
    public UIBar candleBar;

    public bool isSafe = false;

    Timer candleTimer;

    RaycastHit hit;
    float maxHeight = 20f;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {

        candle.SetActive(true);
        candleTimer = new Timer(1f);
        isSafe = false;

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

		if (candleTimer.ExpireReset())
		{
			IncreaseCandle(1f);
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
    }

    public void UpdatePlayerPosition()
    {
        this.gameObject.SetActive(false);

        Debug.Log("Updating Position");

        float xPos = this.gameObject.transform.position.x;
        float zPos = this.gameObject.transform.position.z;
        float yPos = 0f;

        ray.origin = new Vector3(xPos, maxHeight, zPos);
        ray.direction = Vector3.down;
        hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit) && hit.transform.tag == "Floor")
        {
            yPos = hit.point.y + 1f;
            Vector3 newPosition = new Vector3(xPos, yPos, zPos);

            this.gameObject.transform.position = newPosition;
        }

        this.gameObject.SetActive(true);
    }

    void SwitchCandle()
	{
        candle.SetActive(!candle.activeSelf);
    }

    public void TakeDamage(float damage)
	{
        currentHealth -= damage;
        healthBar.SetValue(currentHealth);
	}

    void IncreaseCandle(float value)
	{
        if (isSafe && currentCandle < 100) // Increase Candle
		{
            currentCandle += 2 * value;
            if (currentCandle == 99f)
			{
                currentCandle = 100f;
			}
            candleBar.SetValue(currentCandle);
        }
        else if (candle.activeSelf && !isSafe && currentCandle >= 0) // Decrease Candle
		{
            currentCandle -= value;
            candleBar.SetValue(currentCandle);
        }
	}
}
