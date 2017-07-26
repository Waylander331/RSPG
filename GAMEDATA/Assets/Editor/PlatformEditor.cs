using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Platform))]
[CanEditMultipleObjects]
public class PlatformEditor : Editor 
{
	private Platform pfTarget;

	SerializedProperty canFallProp;
	SerializedProperty fallTimerProp;

	SerializedProperty platformSpeedProp;
	//SerializedProperty speedProp;

	SerializedProperty isTriggerProp;
	SerializedProperty trapProp;

	SerializedProperty isRotatingProp;
	SerializedProperty rotHalfProp;
	SerializedProperty rotFullProp;
	SerializedProperty secondsProp;


	void OnEnable()
	{
		pfTarget = (Platform)target;

		canFallProp = serializedObject.FindProperty("canFall");
		fallTimerProp = serializedObject.FindProperty("fallTimer");

		platformSpeedProp = serializedObject.FindProperty("platformSpeed");

		isTriggerProp = serializedObject.FindProperty("isTrigger");
		trapProp = serializedObject.FindProperty("trap");

		isRotatingProp = serializedObject.FindProperty("trap.isRotating");
		rotHalfProp = serializedObject.FindProperty("trap.rotateHalf");
		rotFullProp = serializedObject.FindProperty("trap.rotateFull");
		secondsProp = serializedObject.FindProperty("trap.seconds");
	}


	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		EditorGUILayout.Space();

		EditorGUILayout.LabelField("Falling Platform",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (canFallProp, new GUIContent("Can Fall"));
		if(canFallProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (fallTimerProp, new GUIContent("Fall Timer"));
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Platform Speed",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (platformSpeedProp, new GUIContent("Speed"));

		/*EditorGUILayout.LabelField("Falling Platform",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;*/
		EditorGUILayout.PropertyField (isTriggerProp, new GUIContent("Is Trigger"));
		if(isTriggerProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (trapProp, new GUIContent("Trap"));
			if(trapProp.isExpanded)
			{
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField (isRotatingProp, new GUIContent("Is Rotating"));
				EditorGUILayout.PropertyField (rotHalfProp, new GUIContent("Half Rotation"));
				EditorGUILayout.PropertyField (rotFullProp, new GUIContent("Full Rotation"));
				EditorGUILayout.PropertyField (secondsProp, new GUIContent("Seconds"));
			}
		}


		serializedObject.ApplyModifiedProperties ();
	}


}
