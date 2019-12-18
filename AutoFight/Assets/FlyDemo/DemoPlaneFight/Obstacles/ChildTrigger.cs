using System;
using UnityEngine;

namespace DemoPlaneFight
{
    public class ChildTrigger:MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            GetComponentInParent<CommonObstacle>()?.OnTriggerEnter(other);
        }
    }
}