using System;
using System.Collections.Generic;
using UnityEngine;

namespace DemoPlaneFight
{
    public class ResourceMgr:BehaviourSingleton<ResourceMgr>
    {
        public GameObject bulletPrefab;
        public GameObject bulletSpherePrefab;
        public GameObject hitExpo;
        public GameObject obstacleExpo;

        public GameObject testObj;

        public GameObject BossObj;
        
        public List<ObstacleConf> obstaclePrefabs;
    }

    [Serializable]
    public class ObstacleConf
    {
        public string tag;
        public GameObject GOPrefab;
    }
}