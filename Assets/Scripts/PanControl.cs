using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanControl : MonoBehaviour
{
    public GameObject pan;
    public Vector3 worldPosition;
    Plane plane = new Plane(Vector3.forward, -9.5f);

    // Update is called once per frame
    void Update()
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
            pan.transform.position = worldPosition;
        }

    }
}
