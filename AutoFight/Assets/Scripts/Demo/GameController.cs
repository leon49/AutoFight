using System;
using UnityEngine;

namespace Demo
{
    public class GameController:BehaviourSingleton<GameController>
    {
        public GameObject[] GameObjectHeroes;
        public GameObject[] GameObjectsMonsters;

        private void Awake()
        {
            GameObjectHeroes = GameObject.FindGameObjectsWithTag("Hero");
            GameObjectsMonsters = GameObject.FindGameObjectsWithTag("Monster");
        }

        private void Start()
        {
            
        }

        private void Update()
        {

        }
    }
}