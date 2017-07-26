using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (Door))]
[CanEditMultipleObjects]
public class DoorEditor : Editor {

	SerializedProperty doorDirectionOProp;
	SerializedProperty openSpeedProp;
	SerializedProperty timerOpenProp;
	
	SerializedProperty closeSpeedProp;
	SerializedProperty timerCloseProp;

	SerializedProperty startPositionProp;
	SerializedProperty timerResetProp;
	
	void OnEnable () {
		doorDirectionOProp = serializedObject.FindProperty ("doorDirectionO");
		openSpeedProp = serializedObject.FindProperty ("openSpeed");
		timerOpenProp = serializedObject.FindProperty ("timerOpen");

		closeSpeedProp = serializedObject.FindProperty ("closeSpeed");
		timerCloseProp = serializedObject.FindProperty ("timerClose");

		startPositionProp = serializedObject.FindProperty ("startPosition");
		timerResetProp = serializedObject.FindProperty ("timerReset");
	}
	
	public override void OnInspectorGUI(){
		serializedObject.Update ();

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(startPositionProp, new GUIContent ("Start Position"));
		EditorGUILayout.PropertyField(timerResetProp, new GUIContent ("Reset Delay"));
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Open", EditorStyles.boldLabel);
		EditorGUI.indentLevel ++;
		EditorGUILayout.PropertyField(doorDirectionOProp, new GUIContent ("Open Direction"));
		EditorGUILayout.PropertyField(openSpeedProp, new GUIContent ("Open Speed"));
		EditorGUILayout.PropertyField(timerOpenProp, new GUIContent ("Open Delay"));
		EditorGUI.indentLevel --;
		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Close", EditorStyles.boldLabel);
		EditorGUI.indentLevel ++;
		EditorGUILayout.PropertyField(closeSpeedProp, new GUIContent ("Close Speed"));
		EditorGUILayout.PropertyField(timerCloseProp, new GUIContent ("Close Delay"));
		EditorGUI.indentLevel --;
		EditorGUILayout.Space();

		serializedObject.ApplyModifiedProperties ();
	}
}
