using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalTriangle : MonoBehaviour
{
    // set endpoints in spherical coordinates
    public float rho = 1.0f; // radius/scaling factor
    public float theta1 = 0.0f; // azimuthal angle
    public float phi1 = 0.0f; // angle of elevation
    public float theta2 = 0.0f;
    public float phi2 = Mathf.PI / 2;
    public float theta3 = Mathf.PI / 2;
    public float phi3 = Mathf.PI / 2;
    public int lengthOfLineRenderer = 1000;

    void Start()
    {
        LineRenderer triangle = gameObject.AddComponent<LineRenderer>();
        triangle.widthMultiplier = 0.1f;
        triangle.positionCount = lengthOfLineRenderer * 3;
    }

    void Update()
    {
        LineRenderer triangle = gameObject.GetComponent<LineRenderer>();

        Vector3 v1 = new Vector3(
            Mathf.Sin(phi1) * Mathf.Cos(theta1),
            Mathf.Cos(phi1),
            Mathf.Sin(phi1) * Mathf.Sin(theta1));
        Vector3 v2 = new Vector3(
            Mathf.Sin(phi2) * Mathf.Cos(theta2),
            Mathf.Cos(phi2),
            Mathf.Sin(phi2) * Mathf.Sin(theta2));
        Vector3 v3 = new Vector3(
            Mathf.Sin(phi3) * Mathf.Cos(theta3),
            Mathf.Cos(phi3),
            Mathf.Sin(phi3) * Mathf.Sin(theta3));

        Vector3 w1 = Vector3.Cross(Vector3.Cross(v1, v2), v1).normalized;
        Vector3 w2 = Vector3.Cross(Vector3.Cross(v2, v3), v2).normalized;
        Vector3 w3 = Vector3.Cross(Vector3.Cross(v3, v1), v3).normalized;

        float angleBetween1 = Mathf.Acos(Vector3.Dot(v1, v2));
        float angleBetween2 = Mathf.Acos(Vector3.Dot(v2, v3));
        float angleBetween3 = Mathf.Acos(Vector3.Dot(v3, v1));

        var points = new Vector3[lengthOfLineRenderer * 3];
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            float angle_i = i * angleBetween1 / (lengthOfLineRenderer - 1);
            points[i] = rho * (Mathf.Sin(angle_i) * w1 + Mathf.Cos(angle_i) * v1);
        }
        for (int i = 1; i < lengthOfLineRenderer; i++)
        {
            float angle_i = i * angleBetween2 / (lengthOfLineRenderer - 1);
            points[i + 1000] = rho * (Mathf.Sin(angle_i) * w2 + Mathf.Cos(angle_i) * v2);
        }
        for (int i = 1; i < lengthOfLineRenderer; i++)
        {
            float angle_i = i * angleBetween3 / (lengthOfLineRenderer - 1);
            points[i + 2000] = rho * (Mathf.Sin(angle_i) * w3 + Mathf.Cos(angle_i) * v3);
        }
        triangle.SetPositions(points);
    }


}