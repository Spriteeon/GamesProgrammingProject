using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{

    public Vector3 position;

    public float damage = 5f;

    RaycastHit hit;
    float maxHeight = 20f;
    Ray ray;

    public float lookRadius = 10f;
    Transform target;
    NavMeshAgent agent;

    float distanceToPlayer;

    public Vector3[] patrolPoints;
    private int currentPoint;

    // Start is called before the first frame update
    void Start()
    {
        position = this.gameObject.transform.position;

        target = PlayerManager.instance.player.transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        position = this.gameObject.transform.position;

        distanceToPlayer = Vector3.Distance(target.position, transform.position);

        if (distanceToPlayer <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distanceToPlayer <= agent.stoppingDistance)
            {
                // Face Player
                FaceTarget();

                // Attack Player
            }
        }
        else
        {
            // Patrol Waypoints
            Patrol();
        }
    }

    void Patrol()
	{

	}

    public void GetPatrolPoints(Vector3[] points)
	{
        patrolPoints = points;
	}

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
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
