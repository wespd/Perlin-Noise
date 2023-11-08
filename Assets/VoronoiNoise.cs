using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiNoise : MonoBehaviour
{
    [Range(50,1000)]
    public int numberOfPoints;
    public float minX = 0f;
    public float maxX = 10f;
    public float minY = 0f;
    public float maxY = 10f;

    public float sphereHeight = 500;

    List<Vector2> currentPoints = new();

    public bool randomizePoints;

    List<Vector2> GenerateRandomPoints()
    {
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            points.Add(new Vector2(x, y));
        }

        return points;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for(int i =  0; i < currentPoints.Count; i++)
        {
            Gizmos.DrawSphere(new Vector3(currentPoints[i].x, sphereHeight, currentPoints[i].y), 10f);
        }
    }
    void OnValidate()
    {
        currentPoints = GenerateRandomPoints();
        if(randomizePoints == true) 
        {
            randomizePoints = false;
        }
    }
}