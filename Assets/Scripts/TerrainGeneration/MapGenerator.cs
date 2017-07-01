using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap, ColorMap, Mesh
    }

    public DrawMode SelectedDrawMode;
    public int MapWidth;
    public int MapHeight;
    public float NoiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 Offset;

    public TerrainType[] Regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(MapWidth, MapHeight, seed, NoiseScale, octaves, persistance, lacunarity, Offset);

        Color [] colorMap = new Color[MapWidth*MapHeight];
        for (int y = 0; y < MapHeight; y++)
        {
            for (int x = 0; x < MapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < Regions.Length; i++)
                {
                    if (currentHeight <= Regions[i].height)
                    {
                        float maxRegion = Regions[i].height;

                        float minRegion = Regions[i].height;
                        if(i!=0)
                            minRegion = Regions[i - 1].height;

                        colorMap[y * MapWidth + x] = Color.Lerp(i!=0 ? Regions[i-1].color : Regions[i].color, Regions[i].color, (currentHeight - minRegion) / (maxRegion - minRegion));
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();

        if(SelectedDrawMode == DrawMode.NoiseMap)
             display.DrawTextureMap(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (SelectedDrawMode == DrawMode.ColorMap)
            display.DrawTextureMap(TextureGenerator.TextureFromColorMap(colorMap, MapWidth, MapHeight));
        else if (SelectedDrawMode == DrawMode.Mesh)
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.TextureFromColorMap(colorMap, MapWidth, MapHeight));
    }

    void OnValidate()
    {
        if (MapWidth < 1)
            MapWidth = 1;
        if (MapHeight < 1)
            MapHeight = 1;

        if (lacunarity < 1)
            lacunarity = 1;
        if (octaves < 0)
            octaves = 0;
    }

    [System.Serializable]
    public struct TerrainType
    {
        public string name;
        public float height;
        public Color color;
    }
}
