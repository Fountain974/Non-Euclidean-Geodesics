using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphericalGeodesic : MonoBehaviour
{
    // set endpoints in spherical coordinates
    public float rho = 1.0f; // radius/scaling factor
    public float ntheta1 = 0.0f; // azimuthal angle/pi
    public float nphi1 = 0.0f; // angle of elevation/pi
    public float ntheta2 = 0.0f;
    public float nphi2 = 0.5f;

    public int lengthOfLineRenderer = 1000;
    public float widthMultiplier = .05f;

    void Start()
    {
        LineRenderer geodesic = gameObject.AddComponent<LineRenderer>();
        geodesic.widthMultiplier = widthMultiplier;
        geodesic.positionCount = lengthOfLineRenderer;
    }

    void Update()
    {
        LineRenderer geodesic = gameObject.GetComponent<LineRenderer>();

        Vector3 v1 = new Vector3(
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Cos(ntheta1 * Mathf.PI),
            Mathf.Cos(nphi1 * Mathf.PI),
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Sin(ntheta1 * Mathf.PI));
        Vector3 v2 = new Vector3(
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Cos(ntheta2 * Mathf.PI),
            Mathf.Cos(nphi2 * Mathf.PI),
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Sin(ntheta2 * Mathf.PI));

        Vector3 w = (v2 - Vector3.Dot(v1, v2) * v1).normalized;

        float angleBetween = Mathf.Acos(Vector3.Dot(v1, v2));

        var points = new Vector3[lengthOfLineRenderer];
        for (int i = 0; i < lengthOfLineRenderer; i++)
        {
            float angle_i = i * angleBetween / (lengthOfLineRenderer - 1);
            points[i] = rho * (Mathf.Sin(angle_i) * w + Mathf.Cos(angle_i) * v1);
        }
        geodesic.SetPositions(points);
    }
}