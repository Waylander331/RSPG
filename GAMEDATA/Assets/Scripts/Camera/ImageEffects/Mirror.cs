using UnityEngine;
using System.Collections;

public class Mirror : MonoBehaviour {
	public GameObject myCam;
	private Camera myCamCam;
	private GameObject myAvatar;
	private Vector3 pos;
	private bool imActive;

	// Use this for initialization
	void Start () {
		myCamCam = myCam.GetComponent<Camera> ();
		myAvatar = Manager.Instance.Avatar.gameObject;
		myCamCam.fieldOfView = 60f * Mathfx.HighestAbsValue(this.transform.localScale);
	}
	
	// Update is called once per frame
	void Update () {

		imActive = false;

		if (Vector3.Distance (myAvatar.transform.position, this.transform.position) < 15f) {

			float tempY = Quaternion.LookRotation((myAvatar.transform.position - this.transform.position).normalized).eulerAngles.y;

			if (Mathf.Abs( Mathf.DeltaAngle (this.transform.eulerAngles.y,tempY))<=90f) {

				imActive = true;

			}
		}

		if (imActive) {
			myCam.SetActive(true);
		
			pos = this.transform.position - Camera.main.transform.position;
			myCam.transform.position = this.transform.position + Vector3.Reflect (pos, this.transform.right);
			myCam.transform.position = new Vector3(myCam.transform.position.x,Camera.main.transform.position.y,myCam.transform.position.z);
			myCam.transform.LookAt (this.transform.position);
		} else {

			myCam.SetActive(false);
		
		}
	}
}
