using System;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
    public class RoleAnimator:MonoBehaviour
    {
        public string enemyTag = "";
        private Animator anim;
        private Rigidbody Rigidbody;
        private NavMeshAgent navMeshAgent;
        private GameObject obstacle;
     
        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            Transform tTra = GetComponent<Transform>().Find("Obstacle");
            if (tTra != null)
            {
                obstacle = tTra.gameObject;    
                obstacle.SetActive(false);
            }

            anim.Play("Idle");

            TimerManager.Instance.addTimer(200, timer =>
            {
                anim.Play("Run");
            });
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Contains(enemyTag))
            {
                anim.Play("Attacking1");
                GetComponent<Transform>().LookAt(other.transform);

                if (obstacle!=null)
                {
                    obstacle.SetActive( true );    
                }
                
                navMeshAgent.isStopped = true;
                navMeshAgent.enabled = false;
                
                if (null != Rigidbody)
                {
                    Rigidbody.isKinematic = true;
                }
            }
        }

        private void FixedUpdate()
        {

        }
    }
}