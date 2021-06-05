using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public Vector3 position;

    public float damage = 5f;

    RaycastHit hit;
    float maxHeight = 20f;
    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        position = this.gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        position = this.gameObject.transform.position;
    }

    public void UpdateEnemyPosition()
    {
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
    }
}
