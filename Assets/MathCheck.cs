using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MathCheck : MonoBehaviour
{
    public List<Vector2> points;
    public float[] percents;
    public float[] heights;

    public Vector2 addedHeights;

    public float result;
    //trying to figure out percentages if we want the total to add up to 1 and we use the distances of the generated points from 0,0,0
    private void OnValidate()
    {
        addedHeights = Vector2.zero;
        /*heights.OrderBy(v => Vector2.Distance(v, new Vector2(0,0))).ToList();
        foreach(Vector2 height in heights)
        {
            addedHeights += height;
        }*/
        float temp = 0;
        for(int i = 0; i < percents.Length; i++)
        {
            temp += heights[i] * percents[i];
        }
        result = temp;
    }
}
