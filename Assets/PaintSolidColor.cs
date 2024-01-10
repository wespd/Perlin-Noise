using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PaintSolidColor : MonoBehaviour
{
    public Terrain terrain;

    public VoronoiNoise vNoise;

    public float blendDistance;
    
    public TerrainData terrainData => terrain.terrainData;

    // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
    public float[, ,] splatmapData = null;


    public List<Vector3> sortedList = new();

    public int[,] biomeValues;
    public void SetColors () {      
        // Get a reference to the terrain dat
        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
        biomeValues = new int[terrainData.alphamapWidth, terrainData.alphamapHeight];
        Debug.Log("set colors ran");
        for (int x = 0; x < terrainData.alphamapHeight; x++)
        {
            for (int y = 0; y < terrainData.alphamapWidth; y++)
            {
                float[] splatWeights = new float[terrainData.alphamapLayers];
                float distance = 999;
                float res = terrainData.heightmapResolution;
                Vector3 size = terrainData.size;
                sortedList = vNoise.currentPoints.OrderBy(v => Vector3.Distance(v, new Vector3(x,y, v.z))).ToList();
               
                int colorOfPoint = (int)sortedList[0].z;
                int colorOfSecondPoint = (int)sortedList[1].z;
                int[] distancesRelativeToClosestPoint;
                int biomeValue = (int)sortedList[0].z;
                biomeValues[x,y] = biomeValue;
                /*distancesRelativeToClosestPoint.Add(1);
                for(int i = 1; i < sortedList.Count; i++)
                {
                    if((Vector3.Distance(new Vector3(sortedList[i].x, sortedList[i].y, 0), new Vector3(x, y, 0)) - Vector3.Distance(new Vector3(sortedList[0].x, sortedList[0].y, 0), new Vector3(x, y, 0))) < blendDistance)
                    {
                        distancesRelativeToClosestPoint.Add(i);
                    }
                }*/
                for(int i = 0; i < terrainData.alphamapLayers; i ++)
                {
                    splatmapData[x,y,i] = 0;
                }
                float distanceBetweenClosestPoints = Vector3.Distance(new Vector3(sortedList[1].x, sortedList[1].y, 0), new Vector3(x, y, 0)) - Vector3.Distance(new Vector3(sortedList[0].x, sortedList[0].y, 0), new Vector3(x, y, 0));
                if(sortedList[0].z != sortedList[1].z && distanceBetweenClosestPoints < blendDistance)
                {
                    float blendStrength = distanceBetweenClosestPoints/(blendDistance*2) + .5f;
                    splatmapData[x,y,colorOfPoint] = blendStrength;
                    splatmapData[x,y,colorOfSecondPoint] = 1 - blendStrength;

                }
                else
                {
                    splatmapData[x,y,colorOfPoint] = 1f;
                }
            }
        }
        
        // Finally assign the new splatmap to the terrainData:
        terrainData.SetAlphamaps(0, 0, splatmapData);
    }
}
