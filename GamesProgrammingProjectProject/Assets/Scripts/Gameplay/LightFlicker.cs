using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public bool isFlickering = false;
    public float timeDelay;

    void OnEnable()
	{
        isFlickering = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.gameObject.activeSelf)
		{
            if (!isFlickering)
            {
                StartCoroutine(FlickeringLight());
            }
        }
    }

    IEnumerator FlickeringLight()
	{
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        timeDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(timeDelay);
        isFlickering = false;
    }
}
