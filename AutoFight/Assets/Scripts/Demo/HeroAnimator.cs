using System;
using UnityEngine;

namespace Demo
{
    public class HeroAnimator:MonoBehaviour
    {
        public string enemyTag = "";
        public Animator anim;
        public Rigidbody Rigidbody;
        private void Start()
        {
            Rigidbody = gameObject.GetComponent<Rigidbody>();
            anim = gameObject.GetComponent<Transform>().GetChild(0).GetComponent<Animator>();
            
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
                
                if (null != Rigidbody)
                {
//                    Rigidbody.velocity = Vector3.zero;
//                    Rigidbody.angularVelocity = Vector3.zero;
                    Rigidbody.isKinematic = true;
                }
            }
        }

        private void FixedUpdate()
        {

        }
    }
}