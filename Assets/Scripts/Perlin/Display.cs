using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
  public Renderer textureRender2;

	public void DrawPerlinNoiseMap(float[,] noiseMap2) {
		int width = noiseMap2.GetLength (0);
		int height = noiseMap2.GetLength (1);

		Texture2D texture2 = new Texture2D (width, height);

		Color[] colourMap = new Color[width * height];
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				colourMap [y * width + x] = Color.Lerp (Color.black, Color.white, noiseMap2 [x, y]);
			}
		}
		texture2.SetPixels (colourMap);
		texture2.Apply ();

		textureRender2.sharedMaterial.mainTexture = texture2;
		textureRender2.transform.localScale = new Vector3 (width, 1, height);
	}
}

