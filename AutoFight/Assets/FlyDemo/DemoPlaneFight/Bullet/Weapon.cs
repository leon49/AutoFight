using UnityEngine;

namespace DemoPlaneFight.Bullet
{
    public class Weapon:MonoBehaviour
    {
        public float fireInterval = 0.1f;
        private uint fireTimes = 0;
        private float startTime = 0;
        public GameObject curTarget;

        public bool IsAutoAim;
        public GameObject bulletPrefab;
        public float speed;
        public float BulletLifeTime;
        
        public ObjectPool<GameObject> bulletsPool = new ObjectPool<GameObject>(999);
        
        public void FireLoop()
        {
            startTime += Time.deltaTime;
            if ( startTime > fireInterval*fireTimes )
            {
                fire();
                fireTimes++;
            }
        }

        void fire()
        {
            GameObject bulletTmp = bulletsPool.Obtain(() =>
            {
                GameObject bulletGO = Instantiate(bulletPrefab==null? ResourceMgr.Instance.bulletPrefab:bulletPrefab);
                bulletGO.AddComponent<BulletLogic>(); 
                return bulletGO;
            });
            
            bulletTmp.SetActive(true);
            bulletTmp.transform.position = transform.position;
            bulletTmp.transform.rotation = Quaternion.identity;
            bulletTmp.GetComponent<BulletLogic>().isDead = false;
            bulletTmp.GetComponent<BulletLogic>().SetTarget(curTarget);
            bulletTmp.GetComponent<BulletLogic>().IsAutoAim = IsAutoAim;
            bulletTmp.GetComponent<BulletLogic>().speed = speed;
            bulletTmp.GetComponent<BulletLogic>().masterWeaponPool = bulletsPool;
            bulletTmp.GetComponent<BulletLogic>().lifeTime = BulletLifeTime;
        }
    }
}