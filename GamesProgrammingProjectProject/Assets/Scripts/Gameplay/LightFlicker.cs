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

    // This Function causes one of the light objects to turn on and off randomly, this ontop of a constant lighting effect causes a candle flicker
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
