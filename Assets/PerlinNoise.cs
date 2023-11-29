using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    private void OnValidate()
    {
        int bounds = terrain.terrainData.heightmapResolution;
        perlinHeights = new float[bounds,bounds];
        for(int x = 0; x < bounds; x++)
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
        }
        terrain.terrainData.SetHeights(0,0, perlinHeights);
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
    }
    
}
