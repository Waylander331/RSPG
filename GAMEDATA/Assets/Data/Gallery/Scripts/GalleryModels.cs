using UnityEngine;
using System.Collections.Generic;

public class GalleryModels : MonoBehaviour {

	public GalleryManager galleryMan;

	public GameObject[] models;

	private Transform myFocus;
	private Vector3 positionStartMyFocus;
	private Quaternion rotationStartFocus;

	private float camDistanceMax = 6f;
	private float defaultDistanceMax;

	private bool isMoving = false;
	private Vector3 myDirection;
	private Vector3 myInput;
	private LayerMask myLayer;
	private Vector3 myFocusY;

	private bool isZooming = false;
	private bool haveChoose = false;

	private Vector3 positionStart = new Vector3(100,100,100);
	private Vector3 positionFocus = Vector3.zero;

	// Use this for initialization
	void Start () {	
		for(int i = 12; i < galleryMan.MainButtonModelsConcepts.Length; i++){
			GameObject temp = Instantiate(models[i-12], positionStart, models[i-12].transform.rotation) as GameObject;
			galleryMan.MainButtonModelsConcepts[i] = temp;
		}
		//myLayer = UnityEngine.LayerMask.GetMask ("Collision");
		defaultDistanceMax = camDistanceMax;
		this.transform.position = new Vector3(camDistanceMax,2,0);
		positionStartMyFocus = this.transform.position;
		rotationStartFocus = this.transform.rotation;
	}

	void LateUpdate(){
		if(haveChoose){
			myFocusY = Vector3.Lerp (myFocusY, myFocus.transform.position, 10f * (Time.deltaTime*(1/Time.timeScale)));

			myDirection = (this.transform.position - myFocusY).normalized;

			//Move around focus
			myInput.x = Input.GetAxis ("TriggerR") * 1f;
			//myInput.y = Input.GetAxis ("Vertical") * 1f;				
			
			myDirection += (this.transform.right * myInput.x);
			if(!myFocus.parent.name.Contains("Diorama")){
				this.transform.position = Vector3.Lerp (this.transform.position, myFocusY + myDirection * camDistanceMax, 5f * (Time.deltaTime*(1/Time.timeScale)));
			} else this.transform.position = Vector3.Lerp (this.transform.position, myFocusY + myDirection * (camDistanceMax*3), 5f * (Time.deltaTime*(1/Time.timeScale)));

			//Zoom
			if (Input.GetAxis ("VerticalR") > 0f) {
				camDistanceMax = 2.5f;
				isZooming = true;
			}
			if (Input.GetAxis ("VerticalR") < 0f) {
				camDistanceMax = defaultDistanceMax;
				isZooming = false;
			}				

			this.transform.LookAt (myFocusY);
		}
	}

	public void InstanceCreate(GameObject a){
		a.transform.position = positionFocus;
		myFocus = a.transform;
	}

	public void Destroyer(GameObject[] a){
		for(int i = 12; i < galleryMan.MainButtonModelsConcepts.Length; i++){
			a[i].transform.position = positionStart;
		}
	}


	//Accesseurs
	public bool IsZooming {
		get {return isZooming;}
		set {isZooming = value;}
	}

	public float CamDistanceMax {
		get {return camDistanceMax;}
		set {camDistanceMax = value;}
	}

	public bool HaveChoose {
		get {return haveChoose;}
		set {haveChoose = value;}
	}

	public Transform MyFocus {
		get {return myFocus;}
		set {myFocus = value;}
	}

	public Vector3 PositionStartMyFocus {
		get {return positionStartMyFocus;}
		set {positionStartMyFocus = value;}
	}

	public float DefaultDistanceMax {
		get {
			return defaultDistanceMax;
		}
		set {
			defaultDistanceMax = value;
		}
	}

	public Quaternion RotationStartFocus {
		get {
			return rotationStartFocus;
		}
		set {
			rotationStartFocus = value;
		}
	}
}
