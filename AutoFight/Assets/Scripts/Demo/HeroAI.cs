using System;
using Pathfinding;
using UnityEngine;

namespace Demo
{
    public class HeroAI:AIPath
    {
        public float sleepVelocity = 0.4F;

        /** Speed relative to velocity with which to play animations */
        public float animationSpeed = 0.2F;
        public string enemyTag = "";
        private DynamicGridObstacle _dynamicGridObstacle;

        private void Awake()
        {
            _dynamicGridObstacle = GetComponent<DynamicGridObstacle>();
        }

        public new void Start ()
        {
            // Call Start in base script (AIPath)
            base.Start();
        }


        public override void OnTargetReached () {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Contains(enemyTag))
            {
                canMove = false;
                if (_dynamicGridObstacle!=null)
                {
                    _dynamicGridObstacle.enabled = true;    
                }
            }
        }
        

        protected override void Update () {
            base.Update();
        }
    }
}