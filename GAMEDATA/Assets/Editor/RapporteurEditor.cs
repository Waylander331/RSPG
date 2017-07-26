using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (GizmoRapporteur))]
[CanEditMultipleObjects]
public class RapporteurEditor : Editor {

	private bool canCalcul;

	public override void OnInspectorGUI(){
		GizmoRapporteur myTarget = (GizmoRapporteur)target;

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Angle", myTarget.angle.ToString());
		
		if(GUILayout.Button ("Apply")){
			myTarget.CalculAngle();
		}
		EditorGUILayout.EndVertical();
		if(GUILayout.Button((myTarget.showRapporteur)? "Hide\nProtractor" : "Show\nProtractor")){
			myTarget.showRapporteur = !myTarget.showRapporteur;
		}

		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		if(myTarget.showRapporteur){
			myTarget.canShow = true;
			EditorUtility.SetDirty(myTarget);
		}
		else {
			myTarget.canShow = false;
			myTarget.angle = 0;
			EditorUtility.SetDirty(myTarget);
		}
	}
}
