using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Movable : MonoBehaviour {

	public void SetDelay(float seconds){
		SendMessage ("Wait", seconds);
	}

	public void StopTarget(bool isStopped){
		SendMessage ("Stop", isStopped);
	}

	public void SetNewSpeed(float speed){
		SendMessage ("NewSpeed", speed);
	}

	public void SetRotation(int orientation){
		SendMessage ("ChangeRotation", orientation);
	}

	public void TeleportTarget(WaypointScript where){
		SendMessage ("Teleport", where);
	}

	public void SetHalfRotation(MovableRotation rotation){
		SendMessage ("RotateHalf", rotation);
	}

	public void SetFullRotation(MovableRotation rotation){
		SendMessage ("RotateFull", rotation);
	}

	[SerializeField] private WaypointScript wpStart;

	//Pathfinding
	[SerializeField] private List<WaypointScript> wpsList;
	[SerializeField] private List<WaypointScript> lWps;
	[SerializeField] private bool hasWaypoints = false;

	[SerializeField] private bool doesLoop = false;
	[SerializeField] private bool doesReverse = false;

	[SerializeField] private bool isPoney = false;

	private int courant = 0;
	private int next = 1;
	private bool isReversed = false;

	//Instanciation manuelle
	[HideInInspector]
	private bool createWaypoints;
	private bool isEditorMode = false;
	
	[SerializeField] private WaypointScript waypoint;
	
	private static int id = 0;
	[SerializeField] private GameObject group;
	
	//Distances	
	public float distanceCovered; //Position en pourcentage, choisi par le LD sur un slider;
	private float totalPathDistance;
	private float[] distanceArray;
	private float[] percentArray;
	private int startPoint;

	[SerializeField] private bool showPercent = false;
	[SerializeField] private bool notForward = true;

	void Start(){
		if(group != null && wpsList.Count == 0){
			for(int i = 0; i < group.transform.childCount; i++){
				wpsList.Add(group.transform.GetChild(i).GetComponent<WaypointScript>());
			}
			hasWaypoints = true;
		}
	}

	void Update(){
		if(doesLoop){
			doesReverse = false;
		}
		if(hasWaypoints){
			if(isEditorMode){
				if(doesLoop){
					SetLoopPoints(wpsList);
					totalPathDistance = SetDistances(lWps);
				}
				else{			
					totalPathDistance = SetDistances(wpsList); 
				}
				percentArray = PercentOf(totalPathDistance);
				startPoint = FindWaypoint(distanceCovered);
				PositionOnPath(startPoint, distanceCovered, totalPathDistance);
			}
		}
	}
	
	//Instantiation
	/// <summary>
	/// Builds the waypoints in specified location.
	/// </summary>
	/// <param name="where">Where to place.</param>
	public void BuildWaypoints(Vector3 where){ 
		WaypointScript temp = Instantiate(waypoint, where, Quaternion.identity) as WaypointScript;
		wpsList.Add(temp);
		temp.Id = wpsList.Count-1;
		temp.name = "Waypoint" + id;
		id++;
	}

	/// <summary>
	/// Validates the waypoints.
	/// </summary>
	public void ValidateWaypoints(){
		group = new GameObject(this.name + " Waypoints");
		foreach(WaypointScript element in wpsList){
			if(!isPoney){
				Vector3 wpHeight = element.transform.position;
				wpHeight.y = this.transform.position.y;
				element.transform.position = wpHeight; //sets all waypoints to the pathfinding object's height.
			}
			element.Owners = new GameObject[1];
			element.Owners[0] = this.gameObject;
			element.transform.parent = group.transform;
		}

		hasWaypoints = true;
	}

	/// <summary>
	/// Erases all existing waypoints.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel waypoints; otherwise, <c>false</c>.</returns>
	public void CancelWaypoints(){
		for(int i = 0; i < wpsList.Count; i++){
			DestroyImmediate (wpsList[i].gameObject);
		}
		DestroyImmediate(group.gameObject);
		wpsList.Clear();
		lWps.Clear ();
		createWaypoints = false;
		hasWaypoints = false;
		//DestroyImmediate(group.gameObject);
		id = 0;
	}
	
	/// <summary>
	/// Arranges the path so that it will loop.
	/// </summary>
	/// <returns>Array for looping path</returns>
	/// <param name="waypoints">Waypoint array.</param>
	public List<WaypointScript> SetLoopPoints(List<WaypointScript> waypoints){
		lWps = new List<WaypointScript>(waypoints.Count+1);
		for (int i = 0; i < waypoints.Count+1; i++) lWps.Add(null);

		for(int i = 0; i < lWps.Count; i++){
			if(i == waypoints.Count){ 
				lWps[i] = waypoints[0]; //return to the start.
			} else lWps[i] = waypoints[i];
		}
		return lWps;
	}
	
	//Distance
	/// <summary>
	/// Makes array of distances between each waypoint and counts the total path distance.
	/// </summary>
	/// <returns>Total distance of the path.</returns>
	/// <param name="waypoints">Waypoints.</param>
	public float SetDistances(List<WaypointScript> waypoints){
		float total = 0f;
		distanceArray = new float[waypoints.Count];
		for (int i = 0; i <= waypoints.Count; i++){
			if(i == 0){
				distanceArray[i] = 0f;
			}
			else if (i < waypoints.Count){
				distanceArray[i] = Vector3.Distance(waypoints[i-1].transform.position, waypoints[i].transform.position);
				total += distanceArray[i];
			}
		}
		return total;
	}
	
	/// <summary>
	/// Calculates percent of path covered when each waypoint is reached.
	/// </summary>
	/// <returns>Array of percentages.</returns>
	/// <param name="total">Total.</param>
	public float[] PercentOf(float total){
		if(doesLoop){
			float[] addedArray = new float[lWps.Count];
			float[] distArray = new float[addedArray.Length]; //percentage of distance between waypoints
			for(int i = 0; i < addedArray.Length; i++){
				distArray[i] = distanceArray[i]/total; //converts to percentage
				if(i>0) {
					addedArray[i] = distArray[i] + addedArray [i-1]; //adds percentages to make total.
				}
			}
			return addedArray;
		} else{
			float[] addedArray = new float[wpsList.Count]; //percentage of distance from start to waypoint.
			float[] distArray = new float[addedArray.Length]; //percentage of distance between waypoints
			for(int i = 0; i < addedArray.Length; i++){
				distArray[i] = distanceArray[i]/total; //converts to percentage
				if(i>0) {
					addedArray[i] = distArray[i] + addedArray [i-1]; //adds percentages to make total.
				}
			}
			return addedArray;
		}
	}
	
	/// <summary>
	/// Interpolates the object's position along the path and shows it in scene view.
	/// </summary>
	/// <param name="startIndex">Starting waypoint index.</param>
	/// <param name="distCovered">Percentage of distance covered.</param>
	/// <param name="onTotal">On total distance.</param>
	public void PositionOnPath(int startIndex, float distCovered, float onTotal){
		if(doesLoop){
			if(startIndex != lWps.Count-1){
				float fromPointA = distCovered - percentArray[startIndex]; //find remaining distance to interpolate.
				fromPointA = onTotal * fromPointA; //convert from percentage to units
				Vector3 difference = new Vector3();
				
				difference = lWps[startIndex].transform.position - lWps[startIndex + 1].transform.position;
				difference =lWps[startIndex].transform.position - (difference.normalized * fromPointA); //find the exact position between 2 waypoints corresponding to the desired %
				this.transform.position = difference;
				if(!notForward){
					this.transform.forward = lWps[startIndex + 1].transform.position - lWps[startIndex].transform.position; //points the object toward the next waypoint.
				} else this.transform.rotation = Quaternion.identity;
				Debug.DrawLine(lWps[startIndex].transform.position, lWps[startIndex + 1].transform.position,Color.blue);
			}
			else this.transform.position = wpsList[startIndex].transform.position; //is at final waypoint, can no longer interpolate to next waypoint.	
		}else if(startIndex != wpsList.Count-1){
			float fromPointA = distCovered - percentArray[startIndex]; //find remaining distance to interpolate.
			fromPointA = onTotal * fromPointA; //convert from percentage to units
			Vector3 difference = new Vector3();
			
			difference = wpsList[startIndex].transform.position - wpsList[startIndex + 1].transform.position;
			difference =wpsList[startIndex].transform.position - (difference.normalized * fromPointA); //find the exact position between 2 waypoints corresponding to the desired %
			this.transform.position = difference;
			if(!notForward){
				this.transform.forward = wpsList[startIndex + 1].transform.position - wpsList[startIndex].transform.position; //points the object toward the next waypoint.
			} else this.transform.rotation = Quaternion.identity;
			Debug.DrawLine(wpsList[startIndex].transform.position, wpsList[startIndex + 1].transform.position,Color.blue);
		}
		else this.transform.position = wpsList[startIndex].transform.position; //is at final waypoint, can no longer interpolate to next waypoint.	
	}	
	
	/// <summary>
	/// Finds the waypoint that would be reached at the current distance.
	/// </summary>
	/// <returns>The waypoint index.</returns>
	/// <param name="desiredDistance">Desired distance.</param>
	public int FindWaypoint(float desiredDistance){
		int index = 0;
		for(int i = 0; i < percentArray.Length -1; i ++){
			if(percentArray[i] < desiredDistance) //if the waypoint's distance percentage is higher than the total, the previous waypoint is the starting point.
				index = i;	
			else break;
		}
		return index;
	}

	//Set le waypoint suivant et le courant
	public void WpSetNext(){
		if(doesLoop && !doesReverse){
			courant ++;
			next ++;
			if(next >= wpsList.Count) next = 0;
			if(courant >= wpsList.Count) courant = 0;
		}
		if(!doesLoop && !doesReverse){
			if(next == wpsList.Count-1){
				next = wpsList.Count-1;
				courant = wpsList.Count-1;
			} else {
				courant ++;
				next ++;
			}
		}
		if(doesReverse && !doesLoop){
			if(next == wpsList.Count-1) {
				wpsList.Reverse();
				isReversed = !isReversed;
				next = 0;
				courant = -1;
			}
			courant ++;
			next ++;
		}
	}

	//Accesseurs
	public bool CreateWaypoints{
		get {return createWaypoints;}
		set {createWaypoints = value;}
	}

	public bool HasWaypoints{
		get {return hasWaypoints;}
		set {hasWaypoints = value;}
	}

	public bool ShowPercent{
		get {return showPercent;}
		set {showPercent = value;}
	}

	public List<WaypointScript> WpsList {
		get {return wpsList;}
		set {wpsList = value;}
	}

	public bool DoesLoop {
		get {return doesLoop;}
		set {doesLoop = value;}
	}

	public bool IsEditorMode {
		get {return isEditorMode;}
		set {isEditorMode = value;}
	}

	public bool DoesReverse {
		get {return doesReverse;}
		set {doesReverse = value;}
	}

	public int Courant {
		get {return courant;}
		set {courant = value;}
	}

	public int Next {
		get {return next;}
		set {next = value;}
	}

	public bool IsReversed {
		get {return isReversed;}
		set {isReversed = value;}
	}

	public WaypointScript WpStart {
		get {return wpStart;}
		set {wpStart = value;}
	}

	public GameObject Group {
		get {return group;}
	}
}
