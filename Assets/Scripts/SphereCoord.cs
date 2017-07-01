using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCoord
{

    /*
    r ≥ 0
    0° ≤ zenith ≤ 180° (π rad)
    -180° ≤ azimuth ≤ 180° (π rad) 
    */

    public float r, zenith, azimuth;

    public Vector3 Vector3
    {
        get {
            return new Vector3(
                r * Mathf.Sin(zenith) * Mathf.Sin(azimuth),
                r * Mathf.Sin(zenith) * Mathf.Cos(azimuth),
                r * Mathf.Cos(zenith));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="x"> r ≥ 0 </param>
    /// <param name="zen"> 0° ≤ zenith ≤ 180° (π rad) </param>
    /// <param name="azim"> -180° ≤ azimuth ≤ 180° (π rad) </param>
    public SphereCoord(float r, float zen, float azim)
    {
        this.r = r;
        zenith = zen;
        azimuth = azim;
    }

    public static SphereCoord operator + (SphereCoord lhs, Vector2 dir)
    {
        float zen = lhs.zenith + dir.x * dir.magnitude;
        if (zen > 2*Mathf.PI || zen < 0) zen += Mathf.Sign(zen) * -2*Mathf.PI;
        float azim = lhs.azimuth + dir.y  * dir.magnitude;
        if (azim > 2*Mathf.PI || azim < 0) azim += Mathf.Sign(azim) * -(2*Mathf.PI);
        
        return new SphereCoord(lhs.r, zen, azim);
    }
}
