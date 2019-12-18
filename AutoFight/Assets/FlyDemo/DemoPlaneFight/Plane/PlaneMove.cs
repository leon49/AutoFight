using System;
using UnityEngine;

namespace DemoPlaneFight
{
    public class PlaneMove:MonoBehaviour
    {
        public LineRenderer lineRenderer;

        
        private Vector2 _lastMouse = new Vector2();
        private PlaneRotation _planeRotation;
        
        private void Awake()
        {
            _planeRotation = GetComponent<PlaneRotation>();
        }
        
        private bool mouseDown = false;
        private float transX;
        private float transY;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                mouseDown = true;
                _lastMouse = getMousePos();
            }

            if (Input.GetMouseButtonUp(0))
            {
                mouseDown = false;
                _lastMouse = new Vector2(0, 0);
                transX = 0;
                transY = 0;
            }

            Vector2 curMosPos = getMousePos();

            transX = curMosPos.x - _lastMouse.x;
            transY = curMosPos.y - _lastMouse.y;
            
            if (!mouseDown)
            {
                return;
            }

            _lastMouse = curMosPos;

            bool isMouseMoving = Math.Abs(transX) > 0.000001f || Math.Abs(transY) > 0.000001f;
            if (isMouseMoving)
            {
                _planeRotation.Translate(new Vector3(transX, transY, 0));
            }
        }

        private Vector2 getMousePos()
        {
            Vector3 mousepos = Input.mousePosition;
            mousepos.z = 10;
            if (Input.touchCount > 0)
            {
                mousepos = Input.touches[0].position;
                mousepos.z = 10;
            }

            if (Camera.main != null) mousepos = Camera.main.ScreenToWorldPoint(mousepos);
            return mousepos;
        }
    }
}