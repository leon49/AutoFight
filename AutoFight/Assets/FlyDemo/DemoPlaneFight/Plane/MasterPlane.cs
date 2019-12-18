using System;
using DemoPlaneFight.Bullet;
using DG.Tweening;
using FlyDemo.DemoPlaneFight.Plane;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DemoPlaneFight.Plane
{
    public class MasterPlane:BasePlane
    {
        public GameObject body;
        public Slider SliderHP;

        public static GameObject PlayerPlane;

        private void Awake()
        {
            PlayerPlane = gameObject;
        }

        private void Start()
        {
            Hp = MaxHP;
        }


        public TextMeshProUGUI textHp;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Contains("Enemy"))
            {
                Hp -= 10;
                body.transform.DOShakePosition(0.3f, 0.3f);
                body.transform.DOShakeRotation(0.5f,0.3f);
            }
        }
        
        private void Update()
        {
        }

        public override void UpdateHP()
        {
            textHp.text = Hp.ToString();
            SliderHP.value = (MaxHP - Hp) / MaxHP;
        }
    }
}