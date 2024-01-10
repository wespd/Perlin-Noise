using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiNoise : MonoBehaviour
{
    [Range(5,50)]
    public int numberOfPoints;
    public float minX = 0f;
    public float maxX = 10f;
    public float minY = 0f;
    public float maxY = 10f;

    public float sphereHeight = 500;

    public List<Vector3> currentPoints = new();

    public bool randomizePoints;

    List<Vector3> GenerateRandomPoints()
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = Random.Range(minX, maxX);
            float y = Random.Range(minY, maxY);
            int z = Random.Range(0,4);
            points.Add(new Vector3(x, y, z));
        }

        return points;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for(int i =  0; i < currentPoints.Count; i++)
        {
            //(x / (res-1) * size.x,y / (res-1) * size.x
            Gizmos.DrawSphere(new Vector3(currentPoints[i].x/maxX*1000, sphereHeight, currentPoints[i].y/maxY*1000), 10f);
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
