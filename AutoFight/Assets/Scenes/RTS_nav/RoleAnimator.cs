using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
    public class RoleAnimator : MonoBehaviour
    {
        public GameObject bloodImg;
        public Canvas imgCan;
        
        private long HpValue = 0;
        public long MaxHpValue = 200;

        public void BeAttack( long aDamage )
        {
            HpValue -= aDamage;
            if (HpValue < 0)
            {
                HpValue = 0;
            }
            bloodImg.transform.localScale = new Vector3((float)(MaxHpValue-HpValue) / (float)MaxHpValue,1,1);
            
            if (HpValue==0)
            {
                EventCenter.Instance.Raise(new RoleDeadEvent(gameObject));
                GameController.Instance.DestroyARole(gameObject);
                Destroy(gameObject);
            }
        }
        
        public string enemyTag = "";
        public uint SightRange = 14;
        public uint AttackRange = 2;

        private Animator anim;
        private Rigidbody Rigidbody;
        private NavMeshAgent navMeshAgent;
        public GameObject obstacle;

        
        public string endTargetName;
        
        private void Awake()
        {
            HpValue = MaxHpValue;
            EventCenter.Instance.AddListener<RoleDeadEvent>(EventHandler);
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveListener<RoleDeadEvent>(EventHandler);
        }

        void EventHandler( RoleDeadEvent evt )
        {
            if (lastEnemy == evt.GameObject)
            {
                CancelAttack();
            }
        }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();

            navMeshAgent.SetDestination(GameObject.Find(endTargetName).transform.position);

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


        private bool isAttacking = false;
        public GameObject lastEnemy;
        
        private void attack(GameObject gameObject)
        {
            if (isAttacking)
            {
                return;
            }

            lastEnemy = gameObject;
            isAttacking = true;
            anim.Play("Attacking1");

            if (gameObject != null)
            {
                transform.LookAt(gameObject.transform);    
            }
            

            if (obstacle != null)
            {
                obstacle.SetActive(true);
            }

            navMeshAgent.enabled = false;
//            navMeshAgent.velocity = Vector3.zero;
//            navMeshAgent.Stop(true);
        }

        void CancelAttack()
        {
            if (!isAttacking)
            {
                return;
            }
            lastEnemy = null;

            GameObject tEnemy = findEnemy2Attack();
            if (tEnemy == null)
            {
                if (obstacle != null  )
                {
                    obstacle.SetActive(false);
                }

                TimerManager.Instance.addTimer(10, timer =>
                {
                    isAttacking = false;
                    anim.Play("Run");
                    navMeshAgent.enabled = true;
                    navMeshAgent.SetDestination(GameObject.Find(endTargetName).transform.position);
                });
            }
        }

        private void Update()
        {
            imgCan.transform.rotation = Quaternion.identity;
            
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

            if (lastEnemy!=null)
            {
                    float dis = Vector3.Distance(lastEnemy.transform.position,
                        transform.position);
                    if (dis >= AttackRange * 2)
                    {
                        lastEnemy = null;
                    }
            }

            refreshEnemy();
        }

        void refreshEnemy()
        {
            List<GameObject> gameObjects = enemyTag == "Monster"
                ? GameController.Instance.GameObjectsMonsters
                : GameController.Instance.GameObjectHeroes;

            GameObject tEnemyToChase = null;
            float minDis = 99999999;
            bool hasEnemy2Attack = false;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject tEnemy = gameObjects[i];
                float dis = Vector3.Distance(tEnemy.transform.position,
                    transform.position);

                if (dis <= AttackRange * 2)
                {
                    hasEnemy2Attack = true;
                    if ( lastEnemy == null )
                    {
                        attack(tEnemy);        
                    }
                    break;
                }

                if (dis <= SightRange && dis > AttackRange * 2 )
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

        GameObject findEnemy2Attack()
        {
            List<GameObject> gameObjects = enemyTag == "Monster"
                ? GameController.Instance.GameObjectsMonsters
                : GameController.Instance.GameObjectHeroes;

            GameObject tEnemyToChase = null;
            float minDis = 99999999;
            bool hasEnemy2Attack = false;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject tEnemy = gameObjects[i];
                float dis = Vector3.Distance(tEnemy.transform.position,
                    transform.position);

                if (dis <= AttackRange * 2)
                {
                    hasEnemy2Attack = true;
                    if (lastEnemy == null)
                    {
                        return tEnemy;
                    }

                    break;
                }
            }

            return null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
//            Transform tTra = GetComponent<Transform>().Find("Obstacle");
//            if (tTra != null)
//            {
//                obstacle = tTra.gameObject;
//            }
//
//            if (obstacle != null)
//            {
//                if ((Selection.activeGameObject == gameObject || Selection.activeGameObject == obstacle) &&
//                    !obstacle.GetComponent<MeshRenderer>().enabled)
//                {
//                    obstacle.GetComponent<MeshRenderer>().enabled = true;
//                }
//
//                if (Selection.activeGameObject != gameObject && Selection.activeGameObject != obstacle &&
//                    obstacle.GetComponent<MeshRenderer>().enabled)
//                {
//                    obstacle.GetComponent<MeshRenderer>().enabled = false;
//                }
//            }
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