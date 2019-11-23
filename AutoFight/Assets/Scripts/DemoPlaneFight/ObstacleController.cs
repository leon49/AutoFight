using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace DemoPlaneFight
{
    public class ObstacleController:MonoBehaviour
    {
        private uint maxNum = 10;
        private List<GameObject> _objects = new List<GameObject>();
        public GameObject obstaclePrefab;

        public float minX;
        public float maxX;

        public float minY;
        public float maxY;
        
        public void Start()
        {
            StartTime();
        }

        void StartTime()
        {
            TimerManager.Instance.addTimer(UnityEngine.Random.Range(0, 700), OnTime);
        }

        public void OnTime(Timer timer)
        {
            if (_objects.Count > maxNum)
            {
                StartTime();
                return;
            }
            
            int num = UnityEngine.Random.Range(1, 5);
            for (int i = 0; i < num; i++)
            {
                GameObject tOb = Instantiate(obstaclePrefab, new Vector3(
                    UnityEngine.Random.Range(minX,maxX),
                    UnityEngine.Random.Range(minY,maxY),
                    UnityEngine.Random.Range(100,120)
                ),Quaternion.identity);
                _objects.Add(tOb); 
            }

            StartTime();
        }
        
        public void Update()
        {
            for (int i = 0; i < _objects.Count; i++)
            {
                GameObject tOb = _objects[i];
                if (tOb.transform.position.z < -6)
                {
                    _objects.RemoveAt(i);
                    Destroy(tOb);
                    i--;
                }
            }
        }
    }
}