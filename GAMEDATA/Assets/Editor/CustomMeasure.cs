using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof (GizmoRegleFixe))]
[CanEditMultipleObjects]
public class CustomMeasure : Editor {


	public override void OnInspectorGUI(){
		GizmoRegleFixe myTarget = (GizmoRegleFixe)target;

		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Lenght", myTarget.lenght.ToString());
		EditorGUILayout.LabelField("Height", myTarget.jumpHeight.ToString());
		EditorGUILayout.EndVertical();
		if(GUILayout.Button((myTarget.showMeasur1)? "Hide\nJump" : "Show\nJump")){
			myTarget.showMeasur1 = !myTarget.showMeasur1;
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("Lenght", myTarget.lenght2.ToString());
		EditorGUILayout.LabelField("Height", myTarget.jumpHeight2.ToString());
		EditorGUILayout.EndVertical();
		if(GUILayout.Button((myTarget.showMeasur2)? "Hide\nJump\nSprint" : "Show\nJump\nSprint")){
			myTarget.showMeasur2 = !myTarget.showMeasur2;
		}
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();

		if(myTarget.showMeasur1){
			myTarget.canShow = true;
			EditorUtility.SetDirty(myTarget);
		}
		else {
			myTarget.canShow = false;
			EditorUtility.SetDirty(myTarget);
		}

		if(myTarget.showMeasur2){
			myTarget.canShow2 = true;
			EditorUtility.SetDirty(myTarget);
		}
		else {
			myTarget.canShow2 = false;
			EditorUtility.SetDirty(myTarget);
		}
	}
}
