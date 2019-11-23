using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Demo
{
    public class GameController : BehaviourSingleton<GameController>
    {
        public List<GameObject> GameObjectHeroes;
        public List<GameObject> GameObjectsMonsters;

        public GameObject hero;
        public GameObject monster;
        
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
                    
//                    Vector3 mousepos = Input.mousePosition;
//                    mousepos.z = 10;
//                    mousepos = Camera.main.ScreenToWorldPoint(mousepos);
//                    mousepos.y = 0;
//                    
//                    if (hit.collider.gameObject.name == "HeroSpawner")
//                    {
//                        mousepos.z = 10;
//                        GameObjectHeroes.Add(Instantiate(hero, mousepos , Quaternion.Inverse(Quaternion.identity)));
//                    }
//
//                    if (hit.collider.gameObject.name == "MonsterSpawner")
//                    {
//                        mousepos.z = -10;
//                        GameObjectsMonsters.Add(Instantiate(monster, mousepos , Quaternion.identity));
//                    }
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

        public void DestroyARole( GameObject gameObject )
        {
                GameObjectHeroes.Remove(gameObject);
                GameObjectsMonsters.Remove(gameObject);
        }
        
        private void Awake()
        {
            GameObjectHeroes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Hero"));
            GameObjectsMonsters =  new List<GameObject>(GameObject.FindGameObjectsWithTag("Monster"));
        }

        private void Start()
        {
        }
    }
}