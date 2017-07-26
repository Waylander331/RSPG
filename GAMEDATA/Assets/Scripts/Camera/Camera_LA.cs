using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Camera_LA : MonoBehaviour {

	private DepthOfField myDoF;
	private Vortex myVortex;
	private Camera myCam;
	private Camera myDaddyCam;
	void Start () {
		if (this.GetComponent<DepthOfField> () != null) {
			myDoF = this.GetComponent<DepthOfField> ();
		}
		myCam = this.GetComponent<Camera> ();
		myDaddyCam =  Manager.Instance.MainCam.GetComponent<Camera> ();
		myVortex = this.GetComponent<Vortex> ();
		this.transform.parent = Manager.Instance.MainCam.transform;
		this.transform.localPosition = Vector3.zero;
		this.transform.localRotation = Quaternion.Euler (Vector3.zero);
		myVortex.center = new Vector2 (0.6f,0.6f);
		myVortex.angle = 30f;
		myDaddyCam.cullingMask = 0;
		myCam.depth = -1f;
	}
	

	void Update () {
		myCam.fieldOfView = myDaddyCam.fieldOfView;
		myVortex.enabled = Manager.Instance.Avatar.IsDazed;
		if(myDoF != null){
			myDoF.focalLength = Vector3.Distance (this.transform.position, Manager.Instance.Avatar.transform.position);
		}
	}
}
