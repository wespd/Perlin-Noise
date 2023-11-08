using UnityEngine;

public class PaintSolidColor : MonoBehaviour
{
    public Terrain terrain;
    public Color solidColor = Color.white; // The color to paint the terrain

    void Start()
    {
        if (terrain != null)
        {
            TerrainData terrainData = terrain.terrainData;

            float[,,] splatmapData = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
            Debug.Log(terrainData.alphamapWidth);
            Debug.Log(terrainData.alphamapHeight);
            Debug.Log(terrainData.alphamapLayers);
            // Set the entire splatmap to use the solid color
            for (int i = 0; i < terrainData.alphamapLayers; i++)
            {

                for (int x = 0; x < terrainData.alphamapWidth; x++)
                {
                    for (int y = 0; y < terrainData.alphamapHeight; y++)
                    {
                        splatmapData[x, y, i] = 0f;
                    }
                }
            }

            // Set the solid color
            splatmapData[0, 0, 0] = solidColor.r; // Red channel
            splatmapData[0, 0, 1] = solidColor.g; // Green channel
            splatmapData[0, 0, 2] = solidColor.b; // Blue channel
            splatmapData[0, 0, 3] = solidColor.a; // Alpha channel

            terrainData.SetAlphamaps(0, 0, splatmapData);

            // Update the terrain
            terrain.Flush();
        }
        else
        {
            Debug.LogError("No terrain assigned!");
        }
    }
}
