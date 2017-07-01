using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parcel {

    public Vector2 Center, Size;

    public Vector2 TopLeft
    {
        get { return Center - (Size * 0.5f); }
    }

	public Parcel(Vector2 center, Vector2 size)
    {
        Center = center;
        Size = size;
    }

    /// <summary>
    /// Splits the Parcel up and returns the newly created one
    /// </summary>
    /// <returns></returns>
    public Parcel Split(int dimension, float percentage)
    {
        Parcel result;
        if(dimension == 0)
        {
            Vector2 resultSize = new Vector2(Size.x * (1 - percentage), Size.y);
            result = new Parcel(new Vector2(TopLeft.x + Size.x * percentage + resultSize.x * 0.5f, Center.y), resultSize);

            Vector2 topleft = TopLeft;
            Size = new Vector2(Size.x * percentage, Size.y);
            Center = topleft + Size * 0.5f;
        }
        else
        {
            Vector2 resultSize = new Vector2(Size.x, Size.y * (1 - percentage));
            result = new Parcel(new Vector2(Center.x, TopLeft.y + Size.y * percentage + resultSize.y * 0.5f), resultSize);

            Vector2 topleft = TopLeft;
            Size = new Vector2(Size.y, Size.y * percentage);
            Center = topleft + Size * 0.5f;
        }

        return result;
    }
}
