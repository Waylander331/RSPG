using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class camBehavior : MonoBehaviour {

	public Transform myFocus;
	private float camDistanceMax = 4f;
	private float lowestAutomaticPoint = 0.3f;


	private float defaultDistanceMax;

	private bool isPlayerControlled;
	private bool isLookAting;
	private bool isMoving;
	private bool setPos;
	private bool isShaking;
	private Vector3 myDirection;
	private Vector3 myDirectionToView;
	private Vector3 myInput;
	private float camDistance;
	private float camDistanceTarget;
	private RaycastHit camHit;
	private RaycastHit focalHit;
	private int searchPassNumb = 20;
	private float searchResolution = 0.05f;
	private Vector3 myPos;
	private Vector3 myFocusPos;
	private LayerMask myLayer;
	private Vector3 drunkOffset;
	private Vector3 drunkOffsetTarget;
	private Vector3 drunkCamOffset;
	private Vector3 drunkCamOffsetTarget;
	private bool toView;
	private bool isDrunk;
	private Vector3 camEffectPos;
	private Vector3 myFocusY;
	private bool invertedY = true;
	private bool invertedX = true;
	private float drunkOffsetTimer = 0f;
	private Vector3 fixedView;
	private Vector3 fixedPos;
	private Camera myCam;
	private VignetteAndChromaticAberration myVignette;
	private bool hasInvisAvatar;



	private Color guiColor;
	private float fadeF;
	private float flickerTime;
	//[HideInInspector]
	public Texture solidTexture;
	//private DepthOfField myDoF;

	//private Vortex myVortex;
	//private BloomOptimized myBloom;


	private Avatar myAvatar;

	private Vector3 lastAvatarPos;
	private bool isMoved;

	void Start () {
		if(Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits"
		   && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit"){
			guiColor = Color.black;

			FadeIn ();
		
			if (this.GetComponent<VignetteAndChromaticAberration> ()) {
				myVignette = this.GetComponent<VignetteAndChromaticAberration> ();
			}
			//myDoF = this.GetComponent<DepthOfField> ();
			//myVortex = this.GetComponent<Vortex> ();
			//myBloom = this.GetComponent<BloomOptimized> ();

			myCam = this.GetComponent<Camera> ();


			toView = true;
			isPlayerControlled = true;
			isLookAting = true;
			isMoving = true;
			drunkCamOffsetTarget = Random.insideUnitSphere/2f;
			drunkOffsetTarget = Random.insideUnitSphere/4f;
			myLayer = UnityEngine.LayerMask.GetMask ("Collision");
			myLayer += UnityEngine.LayerMask.GetMask ("WallJumpable");
			myFocusY = new Vector3 (myFocus.position.x, myFocus.position.y + 1.8f, myFocus.position.z);
			myDirection = (this.transform.position - myFocusY).normalized;

			defaultDistanceMax = camDistanceMax;
		}
	}


	void Update () {
		if(Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits"
		   && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit"){
			if (myAvatar == null) {
				myAvatar = Manager.Instance.Avatar;
				//myAvatar = GameObject.FindGameObjectWithTag ("Player").GetComponent<Avatar>();
			}

			if (myAvatar.IsDead && !IsInvoking ("FadeOut")) {
				Invoke("FadeOut",0.25f);
			}

			//myDoF.focalLength = Vector3.Distance (this.transform.position, myFocusY);


			if(myAvatar != null){
				isDrunk = myAvatar.IsDazed;
			}

			//myVortex.enabled = isDrunk;
			if (isDrunk) {
				drunkOffset = Vector3.Lerp (drunkOffset, drunkOffsetTarget, 1f * Time.deltaTime);
				drunkCamOffset = Vector3.Lerp (drunkCamOffset, drunkCamOffsetTarget, 0.5f * Time.deltaTime);
				drunkOffsetTimer += Time.deltaTime;
				if (drunkOffsetTimer >= 1f) {
					drunkOffsetTimer = 0f;
					drunkOffsetTarget = Random.insideUnitSphere / 4f;
					drunkCamOffsetTarget = Random.insideUnitSphere / 2f;
				}
			}
		}
	}

	void LateUpdate(){
		if(Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits"
		   && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit"){
			if(!Manager.IsPaused){

				if (flickerTime > 0f) {
					flickerTime -= 1f*(Time.deltaTime*(1/Time.timeScale));
					if (flickerTime < 0f) {

						fadeF = 0f;
					}
					if (flickerTime > 0f) {

						fadeF = Random.Range (-10f, 10f);
					}
					
				}
				//fadeF = Mathf.Lerp(fadeF, 0f,(Time.deltaTime*(1/Time.timeScale)));
				guiColor.a = Mathf.Lerp(guiColor.a, fadeF,(Time.deltaTime*(1/Time.timeScale)));
				guiColor.a = Mathf.Clamp(guiColor.a,0f,1f);
				//if(Input.GetButton("Fire2")){

				//}
			
				
				myFocusY = Vector3.Lerp (myFocusY, myFocus.transform.position + (Vector3.up * 1.8f), 10f * (Time.deltaTime*(1/Time.timeScale)));  

				if (Input.GetAxis ("HorizontalR") != 0f || Input.GetAxis ("VerticalR") != 0f) {
					isMoved = false;
				}

				if (!Mathfx.Approx(lastAvatarPos, myFocus.transform.position,0.1f)) {
					isMoved = true;
				} 
		//		else {
		//			isMoved = false;
		//		}
				

				if (setPos) {
					myDirection = camEffectPos;
					toView = true;
				}

				if(Manager.Instance.IsSlow && !setPos && isMoving){
					myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, 110f,1f * (Time.deltaTime*(1/Time.timeScale))); 
					camDistanceMax = Mathf.Lerp(camDistanceMax, defaultDistanceMax/2f,1f * (Time.deltaTime*(1/Time.timeScale))); 
					if(myVignette != null){
						myVignette.blur = Mathf.Lerp(myVignette.blur, 0.8f,1f * (Time.deltaTime*(1/Time.timeScale))); 
						myVignette.blurSpread = Mathf.Lerp(myVignette.blurSpread, 2.5f,1f * (Time.deltaTime*(1/Time.timeScale))); 
						myVignette.chromaticAberration = Mathf.Lerp(myVignette.chromaticAberration, 10f,1f * (Time.deltaTime*(1/Time.timeScale))); 
						myVignette.intensity = Mathf.Lerp(myVignette.intensity, 2f,1f * (Time.deltaTime*(1/Time.timeScale))); 
						
					}
				}else{
					if(Mathfx.Approx(myCam.fieldOfView,60f,2f)){
						if(myCam.fieldOfView != 60f){
							myCam.fieldOfView = 60f;
							camDistanceMax = defaultDistanceMax;
							if(myVignette != null){
								myVignette.blur = 0f; 
								myVignette.blurSpread = 0f;
								myVignette.chromaticAberration = 0f;
								myVignette.intensity = 0f;
							}
						}
					}else{
						myCam.fieldOfView = Mathf.Lerp(myCam.fieldOfView, 60f,1f * (Time.deltaTime*(1/Time.timeScale))); 
						camDistanceMax = Mathf.Lerp(camDistanceMax, defaultDistanceMax,1f * (Time.deltaTime*(1/Time.timeScale))); 
						if(myVignette != null){
							myVignette.blur = Mathf.Lerp(myVignette.blur, 0f,1f * (Time.deltaTime*(1/Time.timeScale))); 
							myVignette.blurSpread = Mathf.Lerp(myVignette.blurSpread, 0f,1f * (Time.deltaTime*(1/Time.timeScale))); 
							myVignette.chromaticAberration = Mathf.Lerp(myVignette.chromaticAberration, 0f,1f * (Time.deltaTime*(1/Time.timeScale))); 
							myVignette.intensity = Mathf.Lerp(myVignette.intensity, 0f,1f * (Time.deltaTime*(1/Time.timeScale))); 
						}
					}
				}

				if (isMoving) {
						

					myPos = new Vector3 (this.transform.position.x, 0f, this.transform.position.z);
					myFocusPos = new Vector3 (myFocusY.x, 0f, myFocusY.z);

					if (Vector3.Distance (myPos, myFocusPos) < 1f && myDirection.y >= 0.6f) {	

						Vector3 tempPos = this.transform.position + (((myPos - myFocusPos).normalized) * (1f - Vector3.Distance (myPos, myFocusPos)));
							if(!Physics.Raycast(this.transform.position,(tempPos-this.transform.position).normalized,1f,myLayer)){
								this.transform.position = this.transform.position + (((myPos - myFocusPos).normalized) * (1f - Vector3.Distance (myPos, myFocusPos)));
							}
					}
			
					myDirection = (this.transform.position - myFocusY).normalized;

					if (setPos) {
						myDirection = camEffectPos;
						toView = true;
					}

					if (isPlayerControlled) {

						

						myInput.x = Input.GetAxis ("HorizontalR") * 1f * (invertedX ? -1 : 1) * (setPos ? 0.5f : 1f);
						myInput.y = Input.GetAxis ("VerticalR") * 10f * (invertedY ? -1 : 1) * (setPos ? 0.1f : 1f);
						if (isDrunk) {
							myInput += drunkOffset;
						}
						//myInput.x += Mathf.Clamp(Input.GetAxis ("Mouse X"),-0.5f,0.5f) * -5f*(invertedX?-1:1);
						//myInput.y += Mathf.Clamp(Input.GetAxis ("Mouse Y"),-0.5f,0.5f) * -5f*(invertedY?-1:1);


						myDirection += (this.transform.right * myInput.x);
				
						if ((myDirection + (this.transform.up * myInput.y)).y <= 0.8f || myInput.y < 0f) {
							if ((myDirection + (this.transform.up * myInput.y)).y >= -0.6f || myInput.y > 0f) {
								myDirection += (this.transform.up * myInput.y);
							}
						}

						if ((myDirection + (this.transform.up * myInput.y)).y >= 0.8f && myInput.y > 0f) {
							myDirection.y = 0.8f;
						}
						if(isMoved){
							if ((myDirection + (this.transform.up * myInput.y)).y <= -0.5f && myInput.y < 0f) {
								myDirection.y = -0.2f;
							}
						}else{
							if ((myDirection + (this.transform.up * myInput.y)).y <= -0.5f && myInput.y < 0f) {
								myDirection.y = -0.8f;
							}
						}
				
						///Get behind character
						if ((Input.GetMouseButton (2) || Input.GetButton ("RightJoystickPress")) && Input.GetAxis ("Vertical") >= 0f) {
					
							myDirection = myFocus.transform.forward * -1f;
							myInput = Vector3.zero;
					
						}

					}
				


					//////////////////////////////////////////////////////////////////////////////
					/// if dont see character

					if (Physics.Raycast (myFocusY, myDirection, out camHit, camDistanceMax, myLayer)) {
						if (isMoved && !setPos && camHit.distance < (camDistanceMax - (camDistanceMax / 2f))) {

							myDirection = SearchForView ();
							myDirectionToView = myDirection;
							toView = false;


						} else {
							toView = true;
							myDirectionToView = myDirection;
							
							camDistanceTarget = camHit.distance - (camHit.distance / 2f) - 0.1f; 
						}
						
					} else {
						if (Vector3.Angle (myDirectionToView, myDirection) < 0.1f) {
							toView = true;
						}
						if (myInput == Vector3.zero) {
							if (!toView) {
								myDirection = myDirectionToView;
							} else {
								myDirectionToView = myDirection;
							}

						} else {
							myDirectionToView = myDirection;
							toView = true;
					
						}
						//Debug.Log("max1");
						camDistanceTarget = camDistanceMax;
						
					}

					myInput = new Vector3(myInput.x,myInput.y,0f);

					//////////////////////////////////////////////////////////////////////////////
					/// Submit change
					if(myInput == Vector3.zero && !setPos){
						if(isMoved){
							myDirection.y = Mathf.Clamp (myDirection.y, lowestAutomaticPoint, 0.9f);

						}

					}else{
						myDirection.y = Mathf.Clamp (myDirection.y, -0.4f, 0.9f);
					}
				
					if (camDistance <= camDistanceTarget) {
						camDistance = Mathf.Lerp (camDistance, camDistanceTarget, 4f * (Time.deltaTime*(1f/Time.timeScale)));
					} else {
						camDistance = camDistanceTarget;
					}

					if(Vector3.Distance(this.transform.position,myFocusY) <=0.75f){
						//myAvatar.Disparition();
						myAvatar.Transparency();
						hasInvisAvatar = true;
					}
					else if(hasInvisAvatar && !Manager.Instance.IsRespawning){
						//	myAvatar.Apparition();
						myAvatar.Opacity();
						hasInvisAvatar = false;
					}

					//DrawShape.DrawXForDebug(myFocusY + (myDirection.normalized * camDistance),Color.red);
					//DrawShape.DrawXForDebug(myFocusY + (myDirection.normalized * camDistanceTarget),Color.blue);
					this.transform.position = Vector3.Lerp (this.transform.position, myFocusY + (myDirection.normalized * camDistance), 5f * (Time.deltaTime*(1/Time.timeScale)));


		//			if (Physics.Raycast (myFocusY, myDirection, camDistance, myLayer)) {
		//				Debug.Log("j'vois pas");
		//			} else {
		//				Debug.Log("j'vois");
		//			}


				} else {
				
					this.transform.position = Vector3.Lerp(this.transform.position,fixedPos,2f * (Time.deltaTime*(1/Time.timeScale)));
					this.transform.rotation = Quaternion.Lerp(this.transform.rotation,Quaternion.Euler(fixedView),2f * (Time.deltaTime*(1/Time.timeScale)));
				}


		}

			if (isLookAting/* && Manager.Instance.Avatar.ImpulSaut.y ==0f*/) {
				if (isShaking) {
					this.transform.LookAt (myFocusY);
					this.transform.Rotate (Random.insideUnitSphere);
				}
				if (isDrunk) {
					Vector3 tempRotDrunk = this.transform.eulerAngles;
					this.transform.LookAt (myFocusY);
					Vector3 tempRotCam = this.transform.eulerAngles;
					this.transform.eulerAngles = tempRotDrunk;

					this.transform.Rotate (drunkCamOffset);		
					this.transform.eulerAngles = new Vector3 (Mathfx.ClampAngle (this.transform.eulerAngles.x, tempRotCam.x - 10f, tempRotCam.x + 10f), Mathfx.ClampAngle (this.transform.eulerAngles.y, tempRotCam.y - 10f, tempRotCam.y + 10f), Mathfx.ClampAngle (this.transform.eulerAngles.z, tempRotCam.z - 10f, tempRotCam.z + 10f));

				}
				if (!isShaking && !isDrunk) {
					this.transform.LookAt (myFocusY);
				}
			} else {
				if (isShaking) {
					if(!isMoving){this.transform.rotation = Quaternion.Euler(fixedView);}
					this.transform.Rotate (Random.insideUnitSphere);
				}
			}
			lastAvatarPos = myFocus.transform.position;
			//Debug.DrawRay (myFocusY, myDirection);
			//Debug.DrawRay (myFocusY, (this.transform.position - myFocusY).normalized);
		}
	}

	void OnGUI()
	{
		GUI.color = guiColor;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), solidTexture);
	}
//	void FixedUpdate(){
//		if (isLookAting && Manager.Instance.Avatar.ImpulSaut.y !=0f) {
//			if (isShaking) {
//				this.transform.LookAt (myFocusY);
//				this.transform.Rotate (Random.insideUnitSphere);
//			}
//			if (isDrunk) {
//				Vector3 tempRotDrunk = this.transform.eulerAngles;
//				this.transform.LookAt (myFocusY);
//				Vector3 tempRotCam = this.transform.eulerAngles;
//				this.transform.eulerAngles = tempRotDrunk;
//				
//				this.transform.Rotate (drunkCamOffset);		
//				this.transform.eulerAngles = new Vector3 (Mathfx.ClampAngle (this.transform.eulerAngles.x, tempRotCam.x - 10f, tempRotCam.x + 10f), Mathfx.ClampAngle (this.transform.eulerAngles.y, tempRotCam.y - 10f, tempRotCam.y + 10f), Mathfx.ClampAngle (this.transform.eulerAngles.z, tempRotCam.z - 10f, tempRotCam.z + 10f));
//				
//			}
//			if (!isShaking && !isDrunk) {
//				this.transform.LookAt (myFocusY);
//			}
//		} else {
//			if (isShaking) {
//				if(!isMoving){this.transform.rotation = Quaternion.Euler(fixedView);}
//				this.transform.Rotate (Random.insideUnitSphere);
//			}
//		}
//	}
	////////////////////////////////////////////////////////////////////////////////////////////////
	/// ////////////////////////////////////////////////////
	/// ////////////////////////////////
	public bool InvertedY{
		set{invertedY = value;}
	}

	public bool InvertedX{
		set{invertedX = value;}
	}

	public bool IsDrunk{
		set{isDrunk = value;}
		get{return isDrunk;}
	}
	public Transform MyFocus{
		set{myFocus = value;}
		get{return myFocus;}
	}

	public bool IsPlayerControlled{
		set{isPlayerControlled = value;}
		get{return isPlayerControlled;}
	}
	public bool IsLookAting{
		set{isLookAting = value;}
		get{return isLookAting;}
	}
	public bool IsMoving{
		set{isMoving = value;}
		get{return isMoving;}
	}
	public bool IsShaking{
		set{isShaking = value;}
		get{return isShaking;}
	}

	public Vector3 CamEffectPos{
		set{camEffectPos = value;}
	}

	public Vector3 FixedView{
		set{fixedView = value;}
	}
	public Vector3 FixedPos{
		set{fixedPos = value;}
	}
	public bool SetPos{
		set{setPos = value;}
		get{return setPos;}
	}

	public float DefaultDistanceMax{
		get{return defaultDistanceMax;}

	}

	public float CamDistanceMax{
		get{return camDistanceMax;}
		set{ camDistanceMax = value;}
	}

	public float LowestAutomaticPoint{
		get{return lowestAutomaticPoint;}
		set{lowestAutomaticPoint = value;}
	}
		
	public void FadeOut (){

		fadeF = 1.5f;
		flickerTime = 0f;
	}

	public void FadeIn (){
	
		fadeF = -1.5f;
		flickerTime = 0f;
	}


	public void FadeOut (float value){
		
		fadeF = value;
		flickerTime = 0f;
	}
	
	public void FadeIn (float value){
		
		fadeF = value*-1f;
		flickerTime = 0f;
	}
	public void Flicker(float time){
		flickerTime = time;
	}
	Vector3 SearchForView(){
		for (int i = 0; i<searchPassNumb; i++) {
			if (!Physics.Raycast (myFocusY, myDirection+(this.transform.right*(i*searchResolution)),camDistanceMax, myLayer)) {
				//Debug.Log("max2");
				camDistanceTarget = camDistanceMax;
				return (myDirection+(this.transform.right*(i*searchResolution)));
				
			}
			if (!Physics.Raycast (myFocusY, myDirection+(this.transform.right*-1f*(i*searchResolution)),camDistanceMax, myLayer)) {
				//Debug.Log("max3");
				camDistanceTarget = camDistanceMax;
				return (myDirection+(this.transform.right*-1f*(i*searchResolution)));
				
			}
		}
		if (Physics.Raycast (myFocusY, myDirection, out camHit, camDistanceMax, myLayer)) {
			camDistanceTarget = camHit.distance - (camHit.distance / 2f) - 0.1f; 
		} else {
		//	Debug.Log("max4");
			camDistanceTarget = camDistanceMax;
		}
		return myDirection;
	}
}
