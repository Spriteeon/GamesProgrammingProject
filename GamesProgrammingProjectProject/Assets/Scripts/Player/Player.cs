using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

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

    public bool isSafe = true;
    public bool isInside = false;

    Timer candleTimer;

    RaycastHit hit;
    float maxHeight = 20f;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {

        candle.SetActive(true);
        candleTimer = new Timer(1f);
        isSafe = true;

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

        // Pause Menu
		if (pauseMenu.isPaused)
		{
            this.GetComponent<FirstPersonController>().enabled = false;
        }
		else
		{
            this.GetComponent<FirstPersonController>().enabled = true;
        }

        // Candle Functionality
        if (candleTimer.ExpireReset())
        {
            AutoCandle(4f);
        }
        if (currentCandle > 0)
		{
            if (Input.GetKeyDown(KeyCode.F))
            {
                SwitchCandle();
            }
        }
        if(currentCandle <= 0)
		{
            candle.SetActive(false);
            isSafe = false;
        }

        // Health Functionality
        if(currentHealth <= 0)
		{
            Die();
		}
    }

    void OnTriggerEnter(Collider col)
	{
        if(col.gameObject.tag == "HealthItem")
		{
            col.gameObject.GetComponent<HealthItem>().Interact();
        }
        if (col.gameObject.tag == "CandleItem")
		{
            col.gameObject.GetComponent<CandleItem>().Interact();
        }

        if(col.gameObject.tag == "Well")
		{
            // Add Screen Fade?

            // Start 2 second Timer then open Win Scene
            StartCoroutine(WinGame(2f));
        }
    }

    IEnumerator WinGame(float time)
	{
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(3);
    }

    void Die()
	{
        SceneManager.LoadScene(4);
    }

    // This makes sure the Player is not under the floor when game starts
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

    // Turns Candle on and off
    void SwitchCandle()
	{
        candle.SetActive(!candle.activeSelf);

        if(candle.activeSelf)
		{
            isSafe = true;
		}
        else if (!candle.activeSelf)
        {
            isSafe = false;
        }
    }

    public void IncreaseCandle(float value)
	{
        currentCandle += value;
        candleBar.SetValue(currentCandle);
	}

    public void TakeDamage(float damage)
	{
        currentHealth -= damage;
        healthBar.SetValue(currentHealth);
    }

    // Decreases candle level if using outside, re-charges it if inside
    void AutoCandle(float value)
	{
        if (isInside && currentCandle < 100) // Increase Candle
		{
            currentCandle += 2 * value;
            if (currentCandle == 99f)
			{
                currentCandle = 100f;
			}
            candleBar.SetValue(currentCandle);
        }
        else if (candle.activeSelf && !isInside && currentCandle >= 0) // Decrease Candle
		{
            currentCandle -= value;
            candleBar.SetValue(currentCandle);
        }
	}
}
