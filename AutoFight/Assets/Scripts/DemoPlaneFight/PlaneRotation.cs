using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace DemoPlaneFight
{
    public class PlaneRotation : MonoBehaviour
    {

        private Vector3 desPos;
        private float dura = 0.4f;
        private float duraSpeed = 0.2f;

        private float maxX = 1;
        private float maxY = 2;

        private float rotationFic = 2f;
        
        private uint hp = 100;
        public TextMeshProUGUI textHp;

        public Vector3 posStart;
        private void Awake()
        {
            posStart = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            hp -= 10;
        }
        
        public void Translate( Vector3 vector3 )
        {
            posStart = posStart + vector3;
            if (posStart.x < -1.7)
            {
                posStart.x = -1.7f;
            }
            if (posStart.x > 1.7)
            {
                posStart.x = 1.7f;
            }
            
            if (posStart.y > 0)
            {
                posStart.y = 0f;
            }
            
            if (posStart.y < -6)
            {
                posStart.y = -6f;
            }

            
            SetDesination( posStart );
        }
        
        public void SetDesination(Vector3 desinationPos)
        {
            desPos = desinationPos;
            transform.DOMove(desinationPos, duraSpeed);
            
            var position = transform.position;
            float rx = position.x - desPos.x;

            float desz = 0;
            if (rx < 0)
            {
                desz = -50*rotationFic;
            }
            if (rx > 0)
            {
                desz = 50*rotationFic;
            }

            desz = desz * Math.Abs(rx) / maxX;
            
            float ry = position.y - desPos.y;
            float desx = 0;
            if (ry>0)
            {
                desx = -70*rotationFic;
            }

            if (ry<0)
            {
                desx = 30*rotationFic;
            }
            desx *= Math.Abs(ry) / maxY;
            
            transform.DORotate(new Vector3(desx, 0,  desz), dura/2).OnComplete(() =>
            {
                transform.DORotate(new Vector3(0, 0,  0), dura+0.4f).SetEase(Ease.OutBack) ;
            }).SetEase(Ease.InBack) ;
        }

        private void Update()
        {
            textHp.text = hp.ToString();
        }

    }
}