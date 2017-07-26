/*
Author : Léa
Resume : Tool to create groups and clear groups
*/

using UnityEngine;
using System.Collections;
using UnityEditor;

public class ToolsGroup : Editor {

	//Crée un empty parent de la selection, ctrl+g
	[MenuItem("Tools/Create group %g",false,1)]
	private static void NewMenuCreateGroup(){
		GameObject group = new GameObject("Group");
		group.tag = "Group";
		Transform groupTransform = group.GetComponent<Transform>();
		Transform[] selected = Selection.GetTransforms(SelectionMode.TopLevel);
		Vector3 temp = Vector3.zero;
		//Quaternion temp2 = Quaternion.identity;
		for(int i = 0 ; i < selected.Length; i++){
			temp.x += selected[i].position.x;
			temp.y += selected[i].position.y;
			temp.z += selected[i].position.z;

			/*temp2.x += selected[i].rotation.x;
			temp2.y += selected[i].rotation.y;
			temp2.z += selected[i].rotation.z;*/
		
			//temp2 = selected[i].rotation;
		}

		group.transform.position = temp/selected.Length;
		//group.transform.rotation = temp2/*/selected.Length*/;

		foreach(Transform t in selected){
			t.parent = groupTransform;
		}
	}

	//Set le parent des items selectionnés a null, supprime les empty vides, ctrl+shift+g
	[MenuItem("Tools/Clear group %#g",false,1)]
	private static void NewMenuClearGroup(){
		Transform[] selected = Selection.GetTransforms(SelectionMode.TopLevel);
		foreach(Transform t in selected){
			t.parent = null;
		}
		Transform[] group = GameObject.FindObjectsOfType<Transform>();
		foreach(Transform t in group){
			if(t.tag == "Group"){
				if(t.childCount == 0){
					Object.DestroyImmediate(t.gameObject);
				}
			}
		}
	}
}
