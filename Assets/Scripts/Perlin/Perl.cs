using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Perl
{
    public static float[,] GeneratePerlinNoiseMap(int mapWidth, int mapHeight, float scale) {
		float[,] noiseMap2 = new float[mapWidth,mapHeight];

		if (scale <= 0) {
			scale = 0.0001f;
		}

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				float sampleX = x / scale;
				float sampleY = y / scale;

				float perlinValue = Mathf.PerlinNoise (sampleX, sampleY);
				noiseMap2 [x, y] = perlinValue;
			}
		}

		return noiseMap2;
	}
}
