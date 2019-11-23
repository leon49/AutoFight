using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DemoPlaneFight
{
    public class ObstacleCube:MonoBehaviour
    {
        private float speed = -0.5f;

        private void Start()
        {
            speed = Random.Range(-1f, -2.5f);
        }

        private void Update()
        {
            var position = transform.position;
            transform.position = new Vector3(position.x,position.y,position.z+speed);
        }
    }
}