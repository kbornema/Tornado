using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{

    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random random = new System.Random(seed);

        Vector2[] octaveOffset = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-10000, 100000) + offset.x;
            float offsetY = random.Next(-10000, 100000) + offset.y;

            octaveOffset[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
            scale = 0.0001f;

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;



        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                //float deltaX = (float)x/ (float)mapWidth;
                //float deltaY = float/ (float)mapHeight;


                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffset[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffset[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }

    public static float[,] GenerateBorder(int mapWidth, int mapHeight, float saveZoneRadius, float saveZoneValue, float hillZoneRadius, float maxBound)
    {
        float[,] border = new float[mapWidth, mapHeight];

        var center = new Vector2(mapWidth / 2, mapHeight / 2);

        for (int x = 0; x < mapHeight; x++)
        {
            for (int y = 0; y < mapWidth; y++)
            {
                var distance = Vector3.Distance(center, new Vector3(x, y));
                if (distance < saveZoneRadius)
                    border[x, y] = saveZoneValue;
                else
                {
                    var progress = (distance - saveZoneRadius) / (hillZoneRadius);

                    border[x, y] = (1 - progress) * saveZoneValue + (progress) * maxBound;
                }

            }
        }

        return border;
    }

    public static float[,] MergeNoiseBorder(float[,] noise, float[,] border)
    {
        float[,] result = new float[noise.GetLength(0), noise.GetLength(1)];

        for (int y = 0; y < noise.GetLength(1); y++)
        {
            for (int x = 0; x < noise.GetLength(0); x++)
            {
                result[x, y] = noise[x, y] + border[x, y];
            }
        }
        return result;


    }
}
