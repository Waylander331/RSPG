/*
Author : Léa
Resume : Tool to replace the selection in the scene with the gameobject of your choice
*/

using UnityEngine;
using System.Collections;
using UnityEditor;

public class ToolsReplace : EditorWindow {
	public Vector3 oldModelPosition;
	public GameObject newModel;
	public bool scale = true;
	
	[MenuItem ("Tools/Replace",false,40)]
	static void Init (){
		ToolsReplace window = (ToolsReplace)EditorWindow.GetWindow (typeof (ToolsReplace));
		window.Show();
	}
	
	void OnGUI () {		
		Transform[] sel = Selection.GetTransforms(SelectionMode.TopLevel);
		var editor = Editor.CreateEditor(this);
		editor.OnInspectorGUI();
		EditorGUILayout.Space();
		if(GUILayout.Button("Replace")){
			foreach(Transform temp in sel){
				oldModelPosition = temp.position;
				GameObject newInst = Instantiate(newModel, temp.position, temp.rotation) as GameObject;
				if(scale){
					newInst.transform.localScale = temp.localScale;
				}
				//DestroyImmediate (temp.gameObject);
			}
		}
		EditorGUILayout.Space();
		GUILayout.Label ("Careful, use with caution!", EditorStyles.boldLabel);
	}
}
