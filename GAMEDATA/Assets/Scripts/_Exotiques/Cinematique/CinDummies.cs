using UnityEngine;
using System.Collections;

public class CinDummies : MonoBehaviour {

	private CinCamera cam;

	void CamFix(){
		cam.Fade();
	}

	public CinCamera Cam{
		get {return cam;}
		set {cam = value;}
	}
}
