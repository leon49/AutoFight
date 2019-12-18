using System;
using System.Resources;
using DemoPlaneFight;
using DemoPlaneFight.Bullet;
using DemoPlaneFight.Plane;

namespace UnityEngine.UI
{
    public class NormalEnemy:BaseEnemy
    {
        private Weapon _weapon;
        
        public Slider Slider;
        public bool isDead;
        public static ObjectPool<GameObject> _objectPool = new ObjectPool<GameObject>(100);

        private void Start()
        {
            _weapon = gameObject.AddComponent<Weapon>();
            _weapon.curTarget = MasterPlane.PlayerPlane;

            _weapon.fireInterval = 0.2f;
            _weapon.speed = 0.4f;
            _weapon.bulletPrefab = ResourceMgr.Instance.bulletSpherePrefab;
            _weapon.BulletLifeTime = 7;
        }

        private void Update()
        {
            _weapon.FireLoop();
        }

        public override void UpdateHP()
        {
            Slider.value = (MaxHP - _hp) / MaxHP;
            if (_hp<0)
            {
                DieEffect();
                Reset();
            }
        }

        private void Reset()
        {
            isDead = true;
            Slider.gameObject.SetActive(false);
            Hp = MaxHP;
            _objectPool.Free(gameObject);
            gameObject.SetActive(false);
        }

        private void DieEffect()
        {
            
        }
    }
}