using UnityEngine;
using System.Collections;

public class CutScene_Keyframe : MonoBehaviour {
	public float waitTime;

	public enum enum1 {idle = 0, walkcycle = 1, runcycle = 2, sprintcycle = 3, jumpCycle = 4, slideCycle = 5, knockBack = 6, idleloading = 7};	
	public enum1 animationId;

	public enum enum2 {Camera = 0, Avatar = 1};	
	[HideInInspector]
	[SerializeField]private enum2 keyframeType;
	[HideInInspector]
	[SerializeField]private CutSceneMaker myMaker;

	void Start () {
	
	}
	

	void Update () {
	
	}

	void OnDestroy() {
		if (keyframeType == enum2.Camera) {
			//myMaker.ReIDCamList (myMaker.CamKFList.IndexOf (this));
		}
		if (keyframeType == enum2.Avatar) {
			//myMaker.ReIDAvList (myMaker.AvatarKFList.IndexOf (this));
		}
	}

	public CutSceneMaker MyMaker{
		get{return myMaker;}
		set{myMaker = value;}
	}

	public enum2 KeyframeType{
		get{return keyframeType;}
		set{keyframeType = value;}
	}

	void OnDrawGizmos(){
		if (keyframeType == enum2.Camera) {
			DrawShape.DrawArrowForGizmo (transform.position, transform.forward, Color.yellow, 0.40f);
		}
		Gizmos.color = Color.yellow;
		Gizmos.color = new Color (Color.yellow.r, Color.yellow.g, Color.yellow.b, 0.5f);
		Gizmos.DrawWireCube(transform.position, new Vector3(0.2f,0.2f,0.2f));

		if (keyframeType == enum2.Camera) {
			if( myMaker.CamKFList.Count-1 > myMaker.CamKFList.IndexOf(this)){		                                                                                          
				//DrawShape.DrawArrowForGizmo (this.transform.position,(myMaker.CamKFList[myMaker.CamKFList.IndexOf(this)+1].transform.position - this.transform.position), Color.yellow,0.40f);

			//	DrawShape.DrawArrowForGizmo (this.transform.position,(myMaker.CamKFList[myMaker.CamKFList.IndexOf(this)+1].transform.position - this.transform.position).normalized * (Vector3.Distance(this.transform.position,myMaker.CamKFList[myMaker.CamKFList.IndexOf(this)+1].transform.position)/2), Color.yellow,0.40f);
				Gizmos.DrawLine(this.transform.position,myMaker.CamKFList[myMaker.CamKFList.IndexOf(this)+1].transform.position);
			}
		}
		if (keyframeType == enum2.Avatar) {
			if( myMaker.AvatarKFList.Count-1 > myMaker.AvatarKFList.IndexOf(this)){			
				//DrawShape.DrawArrowForGizmo (this.transform.position,(myMaker.AvatarKFList[myMaker.AvatarKFList.IndexOf(this)+1].transform.position - this.transform.position).normalized * (Vector3.Distance(this.transform.position,myMaker.AvatarKFList[myMaker.AvatarKFList.IndexOf(this)+1].transform.position)/2), Color.yellow,0.40f);
				Gizmos.DrawLine(this.transform.position,myMaker.AvatarKFList[myMaker.AvatarKFList.IndexOf(this)+1].transform.position);
			}
		}
	}
}
