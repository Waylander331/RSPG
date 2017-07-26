/*using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lethal))]
public class LethalEditor : Editor 
{
	//public GameObject parent;
	private Lethal myLethal;
	
	void Awake(){
		myLethal = (Lethal) target;
	}
	
	public  override void OnInspectorGUI()
	{
		myLethal.hasParent = GUILayout.Toggle(myLethal.hasParent, "Has a parent");
		if(myLethal.hasParent){
			myLethal.parent = EditorGUILayout.ObjectField(myLethal.parent,typeof(GameObject),true) as GameObject;
		}
		else{
			myLethal.parent = null;
		}
	}
}*/
