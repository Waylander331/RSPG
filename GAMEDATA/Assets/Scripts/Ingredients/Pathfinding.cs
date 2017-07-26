/*using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Pathfinding : MonoBehaviour {
	
	//Pathfinding
	private bool hasWaypoints;
	private WaypointScript[] wps;
	private WaypointScript[] finalWps;
	private WaypointScript goTo;
	private Queue<WaypointScript> loop = new Queue<WaypointScript>();
	private Stack<WaypointScript> reverse = new Stack<WaypointScript>();
	public bool doesLoop = false;
	[HideInInspector]
	public bool doesReverse = false;

	//Instanciation manuelle
	 bool createWaypoints;
	 bool fixedAmount;
	 int amount;
	//[HideInInspector]
	public WaypointScript waypoint;
	private Queue<WaypointScript> path = new Queue<WaypointScript>();

	private static int id = 0;
	[SerializeField][HideInInspector]
	private int ownerID = 0;
	[SerializeField][HideInInspector]
	private bool isSet = false;
	[SerializeField][HideInInspector]
	private int wpSize;
	
	//Distances	
	[Range (0f,1f)]
	public float distanceCovered; //Position en pourcentage, choisi par le LD sur un slider;
	private float totalPathDistance;
	private float[] distanceArray;
	private float[] percentArray;
	private int startPoint;
	public bool showPercent = false;

	void Start(){
		if(!isSet){	//if id has not been set, set it.	
			ownerID = id;
			id ++;
			isSet = true;
		}
	}
	
	void Update(){
		if(hasWaypoints){
			if(doesLoop){
				finalWps = SetLoopPoints(wps);
				doesReverse = false;
				wpSize = finalWps.Length;
			}
			else {
				finalWps = wps;
				wpSize = finalWps.Length;
			}


			totalPathDistance = SetDistances(finalWps); 
			percentArray = PercentOf(totalPathDistance);
			startPoint = FindWaypoint(distanceCovered);
			PositionOnPath(startPoint, distanceCovered, totalPathDistance);
		}
	}

	void OnDestroy(){
		if(hasWaypoints){
			for(int i = 0; i<finalWps.Length; i++){
			DestroyImmediate(finalWps[i]); //when you delete the pathfinding object, you also delete its waypoints.
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
		path.Enqueue(temp);
		temp.Id = path.Count;
		temp.name = "Waypoint";
	}
	/// <summary>
	/// Validates the waypoints.
	/// </summary>
	public void ValidateWaypoints(){
		
		foreach(WaypointScript element in path){

			Vector3 wpHeight = element.transform.position;
			wpHeight.y = this.transform.position.y;
			element.transform.position = wpHeight; //sets all waypoints to the pathfinding object's height.
			element.Owner = ownerID;
		}
		wps = QueueToTab(path); 
		hasWaypoints = true;
	}

	public void RefreshWaypoints(){
		
		WaypointScript[] temp1 = GameObject.FindObjectsOfType<WaypointScript>() as WaypointScript[];
		wps = new WaypointScript[wpSize-1];
		for(int i =0; i < temp1.Length; i++){
			//WaypointScript foo = temp1[i].GetComponent<WaypointScript>(); 
			if (temp1[i].Owner == ownerID) { //makes sure to attach the waypoint to the correct object.
				wps[temp1[i].Id-1] = temp1[i]; //ids run 1 and up, in order of path.
			}
		}
		Debug.Log ("I'm here!!!");
		hasWaypoints = true;
	}

	/// <summary>
	/// Determines which type of structure to cancel.
	/// </summary>
	/// <param name="whatToDelete">Delete "queue" or "array"</param>
	public void CancelWaypoints(string whatToDelete){ //parent overload, because cannot access queue from editor script.
		whatToDelete.ToLower();
		if(whatToDelete == "queue") CancelWaypoints(path);
		else CancelWaypoints();
	}
	/// <summary>
	/// Cancels all previously created waypoints.
	/// </summary>
	/// <param name="queue">Queue.</param>
	public void CancelWaypoints(Queue<WaypointScript> queue){
		int j = path.Count;
		for(int i = 0; i < j; i++){
			WaypointScript temp = path.Dequeue();
			DestroyImmediate(temp.gameObject); 
		}
		createWaypoints = false;
		hasWaypoints = false;
	}
	/// <summary>
	/// Erases all existing waypoints.
	/// </summary>
	/// <returns><c>true</c> if this instance cancel waypoints; otherwise, <c>false</c>.</returns>
	public void CancelWaypoints(){
		for(int i = 0; i < finalWps.Length-1; i++){
			DestroyImmediate (finalWps[i].gameObject);
		}
		createWaypoints = false;
		hasWaypoints = false;
	}
	
	/// <summary>
	/// Arranges the path so that it will loop.
	/// </summary>
	/// <returns>Array for looping path</returns>
	/// <param name="waypoints">Waypoint array.</param>
	public WaypointScript[] SetLoopPoints(WaypointScript[] waypoints){
		WaypointScript[] lWps = new WaypointScript[waypoints.Length + 1];
		for(int i = 0; i <= waypoints.Length; i++){
			if(i == waypoints.Length) 
				lWps[i] = waypoints[0]; //return to the start.
			else lWps[i] = waypoints[i];
		}
		return lWps;
	}
	
//Distance
	/// <summary>
	/// Makes array of distances between each waypoint and counts the total path distance.
	/// </summary>
	/// <returns>Total distance of the path.</returns>
	/// <param name="waypoints">Waypoints.</param>
	public float SetDistances(WaypointScript[] waypoints){
		float total = 0f;
		distanceArray = new float[waypoints.Length];
		for (int i = 0; i<=waypoints.Length; i++){
			if(i == 0) distanceArray[i] = 0f;
			else if (i < (waypoints.Length)){
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
		float[] addedArray = new float[finalWps.Length]; //percentage of distance from start to waypoint.
		float[] distArray = new float[addedArray.Length]; //percentage of distance between waypoints
		for(int i = 0; i < addedArray.Length; i++){
			distArray[i] = distanceArray[i]/total; //converts to percentage
			if(i>0) {
				addedArray[i] = distArray[i] + addedArray [i-1]; //adds percentages to make total.
			}
		}
		return addedArray;
	}

	/// <summary>
	/// Interpolates the object's position along the path and shows it in scene view.
	/// </summary>
	/// <param name="startIndex">Starting waypoint index.</param>
	/// <param name="distCovered">Percentage of distance covered.</param>
	/// <param name="onTotal">On total distance.</param>
	public void PositionOnPath(int startIndex, float distCovered, float onTotal){
		if(startIndex != finalWps.Length-1){
			float fromPointA = distCovered - percentArray[startIndex]; //find remaining distance to interpolate.
			fromPointA = onTotal * fromPointA; //convert from percentage to units
			Vector3 difference = new Vector3();
			
			difference = finalWps[startIndex].transform.position - finalWps[startIndex + 1].transform.position;
			difference =finalWps[startIndex].transform.position - (difference.normalized * fromPointA); //find the exact position between 2 waypoints corresponding to the desired %
			this.transform.position = difference;
			this.transform.forward = finalWps[startIndex].transform.position - finalWps[startIndex + 1].transform.position; //points the object toward the next waypoint.
			Debug.DrawLine(finalWps[startIndex].transform.position, finalWps[startIndex + 1].transform.position);
		}
		else this.transform.position = finalWps[startIndex].transform.position; //is at final waypoint, can no longer interpolate to next waypoint.	
	}	

	/// <summary>
	/// Finds the waypoint that would be reached at the currecnt distance.
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
	
//Gestion des Arrays
	/// <summary>
	/// Converts stack to Array
	/// </summary>
	/// <returns>An array of Game Objects.</returns>
	/// <param name="stack">Stack of Game Objects.</param>
	public WaypointScript[] StackToTab(Stack<WaypointScript> stack){
		WaypointScript[] newT = new WaypointScript[stack.Count -1];
		for(int i = 0; i<newT.Length; i++)
			newT[i] = stack.Pop();		
		return newT;
	}
	/// <summary>
	/// Adds a stack to the end of an array.
	/// </summary>
	/// <returns>A new Array of game objects.</returns>
	/// <param name="array">Array of game objects.</param>
	public WaypointScript[] StackOnTab(WaypointScript[] array){
		Stack<WaypointScript> newS = new Stack<WaypointScript>();
		newS = TabToStack(array);
		WaypointScript[] newT = new WaypointScript[(newS.Count -1) + (array.Length -1)];		
		for(int i = 0; i < newT.Length; i++){
			if(array[i] == null){
				if(array[i-1] == newS.Peek()) newS.Pop (); //Takes out repeat of last waypoint.
				array[i] = newS.Pop ();
			}
		}		
		return newT;
	}
	/// <summary>
	/// Converts Array to Stack.
	/// </summary>
	/// <returns>A stack of game objects</returns>
	/// <param name="array">Array of game objects</param>
	public Stack<WaypointScript> TabToStack(WaypointScript[] array){
		Stack<WaypointScript> NewS = new Stack<WaypointScript>();
		foreach(WaypointScript element in array){
			NewS.Push(element);
		}
		return NewS;
	}
	/// <summary>
	/// Converts a queue to an array
	/// </summary>
	/// <returns>An array of game objects</returns>
	/// <param name="queue">Queue of game objects.</param>
	public WaypointScript[] QueueToTab(Queue<WaypointScript> queue){
		WaypointScript[] newT = new WaypointScript[queue.Count];
		for(int i = 0; i<newT.Length; i++){
			newT[i] = queue.Dequeue();
		}
		return newT;		
	}
	/// <summary>
	/// Converts an array to a queue.
	/// </summary>
	/// <returns>A queue of game objects.</returns>
	/// <param name="array">Array of game objects.</param>
	public Queue<WaypointScript> TabToQueue(WaypointScript[] array){
		Queue<WaypointScript> newQ = new Queue<WaypointScript>();
		int j = array.Length;
		for(int i = 0; i<j; i++){
			newQ.Enqueue(array[i]);
		}
		return newQ;
	}

//Accesseurs
	public bool CreateWaypoints{
		get {return createWaypoints;}
		set {createWaypoints = value;}
	}
	public bool HasWaypoints{
		get {return hasWaypoints;}
	}
	public bool ShowPercent{
		get{return showPercent;}
	}
	public WaypointScript[] FinalWaypoints{
		get{return finalWps;}
	}
	public int OwnerID{
		get {return ownerID;}
	}
}*/
