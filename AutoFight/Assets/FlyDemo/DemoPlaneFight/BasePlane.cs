using UnityEngine;

namespace FlyDemo.DemoPlaneFight.Plane
{
    public abstract class BasePlane:MonoBehaviour
    {
        protected float _hp = 100;
        protected float MaxHP = 100;
        public float Hp
        {
            get => _hp;
            set
            {
                _hp = value;
                UpdateHP();
            }
        }

        public abstract void UpdateHP();
    }
}