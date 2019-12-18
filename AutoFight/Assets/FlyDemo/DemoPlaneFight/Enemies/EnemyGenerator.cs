using System;
using System.Resources;
using DemoPlaneFight;

namespace UnityEngine.UI
{
    public class EnemyGenerator:MonoBehaviour
    {
        public static GameObject curBoss;
        private void Start()
        {
            GameObject g = Instantiate(ResourceMgr.Instance.BossObj);
            g.transform.position = new Vector3(0,0,10);
            curBoss = g;
        }
    }
}