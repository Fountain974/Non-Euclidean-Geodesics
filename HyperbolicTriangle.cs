using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperbolicTriangle : MonoBehaviour
{
    //Set vertices in spherical coordinates
    public float rho1 = 1.0f; // radius, <=1
    public float ntheta1 = 0.0f; // azimuthal angle/pi
    public float nphi1 = 0.0f; // angle of elevation/pi

    public float rho2 = 1.0f;
    public float ntheta2 = 0.0f;
    public float nphi2 = 0.5f;

    public float rho3 = 1.0f;
    public float ntheta3 = 0.5f;
    public float nphi3 = 0.5f;

    public int lengthPerLine = 1000;
    public float widthMultiplier = .05f;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 10;
    }

    void Start()
    {
        LineRenderer triangle = gameObject.AddComponent<LineRenderer>();
        triangle.widthMultiplier = widthMultiplier;
        triangle.positionCount = 3 * lengthPerLine;
    }

    void Update()
    {
        LineRenderer triangle = gameObject.GetComponent<LineRenderer>();

        Vector3 v1 = new Vector3(
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Cos(ntheta1 * Mathf.PI),
            Mathf.Cos(nphi1 * Mathf.PI),
            Mathf.Sin(nphi1 * Mathf.PI) * Mathf.Sin(ntheta1 * Mathf.PI)) * rho1;
        Vector3 v2 = new Vector3(
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Cos(ntheta2 * Mathf.PI),
            Mathf.Cos(nphi2 * Mathf.PI),
            Mathf.Sin(nphi2 * Mathf.PI) * Mathf.Sin(ntheta2 * Mathf.PI)) * rho2;
        Vector3 v3 = new Vector3(
            Mathf.Sin(nphi3 * Mathf.PI) * Mathf.Cos(ntheta3 * Mathf.PI),
            Mathf.Cos(nphi3 * Mathf.PI),
            Mathf.Sin(nphi3 * Mathf.PI) * Mathf.Sin(ntheta3 * Mathf.PI)) * rho3;

        float angle1 = Mathf.Acos(Vector3.Dot(v1.normalized, v2.normalized));
        float angle2 = Mathf.Acos(Vector3.Dot(v2.normalized, v3.normalized));
        float angle3 = Mathf.Acos(Vector3.Dot(v3.normalized, v1.normalized));

        var points = new Vector3[3 * lengthPerLine];

        if (angle1 == 0 | angle1 == Mathf.PI | rho1 == 0 | rho2 == 0)
        {
            for (int i = 0; i < lengthPerLine; i++)
            {
                float it = i / (lengthPerLine - 1);
                Vector3 point_i = (1 - it) * v1 + it * v2;
                points[i] = point_i;
            }
        }
        else
        {
            Vector3 orth1 = (v2 - Vector3.Project(v2, v1)).normalized; // vector normal to p in plane
            float mid1 = (1 + Mathf.Pow(rho1, 2)) / (rho1 * 2);//Midpoint of p and inversion of p
            float mid2 = (1 + Mathf.Pow(v2.magnitude, 2)) / (rho2 * 2);
            float orthLength1 = mid2 / Mathf.Sin(angle1) - mid1 / Mathf.Tan(angle1);

            Vector3 center = mid1 * v1.normalized + orthLength1 * orth1;
            float newAngle = Mathf.Acos(Vector3.Dot((center - v1).normalized, (center - v2).normalized)); // angle between cp and cq
            float rad = (center - v1).magnitude;
            Vector3 cos = (center - v1).normalized;
            Vector3 sin = ((center - v2) - Vector3.Project(center - v2, cos)).normalized;

            for (int i = 0; i < lengthPerLine; i++)
            {
                float angle_i = i * newAngle / (lengthPerLine - 1);
                //float angle_i = i * 2 * Mathf.PI / (lengthPerLine - 1);
                Vector3 point_i = center - rad * (Mathf.Cos(angle_i) * cos + Mathf.Sin(angle_i) * sin);
                points[i] = point_i;
            }
        }

        if (angle2 == 0 | angle2 == Mathf.PI | rho2 == 0 | rho3 == 0)
        {
            for (int i = 0; i < lengthPerLine; i++)
            {
                float it = i / (lengthPerLine - 1);
                Vector3 point_i = (1 - it) * v2 + it * v3;
                points[i + lengthPerLine] = point_i;
            }
        }
        else
        {
            Vector3 orth2 = (v3 - Vector3.Project(v3, v2)).normalized; // vector normal to p in plane
            float mid2 = (1 + Mathf.Pow(rho2, 2)) / (rho2 * 2);//Midpoint of p and inversion of p
            float mid3 = (1 + Mathf.Pow(rho3, 2)) / (rho3 * 2);
            float orthLength2 = mid3 / Mathf.Sin(angle2) - mid2 / Mathf.Tan(angle2);

            Vector3 center = mid2 * v2.normalized + orthLength2 * orth2;
            float newAngle = Mathf.Acos(Vector3.Dot((center - v2).normalized, (center - v3).normalized)); // angle between cp and cq
            float rad = (center - v2).magnitude;
            Vector3 cos = (center - v2).normalized;
            Vector3 sin = ((center - v3) - Vector3.Project(center - v3, cos)).normalized;

            for (int i = 0; i < lengthPerLine; i++)
            {
                float angle_i = i * newAngle / (lengthPerLine - 1);
                //float angle_i = i * 2 * Mathf.PI / (lengthPerLine - 1);
                Vector3 point_i = center - rad * (Mathf.Cos(angle_i) * cos + Mathf.Sin(angle_i) * sin);
                points[i + lengthPerLine] = point_i;
            }
        }

        if (angle3 == 0 | angle3 == Mathf.PI | rho3 == 0 | rho1 == 0)
        {
            for (int i = 0; i < lengthPerLine; i++)
            {
                float it = i / (lengthPerLine - 1);
                Vector3 point_i = (1 - it) * v3 + it * v1;
                points[i + 2 * lengthPerLine] = point_i;
            }
        }
        else
        {
            Vector3 orth3 = (v1 - Vector3.Project(v1, v3)).normalized; // vector normal to p in plane
            float mid3 = (1 + Mathf.Pow(rho3, 2)) / (2 * rho3);//Midpoint of p and inversion of p
            float mid1 = (1 + Mathf.Pow(rho1, 2)) / (2 * rho1);
            float orthLength3 = mid1 / Mathf.Sin(angle3) - mid3 / Mathf.Tan(angle3);

            Vector3 center = mid3 * v3.normalized + orthLength3 * orth3;
            float newAngle = Mathf.Acos(Vector3.Dot((center - v3).normalized, (center - v1).normalized)); // angle between cp and cq
            float rad = (center - v3).magnitude;
            Vector3 cos = (center - v3).normalized;
            Vector3 sin = ((center - v1) - Vector3.Project(center - v1, cos)).normalized;

            for (int i = 0; i < lengthPerLine; i++)
            {
                float angle_i = i * newAngle / (lengthPerLine - 1);
                //float angle_i = i * 2 * Mathf.PI / (lengthPerLine - 1);
                Vector3 point_i = center - rad * (Mathf.Cos(angle_i) * cos + Mathf.Sin(angle_i) * sin);
                points[i + 2 * lengthPerLine] = point_i;
            }
        }

        triangle.SetPositions(points);
    }
}
