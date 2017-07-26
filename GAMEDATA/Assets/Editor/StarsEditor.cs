using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(Stars))]
public class StarsEditor : Editor {

	public override void OnInspectorGUI(){

		DrawDefaultInspector();

		Stars star = (Stars)target;

		if(star.IsTaken){

			if(GUILayout.Button ("Reset Star")){
				star.ResetTaken();
				Manager.Instance.RevertStar(star);
			}
		}
		else{
			if(GUILayout.Button ("Set as Taken")){
				star.Taken(true);
				Manager.Instance.GotStar(star); 
			}
		}
	}

}
