using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class CutSceneMaker : MonoBehaviour, ITriggerable {

	[SerializeField] private bool playOnStart;
	[SerializeField] private bool avatarSpawnAtEnd;
	[SerializeField] private bool actorSpawnAtStart;
	[SerializeField] private int camPosition;
	[SerializeField] private int avatarPosition;
	private int lastCamPosition;
	private int lastAvatarPosition;
	private bool isActive;
	public GameObject myAvatar;
	public GameObject myCam;
	public CutScene_Avatar myAvatarScr;
	public CutScene_Camera myCamScr;

	[SerializeField] private CutScene_Keyframe myKF;
	[SerializeField] private List<CutScene_Keyframe> avatarKFList;
	[SerializeField] private List<CutScene_Keyframe> camKFList;

	[SerializeField] private GameObject camGroup;
	[SerializeField] private GameObject avatarGroup;

	void Awake(){
		myCam.GetComponent<CutScene_Camera> ().MyMaker = this;
		myAvatar.GetComponent<CutScene_Avatar> ().MyMaker = this;
		myCamScr = myCam.GetComponent<CutScene_Camera> ();
		myAvatarScr = myAvatar.GetComponent<CutScene_Avatar> ();
		UpdateKeyframe ();
	}

	void Start () {

		if (Application.isPlaying) {
			myAvatar.SetActive (false);
			myCam.SetActive (false);
			if (playOnStart) {
				activeCutScene ();
			}
		} else {
			myAvatar.SetActive (true);
			myCam.SetActive (true);
		}


	}
	

	void Update () {
	
		if(myAvatarScr.ImDone && myCamScr.ImDone){
			desactiveCutScene();
		}
		if(isActive){
			isActive = true;
			myAvatar.SetActive (true);
			myCam.SetActive (true);
			Manager.Instance.Avatar.OnPause = true;
			Manager.Instance.Avatar.GetComponent<Animator> ().speed = 0f;
			Manager.Instance.Avatar.Disparition();

		}
		if (camKFList.Count != 0 && camPosition != lastCamPosition) {
			myCam.transform.position = camKFList[camPosition].transform.position;
			myCam.transform.rotation = camKFList[camPosition].transform.rotation;
		}
		if (avatarKFList.Count != 0 && avatarPosition != lastAvatarPosition) {
			myAvatar.transform.position = avatarKFList[avatarPosition].transform.position;
			myAvatar.transform.rotation = avatarKFList[avatarPosition].transform.rotation;
		}

		lastCamPosition = camPosition;
		lastAvatarPosition = avatarPosition;

	}
	public void UpdateKeyframe(){
	
		avatarGroup = transform.Find("Avatar_KF").gameObject;
		camGroup = transform.Find("Cam_KF").gameObject;
		avatarKFList.Clear ();
		camKFList.Clear ();
		foreach (CutScene_Keyframe kf in avatarGroup.GetComponentsInChildren<CutScene_Keyframe>()) {
			avatarKFList.Add (kf);
			kf.name = "Avatar_Keyframe_" + avatarKFList.IndexOf (kf);
			kf.MyMaker = this;
		}
		foreach (CutScene_Keyframe kf in camGroup.GetComponentsInChildren<CutScene_Keyframe>()) {
			camKFList.Add (kf);
			kf.name = "Camera_Keyframe_" + camKFList.IndexOf (kf);
			kf.MyMaker = this;
		}
	}

//	public void CreateAvKF(){
//		
//		if(avatarKFList.Count == 0){
//			avatarGroup = new GameObject("Avatar_KF");
//			avatarGroup.transform.SetParent (this.transform);
//		}
//		
//		CutScene_Keyframe tempKF;
//		tempKF = Instantiate (myKF, myAvatar.transform.position, myAvatar.transform.rotation) as CutScene_Keyframe;
//		avatarKFList.Add (tempKF);
//		tempKF.name = "Avatar_Keyframe_" + avatarKFList.IndexOf (tempKF);
//		tempKF.transform.SetParent (avatarGroup.transform);
//		tempKF.MyMaker = this;
//		tempKF.KeyframeType = CutScene_Keyframe.enum2.Avatar;
//	}
//	
//	public void ClearAvKF(){
//		
//		DestroyImmediate (avatarGroup);
//		avatarKFList.Clear ();
//	}
//
//
//	public void CreateCamKF(){
//
//		if(camKFList.Count == 0){
//			camGroup = new GameObject("Cam_KF");
//			camGroup.transform.SetParent (this.transform);
//		}
//
//		CutScene_Keyframe tempKF;
//		tempKF = Instantiate (myKF, myCam.transform.position, myCam.transform.rotation) as CutScene_Keyframe;
//		camKFList.Add (tempKF);
//		tempKF.name = "Cam_Keyframe_" + camKFList.IndexOf (tempKF);
//		tempKF.transform.SetParent (camGroup.transform);
//		tempKF.MyMaker = this;
//		tempKF.KeyframeType = CutScene_Keyframe.enum2.Camera;
//	}
//
//	public void ClearCamKF(){
//
//		DestroyImmediate (camGroup);
//		camKFList.Clear ();
//	}
//
//	public void ReIDCamList(int i){
//
//		camKFList.RemoveAt(i);
//		foreach (CutScene_Keyframe obj in camKFList) {
//			obj.name = "Cam_Keyframe_" + camKFList.IndexOf (obj);
//		}
//	}
//	public void ReIDAvList(int i){
//		
//		avatarKFList.RemoveAt(i);
//		foreach (CutScene_Keyframe obj in avatarKFList) {
//			obj.name = "Avatar_Keyframe_" + avatarKFList.IndexOf (obj);
//		}
//	}

	public void Triggered(EffectList effect){
		if(effect.GetType() == typeof(Effect_Default)){
			activeCutScene();

		}

	}
	public void UnTriggered(EffectList effect){
		if(effect.GetType() == typeof(Effect_Default)){
			desactiveCutScene();
		}

	}

	public void activeCutScene(){
		if(actorSpawnAtStart){
			if (camKFList.Count != 0) {
				camKFList[0].transform.position = Manager.Instance.MainCam.gameObject.transform.position;
				camKFList[0].transform.rotation = Manager.Instance.MainCam.gameObject.transform.rotation;
				myCam.transform.position = camKFList[0].transform.position;
				myCam.transform.rotation = camKFList[0].transform.rotation;
			}
			if (avatarKFList.Count != 0) {
				avatarKFList[0].transform.position = Manager.Instance.Avatar.gameObject.transform.position;
				avatarKFList[0].transform.rotation = Manager.Instance.Avatar.gameObject.transform.rotation;
				myAvatar.transform.position = avatarKFList[0].transform.position;
				myAvatar.transform.rotation = avatarKFList[0].transform.rotation;
			}


			
		}
		isActive = true;
		myAvatar.SetActive (true);
		myCam.SetActive (true);
		Manager.Instance.Avatar.OnPause = true;
		Manager.Instance.Avatar.GetComponent<Animator> ().speed = 0f;
		Manager.Instance.Avatar.Disparition();
	
	}

	public void desactiveCutScene(){
		isActive = false;
		myAvatarScr.ImDone = false;
		myCamScr.ImDone = false;
		Manager.Instance.Avatar.OnPause = false;
		Manager.Instance.Avatar.GetComponent<Animator> ().speed = 1f;
		Manager.Instance.Avatar.Apparition();


		if(avatarSpawnAtEnd){
			Manager.Instance.Avatar.gameObject.transform.position = myAvatar.transform.position;
			Manager.Instance.Avatar.gameObject.transform.rotation = myAvatar.transform.rotation;
			Manager.Instance.MainCam.gameObject.transform.position = myCam.transform.position;
			Manager.Instance.MainCam.gameObject.transform.rotation = myCam.transform.rotation;
		}

		myAvatar.SetActive (false);
		myCam.SetActive (false);

	
		myAvatarScr.MyKF = avatarKFList[0].transform;
		myCamScr.MyKF = camKFList[0].transform;

		myAvatarScr.MyAnimId = (CutScene_Avatar.enum1)avatarKFList [0].animationId;
		myAvatarScr.MyLastAnimId = CutScene_Avatar.enum1.idle;
		myAvatar.transform.position = myAvatarScr.MyKF.position;
		myAvatar.transform.rotation = myAvatarScr.MyKF.rotation;
		myCam.transform.position = myCamScr.MyKF.position;
		myCam.transform.rotation = myCamScr.MyKF.rotation;

		if(transform.parent != null)
		{
			ElevatorAnimationController elevator = transform.parent.GetComponent<ElevatorAnimationController>();
			if(elevator != null)
			{
				elevator.InScriptedEvent = false;
				elevator.UpdateInElevatorForAvatar();
			}
		}
	}

	public List<CutScene_Keyframe> CamKFList{
		get{return camKFList;}
		set{camKFList = value;}
	}

	public int CamPosition{
		get{return camPosition;}
		set{camPosition = value;}
	}
	public int AvatarPosition{
		get{return avatarPosition;}
		set{avatarPosition = value;}
	}

	public List<CutScene_Keyframe> AvatarKFList{
		get{return avatarKFList;}
		set{avatarKFList = value;}
	}
}
