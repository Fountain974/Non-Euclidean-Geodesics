using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalTriangle : MonoBehaviour
{
    //Set vertices in spherical coordinates
    public float rho = 1.0f; // radius/scaling factor
    public float ntheta1 = 0.0f; // azimuthal angle/pi
    public float nphi1 = 0.0f; // angle of elevation/pi
    public float ntheta2 = 0.0f;
    public float nphi2 = 0.5f;
    public float ntheta3 = 0.5f;
    public float nphi3 = 0.5f;
    public int lengthPerLine = 1000;
    public float widthMultiplier = .05f;

    void Start()
    {
        LineRenderer triangle = gameObject.AddComponent<LineRenderer>();
        triangle.widthMultiplier = widthMultiplier;
        triangle.positionCount = 3*lengthPerLine;
    }

    void Update()
    {
        LineRenderer triangle = gameObject.GetComponent<LineRenderer>();

        Vector3 v1 = new Vector3(
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Cos(ntheta1 * Mathf.PI),
            Mathf.Cos(nphi1 * Mathf.PI),
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Sin(ntheta1 * Mathf.PI));
        Vector3 v2 = new Vector3(
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Cos(ntheta2 * Mathf.PI),
            Mathf.Cos(nphi2 * Mathf.PI),
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Sin(ntheta2 * Mathf.PI));
        Vector3 v3 = new Vector3(
            Mathf.Sin(nphi3 * Mathf.PI) * Mathf.Cos(ntheta3 * Mathf.PI),
            Mathf.Cos(nphi3 * Mathf.PI),
            Mathf.Sin(nphi3 * Mathf.PI) * Mathf.Sin(ntheta3 * Mathf.PI));

        Vector3 w1 = (v2 - Vector3.Dot(v1, v2) * v1).normalized;
        Vector3 w2 = (v3 - Vector3.Dot(v2, v3) * v2).normalized;
        Vector3 w3 = (v1 - Vector3.Dot(v3, v1) * v3).normalized;

        float angle1 = Mathf.Acos(Vector3.Dot(v1, v2));
        float angle2 = Mathf.Acos(Vector3.Dot(v2, v3));
        float angle3 = Mathf.Acos(Vector3.Dot(v3, v1));

        var points = new Vector3[3*lengthPerLine];
        for (int i = 0; i < lengthPerLine; i++)
        {
            float angle1_i = i * angle1 / (lengthPerLine - 1);
            points[i] = rho * (Mathf.Sin(angle1_i) * w1 + Mathf.Cos(angle1_i) * v1);
        }
        for (int i = 0; i < lengthPerLine; i++)
        {
            float angle2_i = i * angle2 / (lengthPerLine - 1);
            points[i + 1000] = rho * (Mathf.Sin(angle2_i) * w2 + Mathf.Cos(angle2_i) * v2);
        }
        for (int i = 0; i < lengthPerLine; i++)
        {
            float angle3_i = i * angle3 / (lengthPerLine - 1);
            points[i + 2000] = rho * (Mathf.Sin(angle3_i) * w3 + Mathf.Cos(angle3_i) * v3);
        }

        triangle.SetPositions(points);
    }
}