using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetNavAgentToObject : MonoBehaviour
{

    public GameObject destination;

    public GameObject NavAgent;

    public GameObject car;

    private GameObject guide;

    private NavMeshAgent agent;


    void FixedUpdate()
    {
        if (agent != null)
        {
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        Destroy(guide);
                    }
                }
            }
        }


    }

    public void SpawnAgent()
    {
        //isActive = NavAgent.activeSelf;
        //NavAgent.SetActive(!isActive);
        if (!GameObject.Find("Guide(Clone)"))
        {
            guide = Instantiate(NavAgent, car.transform.position+(car.transform.forward*6), car.transform.rotation);
        }
        //NavAgent.transform.position = new Vector3(car.transform.position.x, car.transform.position.y + 1, car.transform.position.z + 8);
        if (GameObject.Find("Guide(Clone)"))
        {
            agent = guide.GetComponent<NavMeshAgent>();
            if (destination.name.ToLower().Contains("garage"))
            {
                agent.destination = destination.transform.Find("Door Pivot").Find("Door").position;
            }
            else
            {
                agent.destination = destination.transform.position;
            }
        }
    }
}
