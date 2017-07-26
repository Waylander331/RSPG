using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (GizmoRegle))]
[CanEditMultipleObjects]
public class CustomMeasureMovable : Editor {

	SerializedProperty widthProp;
	SerializedProperty heightProp;

	void OnEnable () {
		widthProp = serializedObject.FindProperty ("width");
		heightProp = serializedObject.FindProperty ("height");
	}

	public override void OnInspectorGUI(){
		serializedObject.Update ();

		EditorGUILayout.Slider(widthProp, 0, 50, new GUIContent ("Width"));
		EditorGUILayout.Slider(heightProp, 0, 50, new GUIContent ("Height"));

		serializedObject.ApplyModifiedProperties ();
	}
}
