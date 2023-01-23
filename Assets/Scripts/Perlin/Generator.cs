using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public bool autoUpdate;

	public void GenerateMap2() {
		float[,] noiseMap2 = Perl.GeneratePerlinNoiseMap (mapWidth, mapHeight, noiseScale);


		Display display2 = FindObjectOfType<Display> ();
		display2.DrawPerlinNoiseMap (noiseMap2);
	}
}
