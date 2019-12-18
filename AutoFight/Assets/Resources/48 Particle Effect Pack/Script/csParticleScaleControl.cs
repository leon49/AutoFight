using System;
using UnityEngine;

namespace Resources._48_Particle_Effect_Pack.Script
{
    [ExecuteInEditMode]
    public class csParticleScaleControl:MonoBehaviour
    {
        private void Start()
        {
            SetScale();
        }
        
        public void SetScale()
        {
            ParticleSystem[] sys = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < sys.Length; i++)
            {
                sys[i].scalingMode = ParticleSystemScalingMode.Hierarchy;
            }
        }
    }
}