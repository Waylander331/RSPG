/*
Author : Léa
Resume : Tool to open a window. In we can : select a target, instantiate the prefab Annotation to the target
*/

using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class AnnotationWindow : EditorWindow {	
	Vector3 myTarget;
	GameObject leModele;
	string temptext;
	public static List<TextMesh> myTextMesh = new List<TextMesh>();

	//Ouvre la fenetre AnnotationWindow
	[MenuItem ("Tools/Annotation",false,20)]
	static void Init () {
		AnnotationWindow window = (AnnotationWindow)EditorWindow.GetWindow (typeof (AnnotationWindow));
		window.Show();
	}
	
	void OnGUI () {
		if(Selection.activeTransform != null){
			myTarget = Selection.activeTransform.position;
		}
		else myTarget = Vector3.zero;
		GUILayout.Label ("Annotation", EditorStyles.boldLabel);
		EditorGUILayout.Space();
		GUILayout.Label ("Target");
		myTarget = EditorGUILayout.Vector3Field("", myTarget);
		GUILayout.Label ("Comment");
		temptext = EditorGUILayout.TextField (temptext, GUILayout.Height(20));

		//Instancie un prefab Annotation sur l'objet sélectionné, ajoute le TextMesh a la liste
		if(GUILayout.Button("Instantiate")){
			GameObject leModele = AssetDatabase.LoadAssetAtPath("Assets/Outils/PourTous/Annotation.prefab", typeof(GameObject)) as GameObject;
			GameObject newAnn = Instantiate(leModele, myTarget, Quaternion.identity) as GameObject;
			TextMesh temp = newAnn.GetComponentInChildren<TextMesh>();
			temp.text = temptext;
			myTextMesh.Add(temp);
		}
		EditorGUILayout.Space();
		GUILayout.Label ("Show/Hide Annotation  Ctrl+Alt+A", EditorStyles.boldLabel);
		EditorGUILayout.Space();

		//Vide la liste puis ajoute le TextMesh des Annotations a la liste
		if(GUILayout.Button("Refresh List")){
			TextMesh[] ann = GameObject.FindObjectsOfType<TextMesh>();
			myTextMesh.Clear();
			foreach(TextMesh temp in ann){
				if(temp.name == "TextAnnotation"){
					myTextMesh.Add (temp);
				}
			}
			//Debug.Log (myTextMesh.Count);
		}
		EditorGUILayout.Space();
		GUILayout.Label ("Please, refresh regularly", EditorStyles.boldLabel);
	}
}