using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "biome")]
public class biome : ScriptableObject
{
    [Range(0f,1f)]
    public float amplitude;
    [Range(0.001f, 10f)]
    public float frequency = 0.001f;
    [Range(1,8)]
    public int octaves = 1;
    [Range(0,6.28f)]
    public float xPhase;
    [Range(0,6.28f)]
    public float yPhase;
    [Range(0,0.99f)]
    public float persistance;
    [Range(1.001f,10)]
    public float lacunarity = 1.001f;
    [Range(0,0.1f)]
    public float terrace;
    [Range(0.001f, 0.9f)]
    public float cube = 0.001f;
}
