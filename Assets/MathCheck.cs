using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MathCheck : MonoBehaviour
{
    public Vector2[] points;
    public float[] distances;
    public float[] normalizedPercentages;
    private void OnValidate()
    {
        for(int i = 0; i < points.Length; i++)
        {
            distances[i] = points[i].magnitude;
        }
        float[] percents = distances.Select(d => 1 / d).ToArray();

        float sum = percents.Sum();
        normalizedPercentages = percents.Select(p => p / sum).ToArray();
    }
}
