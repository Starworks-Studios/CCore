using UnityEngine;
using System;

public static class MathUtils
{
    public static float SigFigs(double v, int digits)
    {
        if (v == 0f)
            return 0f;

        double scale = Math.Pow(10, Math.Floor(Math.Log10(Abs(v))) + 1);

        return (float)(scale * Math.Round(v / scale, digits));

    }
    static double Abs(double d)
    {
        return d < 0 ? -d : d;
    }
    /// <summary>
    /// Evaluates the animation curve as if it begins at time 0 and ends at time 1
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="normalizedT">Value [0-1]</param>
    /// <returns></returns>
    public static float EvaluateByNormalizedTime(this AnimationCurve curve, float normalizedT)
    {
        Vector2 curveTimeRange = new(curve.keys[0].time, curve.keys[curve.keys.Length - 1].time);
        return curve.Evaluate(curveTimeRange.Lerp(normalizedT));
    }
    /// <summary>
    /// Returns the value of a random point along the curve
    /// </summary>
    public static float EvaluateRandomized(this AnimationCurve curve)
    {
        return curve.EvaluateByNormalizedTime(UnityEngine.Random.Range(0f, 1f));
    }

    public static int PickDiscreteFromDistribution(this AnimationCurve curve, Vector2Int outputRange)
    {
        return curve.PickDiscreteFromDistribution(outputRange, outputRange);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="curve"></param>
    /// <param name="outputRange">The possible integers that may be returned within the curveRange</param>
    /// <param name="curveRange">The integers corresponding to the start and end of the curve</param>
    /// <returns></returns>
    public static int PickDiscreteFromDistribution(this AnimationCurve curve, Vector2Int outputRange, Vector2Int curveRange)
    {
        int bucketCount = outputRange.y - outputRange.x + 1;
        if (bucketCount == 1) return outputRange.x;
        var buckets = new float[bucketCount];
        float minCurveTime = 0f;
        float maxCurveTime = curve.keys[curve.keys.Length - 1].time;
        for (int i = 0; i < buckets.Length; ++i)
        {
            float idk = curveRange.InverseLerp(i + outputRange.x);
            buckets[i] = curve.Evaluate(Mathf.Lerp(minCurveTime, maxCurveTime, idk));
        }
        buckets.Normalize();
        float roll = UnityEngine.Random.Range(0f, 1f);
        float prob = 0f;
        for (int i = 0; i < buckets.Length; ++i)
        {
            prob += buckets[i];
            if (prob >= roll) return i + outputRange.x;
        }
        Debug.LogError("Failed to pick an int from the distribution");
        return outputRange.x;
    }
}
