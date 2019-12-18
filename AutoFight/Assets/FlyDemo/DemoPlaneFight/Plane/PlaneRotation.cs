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

        private float widthLimit = 3f;
        private float heightLminit = -6f;
        
        public Vector3 posStart;
        private void Awake()
        {
            posStart = transform.position;
        }
        
        public void Translate( Vector3 vector3 )
        {
            posStart = posStart + vector3;
            if (posStart.x < -widthLimit)
            {
                posStart.x = -widthLimit;
            }
            if (posStart.x > widthLimit)
            {
                posStart.x = widthLimit;
            }
            
            if (posStart.y > 0)
            {
                posStart.y = 0f;
            }
            
            if (posStart.y < heightLminit)
            {
                posStart.y = heightLminit;
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



    }
}