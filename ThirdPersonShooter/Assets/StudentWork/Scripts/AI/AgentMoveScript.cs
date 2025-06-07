using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMoveScript : MonoBehaviour
{
    [SerializeField] private Transform MoveToPoint;
    [SerializeField] private NavMeshAgent NavMeshAgent;

    // Update is called once per frame
    void Update()
    {
        NavMeshAgent.destination = PlayerLocatorSingleton.Instance.transform.position;
    }
}
