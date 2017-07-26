using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;
[ExecuteInEditMode]
public class CutScene_Camera : MonoBehaviour {

	private DepthOfField myDoF;
	private RaycastHit focalHit;
	private bool imDone;
	private Transform myKF;
	private Transform myLastKF;
	private CutSceneMaker myMaker;
	private float waitTime;

	void Start () {
		myDoF = this.GetComponent<DepthOfField> ();
		myKF = myMaker.CamKFList [0].transform;
		this.transform.position = myKF.position;
		this.transform.rotation = myKF.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		if(!imDone){
			if (myKF != null) {
				waitTime -= 1f*Time.deltaTime;
				if(waitTime<0f){
					waitTime=0f;
				}


				if(waitTime == 0f){
					this.transform.position = Vector3.MoveTowards(this.transform.position, myKF.position,2f * Time.deltaTime);
				}

				if(myLastKF != default(Transform)){
					if(Vector3.Distance(myKF.position,myLastKF.position)>1f){
						this.transform.rotation = Quaternion.Lerp (myLastKF.rotation, myKF.rotation,1f- (Vector3.Distance (transform.position, myKF.position) / Vector3.Distance (myLastKF.position, myKF.position)));
					}else{
						this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.Euler (myKF.rotation.eulerAngles),2f*Time.deltaTime);
					}
				}else{
					this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.Euler (myKF.rotation.eulerAngles),2f*Time.deltaTime);
				}
			}
			
			if(Mathfx.Approx(this.transform.position,myKF.position,0.01f) && Mathfx.Approx(this.transform.rotation.eulerAngles,myKF.rotation.eulerAngles,1f) && waitTime ==0f){

				if( myMaker.CamKFList.Count-1 > myMaker.CamKFList.IndexOf(myKF.gameObject.GetComponent<CutScene_Keyframe>())){			
					myLastKF = myKF;
					myKF = myMaker.CamKFList [myMaker.CamKFList.IndexOf(myKF.gameObject.GetComponent<CutScene_Keyframe>())+1].transform;
					waitTime = myLastKF.gameObject.GetComponent<CutScene_Keyframe>().waitTime;
				}
				else{
					imDone = true;
				}

			}

			if (Physics.Raycast (this.transform.position, this.transform.forward, out focalHit, 10f)) {
				myDoF.focalLength = Mathf.Lerp(myDoF.focalLength, focalHit.distance,2f*Time.deltaTime);
				myDoF.focalSize = Mathf.Lerp(myDoF.focalSize, 0.5f,2f*Time.deltaTime);

			} else {
				myDoF.focalLength = Mathf.Lerp(myDoF.focalLength,10f,2f*Time.deltaTime);
				myDoF.focalSize = Mathf.Lerp(myDoF.focalSize,10f,2f*Time.deltaTime);
			}
		}

	}
	public Transform MyKF{
		get{return myKF;}
		set{myKF = value;}
	}

	public bool ImDone{
		get{return imDone;}
		set{imDone = value;}
	}

	public CutSceneMaker MyMaker{
		get{return myMaker;}
		set{myMaker = value;}
	}
}
