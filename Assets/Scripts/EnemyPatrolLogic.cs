using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolLogic : MonoBehaviour
{
    private NavMeshAgent agent;

    // Ennemi en patrouille
    [SerializeField]
    private GameObject waypointsParent;

    private List<Vector3> waypointsPosition;
    private int currentWayPointIndex;

    void Start()
    {
        // Ennemi en patrouille
        agent = GetComponent<NavMeshAgent>();
        if (waypointsParent != null)
        {
            waypointsPosition = new List<Vector3>();
            foreach (Transform t in waypointsParent.GetComponentsInChildren<Transform>())
            {
                waypointsPosition.Add(t.position);
            }
            waypointsPosition.Remove(waypointsParent.transform.position);
            currentWayPointIndex = 0;
            agent.SetDestination(waypointsPosition[currentWayPointIndex]);
        }
    }


    void Update()
    {
        if (agent.enabled ) {
            EnemyMoving();
        }        
    }


    // ennemi en patrouille
    private void EnemyMoving()
    { 
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            currentWayPointIndex = ++currentWayPointIndex % waypointsPosition.Count;
            agent.SetDestination(waypointsPosition[currentWayPointIndex]);
        }
    }
}
