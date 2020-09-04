using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGeodesic : MonoBehaviour
{
    // set endpoints in spherical coordinates
    public float rho = 1.0f; // radius/scaling factor
    public float theta1 = 0.0f; // azimuthal angle
    public float phi1 = 0.0f; // angle of elevation
    public float theta2 = 0.0f;
    public float phi2 = Mathf.PI / 2;
    public int lengthOfLineRenderer = 1000;

    void Start()
    {
        LineRenderer geodesic = gameObject.AddComponent<LineRenderer>();
        geodesic.widthMultiplier = 0.1f;
        geodesic.positionCount = lengthOfLineRenderer * 3;
    }

    void Update()
    {
        LineRenderer geodesic = gameObject.GetComponent<LineRenderer>();

        Vector3 v1 = new Vector3(
            Mathf.Sin(phi1) * Mathf.Cos(theta1),
            Mathf.Cos(phi1),
            Mathf.Sin(phi1) * Mathf.Sin(theta1));
        Vector3 v2 = new Vector3(
            Mathf.Sin(phi2) * Mathf.Cos(theta2),
            Mathf.Cos(phi2),
            Mathf.Sin(phi2) * Mathf.Sin(theta2));

        Vector3 w1 = Vector3.Cross(Vector3.Cross(v1, v2), v1).normalized;

        float angleBetween1 = Mathf.Acos(Vector3.Dot(v1, v2));

        var points = new Vector3[lengthOfLineRenderer];
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            float angle_i = i * angleBetween1 / (lengthOfLineRenderer - 1);
            points[i] = rho * (Mathf.Sin(angle_i) * w1 + Mathf.Cos(angle_i) * v1);
        }
        geodesic.SetPositions(points);
    }


}