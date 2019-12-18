using System;
using DemoPlaneFight.Plane;
using FlyDemo.DemoPlaneFight.Plane;
using UnityEngine;

namespace DemoPlaneFight.Bullet
{
    public class BulletLogic : MonoBehaviour
    {
        public float speed = 1;
        public bool isDead = false;
        public float lifeTime = 2;
        float startTime = 0;
        private GameObject target;
        public Vector3 targetPos;
        public ObjectPool<GameObject> masterWeaponPool;
        public bool IsAutoAim;

        public void SetTarget(GameObject aTarget)
        {
            if (aTarget != null)
            {
                target = aTarget;
                targetPos = aTarget.transform.position;
            }
        }

        private void Update()
        {
            startTime += Time.deltaTime;

            if (target != null )
            {
                if (IsAutoAim )
                {
                    targetPos = target.transform.position;
                }
                
                transform.LookAt(targetPos);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);

                if (Vector3.Distance(transform.position, targetPos) < 0.1)
                {
                    Reset();
                }
                Debug.Log($"target:{targetPos}");
            }
            else
            {
                transform.Translate(new Vector3(0, 0, speed));
            }

            if (startTime > lifeTime
                || transform.position.z < -100)
            {
                Reset();
            }
        }

        private void Reset()
        {
            isDead = true;
            startTime = 0;
            gameObject.SetActive(false);
            masterWeaponPool.Free(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            Reset();
        }
    }
}