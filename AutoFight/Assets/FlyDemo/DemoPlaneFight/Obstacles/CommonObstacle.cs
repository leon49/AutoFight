using System;
using DG.Tweening;
using FlyDemo.DemoPlaneFight.Plane;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace DemoPlaneFight
{
    public class CommonObstacle:BasePlane
    {
        public float speed = -0.5f;
        public Slider Slider;

        private float minSpeed = -0.05f;
        private float MaxSpeed = -0.5f;
        public bool isDead = false;
        private GameObject tGO;
        private float rX;
        private float rY;
        private float rZ;

        public override void UpdateHP()
        {
            Slider.value = (MaxHP - _hp) / MaxHP;
            if (_hp<0)
            {
                DieEffect();
                Reset();
            }
        }

        private void Start()
        {
            MaxHP = Random.Range(10, 30);
            Slider.gameObject.SetActive(false);
            Hp = MaxHP;
            speed = MaxSpeed * (MaxHP / 30);

            GameObject prefab = ResourceMgr.Instance.obstaclePrefabs[Random.Range(0,ResourceMgr.Instance.obstaclePrefabs.Count)].GOPrefab;
            tGO = Instantiate(prefab, gameObject.transform.Find("View"));

            float scale = Random.Range(0.2f, 0.6f);
            tGO.transform.localScale = new Vector3(scale,scale,scale);
            
            rX = Random.Range(0f, 1f);
            rY = Random.Range(0f, 1f);
            rZ = Random.Range(0f, 1f);
//            float scale = Hp / 10;
//            transform.localScale = new Vector3(scale,scale,scale);
            
            isDead = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag.Contains("Hero"))
            {
                DieEffect();
                Reset();
                return;
            }
            
            if (!Slider.gameObject.activeSelf)
            {
                Slider.gameObject.SetActive(true);
            }
            Hp--;
            hitted(other.gameObject);
        }

        void hitted(GameObject aGameObject)
        {
            if (Hp>0)
            {
                GameObject goHitExpo = Instantiate(ResourceMgr.Instance.hitExpo,gameObject.transform);
                goHitExpo.transform.position = aGameObject.transform.position;
//                goHitExpo.transform.localPosition = new Vector3(0,0,-0.5f);

                TimerManager.Instance.addTimer(2000, timer =>
                {
                    Destroy(goHitExpo);
                }, goHitExpo);
            }
        }

        void DieEffect()
        {
            GameObject goExpo = Instantiate(ResourceMgr.Instance.obstacleExpo);
            goExpo.transform.position = gameObject.transform.position;
            TimerManager.Instance.addTimer(2000, timer =>
            {
                Destroy(goExpo);
            }, goExpo);
        }
        
        void Reset()
        {
            isDead = true;
            Slider.gameObject.SetActive(false);
            Hp = MaxHP;
            ObstacleLogic._objectPool.Free(gameObject);
            gameObject.SetActive(false);
            
            EventCenter.Instance.Raise(new ObstacleReleaseEvent(gameObject));
        }

        private void Update()
        {
            if (gameObject.active)
            {
                var transform1 = transform;
                
                tGO.transform.Rotate(new Vector3(rX,rY,rZ));

                var position = transform1.position;
                transform1.position = new Vector3(position.x,position.y,position.z+speed);

                if (transform.position.z < -6)
                {
                    Reset();
                }
            }
        }
    }

    public class ObstacleReleaseEvent : BaseEvent
    {
        public GameObject GO;

        public ObstacleReleaseEvent(GameObject go)
        {
            GO = go;
        }
    }
}