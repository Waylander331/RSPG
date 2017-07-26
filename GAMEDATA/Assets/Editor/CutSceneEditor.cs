using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(CutSceneMaker))]
public class CutSceneEditor : Editor {

	private SerializedProperty kfObj;
	private CutSceneMaker myMaker;
	private SerializedProperty playOnStart;
	private SerializedProperty avatarSpawnAtEnd;
	private SerializedProperty actorSpawnAtStart;
	private SerializedProperty avPosition;
	private SerializedProperty camPosition;

	void OnEnable () {
		myMaker = (CutSceneMaker)target;
		kfObj = serializedObject.FindProperty ("myKF");
		camPosition = serializedObject.FindProperty ("camPosition");
		avPosition = serializedObject.FindProperty ("avatarPosition");
		playOnStart = serializedObject.FindProperty ("playOnStart");
		avatarSpawnAtEnd = serializedObject.FindProperty ("avatarSpawnAtEnd");
		actorSpawnAtStart = serializedObject.FindProperty ("actorSpawnAtStart");
	}
	
	public override void OnInspectorGUI(){

		serializedObject.Update();
		EditorGUILayout.PropertyField (kfObj, new GUIContent ("keyframe"));
		
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (playOnStart);
		EditorGUILayout.PropertyField (avatarSpawnAtEnd);
		EditorGUILayout.PropertyField (actorSpawnAtStart);

		EditorGUILayout.Space();
		EditorGUILayout.IntSlider (avPosition,0,(myMaker.AvatarKFList.Count!=0)?myMaker.AvatarKFList.Count-1:0);
		EditorGUILayout.IntSlider (camPosition,0,(myMaker.CamKFList.Count!=0)?myMaker.CamKFList.Count-1:0);

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if(GUILayout.Button ("Update Keyframe")){ 
			myMaker.UpdateKeyframe();
		}
//		if(GUILayout.Button ("Create Avatar Keyframe")){ 
//			myMaker.CreateAvKF();
//		}
//		if(GUILayout.Button ("Create Camera Keyframe")){ 
//			myMaker.CreateCamKF();
//		}
//		EditorGUILayout.Space ();
//		if(GUILayout.Button ("Clear Avatar Keyframe")){ 
//			myMaker.ClearAvKF();
//		}
//		if(GUILayout.Button ("Clear Camera Keyframe")){ 
//			myMaker.ClearCamKF();
//		}

		
		serializedObject.ApplyModifiedProperties();
	}
	
	public void OnSceneGUI(){
		
		
	}
}
