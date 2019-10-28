using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
    public class RoleAnimator : MonoBehaviour
    {
        public string enemyTag = "";
        public uint SightRange = 4;
        public uint AttackRange = 2;

        private Animator anim;
        private Rigidbody Rigidbody;
        private NavMeshAgent navMeshAgent;
        public GameObject obstacle;

        public Transform endTarget;
        
        private void Awake()
        {
        }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            navMeshAgent.SetDestination(endTarget.position);

            Transform tTra = GetComponent<Transform>().Find("Obstacle");
            if (tTra != null)
            {
                obstacle = tTra.gameObject;
                obstacle.SetActive(false);
            }

            anim.Play("Idle");

            TimerManager.Instance.addTimer(200, timer => { anim.Play("Run"); });
        }

        private bool isForcing;

        public void ForceToNewDestination(Vector3 aDes)
        {
            isForcing = true;
            if (obstacle != null)
            {
                obstacle.SetActive(false);
            }

            TimerManager.Instance.addTimer(20, timer =>
            {
                anim.Play("Run");
                isAttacking = false;

                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(aDes);
                transform.LookAt(aDes);
            });
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Contains(enemyTag))
            {
//                attack(other.gameObject);
            }
        }

        private bool isAttacking = false;
        private GameObject lastEnemy;

        private void attack(GameObject gameObject)
        {
            if (isAttacking)
            {
                return;
            }

            lastEnemy = gameObject;
            isAttacking = true;
            anim.Play("Attacking1");
            transform.LookAt(gameObject.transform);

            if (obstacle != null)
            {
                obstacle.SetActive(true);
            }

            navMeshAgent.enabled = false;
        }

        void CancelAttack()
        {
            if (!isAttacking)
            {
                return;
            }
            lastEnemy = null;
            
            if (obstacle != null)
            {
                obstacle.SetActive(false);
            }

            TimerManager.Instance.addTimer(20, timer =>
            {
                isAttacking = false;
                anim.Play("Run");
                navMeshAgent.enabled = true;
                navMeshAgent.SetDestination(endTarget.position);
            });
        }

        private void Update()
        {
            if ( isForcing && navMeshAgent.enabled && !navMeshAgent.pathPending)
            {
                if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
                {
                    if (!navMeshAgent.hasPath || Math.Abs(navMeshAgent.velocity.sqrMagnitude) < 0.01)
                    {
                        isForcing = false;
                    }
                }
            }
            
            if (isForcing)
            {
                return;
            }

            GameObject[] gameObjects = enemyTag == "Monster"
                ? GameController.Instance.GameObjectsMonsters
                : GameController.Instance.GameObjectHeroes;

            GameObject tEnemyToChase = null;
            float minDis = 99999999;
            bool hasEnemy2Attack = false;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                GameObject tEnemy = gameObjects[i];
                float dis = Vector3.Distance(tEnemy.transform.position,
                    transform.position);

                if (dis <= AttackRange * 2)
                {
                    hasEnemy2Attack = true;
                    if ( lastEnemy != null && lastEnemy != tEnemy)
                    {
                        transform.LookAt(tEnemy.transform);
                    }
                    else
                    {
                        attack(tEnemy);        
                    }
                    break;
                }

                if (dis <= SightRange && dis > AttackRange * 2)
                {
                    if (minDis > dis)
                    {
                        minDis = dis;
                        tEnemyToChase = tEnemy;
                    }
                }
            }

            if (!hasEnemy2Attack)
            {
                CancelAttack();
            }
            
            if (tEnemyToChase != null && !isAttacking )
            {
                var position = tEnemyToChase.transform.position;
                navMeshAgent.SetDestination(position);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Transform tTra = GetComponent<Transform>().Find("Obstacle");
            if (tTra != null)
            {
                obstacle = tTra.gameObject;
            }

            if (obstacle != null)
            {
                if ((Selection.activeGameObject == gameObject || Selection.activeGameObject == obstacle) &&
                    !obstacle.GetComponent<MeshRenderer>().enabled)
                {
                    obstacle.GetComponent<MeshRenderer>().enabled = true;
                }

                if (Selection.activeGameObject != gameObject && Selection.activeGameObject != obstacle &&
                    obstacle.GetComponent<MeshRenderer>().enabled)
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
            UnityEditor.Handles.DrawWireDisc(position1, transform1.up, AttackRange);
            UnityEditor.Handles.DrawWireDisc(position1, transform.up, SightRange);
        }
#endif


        private void OnMouseDown()
        {
        }



        private void FixedUpdate()
        {
        }
    }
}