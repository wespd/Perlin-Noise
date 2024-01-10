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
    public string name;

    public PaintSolidColor painter;

    public bool changeColor;
    public VoronoiNoise vNoise;

    private void OnValidate()
    {
        int bounds = terrain.terrainData.heightmapResolution;
        perlinHeights = new float[bounds,bounds];
        /*for(int x = 0; x < bounds; x++)
        {
            for(int y = 0; y < bounds; y++)
            {
                float perlinValue = 0;
                float __amplitude = _amplitude;
                float __frequency= _frequency;
                for(int octave = 0; octave < _octaves; octave++)
                {
                    float xCord = (x/(float)bounds*__frequency) + _xPhase;
                    float yCord = (y/(float)bounds*__frequency) + _yPhase;
                    __frequency *= _lacunarity;
                    perlinValue += Mathf.PerlinNoise(xCord - xCord % _cube,yCord - yCord % _cube)*__amplitude;
                    __amplitude *= _persistance;
                }
                
                perlinHeights[x,y] =  perlinValue - (perlinValue % _terrace);
            }
        }*/
        //terrain.terrainData.SetHeights(0,0, perlinHeights);
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
                float perlinValue = 0;
                int indexValue1 = (int)GetBiomeIndex(x,y).x;
                int indexValue2 = (int)GetBiomeIndex(x,y).y;
                float __amplitude;
                float __frequency;
                float __lacunarity;
                float __cube;
                float __persistance;
                float __terrace;
                int __octaves;
                float distanceBetweenClosestPoints = Vector3.Distance(new Vector3(indexValue1, indexValue2, 0), new Vector3(x, y, 0)) - Vector3.Distance(new Vector3(indexValue1, indexValue2, 0), new Vector3(x, y, 0));
                float percent = distanceBetweenClosestPoints/(painter.blendDistance*2) + .5f;
                if(indexValue1 != indexValue2 && distanceBetweenClosestPoints < painter.blendDistance)
                {
                    __amplitude = Mathf.Lerp(biomes[indexValue2].amplitude, biomes[indexValue1].amplitude, percent);
                    __frequency= Mathf.Lerp(biomes[indexValue2].frequency, biomes[indexValue1].frequency, percent);
                    __lacunarity = Mathf.Lerp(biomes[indexValue2].lacunarity, biomes[indexValue1].lacunarity, percent);
                    __cube = Mathf.Lerp(biomes[indexValue2].cube, biomes[indexValue1].cube, percent);
                    __persistance = Mathf.Lerp(biomes[indexValue2].persistance, biomes[indexValue1].persistance, percent);
                    __terrace = Mathf.Lerp(biomes[indexValue2].terrace, biomes[indexValue1].terrace, percent);
                    __octaves = (int)Mathf.Lerp(biomes[indexValue2].octaves, biomes[indexValue1].octaves, percent);
                }
                else
                {
                    __amplitude = biomes[indexValue1].amplitude;
                    __frequency= biomes[indexValue1].frequency;
                    __lacunarity =biomes[indexValue1].lacunarity;
                    __cube = biomes[indexValue1].cube;
                    __persistance =biomes[indexValue1].persistance;
                    __terrace = biomes[indexValue1].terrace;
                    __octaves = biomes[indexValue1].octaves;
                }
                
                for(int octave = 0; octave < __octaves; octave++)
                {
                    float xCord = (x/(float)bounds*__frequency) + _xPhase;
                    float yCord = (y/(float)bounds*__frequency) + _yPhase;
                    __frequency *= __lacunarity;
                    perlinValue += Mathf.PerlinNoise(xCord - xCord % __cube,yCord - yCord % __cube)*__amplitude;
                    __amplitude *= __persistance;
                }
                
                perlinHeights[x,y] =  perlinValue - (perlinValue % __terrace);
            }
        }
        terrain.terrainData.SetHeights(0,0, perlinHeights);
    }
    public Vector2 GetBiomeIndex(int x, int y)
    {

        // float highest = 0;
        // float returnValueIndex = 0;
        // int convertedX = x/1000*512;
        // int convertedY = y/1000*512;
        // /*if(convertedX >= list.GetLength(0) || convertedY >= list.GetLength(1))
        // {
        //     Debug.Log(convertedX + " " + convertedY);
        // }
        // for(int i = 0; i < biomes.Length; i++)
        // {
        //     if(list[convertedX,convertedY,i] > highest)
        //     {
        //         highest = list[convertedX,convertedY,i];
        //         returnValueIndex = i;
        //     }
        //     if(x == 0)
        //     {
        //         Debug.Log(list[convertedX,convertedY,i] + " " + i);
        //     }
        //     //returnValue += list[x,y,i]*i;
        // }*/
        // //we need to make a list that takes an x and y and stores the biome value in the painter script
        // return painter.biomeValues[convertedX, convertedY];
        List<Vector3> sortedList = new();
        sortedList = vNoise.currentPoints.OrderBy(v => Vector3.Distance(v, new Vector3(x,y, v.z))).ToList();
        return new Vector2(sortedList[0].z, sortedList[1].z);
    }
    
    
}
