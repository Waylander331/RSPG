using UnityEngine;
using System.Collections;

public class BMGunSnap : MonoBehaviour {

	private Transform gunHandle;
	private Transform gunSnapPosition;

	void Awake(){
		gunHandle = transform.GetChild (3).GetChild (2);
		gunSnapPosition = transform.GetChild (0).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (0).GetChild (0);
	}

	// Use this for initialization
	void Start () {
		gunHandle.position = gunSnapPosition.position;
	}
	
	// Update is called once per frame
	void Update () {
		gunHandle.position = gunSnapPosition.position;
	}
}
