//Quelle: https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3

using UnityEngine;
using System.Collections;

public static class MeshGenerator {
	public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMultiplier, AnimationCurve heightCurve, int levelOfDetail) {
		int width = heightMap.GetLength (0);		//Define "Withd" and set to length of first dimensions of "heightMap" array
		int height = heightMap.GetLength (1);		//Define "Height" and set to length of second dimensions of "heightMap" array
		float topLeftX = (width - 1) / -2f;			//Set "topLeftX" to the negative half of difference between "width" and 1
		float topLeftZ = (height - 1) / 2f;			//Set "topLeftZ" to half of difference between "height" and 1

		int meshSimplificationIncrement = (levelOfDetail == 0)?1:levelOfDetail * 2;
		int verticesPerLine = (width - 1) / meshSimplificationIncrement + 1;

		MeshData meshData = new MeshData (verticesPerLine, verticesPerLine);
		int vertexIndex = 0;

		for (int y = 0; y < height; y += meshSimplificationIncrement) {
			for (int x = 0; x < width; x += meshSimplificationIncrement) {
				//"vertices" array of "meshData" is set at current "vertexIndex" --> new "Vector3"
				meshData.vertices [vertexIndex] = new Vector3 (topLeftX + x, heightCurve.Evaluate(heightMap [x, y]) * heightMultiplier, topLeftZ - y);
				meshData.uvs [vertexIndex] = new Vector2 (x / (float)width, y / (float)height);

				//If current "x" is less than "width" minus 1 and "y" is less than "height" minus 1, 
				if (x < width - 1 && y < height - 1) {
					//"AddTriangle" of "meshData" is called with current "vertexIndex", 
					meshData.AddTriangle (vertexIndex, vertexIndex + verticesPerLine + 1, vertexIndex + verticesPerLine);
					meshData.AddTriangle (vertexIndex + verticesPerLine + 1, vertexIndex, vertexIndex + 1);
				}
				vertexIndex++;
			}
		}
		return meshData;
	}
}

//"MeshData" class to store and manage the vertices, triangles, and UVs of the mesh
public class MeshData {
	public Vector3[] vertices;
	public int[] triangles;
	public Vector2[] uvs;

	int triangleIndex;

	public MeshData(int meshWidth, int meshHeight) {
		vertices = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth-1)*(meshHeight-1)*6];
	}

	public void AddTriangle(int a, int b, int c) {
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh() {
		Mesh mesh = new Mesh ();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}
}