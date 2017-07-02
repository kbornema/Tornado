using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{

    public static Vector3 GetVectorPlaneProjection(Vector3 u, Vector3 n)
    {
        Vector3 result = u - (Vector3.Dot(u, n) / n.sqrMagnitude) * n;
        return result;
    }
}
