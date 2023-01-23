//Quelle: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3

using UnityEngine;
using System.Collections;

public static class Noise {
	//"GenerateNoiseMap" that returns a 2D float array. The method takes in several parameters: 
	//"mapWidth" and "mapHeight" for the dimensions of the noise map, 
	//"seed" for the random seed, "scale" for the overall scale of the noise, 
	//"octaves" for the number of layers of noise, "persistance" and "lacunarity" for affecting the characteristics of the noise, 
	//"offset" for offsetting the noise
	public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset) {
		float[,] noiseMap = new float[mapWidth,mapHeight];					//Create 2D float array

		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];						//Create array with a length of "octaves" 
		for (int i = 0; i < octaves; i++) {
			//Generate a random float value between -100000 and 100000 using the "prng" random generator
			float offsetX = prng.Next (-100000, 100000) + offset.x;
			float offsetY = prng.Next (-100000, 100000) + offset.y;
			octaveOffsets [i] = new Vector2 (offsetX, offsetY);
		}
		//Check if "scale" is less than or equal to zero
		if (scale <= 0) {
			scale = 0.0001f;
		}
		//Minimum and Maximum value of float 
		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		float halfWidth = mapWidth / 2f;
		float halfHeight = mapHeight / 2f;

		//For-loop iterate y-coordinates of "noiseMap" array
		for (int y = 0; y < mapHeight; y++) {
			//For-loop iterate x-coordinates of "noiseMap" array
			for (int x = 0; x < mapWidth; x++) {
		
				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				//For-loop iterate the number of "octaves"
				for (int i = 0; i < octaves; i++) {
					//Calculate the x sample value using the current x-coordinate of the loop
					float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x * frequency;
					//Calculates the y sample value using the current y-coordinate of the loop
					float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y * frequency;

					//Use Mathf.PerlinNoise method to generate a value between 0 and 1, then scale it to a range of -1 to 1
					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					//Scale "perlinValue" by "amplitude"
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
				}
				if (noiseHeight > maxNoiseHeight) {				//Check if "noiseHeight" is greater than "maxNoiseHeight"
					maxNoiseHeight = noiseHeight;				//Update "maxNoiseHeight" with "noiseHeight"
				} else if (noiseHeight < minNoiseHeight) {		//Check if "noiseHeight" is less than "minNoiseHeight"
					minNoiseHeight = noiseHeight;				//Update "minNoiseHeight" with "noiseHeight"
				}
				noiseMap [x, y] = noiseHeight;
			}
		}
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				//Normalize the "noiseMap" values by using the Mathf.InverseLerp method
				noiseMap [x, y] = Mathf.InverseLerp (minNoiseHeight, maxNoiseHeight, noiseMap [x, y]);
			}
		}
		return noiseMap;
	}
}