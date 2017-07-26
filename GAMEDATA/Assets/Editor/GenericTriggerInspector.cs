/*using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(GenericTrigger))]
public class GenericTriggerInspector : Editor 
{
	GenericTrigger script;*/
	//private SerializedProperty tempProp;

	/*public bool agentExpanded = false;

	public int agentSize = 0;

	public bool trigObjExpanded = false;
	public bool[] trigElementExpanded;
	public int trigSize = 0;

	public bool[]
		frontExpanded,
		backExpanded,
		leftExpanded,
		rightExpanded;*/

/*
	void OnEnable()
	{
		script = (GenericTrigger)target;
	}

	public override void OnInspectorGUI()
	{
		script = (GenericTrigger)target;
		serializedObject.Update ();

		//base.OnInspectorGUI ();
		EditorGUILayout.Space ();
		EditorGUILayout.Space ();
		EditorGUILayout.BeginVertical ();


		// Behaviour
		script.behaviour = (GenericTrigger.TriggerBehaviour) EditorGUILayout.EnumPopup ("Behaviour", script.behaviour);

		// Direction
		script.direction = (GenericTrigger.TriggerDirection) EditorGUILayout.EnumPopup ("Direction", script.direction);
		if (script.direction == GenericTrigger.TriggerDirection.Specific) 
		{
			EditorGUI.indentLevel+= 2;
			script.front = EditorGUILayout.Toggle("Front", script.front);
			script.back = EditorGUILayout.Toggle("Back", script.back);
			script.left = EditorGUILayout.Toggle("Left", script.left);
			script.right = EditorGUILayout.Toggle("Right", script.right);
			EditorGUI.indentLevel -= 2;
		}

		EditorGUILayout.Space ();

		// Agent Response
		script.agentResponse = (GenericTrigger.AgentResponse) EditorGUILayout.EnumPopup ("Agent Response", script.agentResponse);

		switch (script.agentResponse) 
		{
		case GenericTrigger.AgentResponse.single:
			script.agentSize = 1;
			ResizeAgentArray(script.agentSize, ref script.agents);
			script.agents[0] = (GameObject) EditorGUILayout.ObjectField("Agent", script.agents[0], typeof(GameObject), true);
			break;
		case GenericTrigger.AgentResponse.multiple:
			//agentArray = EditorGUILayout.Foldout (agentArray, "Agents");
			script.agentExpanded = EditorGUILayout.PropertyField(serializedObject.FindProperty("agents"));
			if(script.agentExpanded)
			{
				EditorGUI.indentLevel++;
				script.agentSize = EditorGUILayout.IntField("Size", script.agentSize); 
				ResizeAgentArray(script.agentSize, ref script.agents);
				for(int i = 0; i < script.agentSize; i++)
				{
					script.agents[i] = (GameObject) EditorGUILayout.ObjectField("Element " + i, script.agents[i], typeof(GameObject), true);
				}
				EditorGUI.indentLevel--;
			}
			break;
		}
		EditorGUILayout.Space ();
		//trigObjExpanded = EditorGUILayout.Foldout (trigObjExpanded, "Triggered Objects");
		script.trigObjExpanded = EditorGUILayout.PropertyField(serializedObject.FindProperty("triggeredObjects"));
		if(script.trigObjExpanded)
		{
			EditorGUI.indentLevel++;
			script.trigSize = EditorGUILayout.IntField("Size", script.trigSize);
			ResizeBoolArray(script.trigSize, ref script.trigElementExpanded);
			ResizeTriggeredObjArray(script.trigSize, ref script.triggeredObjects);
			ResizeBoolArray(script.trigSize, ref script.frontExpanded);
			ResizeBoolArray(script.trigSize, ref script.backExpanded);
			ResizeBoolArray(script.trigSize, ref script.leftExpanded);
			ResizeBoolArray(script.trigSize, ref script.rightExpanded);
			ResizeTriggeredObjArray(script.trigSize, ref script.triggeredObjects);
			Debug.Log ("trigObjArray : " + script.trigElementExpanded.Length);
			for(int i = 0; i < script.trigSize; i++)
			{
				TriggeredObjectField(i);
			}
			EditorGUI.indentLevel--;
			
		}

		EditorGUILayout.EndVertical ();
		serializedObject.ApplyModifiedProperties ();
	}

	void ResizeAgentArray(int size, ref GameObject[] toResize)
	{
		GameObject[] newArray = new GameObject[size];
		for (int i = 0; i < toResize.Length && i < size; i++) 
		{
			newArray[i] = toResize[i];
		}
		//toResize = new GameObject[size];
		toResize = newArray;
	}

	void TriggeredObjectField(int i)
	{
		script.trigElementExpanded [i] = EditorGUILayout.Foldout (script.trigElementExpanded [i], "Element " + i);
		EditorGUI.indentLevel++;
		if (script.trigElementExpanded [i]) 
		{
			script.triggeredObjects[i].obj = (IsTriggerable)EditorGUILayout.ObjectField ("Obj", script.triggeredObjects[i].obj, typeof(IsTriggerable), false);
			
			switch (script.direction) {
			case GenericTrigger.TriggerDirection.Any:
				script.triggeredObjects[i].onEnterAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Enter Action", script.triggeredObjects[i].onEnterAction);
				script.triggeredObjects[i].onStayAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Stay Action", script.triggeredObjects[i].onStayAction);
				script.triggeredObjects[i].onExitAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Exit Action", script.triggeredObjects[i].onExitAction);
				break;
			case GenericTrigger.TriggerDirection.Specific:
				if (script.front) {
					script.frontExpanded [i] = EditorGUILayout.Foldout (script.frontExpanded [i], "Front");
					if (script.frontExpanded [i]) {
						script.triggeredObjects[i].onEnterAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Enter Action", script.triggeredObjects[i].onEnterAction);
						script.triggeredObjects[i].onStayAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Stay Action", script.triggeredObjects[i].onStayAction);
						script.triggeredObjects[i].onExitAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Exit Action", script.triggeredObjects[i].onExitAction);
					}
				}
				if (script.back) {
					script.backExpanded [i] = EditorGUILayout.Foldout (script.backExpanded [i], "Back");
					if (script.backExpanded [i]) {
						script.triggeredObjects[i].onEnterAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Enter Action", script.triggeredObjects[i].onEnterAction);
						script.triggeredObjects[i].onStayAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Stay Action", script.triggeredObjects[i].onStayAction);
						script.triggeredObjects[i].onExitAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Exit Action", script.triggeredObjects[i].onExitAction);
					}
				}
				if (script.left) {
					script.leftExpanded [i] = EditorGUILayout.Foldout (script.leftExpanded [i], "Left");
					if (script.leftExpanded [i]) {
						script.triggeredObjects[i].onEnterAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Enter Action", script.triggeredObjects[i].onEnterAction);
						script.triggeredObjects[i].onStayAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Stay Action", script.triggeredObjects[i].onStayAction);
						script.triggeredObjects[i].onExitAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Exit Action", script.triggeredObjects[i].onExitAction);
					}
				}
				if (script.right) {
					script.rightExpanded [i] = EditorGUILayout.Foldout (script.rightExpanded [i], "Right");
					if (script.rightExpanded [i]) {
						script.triggeredObjects[i].onEnterAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Enter Action", script.triggeredObjects[i].onEnterAction);
						script.triggeredObjects[i].onStayAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Stay Action", script.triggeredObjects[i].onStayAction);
						script.triggeredObjects[i].onExitAction = (TriggeredObject.Action)EditorGUILayout.EnumPopup ("On Exit Action", script.triggeredObjects[i].onExitAction);
					}
				}
				break;
			default:
				break;
			}
		}
		EditorGUI.indentLevel--;
	}

	void ResizeBoolArray(int size, ref bool[] toResize)
	{
		Debug.Log ("Hello");
		toResize = new bool[size];
		bool[] newArray = new bool[size];
		for (int i = 0; i < toResize.Length && i < size; i++) 
		{
			newArray[i] = toResize[i];
		}
		toResize = newArray;
	}

	void ResizeTriggeredObjArray(int size, ref TriggeredObject[] toResize)
	{
		TriggeredObject[] newArray = new TriggeredObject[size];
		toResize = new TriggeredObject[size];
		for (int i = 0; i < toResize.Length && i < size; i++) {
			newArray [i] = toResize [i];
		}
		toResize = newArray;
	}
}*/
