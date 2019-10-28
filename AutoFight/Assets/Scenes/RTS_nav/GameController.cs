using System;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
    public class GameController : BehaviourSingleton<GameController>
    {
        public GameObject[] GameObjectHeroes;
        public GameObject[] GameObjectsMonsters;

        private GameObject _gameObjectMouseSelected;
        private Vector3 LastMouseDownPos;

        public GameObject poss;
        Ray ray;
        RaycastHit hit;

        void Update()
        {
            if (Camera.main != null) ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.CompareTag("Hero")
                        ||hit.collider.gameObject.CompareTag("Monster"))
                    {
                        _gameObjectMouseSelected = hit.collider.gameObject;
                    }
                }
                else
                {
                    LastMouseDownPos = Input.mousePosition;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_gameObjectMouseSelected != null)
                {
                    Vector3 mousepos = Input.mousePosition;
                    mousepos.z = 10;
                    mousepos = Camera.main.ScreenToWorldPoint(mousepos);
                    mousepos.y = 0;
                    
                    RoleAnimator tRoleAnimator = _gameObjectMouseSelected.GetComponent<RoleAnimator>();
                    tRoleAnimator.ForceToNewDestination(mousepos);

                    _gameObjectMouseSelected = null;

//                    GameObject t = Instantiate(poss);
//                    t.transform.position = mousepos;
                }

//                if (LastMouseDownPos!=null)
//                {
//                    for (int i = 0; i < GameObjectHeroes.Length; i++)
//                    {
//                        Vector3 curPos = GameObjectHeroes[i].transform.position;
//                        NavMeshAgent navMeshAgent = GameObjectHeroes[i].GetComponent<NavMeshAgent>();
//                        navMeshAgent.enabled = true;
//                        
//                        navMeshAgent.SetDestination(curPos+);
//                        
//                        RoleAnimator tRoleAnimator = GameObjectHeroes[i].GetComponent<RoleAnimator>();
//                        tRoleAnimator.obstacle.SetActive(false);
//                    }
//                }
            }
        }

        private void Awake()
        {
            GameObjectHeroes = GameObject.FindGameObjectsWithTag("Hero");
            GameObjectsMonsters = GameObject.FindGameObjectsWithTag("Monster");
        }

        private void Start()
        {
        }
    }
}