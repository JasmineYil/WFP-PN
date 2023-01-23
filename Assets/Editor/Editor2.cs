using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Generator))]
public class Editor2 : Editor
{
    public override void OnInspectorGUI() {
		Generator mapGen2 = (Generator)target;

		if (DrawDefaultInspector ()) {
			if (mapGen2.autoUpdate) {
				mapGen2.GenerateMap2 ();
			}
		}

		if (GUILayout.Button ("Generate")) {
			mapGen2.GenerateMap2 ();
		}
	}
}
