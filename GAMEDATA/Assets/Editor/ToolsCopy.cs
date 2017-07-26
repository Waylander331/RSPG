/*
Author : Léa
Resume : 
*/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ToolsCopy : EditorWindow {

	private Vector3 distance;
	private int copyNumber;
	private float distanceX;
	private float distanceY;
	private float distanceZ;
	private bool rotation;
	private Vector3 rotationXYZ;
	private float rotationY;
	private float rotationZ;
	private float rayon;

	private bool translation;

	private List<GameObject> gameList = new List<GameObject>();

	[MenuItem ("Tools/Copy",false,41)]
	static void Init (){
		ToolsCopy window = (ToolsCopy)EditorWindow.GetWindow (typeof (ToolsCopy));
		window.Show();
	}
	
	void OnGUI () {	
		Transform[] sel = Selection.GetTransforms(SelectionMode.TopLevel);
		/*var editor = Editor.CreateEditor(this);
		editor.OnInspectorGUI();*/
		EditorGUILayout.Space();

		GUILayout.BeginHorizontal();
			GUILayout.Label("Number of Copy");
			copyNumber = EditorGUILayout.IntField(copyNumber);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();

		translation = EditorGUILayout.BeginToggleGroup("Translation", translation);
			GUILayout.Label("Position"/*,EditorStyles.boldLabel*/);
			GUILayout.BeginHorizontal();
			//EditorGUI.indentLevel ++;
				GUILayout.Label("X");
				distanceX = EditorGUILayout.FloatField(distanceX);
				GUILayout.Label("Y");
				distanceY = EditorGUILayout.FloatField(distanceY);
				GUILayout.Label("Z");
				distanceZ = EditorGUILayout.FloatField(distanceZ);
			//EditorGUI.indentLevel --;
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
		EditorGUILayout.EndToggleGroup();

		rotation = EditorGUILayout.BeginToggleGroup("Rotation", rotation);
			//GUILayout.Label("Rotation"/*,EditorStyles.boldLabel*/);
			//GUILayout.BeginHorizontal();
			//EditorGUI.indentLevel ++;
		GUILayout.BeginHorizontal();
				GUILayout.Label("Rayon");
				rayon = EditorGUILayout.FloatField(rayon);
		GUILayout.EndHorizontal();
				/*GUILayout.Label("X");
				rotationX = EditorGUILayout.FloatField(rotationX);*/
			GUILayout.BeginHorizontal();
				GUILayout.Label("Axe Y");
				rotationY = EditorGUILayout.FloatField(rotationY);
				GUILayout.Label("Axe Z");
				rotationZ = EditorGUILayout.FloatField(rotationZ);
			//EditorGUI.indentLevel --;
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
		EditorGUILayout.EndToggleGroup();

		if(translation){
			rotation = false;
		}
		else rotation = true;
		
		if(GUILayout.Button("Apply")){
			gameList.Clear();
			foreach(Transform temp in sel){
				for(int i = 1; i < copyNumber + 1; i++){
					distance.x = temp.position.x + distanceX * i;
					distance.y = temp.position.y + distanceY * i;
					distance.z = temp.position.z + distanceZ * i;

					rotationXYZ.y = (temp.position.y + rayon) * Mathf.Sin (Mathf.Deg2Rad * rotationY) * i;
					rotationXYZ.z = (temp.position.z + rayon) * Mathf.Cos (Mathf.Deg2Rad * rotationZ) * i;

					//rotation = Quaternion.Euler (rotationX * i, rotationY * i, rotationZ * i);
	
					//GameObject newInst;
					if(translation){
						Transform temp2 = Instantiate (temp, distance, temp.rotation) as Transform;
						GameObject newInst = temp2.gameObject;
						gameList.Add (newInst);
					}
					//gameList.Add (newInst);
					if(rotation){
						Transform temp3 = Instantiate (temp, rotationXYZ, Quaternion.FromToRotation(Vector3.forward, temp.position - rotationXYZ)) as Transform;
						GameObject newInst = temp3.gameObject;
						gameList.Add (newInst);
					}
				}
			}
		}
		if(GUILayout.Button("Revert")){
			foreach(GameObject temp in gameList){
				DestroyImmediate (temp.gameObject);
			}
			gameList.Clear();
		}
	}
}
