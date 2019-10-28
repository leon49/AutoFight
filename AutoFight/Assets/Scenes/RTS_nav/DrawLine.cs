using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    // Start is called before the first frame update
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;

    public List<Vector3> mouseHis = new List<Vector3>();
    private LineRenderer _lineRenderer;
    private Vector3 _initialPosition;
    private Vector3 _currentPosition;
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        _lineRenderer.colorGradient = gradient;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _initialPosition = GetCurrentMousePosition().GetValueOrDefault();
            _lineRenderer.SetPosition(0, _initialPosition);
            _lineRenderer.SetVertexCount(1);
            _lineRenderer.enabled = true;
        } 
        else if (Input.GetMouseButton(0))
        {
            _currentPosition = GetCurrentMousePosition().GetValueOrDefault();
            _lineRenderer.SetVertexCount(2);
            _lineRenderer.SetPosition(1, _currentPosition);
 
        } 
        else if (Input.GetMouseButtonUp(0))
        {
            _lineRenderer.enabled = false;
            var releasePosition = GetCurrentMousePosition().GetValueOrDefault();
            var direction = releasePosition - _initialPosition;
            Debug.Log("Process direction " + direction);
        }
    }
    
    private Vector3? GetCurrentMousePosition()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var plane = new Plane(Vector3.forward, Vector3.zero);
 
        float rayDistance;
        if (plane.Raycast(ray, out rayDistance))
        {
            return ray.GetPoint(rayDistance);
             
        }
 
        return null;
    }
}
