using System;
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
        if (NavMeshAgent!=null && target != null)
        {
            try
            {
                NavMeshAgent.SetDestination(target.position);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
