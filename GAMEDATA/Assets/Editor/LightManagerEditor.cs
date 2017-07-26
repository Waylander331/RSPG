using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(LightsManager))]
[CanEditMultipleObjects]
public class LightManagerEditor : Editor 
{
	private ReorderableList list;
	SerializedProperty enabledProp;
	SerializedProperty intensityProp;
	SerializedProperty lightIntensityProp;


	SerializedProperty jiggleAndWiggleProp;
	SerializedProperty repeatLerpProp;
	SerializedProperty lerpTimerProp;
	SerializedProperty mainColorProp;
	SerializedProperty switchColorProp;

	SerializedProperty flickerProp;
	SerializedProperty timerFlickerProp;

	SerializedProperty lerpAllColorProp;

	SerializedProperty disableUntriggeredProp;

	private LightsManager lmObject;

	void OnEnable()
	{
		lmObject = (LightsManager)target;

		enabledProp = serializedObject.FindProperty("enabled");
		intensityProp = serializedObject.FindProperty("intensity");
		lightIntensityProp = serializedObject.FindProperty("lightIntensity");

		list = new ReorderableList(serializedObject, serializedObject.FindProperty("myLights"), true, true, true, true);

		jiggleAndWiggleProp = serializedObject.FindProperty("jiggleAndWiggleColor");
		repeatLerpProp = serializedObject.FindProperty("repeatLerp");
		lerpTimerProp = serializedObject.FindProperty("lerpTimer");
		mainColorProp = serializedObject.FindProperty("mainColor");
		switchColorProp = serializedObject.FindProperty("switchColor");

		flickerProp = serializedObject.FindProperty("flicker");
		timerFlickerProp = serializedObject.FindProperty("flickerTimer");

		lerpAllColorProp = serializedObject.FindProperty("lerpAllColor");

		disableUntriggeredProp = serializedObject.FindProperty("disableUntriggered");


	}
	
	// Update is called once per frame
	public override void OnInspectorGUI()
	{
		serializedObject.Update();


		EditorGUILayout.Space();
		list.DoLayoutList();
		
		list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			var element = list.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.PropertyField(new Rect(rect.x + 60, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("lights"), GUIContent.none);
		};
		list.drawHeaderCallback= (Rect rect) => {  
			EditorGUI.LabelField(rect, "Lights Tab");
		};

		EditorGUILayout.LabelField("Enable / Intensity",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField(enabledProp, new GUIContent("Enabled"));
		
		EditorGUILayout.PropertyField(intensityProp, new GUIContent("Intensity"));
		if(intensityProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(lightIntensityProp, new GUIContent("Light Intensity"));
			EditorGUI.indentLevel--;
		}

		//EditorGUILayout.Space();
		//EditorGUILayout.LabelField("Main Color",EditorStyles.boldLabel);
		//EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (mainColorProp, new GUIContent("Main Color"));



		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Lerp Options",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (jiggleAndWiggleProp, new GUIContent("Lerp Color"));
		if(jiggleAndWiggleProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (repeatLerpProp, new GUIContent("Repeat Lerp"," Do you want the Lerp to be repeated?"));
			EditorGUI.indentLevel--;
			EditorGUILayout.PropertyField (switchColorProp, new GUIContent("Switch Color"));
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(lerpTimerProp, new GUIContent("Lerp Timer", "The time it takes to lerp the color."));
			EditorGUI.indentLevel--;
		}

		EditorGUILayout.Space();
		//EditorGUILayout.LabelField("Flicker Option",EditorStyles.boldLabel);
		EditorGUI.indentLevel++;
		EditorGUILayout.PropertyField (flickerProp, new GUIContent("Flicker", " Do you want the light to flicker? ( flash )"));
		if(flickerProp.boolValue)
		{
			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField(timerFlickerProp, new GUIContent("Flicker Timer", "Set the timing between light flash."));
			EditorGUI.indentLevel--;
		}

		EditorGUI.indentLevel--;
		EditorGUI.indentLevel--;
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(lerpAllColorProp, new GUIContent("Lerp All Color","Lerp all the color available."));

		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(disableUntriggeredProp, new GUIContent("Disable Out Of Trigger", "Disable the lights and everything when going out of the trigger."));

		serializedObject.ApplyModifiedProperties ();

	}
}
