using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetAgentTarget : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent NavMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (NavMeshAgent!=null)
        {
            NavMeshAgent.SetDestination(target.position);
        }
    }
}
