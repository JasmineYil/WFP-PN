//Quelle: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3

using UnityEngine;
using UnityEngine.Profiling;
using System;
using System.Collections;
using System.Diagnostics;


public class MapGenerator : MonoBehaviour {

	public enum DrawMode {NoiseMap, ColourMap, Mesh};
	public DrawMode drawMode;

	const int mapChunkSize = 241;
	[Range(0,6)]
	public int levelOfDetail;
	public float noiseScale;

	public int octaves;
	[Range(0,1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public float meshHeightMultiplier;
	public AnimationCurve meshHeightCurve;

	public bool autoUpdate;

	public TerrainType[] regions;

	public void GenerateMap() {
		Stopwatch stopwatch = Stopwatch.StartNew();					//Variable "stopwatch": New Stopwatch object is created and started
		long memoryBefore = GC.GetTotalMemory(false);				//Variable "memoryBefore": Total memory usage in bytes before the terrain is created
		Profiler.BeginSample("Perlin Noise");						//Start of performance profiler sample

		//Generate 2D float array by calling the "GenerateNoiseMap" method
		float[,] noiseMap = Noise.GenerateNoiseMap (mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistance, lacunarity, offset);

		//Create 1D Color array called "colourMap"
		Color[] colourMap = new Color[mapChunkSize * mapChunkSize];
		//For-loop to iterate over each element of "noiseMap" array
		for (int y = 0; y < mapChunkSize; y++) {
			for (int x = 0; x < mapChunkSize; x++) {
				float currentHeight = noiseMap [x, y];
				//For-loop to iterate over each element of "regions" array
				for (int i = 0; i < regions.Length; i++) {
					if (currentHeight <= regions [i].height) {					//If currentHeight is less than or equal to the height of the current "regions"
						colourMap [y * mapChunkSize + x] = regions [i].colour;	//"colourMap" array is set to the colour of the current "regions"
						break;													//Then loop is broken
					}
				}
			}
		}
		//Check "drawMode", then call method on the "display" object to draw the map, either as Noise, Colour or Mesh
		MapDisplay display = FindObjectOfType<MapDisplay> ();
		if (drawMode == DrawMode.NoiseMap) {
			display.DrawTexture (TextureGenerator.TextureFromHeightMap (noiseMap));
		} else if (drawMode == DrawMode.ColourMap) {
			display.DrawTexture (TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		} else if (drawMode == DrawMode.Mesh) {
			display.DrawMesh (MeshGenerator.GenerateTerrainMesh (noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap (colourMap, mapChunkSize, mapChunkSize));
		}

		stopwatch.Stop();													//Stops stopwatch
		long memoryAfter = GC.GetTotalMemory(false);						//Variable "memoryAfter": Total memory usage in bytes after the terrain is created
        long memoryUsed = memoryAfter - memoryBefore;						//Calculating the memory: Memory usage before - memory usage after.
		Profiler.EndSample();												//Ends performance profiler sample

        UnityEngine.Debug.Log($"Memory used: {memoryUsed} bytes");
		UnityEngine.Debug.Log($"Time elapsed: {stopwatch.ElapsedMilliseconds} milliseconds");
	}
	void OnValidate() {
		if (lacunarity < 1) {
			lacunarity = 1;
		}
		if (octaves < 0) {
			octaves = 0;
		}
	}
}
[System.Serializable]
public struct TerrainType {
	public string name;
	public float height;
	public Color colour;
}