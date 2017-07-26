using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Movable))]
public class PathfindingEditor : Editor {

	private SerializedProperty distanceCoveredProp;

	private SerializedProperty wpStartProp;
	private SerializedProperty doesLoopProp;
	private SerializedProperty doesReverseProp;
	private SerializedProperty waypointProp;
	private SerializedProperty showPercentProp;
	private SerializedProperty notForwardProp;
	private SerializedProperty isPoneyProp;
	private Movable pathObject;

	void OnEnable () {
		pathObject = (Movable)target;

		wpStartProp = serializedObject.FindProperty ("wpStart");

		distanceCoveredProp = serializedObject.FindProperty ("distanceCovered");
		doesLoopProp = serializedObject.FindProperty ("doesLoop");
		doesReverseProp = serializedObject.FindProperty ("doesReverse");
		waypointProp = serializedObject.FindProperty ("waypoint");
		showPercentProp = serializedObject.FindProperty ("showPercent");
		notForwardProp = serializedObject.FindProperty ("notForward");
		isPoneyProp = serializedObject.FindProperty ("isPoney");
	}

	public override void OnInspectorGUI(){
		serializedObject.Update();
		
		EditorGUILayout.PropertyField (waypointProp, new GUIContent ("Waypoint"));
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (wpStartProp, new GUIContent ("Waypoint Start"));
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (doesLoopProp, new GUIContent ("Loop"));
		EditorGUILayout.PropertyField (doesReverseProp, new GUIContent ("Reverse"));
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField (isPoneyProp, new GUIContent ("Poney"));
		EditorGUILayout.Space();
		pathObject.IsEditorMode = EditorGUILayout.Toggle ("Editor Mode", pathObject.IsEditorMode);

		if(pathObject.IsEditorMode){
			EditorGUI.indentLevel++;
			EditorGUILayout.Slider(distanceCoveredProp, 0f, 1f, new GUIContent ("Distance Covered"));
			EditorGUILayout.PropertyField (showPercentProp, new GUIContent ("Show Percent"));
			EditorGUILayout.PropertyField (notForwardProp, new GUIContent ("Keep orientation"));
		}
		EditorGUILayout.Space();
		if(!pathObject.CreateWaypoints){
			if(GUILayout.Button ("New Waypoints")){ 
				if(pathObject.HasWaypoints)pathObject.CancelWaypoints();
				pathObject.CreateWaypoints = true;
			}
			if(pathObject.HasWaypoints){
				if(GUILayout.Button ("Clear Waypoints"))pathObject.CancelWaypoints();
			}
		}
		else{
			if(GUILayout.Button("Validate")) {
				pathObject.ValidateWaypoints();
				pathObject.CreateWaypoints = false;
				EditorUtility.SetDirty(pathObject);
			}
			if(GUILayout.Button ("Cancel"))pathObject.CancelWaypoints();
		}

		serializedObject.ApplyModifiedProperties();
	}

	public void OnSceneGUI(){

		//Text indicator of pathfinding object's position
		if(pathObject.ShowPercent && pathObject.HasWaypoints)Handles.Label(pathObject.transform.position + Vector3.up * 1.5f, "Ma Position: " + pathObject.transform.position + "\nPourcentage du trajet: " + pathObject.distanceCovered * 100 + "%");	
		//else Handles.Label (pathObject.transform.position + Vector3.up * 1.5f, "Add waypoints!");

		//creation of waypoints
		Event e = Event.current;

		if(pathObject.CreateWaypoints && e.type == EventType.mouseDown && e.button == 0){

			Vector2 spawnPosition = e.mousePosition;
			spawnPosition.y = Screen.height - (e.mousePosition.y + 30); //handles scene screen offset.

			if(isPoneyProp.boolValue == true){
				Ray ray = Camera.current.ScreenPointToRay(spawnPosition);
				RaycastHit hit;
				//LayerMask mask = LayerMask.GetMask("Collision");
				if(Physics.Raycast(ray, out hit, 1000f)){
					if(hit.collider.tag == "Rail"){
						pathObject.BuildWaypoints(hit.point);
					}
				}
			}
			else {
				Ray ray = Camera.current.ScreenPointToRay(spawnPosition);
				RaycastHit hit;
				LayerMask mask = LayerMask.GetMask("Collision");
				if(Physics.Raycast(ray, out hit, 1000f, mask)){
					pathObject.BuildWaypoints(hit.point);
				}
			}
		}
		if(pathObject.CreateWaypoints){
			Selection.activeGameObject = pathObject.gameObject;
		}
	}
}
