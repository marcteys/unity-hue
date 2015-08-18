using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(HueBridge))]
public class HueBridgeEditor : Editor {

	public override void OnInspectorGUI() {
		HueBridge hueBridge = (HueBridge)target;

		hueBridge.hostName = EditorGUILayout.TextField ("Host name:", hueBridge.hostName);
	//	hueBridge.portNumber = EditorGUILayout.IntField ("Port number:", hueBridge.portNumber);
		hueBridge.username = EditorGUILayout.TextField ("Username:", hueBridge.username);

		if (GUILayout.Button ("Discover Lights")) {
			hueBridge.DiscoverLights();
		}

		if (GUI.changed) {
			EditorUtility.SetDirty (target);
		}
	}

}
