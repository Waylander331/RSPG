using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WaypointScript))]
[CanEditMultipleObjects]
public class WaypointEditor : Editor {

	SerializedProperty doingStateProp;

	SerializedProperty timeOfWaitProp;
	SerializedProperty isStoppedProp;

	//SerializedProperty armsSpeedProp;
	SerializedProperty platformSpeedProp;

	SerializedProperty platformOrientationProp;

	SerializedProperty isHalfProp;
	SerializedProperty timeOfWaitHalfProp;
	SerializedProperty isFullProp;
	SerializedProperty timeOfWaitFullProp;

	//SerializedProperty wantToTeleportProp;
	SerializedProperty 	isGoingProp;
	SerializedProperty isReturnedProp;
	SerializedProperty 	wpGoProp;
	SerializedProperty 	wpReturnProp;

	private WaypointScript wpObject;
	
	void OnEnable () {
		wpObject = (WaypointScript)target;

		doingStateProp = serializedObject.FindProperty ("doingState");

		timeOfWaitProp = serializedObject.FindProperty ("timeOfWait");
		isStoppedProp = serializedObject.FindProperty ("isStopped");

		//armsSpeedProp = serializedObject.FindProperty ("armsSpeed");
		platformSpeedProp = serializedObject.FindProperty ("platformSpeed");

		platformOrientationProp = serializedObject.FindProperty ("platformOrientation");

		isHalfProp = serializedObject.FindProperty ("isHalf");
		timeOfWaitHalfProp = serializedObject.FindProperty ("timeOfWaitHalf");
		isFullProp = serializedObject.FindProperty ("isFull");
		timeOfWaitFullProp = serializedObject.FindProperty ("timeOfWaitFull");

		//wantToTeleportProp = serializedObject.FindProperty ("wantToTeleport");
		isGoingProp = serializedObject.FindProperty ("isGoing");
		isReturnedProp = serializedObject.FindProperty ("isReturned");
		wpGoProp = serializedObject.FindProperty ("wpGo");
		wpReturnProp = serializedObject.FindProperty ("wpReturn");
	}

	public override void OnInspectorGUI(){
		serializedObject.Update ();

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (doingStateProp, new GUIContent("Behaviour"));

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (timeOfWaitProp, new GUIContent("Delay Time"));
		
		EditorGUILayout.PropertyField (isStoppedProp, new GUIContent("Stop"));

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("New Speed",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (platformSpeedProp, new GUIContent("Platform New Speed"));
		//EditorGUILayout.PropertyField (armsSpeedProp, new GUIContent("Arms New Speed"));
		EditorGUI.indentLevel--;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Orientation",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (platformOrientationProp);
		EditorGUI.indentLevel--;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Rotation",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (isHalfProp, new GUIContent("Rotation 90°"));
		if(isHalfProp.boolValue == true){
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (timeOfWaitHalfProp, new GUIContent("Delay Time"));
			EditorGUI.indentLevel--;
		}
		
		EditorGUILayout.PropertyField (isFullProp, new GUIContent("Rotation 180°"));
		if(isFullProp.boolValue == true){
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (timeOfWaitFullProp, new GUIContent("Delay Time"));
			EditorGUI.indentLevel--;
		}
		EditorGUI.indentLevel--;

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Teleport",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (isGoingProp, new GUIContent("Go"));
			if(isGoingProp.boolValue == true){
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField (wpGoProp, new GUIContent("Target"));
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.PropertyField (isReturnedProp, new GUIContent("Return"));
			if(isReturnedProp.boolValue == true){
				EditorGUI.indentLevel++;
				EditorGUILayout.PropertyField (wpReturnProp, new GUIContent("Target"));
			}
		EditorGUILayout.Space();

		if(isHalfProp.boolValue == true){
			isFullProp.boolValue = false;
		}
		
		serializedObject.ApplyModifiedProperties ();
	}

	public void OnSceneGUI(){
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;
		Handles.Label(wpObject.transform.position + Vector3.up, wpObject.Id.ToString(),style);
	}
}







