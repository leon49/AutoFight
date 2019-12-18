using System;
using System.Collections.Generic;
using UnityEngine;

namespace DemoPlaneFight
{
    public class ObstacleLogic:MonoBehaviour
    {
        private uint maxNum = 10;
        public GameObject obstaclePrefab;
        
        public static ObjectPool<GameObject> _objectPool = new ObjectPool<GameObject>(100);

        public static List<GameObject> objects = new List<GameObject>();
        
        public float minX;
        public float maxX;

        public float minY;
        public float maxY;

        public void Start()
        {
            StartTime();
            _objectPool.ResetObjectFunc = ob =>
            {
                SetObs(ob);
                ob.SetActive(true);
            };
            
            EventCenter.Instance.AddListener<ObstacleReleaseEvent>(OREHandler);
        }

        void OREHandler( ObstacleReleaseEvent evt)
        {
            objects.Remove(evt.GO);
        }

        void StartTime()
        {
            TimerManager.Instance.addTimer(UnityEngine.Random.Range(0, 2000), OnTime);
        }

        public void OnTime(Timer timer)
        {
            if ( objects.Count > maxNum)
            {
                StartTime();
                return;
            }
            
            int num = UnityEngine.Random.Range(1, 3);
            for (int i = 0; i < num; i++)
            {
                GameObject tOb = _objectPool.Obtain(() =>
                {
                    GameObject g = Instantiate(obstaclePrefab);
                    SetObs(g);
                    return g;
                });
                tOb.GetComponent<CommonObstacle>().isDead = false;
                if (objects.IndexOf(tOb) == -1)
                {
                    objects.Add(tOb);    
                }
            }

            StartTime();
        }

        private void OnDestroy()
        {
            EventCenter.Instance.RemoveListener<ObstacleReleaseEvent>(OREHandler);
        }

        void SetObs(GameObject gameObject)
        {
            gameObject.transform.SetPositionAndRotation( new Vector3(
                UnityEngine.Random.Range(minX,maxX),
                UnityEngine.Random.Range(minY,maxY),
                UnityEngine.Random.Range(100,120)
            ),Quaternion.identity);
        }
        
        public void Update()
        {
        }
    }
}