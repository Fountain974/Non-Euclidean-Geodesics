using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperbolicGeodesic : MonoBehaviour
{
    //Set endpoints in spherical coordinates
    public float rho1 = 1.0f; // radius/scaling factor, <=1
    public float ntheta1 = 0.0f; // azimuthal angle/pi
    public float nphi1 = 0.0f; // angle of elevation/pi

    public float rho2 = 1.0f;
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

        Vector3 p = new Vector3(
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Cos(ntheta1 * Mathf.PI),
            Mathf.Cos(nphi1 * Mathf.PI),
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Sin(ntheta1 * Mathf.PI)) * rho1;
        Vector3 q = new Vector3(
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Cos(ntheta2 * Mathf.PI),
            Mathf.Cos(nphi2 * Mathf.PI),
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Sin(ntheta2 * Mathf.PI)) * rho2;

        float angleBetween = Mathf.Acos(Vector3.Dot(p.normalized, q.normalized));
        var points = new Vector3[lengthOfLineRenderer];


        if (angleBetween == 0 | angleBetween == Mathf.PI)
        {
            for (int i = 0; i < lengthOfLineRenderer; i++)
            {
                float it = i / (lengthOfLineRenderer - 1);
                Vector3 point_i = (1 - it) * p + it * q;
                points[i] = point_i;
            }
        }
        else
        {
            Vector3 pqMid = (p + q) / 2; //midpoint of p and q
            Vector3 pOrth = (pqMid - Vector3.Project(pqMid, p)).normalized; // vector normal to p in plane
            float pMid = 0.5f * (1 + Mathf.Pow(p.magnitude, -2));//Midpoint of p and inversion of p
            float qMid = 0.5f * (1 + Mathf.Pow(q.magnitude, -2));
            float orthLength = Mathf.Abs(qMid - pMid * Mathf.Cos(angleBetween)) / Mathf.Sin(angleBetween);

            Vector3 center = pMid * p + orthLength * pOrth;
            float newAngle = Mathf.Acos(Vector3.Dot((center-p).normalized,(center-q).normalized)); // angle between cp and cq
            float rad = (center - p).magnitude;
            Vector3 cos = (center - p).normalized;
            Vector3 sin = ((center - q) - Vector3.Project(center - q, cos)).normalized;

            for (int i = 0; i < lengthOfLineRenderer; i++)
            {
                float angle_i = i * newAngle / (lengthOfLineRenderer - 1);
                //float angle_i = i * 2 * Mathf.PI / (lengthOfLineRenderer - 1);
                Vector3 point_i = center - rad * (Mathf.Cos(angle_i) * cos + Mathf.Sin(angle_i) * sin);
                points[i] = point_i;
            }
        }


        geodesic.SetPositions(points);
    }
}
