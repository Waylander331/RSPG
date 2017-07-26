/*
Author : Léa
Resume : Tool to reset position, rotation and scale independently
*/
/*
using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor (typeof (GameObject))]
public class ResetTransform : Editor {
	private bool position = false;
	private bool rotation = false;
	private bool scale = false;
	private Transform[] sel = Selection.GetTransforms(SelectionMode.TopLevel);

	public override void OnInspectorGUI(){
		DrawDefaultInspector();
		EditorGUILayout.Space();
		GUILayout.Label("Reset",EditorStyles.boldLabel);
		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal();
		position = GUILayout.Toggle(position, "Position");
		rotation = GUILayout.Toggle(rotation, "Rotation");
		scale = GUILayout.Toggle(scale, "Scale");
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button ("Apply")){
			if(position && rotation && scale){
				ResetPosition();
				ResetRotation();
				ResetScale();
			}
			else if(position && rotation && !scale){
				ResetPosition();
				ResetRotation();
			}
			else if(!position && rotation && scale){
				ResetRotation();
				ResetScale();
			}
			else if(position && !rotation && scale){
				ResetPosition();
				ResetScale();
			}
			else if(position && !rotation && !scale){
				ResetPosition();
			}
			else if(!position && rotation && !scale){
				ResetRotation();
			}
			else if(!position && !rotation && scale){
				ResetScale();
			}
		}
		GUILayout.EndVertical();
		EditorGUILayout.Space();
	}

	void ResetPosition(){
		foreach(Transform temp in sel){
			temp.position = Vector3.zero;
		}
	}

	void ResetRotation(){
		foreach(Transform temp in sel){
			temp.rotation = Quaternion.identity;
		}
	}

	void ResetScale(){
		foreach(Transform temp in sel){
			Vector3 tempScale = temp.transform.localScale;
			tempScale.x = 1;
			tempScale.y = 1;
			tempScale.z = 1;
			temp.transform.localScale = tempScale;
		}
	}

}*/
