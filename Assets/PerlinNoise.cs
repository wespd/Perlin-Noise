 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class PerlinNoise : MonoBehaviour
{
    public biome values;
    public bool useBiome;
    [Range(0f,1f)]
    public float amplitude;
    [Range(0.001f, 10f)]
    public float frequency;
    [Range(1,8)]
    public int octaves;
    [Range(0,6.28f)]
    public float xPhase;
    [Range(0,6.28f)]
    public float yPhase;
    [Range(0.001f,0.99f)]
    public float persistance;
    [Range(1.001f,10)]
    public float lacunarity;

    float _amplitude {get 
    {
        if(useBiome)
        {
            return values.amplitude;
        }
        return amplitude;
    }}
    float _frequency{get 
    {
        if(useBiome)
        {
            return values.frequency;
        }
        return frequency;
    }}
    int _octaves{get 
    {
        if(useBiome)
        {
            return values.octaves;
        }
        return octaves;
    }}
    float _xPhase{get 
    {
        if(useBiome)
        {
            return values.xPhase;
        }
        return xPhase;
    }}
    float _yPhase{get 
    {
        if(useBiome)
        {
            return values.yPhase;
        }
        return yPhase;
    }}
    float _persistance{get 
    {
        if(useBiome)
        {
            return values.persistance;
        }
        return persistance;
    }}
    float _lacunarity{get 
    {
        if(useBiome)
        {
            return values.lacunarity;
        }
        return lacunarity;
    }}
    public biome[] biomes;
    
    public float[,] perlinHeights;
    public Terrain terrain;
    
    [Range(0.00001f, 0.2f)]
    public float terrace;
    
    float _terrace{get 
    {
        if(useBiome)
        {
            return values.terrace;
        }
        return terrace;
    }}
    [Range(0.001f, 0.9f)]
    public float cube;
    
    float _cube{get 
    {
        if(useBiome)
        {
            return values.cube;
        }
        return cube;
    }}
    
    public bool createAsset;
    public string biomeName;

    public PaintSolidColor painter;

    public bool changeColor;
    public VoronoiNoise vNoise;

    private void OnValidate()
    {
        int bounds = terrain.terrainData.heightmapResolution;
        perlinHeights = new float[bounds,bounds];
        if(createAsset)
        {
            createAsset = false;
            biome asset = ScriptableObject.CreateInstance<biome>();
            asset.amplitude = _amplitude;
            asset.frequency = _frequency;
            asset.octaves = _octaves;
            asset.persistance = _persistance;
            asset.lacunarity = _lacunarity;
            asset.terrace = _terrace;
            asset.cube = _cube;
            
            AssetDatabase.CreateAsset(asset, $"Assets/{name}.asset");
            AssetDatabase.SaveAssets();
        }
        if(changeColor)
        {
            changeColor = false;
            painter.SetColors();
        }
        Debug.Log("set hieghts");
        for(int x = 0; x < bounds; x++)
        {
            for(int y = 0; y < bounds; y++)
            {
                List<Vector3> sortedList = GetBiomeIndex(x,y);
                List<Vector2> closestPoints = new();
                for(int i = 0; i < sortedList.Count; i++)
                {
                    float distance = Vector2.Distance(new Vector2(sortedList[i].x, sortedList[i].y), new Vector2(x, y));
                    if(distance - Vector2.Distance(new Vector2(sortedList[0].x, sortedList[0].y), new Vector2(x, y)) < painter.blendDistance)
                    {
                        closestPoints.Add(new Vector2(distance, sortedList[i].z));
                    }
                }
                List<float> distances = new(); 
                foreach(Vector2 point in closestPoints)
                {
                    distances.Add(point.x);
                }
                float[] normalizedPercents = distanceToNormalizedPercents(distances.ToArray());
                float perlinValue = 0;
                for(int i = 0; i < normalizedPercents.Length; i++)
                {
                    perlinValue += perlinHeight(x,y,(int)closestPoints[i].y) * normalizedPercents[i];
                }
                perlinHeights[x,y] = perlinValue; 
                
            }
        }
        terrain.terrainData.SetHeights(0,0, perlinHeights);
    }
    public List<Vector3> GetBiomeIndex(int x, int y)
    {
        List<Vector3> sortedList = new();
        sortedList = vNoise.currentPoints.OrderBy(v => Vector3.Distance(v, new Vector3(x/1000*512,y/1000*512, v.z))).ToList();
        return sortedList;
    }
    public float perlinHeight(int x, int y,int index)
    {
        int bounds = terrain.terrainData.heightmapResolution;
        float perlinValue = 0;
        float __amplitude = biomes[index].amplitude;
        float __frequency = biomes[index].frequency;
        for(int octave = 0; octave < biomes[index].octaves; octave++)
        {
            float xCord = (x/(float)bounds* __frequency);
            float yCord = (y/(float)bounds*__frequency);
            __frequency *= biomes[index].lacunarity;
            perlinValue += Mathf.PerlinNoise(xCord - xCord % biomes[index].cube,yCord - yCord % biomes[index].cube)*__amplitude;
            __amplitude *= biomes[index].persistance;
        }
        return perlinValue - (perlinValue % biomes[index].terrace);
    }
    public float[] distanceToNormalizedPercents(float[] distances)
    {
        float[] percents = distances.Select(d => 1 / d).ToArray();

        float sum = percents.Sum();
        float[] normalizedPercentages = percents.Select(p => p / sum).ToArray();
        return normalizedPercentages;
    }
    
    
}
