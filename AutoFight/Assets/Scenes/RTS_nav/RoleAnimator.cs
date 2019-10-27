using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
    public class RoleAnimator:MonoBehaviour
    {
        public string enemyTag = "";
        public uint SightRange = 4;
        public uint AttackRange = 2;
        
        private Animator anim;
        private Rigidbody Rigidbody;
        private NavMeshAgent navMeshAgent;
        private GameObject obstacle;

        private void Awake()
        {

        }

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
//                attack(other.gameObject);
            }
        }

        private void attack( GameObject gameObject )
        {
            anim.Play("Attacking1");
            transform.LookAt(gameObject.transform);

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

        private void Update()
        {
            GameObject[] gameObjects = enemyTag == "Monster"
                ? GameController.Instance.GameObjectsMonsters
                : GameController.Instance.GameObjectHeroes;

            GameObject tEnemyToChase = null;
            float minDis = 99999999;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject tEnemy = gameObjects[i];
                float dis = Vector3.Distance(tEnemy.transform.position,
                    transform.position);

                if (dis <= AttackRange * 2)
                {
                    attack(tEnemy);
                    break;
                }

                if (dis <= SightRange && dis > AttackRange * 2)
                {
                    if (minDis < dis)
                    {
                        minDis = dis;
                        tEnemyToChase = tEnemy;
                    }
                }
            }

            if (tEnemyToChase != null)
            {
                var position = tEnemyToChase.transform.position;
                navMeshAgent.SetDestination(position);
            }
        }
        
        private void OnDrawGizmos()
        {
            Transform tTra = GetComponent<Transform>().Find("Obstacle");
            if (tTra != null)
            {
                obstacle = tTra.gameObject;    
            }
            
            if (obstacle!=null)
            {
                if ((Selection.activeGameObject == gameObject || Selection.activeGameObject == obstacle ) && !obstacle.GetComponent<MeshRenderer>().enabled)
                {
                    obstacle.GetComponent<MeshRenderer>().enabled = true;
                }
                
                if (Selection.activeGameObject != gameObject && Selection.activeGameObject != obstacle  && obstacle.GetComponent<MeshRenderer>().enabled)
                {
                    obstacle.GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            var transform1 = transform;
            var position1 = transform1.position;
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(position1 , transform1.up, AttackRange);
            UnityEditor.Handles.DrawWireDisc(position1 , transform.up, SightRange);
        }

        private void FixedUpdate()
        {

        }
    }
}