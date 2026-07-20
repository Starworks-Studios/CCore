using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector2 V3toV2(this Vector3 v3)
    {
        return new Vector2(v3.x, v3.y);
    }
    public static Vector3 HorizontalV2toV3(this Vector2 v2)
    {
        return new Vector3(v2.x, 0f, v2.y);
    }
    public static Vector3 Flatten(this Vector3 v3)
    {
        return new Vector3(v3.x, 0f, v3.z);
    }
    public static Vector3 FlattenZ(this Vector3 v3)
    {
        return new Vector3(v3.x, v3.y, 0f);
    }
    public static float RandomBetween(this Vector2 v2)
    {
        return Random.Range(v2.x, v2.y);
    }
    public static int RandomBetweenInclusive(this Vector2Int v2)
    {
        return Random.Range(v2.x, v2.y+1);
    }
    public static float Lerp(this Vector2 v2, float t)
    {
        return Mathf.Lerp(v2.x, v2.y, t);
    }
    public static float Lerp(this Vector2Int v2, float t)
    {
        return Mathf.Lerp(v2.x, v2.y, t);
    }
    public static float InverseLerp(this Vector2 v2, float value)
    {
        return Mathf.InverseLerp(v2.x, v2.y, value);
    }
    public static float InverseLerp(this Vector2Int v2, float value)
    {
        return Mathf.InverseLerp(v2.x, v2.y, value);
    }
    public static Vector2 WindowSpaceToNormalized(this Vector2 v2)
    {
        return new Vector2(v2.x / Screen.width, v2.y / Screen.height);
    }
    public static float Clamp(this Vector2 v2, float value)
    {
        return Mathf.Clamp(value, v2.x, v2.y);
    }
    public static bool IsWithin(this Vector2 v2, float value)
    {
        return v2.x <= value && value <= v2.y;
    }
    /// <summary>
    /// Returns a copy of this vector that is at least as large as the passed vector in that direction
    /// </summary>
    public static Vector3 SetMinimumMagnitudeTowards(this Vector3 velocity, Vector3 minimum)
    {
        var alignedVelocity = Vector3.Project(velocity, minimum);

        var changeToReachMinimum = minimum - alignedVelocity;
        if (Vector3.Dot(changeToReachMinimum, minimum) < 0f) return velocity; // already over the minimum

        return velocity + changeToReachMinimum;
    }
}
