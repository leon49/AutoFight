using System;
using DemoPlaneFight;
using DemoPlaneFight.Bullet;
using UnityEngine;
using UnityEngine.UI;

namespace FlyDemo.DemoPlaneFight.Plane
{
    public class PlaneAttackLogic : MonoBehaviour
    {
        private Weapon _weapon;

        private void Start()
        {
            _weapon = gameObject.AddComponent<Weapon>();
            _weapon.IsAutoAim = true;
            _weapon.speed = 1.5f;
            _weapon.BulletLifeTime = 2f;
            _weapon.fireInterval = 0.1f;
        }

        private void Update()
        {
            _weapon.curTarget = PickATarget();
            _weapon.FireLoop();
        }

        private GameObject PickATarget()
        {
            float minDis = 99999;
            GameObject tTarget = null;
            int obstacleNum = ObstacleLogic.objects.Count;
            for (int i = 0; i < obstacleNum + 1; i++)
            {
                GameObject target = null;
                Vector3 TargetPos = new Vector3();
                if (i == obstacleNum)
                {
                    target = EnemyGenerator.curBoss;
                }
                else
                {
                    target = ObstacleLogic.objects[i];
                }

                if (target==null)
                {
                    continue;
                }
                TargetPos = target.transform.position;

                float dis = Vector3.Distance(TargetPos, gameObject.transform.position);
                if (dis <= 50 /*&& !ObstacleLogic.objects[i].GetComponent<CommonObstacle>().isDead*/)
                {
                    if (dis < minDis && dis > 5)
                    {
                        minDis = dis;
                        tTarget = target;
                    }
                }
            }

            return tTarget;
        }
    }
}