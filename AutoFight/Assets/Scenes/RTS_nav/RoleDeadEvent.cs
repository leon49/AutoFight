using UnityEngine;

namespace Demo
{
    public class RoleDeadEvent:BaseEvent
    {
        public GameObject GameObject;

        public RoleDeadEvent(GameObject gameObject)
        {
            GameObject = gameObject;
        }
    }
}