using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor (typeof (Roller))]
[CanEditMultipleObjects]
public class RollerEditor : Editor {

	private ReorderableList list;

	//SerializedProperty tabRollersProp;

	SerializedProperty speedProp;
	//SerializedProperty rollerSensProp;
	SerializedProperty rollerSurfaceProp;
	//SerializedProperty isReversedProp;
	SerializedProperty newSpeedProp;

	//static int nbPiece = 0;

	private Roller rollerTarget;
	
	void OnEnable () {
		//rollerTarget = (Roller)target;

		list = new ReorderableList(serializedObject, serializedObject.FindProperty("listRollers"), true, true, true, true);
	
		//tabRollersProp = serializedObject.FindProperty ("tabRollers");

		speedProp = serializedObject.FindProperty ("speed");
		//rollerSensProp = serializedObject.FindProperty ("rollerSens");
		rollerSurfaceProp = serializedObject.FindProperty ("rollerSurface");
		//isReversedProp = serializedObject.FindProperty ("isReversed");
		newSpeedProp = serializedObject.FindProperty ("newSpeed");
	}
	
	public override void OnInspectorGUI(){
		serializedObject.Update ();

		EditorGUILayout.Space();
		list.DoLayoutList();

		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("prefab"), GUIContent.none);
		};
		list.drawHeaderCallback= (Rect rect) => {  
			EditorGUI.LabelField(rect, "Roller pieces");
		};
		/*tabRollersProp.arraySize = EditorGUILayout.IntField("Size" , tabRollersProp.arraySize);

		EditorGUILayout.PropertyField(tabRollersProp, new GUIContent ("tab"));*/
		/*tabRollersProp = new GameObject[tabRollersProp.arraySize];*/
		//roller.tabRollers.rollers.Length = EditorGUILayout.IntField("Size", roller.tabRollers.rollers.);

		/*rollerTarget.tabRollers.size = EditorGUILayout.IntField("Size", rollerTarget.tabRollers.size);
		for(int i = 0; i < rollerTarget.tabRollers.size; i++){
			rollerTarget.tabRollers.rollers = (GameObject)EditorGUILayout.ObjectField("Roller piece" + nbPiece, rollerTarget.tabRollers.rollers, typeof(GameObject));
		nbPiece++;
		}*/

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.IntSlider(speedProp, 0, 15, new GUIContent ("Speed"));
		//EditorGUILayout.PropertyField(rollerSensProp, new GUIContent ("Sens"));
		EditorGUILayout.PropertyField(rollerSurfaceProp, new GUIContent ("Surface"));
		EditorGUILayout.Space();
		//EditorGUILayout.PropertyField(isReversedProp, new GUIContent ("Reverse"));
		EditorGUILayout.LabelField("Switch", EditorStyles.boldLabel);
		EditorGUI.indentLevel ++;
		EditorGUILayout.IntSlider(newSpeedProp, 0, 15, new GUIContent ("New Speed"));
		EditorGUI.indentLevel --;
		EditorGUILayout.Space();
		
		serializedObject.ApplyModifiedProperties ();
	}
}
