using UnityEngine;
using System.Collections;

[System.Serializable]
public class MovableRotation  {

	public bool isRotating;
	public bool rotateHalf;
	public bool rotateFull;
	public float seconds;


	public MovableRotation(bool isRotating, float seconds){
		this.isRotating = isRotating;
		this.seconds = seconds;
	}

	/*public bool IsRotating {
		get {return isRotating;}
		set {isRotating = value;}
	}

	public float Seconds {
		get {return seconds;}
		set {seconds = value;}
	}*/
}
