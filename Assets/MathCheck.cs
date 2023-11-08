using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathCheck : MonoBehaviour
{
    [Range(0, 10)]
    public float value1;
    [Range(0,10)]
    public float value2;
    public float result;
    public float result2;
    private void OnValidate()
    {
        result = value1 % value2;
        result2 = value1 - result;
    }
}
