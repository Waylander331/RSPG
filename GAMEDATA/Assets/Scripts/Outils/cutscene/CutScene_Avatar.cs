using UnityEngine;
using System.Collections;

public class CutScene_Avatar : MonoBehaviour {

//	public bool isIdle;
//	public bool isWalking;
//	public bool isRunning;
//	public bool isSprinting;
//	public bool isJumping;
//	public bool isKnockBack;
//	public bool isSpawning;

	public enum enum1 {idle = 0, walkcycle = 1, runcycle = 2, sprintcycle = 3, jumpCycle = 4, slideCycle = 5, knockBack = 6, idleloading = 7};	
	
	public enum1 animationId;

	private enum1 lastAnimationId;
	private Animator myAnimator;

	private bool imDone;
	private Transform myKF;
	private Transform myLastKF;
	private CutSceneMaker myMaker;
	private float waitTime;

	void Start () {
		myAnimator = GetComponent<Animator> ();
		lastAnimationId = animationId;
		myKF = myMaker.AvatarKFList [0].transform;
		this.transform.position = myKF.position;
		this.transform.rotation = myKF.rotation;
	}
	

	void Update () {
		
		if (!imDone) {
			if (myKF != null) {
				waitTime -= 1f * Time.deltaTime;
				if (waitTime < 0f) {
					waitTime = 0f;
				}
				
				
				if (waitTime == 0f) {
		
					switch (animationId) {
					case enum1.walkcycle:
						this.transform.position = Vector3.MoveTowards (this.transform.position, myKF.position, 4f * Time.deltaTime);
						break;
					case enum1.runcycle:
						this.transform.position = Vector3.MoveTowards (this.transform.position, myKF.position, 7f * Time.deltaTime);
						break;
					case enum1.sprintcycle:
						this.transform.position = Vector3.MoveTowards (this.transform.position, myKF.position, 10f * Time.deltaTime);
						break;
					case enum1.jumpCycle:
						this.transform.position = Vector3.MoveTowards (this.transform.position, myKF.position, 10f * Time.deltaTime);
						break;
					default:
						this.transform.position = Vector3.MoveTowards (this.transform.position, myKF.position, 5f * Time.deltaTime);
						break;
					}

				}
				
				if((myKF.transform.position - this.transform.position).normalized != Vector3.zero){
					this.transform.rotation = Quaternion.Lerp (this.transform.rotation, Quaternion.LookRotation ((myKF.transform.position - this.transform.position).normalized), 10f * Time.deltaTime);
					Vector3 tempV = new Vector3 (0f, this.transform.eulerAngles.y, 0f);
					this.transform.eulerAngles = tempV;
				}
				

			}
			
			if (Mathfx.Approx (this.transform.position, myKF.position, 0.01f) && waitTime == 0) {
				
				if (myMaker.AvatarKFList.Count - 1 > myMaker.AvatarKFList.IndexOf (myKF.gameObject.GetComponent<CutScene_Keyframe> ())) {			
					myLastKF = myKF;
					myKF = myMaker.AvatarKFList [myMaker.AvatarKFList.IndexOf (myKF.gameObject.GetComponent<CutScene_Keyframe> ()) + 1].transform;
					waitTime = myLastKF.gameObject.GetComponent<CutScene_Keyframe> ().waitTime;
					animationId = (enum1)myLastKF.gameObject.GetComponent<CutScene_Keyframe> ().animationId;
				} else {
					imDone = true;
				}
				
			}
			
		} else {
			animationId = (enum1)myKF.gameObject.GetComponent<CutScene_Keyframe> ().animationId;
		}

		if (animationId != lastAnimationId) {

			myAnimator.CrossFade(animationId.ToString(),0.05f);
			//Debug.Log(animationId.ToString());
		}





		lastAnimationId = animationId;
	}

	public void SetRunning(){

	}

	public Transform MyKF{
		get{return myKF;}
		set{myKF = value;}
	}

	public enum1 MyAnimId{
		get{return animationId;}
		set{animationId = value;}
	}

	public enum1 MyLastAnimId{
		get{return lastAnimationId;}
		set{lastAnimationId = value;}
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
