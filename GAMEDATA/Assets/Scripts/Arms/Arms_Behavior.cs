using UnityEngine;
using System.Collections;

public class Arms_Behavior : MonoBehaviour, IMovable {
	
	private int cadranLook;
	private int cadranTarget;
	public bool stayOnPlace;
	[Range(12.0f, 16.0f)]
	public float throwForceHorizontale;
	[Range(3.0f, 6.0f)]
	public float throwForceVerticale;
	private Vector3 myTarget;
	private Vector3 myRot;
	//private float getOutForce;
	
	private NavMeshAgent myNavMesh;
	private Animator myAnim;
	private AnimatorStateInfo myAnimInfo;
	private Movable myMovable;
	private Avatar myAvatar;
	//private Transform myAvatarBip;
	//private camBehavior myCam;
	public Transform myRightHand;
	public Transform myShoulder;
	
//	private float lastRotation;
//	private float stickForce;
//	private Vector3 stickRotation;
//	private Vector3 camRotation;
	
	private bool isTooFar;
	private bool isNearPlayer;
	private bool onShoulder;
	private bool onShoulderFix;
//	private bool inHands;
	private bool inHandsFix;
	private bool camFix;
	//private bool canTurn;
	private bool launch;
	private bool isKnock;
	
	private bool isWalking;
	
	public Transform myLeftFoot;
	public Transform myRightFoot;
	private bool turnOnLeft;
	private bool turnOnRight;
	private Quaternion lookRot;
	
	// Use this for initialization
	void Start () {
		
		//myCam = Camera.main.gameObject.GetComponent<camBehavior> ();
		myNavMesh = this.GetComponent<NavMeshAgent> ();
		myAnim = this.GetComponent<Animator> ();
		
		myMovable = this.GetComponent<Movable> ();
		myAvatar = Manager.Instance.Avatar;
		//myAvatarBip = myAvatar.transform.FindChild ("Root").gameObject.transform.FindChild("Bip01");
		//myAvatar = GameObject.FindGameObjectWithTag ("Player").GetComponent<Avatar>();
		if(myMovable.WpStart != null){
			myMovable.Courant = myMovable.WpStart.Id;
			myMovable.Next = myMovable.WpStart.Id + 1;
		}
		
		myNavMesh.updateRotation = stayOnPlace;
		
	}
	
	// Update is called once per frame
	void Update () {
		myAnimInfo = myAnim.GetCurrentAnimatorStateInfo (0);
		//Debug.Log (myAnim.layerCount);
		
		isNearPlayer = (Vector3.Distance (this.transform.position, myAvatar.transform.position) <= 2f)?true:false;
		
		if (!myAnimInfo.IsName ("Locomotion")) {
			turnOnLeft = false;
			turnOnRight = false;
		}
		//		red.SetActive (turnOnRight);
		//		blue.SetActive (turnOnLeft);
		if (isNearPlayer) {
			myAnim.SetLayerWeight(1,Mathf.Lerp(myAnim.GetLayerWeight(1),1f,Time.deltaTime));
		}else{
			myAnim.SetLayerWeight(1,Mathf.Lerp(myAnim.GetLayerWeight(1),0f,Time.deltaTime));
		}
		
		if (onShoulder) {
			myAnim.SetLayerWeight(3,Mathf.Lerp(myAnim.GetLayerWeight(3),1f,2f*Time.deltaTime));
		}else{
			myAnim.SetLayerWeight(3,Mathf.Lerp(myAnim.GetLayerWeight(3),0f,2f*Time.deltaTime));
		}
		
		if (myAnimInfo.IsName ("Push")) {
			myAnim.SetLayerWeight(2,Mathf.Lerp(myAnim.GetLayerWeight(2),1f,Time.deltaTime));
		}else{
			myAnim.SetLayerWeight(2,Mathf.Lerp(myAnim.GetLayerWeight(2),0f,Time.deltaTime));
		}
		
		if (Vector3.Distance(this.transform.position,myAvatar.transform.position)<5f && !onShoulder && !myAnimInfo.IsName("Push")) {
			float tempY = Quaternion.LookRotation ((myAvatar.transform.position - this.transform.position).normalized).eulerAngles.y;
			myAnim.SetFloat ("headRotation", Mathf.Lerp (myAnim.GetFloat ("headRotation"), Mathf.DeltaAngle (this.transform.eulerAngles.y, tempY), 5f * Time.deltaTime));
			myAnim.SetLayerWeight(4,Mathf.Lerp(myAnim.GetLayerWeight(4),1f,Time.deltaTime));
		}else{
			myAnim.SetLayerWeight(4,Mathf.Lerp(myAnim.GetLayerWeight(4),0f,Time.deltaTime));
		}
		
		if (myAnimInfo.IsName ("Locomotion")) {
			
			//myNavMesh.speed = 2f;
		} else {
			myNavMesh.speed = 0f;
		}
		
		
		//myNavMesh.updateRotation = false;
		if (((myNavMesh.steeringTarget - this.transform.position).normalized) != Vector3.zero) {
			lookRot = Quaternion.LookRotation ((myNavMesh.steeringTarget - this.transform.position).normalized);
		}
		//lookRot = Quaternion.LookRotation ((myNavMesh.velocity).normalized);
		
		
		//		if (Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) < 45f && Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) > -45f) {
		//			myNavMesh.speed = 2f;
		//		} else {
		//			myNavMesh.speed = 1f;
		//		}
		//Debug.Log (Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y), lookRot.eulerAngles.y));
		//Debug.Log (myAnim.GetFloat ("velocityXY"));
		if(/*!inHands && */!onShoulder && !isNearPlayer){
			
			//if (Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) < 10f && Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) > -10f) {
			//this.transform.RotateAround (transform.position, Vector3.up, Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) * 5f * Time.deltaTime);
			//Debug.Log("ptit turn");
			//} else {
			if (turnOnLeft && !myAnimInfo.IsName ("Push")/* && !stayOnPlace*/) {
				
				//Debug.Log("gros turnL");
				//myNavMesh.velocity = this.transform.forward * 2f;
				if(Mathfx.Approx(myNavMesh.destination, myMovable.WpsList[0].transform.position,0.1f) && Vector3.Distance (this.transform.position, myMovable.WpsList[0].transform.position) <= 0.4f){
				}else{
					
					this.transform.RotateAround (myLeftFoot.position, Vector3.up, Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) * 3f * Time.deltaTime);
				}
			}
			if (turnOnRight && !myAnimInfo.IsName ("Push")/* && !stayOnPlace*/) {
				
				//Debug.Log("gros turnR");
				//myNavMesh.velocity = this.transform.forward * 2f;
				if(Mathfx.Approx(myNavMesh.destination, myMovable.WpsList[0].transform.position,0.1f) && Vector3.Distance (this.transform.position, myMovable.WpsList[0].transform.position) <= 0.4f){
				}else{
					
					this.transform.RotateAround (myRightFoot.position, Vector3.up, Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) * 3f * Time.deltaTime);
				}
			}
			//}
		}
		
		
//		stickForce = Mathfx.HighestAbsValue( new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0f) );
//		stickRotation = new Vector3 (this.transform.eulerAngles.x, Mathf.Atan2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * Mathf.Rad2Deg, this.transform.eulerAngles.z);
//		camRotation = new Vector3(0f, Camera.main.transform.rotation.eulerAngles.y, 0f);
		
		
		//		if (stickForce > 0.1f && onShoulder) {
		//			if(stickRotation.y >-15f && stickRotation.y<15f){
		//				getOutForce += 1f*Time.deltaTime;
		//				if(getOutForce>0.5f){
		//					getOutForce = 0f;
		//					DropAvatar();
		//				}
		//
		//			}
		//			else{
		//				getOutForce = 0f;
		//			}		
		//		}
		
		if (Input.GetButtonDown ("Fire1") && (onShoulder/* || inHands*/)) {
			launch = true;
		}
		
//		if (camFix) {
//			//			myCam.SetPos = true;
//			myCam.CamEffectPos = this.transform.forward * -1f;
//		}
		//		else {
		//			myAvatar.OnPause = false;
		//			myCam.SetPos = false;
		//		}
		
		if(onShoulderFix){
			myAvatar.transform.position = myShoulder.position - (Vector3.up*0.25f)+ (this.transform.forward*-0.20f);
			//myAvatar.transform.rotation = this.transform.rotation;
			myAvatar.transform.forward = -this.transform.forward;
			//myAvatar.OnPause = true;
		}
		
		if (inHandsFix) {
			//myAvatarBip.transform.position = myRightHand.position;
			//myAvatar.transform.position = myRightHand.position;
			myAvatar.transform.position = myRightHand.position - (Vector3.up*1f) - (this.transform.right*0.5f);
			myAvatar.transform.forward = -this.transform.forward;
			//myAvatar.OnPause = true;
			//myAvatar.transform.rotation = myRightHand.rotation;
		}
		
		
		
		if (Vector3.Distance (this.transform.position, myAvatar.transform.position) <= 5f && !isTooFar) {
			if (Vector3.Distance (this.transform.position, myMovable.WpsList[getNearestWP()].transform.position) <= 20f) {
				myNavMesh.destination = myAvatar.transform.position;
				myNavMesh.stoppingDistance = 2f;
			}else{
				isTooFar = true;
				myMovable.Courant = getNearestWP();
				if(!stayOnPlace){
					myNavMesh.destination = myMovable.WpsList [myMovable.Courant].transform.position;
				}else{
					myNavMesh.destination = myMovable.WpsList[0].transform.position;
				}
			}
			
		} else {
			if(!stayOnPlace){
				myNavMesh.destination = myMovable.WpsList [myMovable.Courant].transform.position;
			}else{
				myNavMesh.destination = myMovable.WpsList[0].transform.position;
			}
			
			myNavMesh.stoppingDistance = 0.5f;
		}
		
		
		if (!stayOnPlace) {
			if (Mathfx.Approx (this.transform.position, myMovable.WpsList [myMovable.Courant].transform.position, 0.5f)) {
				isTooFar = false;
				myMovable.WpSetNext ();
				
			}
		}
		
		
		
		if (isNearPlayer/* && !inHands*/) {
			//			if(myAvatar.IsSprinting){
			//				Invoke("DropAvatar",6f);
			//				inHands = true;
			//				myAvatar.MyBrutus = this;
			if (Vector3.Distance (this.transform.position, myAvatar.transform.position) <= 1.1f && myAvatar.IsGrounded){
				isKnock = true;
				//this.transform.LookAt(myAvatar.transform.position);
			}
		}
		
//		cadranLook = (int)(this.transform.eulerAngles.y / 45f);
//		cadranTarget = cadranLook;
		//cadranTarget = cadranLook<4?cadranLook+4:cadranLook-4;
//		myRot = Quaternion.AngleAxis (cadranTarget * 45f + 22.5f, Vector3.up) * Vector3.forward;
//		myTarget = this.transform.position+(myRot)*throwForceHorizontale;
		//if (isNearPlayer) {
		//this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x,Mathfx.Clerp(this.transform.eulerAngles.y,Quaternion.LookRotation((myAvatar.transform.position - this.transform.position).normalized).eulerAngles.y,4f*Time.deltaTime),this.transform.eulerAngles.z);
		//this.transform.LookAt(myAvatar.transform.position);
		//}
		this.transform.eulerAngles = new Vector3( 0f,this.transform.eulerAngles.y,0f);
		
		/////////////////////////////////////////////////////////////
		
		
		
		
		
		
		if (isNearPlayer && !onShoulder/*&& !inHands*/) {
			float tempY = Quaternion.LookRotation ((myAvatar.transform.position - this.transform.position).normalized).eulerAngles.y;
			
			if (Mathf.Abs (Mathf.DeltaAngle (this.transform.eulerAngles.y, tempY)) <= 10f) {
				myAnim.SetFloat ("rotation", Mathf.Lerp (myAnim.GetFloat ("rotation"), 0f, 20f * Time.deltaTime));
				
			} else {
				if (Mathf.DeltaAngle (this.transform.eulerAngles.y, tempY) > 10f) {
					myAnim.SetFloat ("rotation", Mathf.Lerp (myAnim.GetFloat ("rotation"), 1f, 4f * Time.deltaTime));
				} else {
					myAnim.SetFloat ("rotation", Mathf.Lerp (myAnim.GetFloat ("rotation"), -1f, 4f * Time.deltaTime));
				}
			}
		}
		if(!isNearPlayer &&!onShoulder/* && !inHands*/){
			myAnim.SetFloat ("rotation", Mathf.Lerp (myAnim.GetFloat ("rotation"), 0f, 20f * Time.deltaTime));
		}
		
//		if (inHands) {
//			if(stickForce > 0.1f){
//				float tempY = (stickRotation + camRotation).y;
//				
//				if (Mathf.Abs( Mathf.DeltaAngle (this.transform.eulerAngles.y,tempY))<=60f) {
//					myAnim.SetFloat ("rotation", 0f);
//				} else {
//					if (Mathf.DeltaAngle (this.transform.eulerAngles.y,tempY)>60f) {
//						myAnim.SetFloat ("rotation",1f);
//					} else {
//						myAnim.SetFloat ("rotation",-1f);
//					}
//				}
//			}else{
//				myAnim.SetFloat ("rotation", 0f);
//			}
//		}
		
		if (myAnimInfo.IsName ("Push")) {
			this.transform.RotateAround (this.transform.position, Vector3.up, Mathf.DeltaAngle (this.transform.rotation.eulerAngles.y, lookRot.eulerAngles.y) * 1f * Time.deltaTime);
		}
		
		if (!Mathfx.Approx(myNavMesh.velocity, Vector3.zero,0.1f)) {
			isWalking = true;
		} else {
			isWalking = false;
		}
		
		//myAnim.SetFloat ("velocityXY", Mathf.Lerp(myAnim.GetFloat("velocityXY"), (Mathf.Abs (myNavMesh.velocity.x) + Mathf.Abs (myNavMesh.velocity.z)) / 2,2f*Time.deltaTime));
		if (!stayOnPlace && myNavMesh.stoppingDistance == 0.5f) {
			myAnim.SetFloat ("velocityXY", Mathf.Lerp (myAnim.GetFloat ("velocityXY"), 2f, 2f * Time.deltaTime));
		} else {
			myAnim.SetFloat ("velocityXY", Mathf.Lerp (myAnim.GetFloat ("velocityXY"), Vector3.Distance (this.transform.position, myNavMesh.destination) - myNavMesh.stoppingDistance, 2f * Time.deltaTime));
		}
		
		
		if(myAnim.GetFloat("velocityXY") < 0.02f){
			myAnim.SetFloat ("velocityXY",0f);
		}
		myAnim.SetBool("isWalking",isWalking);
	//	myAnim.SetBool("inHands",inHands);
		myAnim.SetBool("onShoulder",onShoulder);
		myAnim.SetBool("isLaunch",launch);
		myAnim.SetBool("isKnock",isKnock);
		
		if (launch) {
			launch = false;
		}
		if(isKnock){
			isKnock = false;
		}
		
	}
	
//	void OnDrawGizmos() {
//		Gizmos.DrawLine(this.transform.position,this.transform.position+Vector3.up*(throwForceVerticale+3f));
//		for (int i= 0; i<=7; i++) {
//			if(i==cadranTarget){Gizmos.color = Color.red;}
//			else{Gizmos.color = Color.blue;}
//			myRot = Quaternion.AngleAxis(i*45f+22.5f, Vector3.up) * Vector3.forward;
//			Gizmos.DrawLine(this.transform.position,this.transform.position+(myRot)*throwForceHorizontale);
//		}
//	}
	
	void LaunchHor(){
		myAvatar.HasBeenLaunched = true;
		CancelInvoke("DropAvatar");
		//inHands = false;
		//inHandsFix = false;
		FixInHandsF();
		myAvatar.MyTargetFoo = myTarget;
		myAvatar.StartCoroutine("GoingFromTheBar");
		Invoke ("NoMoreLaunched",0.1f);
		//myAvatar.transform.position = myTarget;
		//myAvatar.OnPause = false;
		//myAvatar.InHandsFix = false;
	}
	
	void LaunchVer(){
		CancelInvoke("DropAvatar");
		onShoulder = false;
		//onShoulderFix = false;
		FixOnShoulderF();
		//myAvatar.MyTargetFoo = new Vector3(myAvatar.transform.position.x,myAvatar.transform.position.y + throwForceVerticale, myAvatar.transform.position.z); //TODO
		//myAvatar.StartCoroutine("GoingFromTheBar");
		myAvatar.ImpulSaut = new Vector3 (0f,myAvatar.propVertical(throwForceVerticale,1f),0f);
		//myAvatar.transform.position = this.transform.position + Vector3.up * (throwForceVerticale + 3f);
		myAvatar.HasBeenLaunched = true;
		//	Invoke ("NoMoreLaunched",0.1f);
		//myAvatar.OnPause = false;
		//myAvatar.OnShoulderFix = false;
	}
	void NoMoreLaunched(){
		myAvatar.NoMoreLaunched();
	}
	void Reset(){
		CancelInvoke("DropAvatar");
		onShoulder = false;
		onShoulderFix = false;
		//inHands = false;
		inHandsFix = false;
		//myAvatar.OnShoulderFix = false;
		//myAvatar.InHandsFix = false;
	}
	
	void DropAvatar(){
		CancelInvoke("DropAvatar");
		//inHands = false;
		onShoulder = false;
		//myAvatar.OnShoulderFix = false;
		//myAvatar.InHandsFix = false;
	}
	
	void FixInHands(){
		inHandsFix = !inHandsFix;
	}
	
	void FixInHandsT(){
		inHandsFix = true;
		myAvatar.InHandsFix = true;
		myAvatar.MyCont.enabled = false;
	}
	
	void FixInHandsF(){
		//myAvatar.OnPause = false;
		inHandsFix = false;
		myAvatar.InHandsFix = false;
		myAvatar.MyCont.enabled = true;
	}
	
//	void FixCamT (){
//		//myAvatar.OnPause = false;
//		myCam.SetPos = true;
//		myCam.CamEffectPos = this.transform.forward * -1f;
//		camFix = true;
//	}
//	
//	void FixCamF (){
//		//myAvatar.OnPause = false;
//		myCam.SetPos = false;
//		camFix = false;
//	}
	
	void LaunchF(){
		launch = false;
	}
	
	void FixOnShoulder(){
		onShoulderFix = !onShoulderFix;
	}
	
	void TurnOnLeft(){
		turnOnLeft = !turnOnLeft;
		//this.transform.RotateAround(myLeftFoot.position +(this.transform.forward*2f),Vector3.up, 10f*Time.deltaTime);
	}
	
	void TurnOnRight(){
		turnOnRight = !turnOnRight;
		//this.transform.RotateAround(myRightFoot.position +(this.transform.forward*2f),Vector3.up, 10f*Time.deltaTime);
	}
	
	
	
	void FixOnShoulderT(){

		onShoulderFix = true;
		myAvatar.OnShoulderFix = true;
		myAvatar.MyCont.enabled = false;
	}
	
	void FixOnShoulderF(){
		onShoulderFix = false;
		myAvatar.OnShoulderFix = false;
		myAvatar.MyCont.enabled = true;
	}
	
	void KnockBackAvatar(){
		float tempY = Quaternion.LookRotation((myAvatar.transform.position - this.transform.position).normalized).eulerAngles.y;
		if (Vector3.Distance (this.transform.position, myAvatar.transform.position) <= 2f /*&& myAvatar.IsGrounded*/){
			if (Mathf.Abs( Mathf.DeltaAngle (this.transform.eulerAngles.y,tempY))<=45f) {
				myAvatar.KnockBack (this.transform);
			}
			
		}
		
	}
	
	void SoundStep(){
		//SoundManager.Instance.ArmsFootstepsRandom ();
		int number = UnityEngine.Random.Range(0, 5);
		
		switch(number){
		case 0:
			SoundManager.Instance.PlayAudioInMyself(this.gameObject, "Arms_footsteps_1");
			break;
		case 1:
			SoundManager.Instance.PlayAudioInMyself(this.gameObject, "Arms_footsteps_2");
			break;
		case 2:
			SoundManager.Instance.PlayAudioInMyself(this.gameObject, "Arms_footsteps_3");
			break;
		case 3:
			SoundManager.Instance.PlayAudioInMyself(this.gameObject, "Arms_footsteps_4");
			break;
		case 4:
			SoundManager.Instance.PlayAudioInMyself(this.gameObject, "Arms_footsteps_5");
			break;
		}
	}
	
	void SoundLaunch(){
		SoundManager.Instance.ArmsLaunchRandom ();
	}
	
	void SoundPush(){
		SoundManager.Instance.ArmsPushRandom ();
	}

	void SoundGrab(){
		SoundManager.Instance.PlayAudio("AGrip");
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.GetComponent<Avatar> () != null/* && !inHands*/) {
			Reset();
			Invoke("DropAvatar",2f);
			SoundGrab();
			onShoulder = true;
			myAvatar.MyBrutus = this;
			onShoulderFix = true;
			myAvatar.OnShoulderFix = true;
			//Debug.Log("fix");
		}
	}
	
	
	int getNearestWP(){
		float dist = 100f;
		int id = 0;
		foreach (WaypointScript wp in myMovable.WpsList) {
			if(Vector3.Distance(wp.transform.position,this.transform.position)<=dist){
				id = myMovable.WpsList.IndexOf(wp);
				dist = Vector3.Distance(wp.transform.position,this.transform.position);
			}
		}
		return id;
	}
	
	//Set un delais d'attente au owner
	public void Wait(float seconds){}
	//Arrete le owner
	public void Stop(bool isStopped){}
	//Set une nouvelle vitesse au owner
	public void NewSpeed(float speed){}
	//Set l'orientation du owner : quaternion.identity ou forward ou fullforward
	public void ChangeRotation(int orientation){}
	//Teleporte le owner au waypoint
	public void Teleport(WaypointScript where){}
	//Fait une rotation a 90° avec un delais ou non
	public void RotateHalf(MovableRotation rotation){}
	//Fait une rotation a 180° avec un delais ou non
	public void RotateFull(MovableRotation rotation){}
}
