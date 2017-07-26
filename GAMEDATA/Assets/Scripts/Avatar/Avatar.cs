using UnityEngine;
using System.Collections;

public class Avatar : MonoBehaviour {

	private float frame = 0;
	private bool oneTimeCross;
	#region Variables
	//Variables dev

	private float yTemp;

	private EmptyShadow myShadow;
	private Light myLight;

	public Material[] matReal; //[0] = leg, [1] = torso, [2] = head
	public Material[] matFade; //[0] = leg, [1] = torso, [2] = head
	private bool isTransparent;

	public CapsuleCollider myClothCollider;

	private int compteur = 0;

	public float upDamp;
	public float downDamp;
	//[HideInInspector]
	public bool trail;
	private bool isInvincible;
	//[HideInInspector]
	public LayerMask wJLayer;
	//[HideInInspector]
	public float fallDec;
	private float originalFallDec;
	//[HideInInspector]
	public float stickInc;
	//[HideInInspector]
	public float speed;
	//[HideInInspector]
	public float speedSprintInc;
	//[HideInInspector]
	public float speedSprint;
	private float realSpeed;
	//[HideInInspector]
	public float gravity;
	//[HideInInspector]
	public float gravMax;
	//[HideInInspector]
	public float jumpTime;
	//[HideInInspector]
	public float sprintJumpTime;
	//[HideInInspector]
	public float sprintJumpTest;
	//[HideInInspector]
	public float macSpeed;
	//[HideInInspector]
	public float highJump;
	//[HideInInspector]
	public float distanceJump;
	private float realDistanceJump;
	//[HideInInspector]
	public float sprintDistanceJump;
	//[HideInInspector]
	public float sprintHighJump;
	private float realSprintDistanceJump;
	//[HideInInspector]
	public float stickyTime;
	//[HideInInspector]
	public float wallSlide;
	//[HideInInspector]
	public Transform wallCheckOne;
	//[HideInInspector]
	public Transform wallCheckTwo;
	//[HideInInspector]
	public Transform groundCheck;
	//[HideInInspector]
	public Transform ceilingCheck;
	//[HideInInspector]
	public Transform controlCheck;
	//[HideInInspector]
	public Transform slopeCheck;
	//[HideInInspector]
	public Transform censor;

	private CharacterController myCont;
	private Animator animator;

	private RaycastHit wallHitOne;
	private RaycastHit wallHitTwo;
	private RaycastHit groundHit;
	//Edging
	private RaycastHit groundCheckHitC;//Center
	private RaycastHit groundCheckHitF;//Front
	private RaycastHit groundCheckHitB;//Back
	private RaycastHit groundCheckHitL;//Left
	private RaycastHit groundCheckHitR;//Right

	//Edging Sphere(charater controller de caca qui se coince partout)
	private RaycastHit sphereCastHit;

	private RaycastHit ceilingHit;
	//[HideInInspector]
	public Vector3 camRotation;
	//[HideInInspector]
	public Vector3 stickRotation;
	//[HideInInspector]
	public float stickForce;


	private bool isGrounded = false;
	private bool inAir;
	private bool isSprinting;
	private bool isFromSprint;
	private bool onWall;
	private bool canSticky;
	private bool onSticky;
	private bool canFall;
	private bool isCeiling = false;
	private bool canCeiling = true;
	private bool canStartMAC;
	private bool isFromWall;
	private bool isWallFiring;
	private bool hasJumped;
	private bool isFromJump;
	private bool isKnocked;
	private bool isKnockedB;
	private bool isDazed;
	private bool isDead;
	private bool isRolling;
	private bool isBarFront;
	private bool inTransit;
	private bool barOneShot;
	//private bool fireOnce;
	private bool inBarTrigger;
	private bool towardBar;
	private bool willBarTurn;
	private bool willBarTurnInput;
	private bool isBarTurning;
	private bool canTurnJump;
	private bool canBarTurn;
	private bool inElevator;
	private bool goingFromABar;
	private bool canJump; //Used to stop the avatar from jumping after going on pause
	private bool isSprintJumping;

	private bool floating;
	private bool canFloat;

	private bool canEdge;

	private bool oneSlideSound;
	private bool oneGroundSound;
	private bool oneKnockSound;

	private bool hasLeftFoot; //Regarde si le pied gauche touche terre
	private bool hasRightFoot; //Regarde si le pied droit touche terre
	private bool sprintJumpFromLeft; //Regarde que le joueur saut en sprint a partir du pied gauche, pour les anim

	private bool willFall;

	//***ARMS***//
	private bool inHandsFix;
	private bool onShoulderFix;
	private bool onShoulderOneTime;
	private bool withArms;
	private Arms_Behavior myBrutus;
	private bool hasBeenLaunched;
	//**********//

	private bool willMove;
	private bool barSucces;
	private bool canMove;

	private bool onBar;
	private bool isFromBar;
	private bool inTheRightZone;
	private int barPriority;
	//[HideInInspector]
	private bool onPause;

	private Vector3 impulSaut;
	private float gravMin = 0f;

	private Vector3 vecDep;
	private Vector3 vecRoller;
	private float originalForce;

	private PointBarB emptyBarB;
	private PointBarF emptyBarF;
	private EmptyB emptyB;
	private EmptyF emptyF;
	private Denture myDenture;

	private Vector3 wallNorm;
	private float stickForceOriginal;
	private float stickForceDamp;
	private Vector3 push;

	private AnimationBridgeman bridgeMan;
	//DEBUG
	//[HideInInspector]
	public GameObject sphere;
	//[HideInInspector]
	public Billboard myCensor;

	private BarFoo myTarget;
	private Vector3 myTargetFoo;

	private Bar myBar;
	private Bar myOldBar;

	private Birdies myBirds;

	//[HideInInspector]
	private Renderer myRender;

	private RaycastHit edgeHit;
	private Vector3 edgeDistance;

	// Avatar SFX
	private AvatarSFX avatarSFX;

	#endregion
	public int iTest = 1;
	// Use this for initialization

	void Awake()
	{
		myCont = GetComponent<CharacterController>();
		myRender = GetComponentInChildren<SkinnedMeshRenderer>();
	}

	void Start () {
		
		myShadow = GetComponentInChildren<EmptyShadow>();

		isTransparent = false;

		canJump = true;
		willBarTurn = false;
		barOneShot = true;
		realSprintDistanceJump = sprintDistanceJump;
		realDistanceJump = distanceJump;
		inTransit = false;
		myDenture = GetComponentInChildren<Denture>();

		if(GetComponentInChildren<Birdies>() != null){
			myBirds = GetComponentInChildren<Birdies>();
		}

		myBirds.MyParent = GetComponent<Avatar>();

		isDead = false;

		originalFallDec = fallDec;
		canStartMAC = true;

		animator = GetComponent<Animator>();
		
		if(Manager.Instance.IsRespawning){
			Disparition();
		}
	}

	// Update is called once per frame
	void Update (){

		if(Input.GetKeyDown("backspace")){
			if(isTransparent){
				Opacity();
			}
			else{
				Transparency();
			}
		}

		if(onPause){
			canJump = false;
			Invoke("CanJump", 0.3f);
		}

		if(impulSaut.y <= 0){
			hasBeenLaunched = false;
		}
		/********** StickForce Lerp / Damp ***********/
		float tempStickForce = Mathfx.HighestAbsValue( new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0f) );
		if (!Manager.IsPaused) {
			stickForceDamp = Mathf.Lerp (stickForceDamp, tempStickForce,(stickForceDamp>tempStickForce)? downDamp:upDamp * Time.deltaTime);
		} else {
			stickForceDamp = Mathf.Lerp (stickForceDamp, tempStickForce,(stickForceDamp>tempStickForce)? downDamp:upDamp * Time.deltaTime);
		}
		/*********************************************/

		frame++;
		
		if(Time.timeScale == 0f){
			animator.speed = 0f;
		}
		if(inHandsFix){
			StickForce();
		}
		else{
			animator.speed = 1f;
		}
		if(onWall){
			controlCheck.forward = wallHitOne.normal;
		}

		//ControlCheck//
		if(!isKnocked && (Input.GetAxis("Horizontal") >= 0.01f || Input.GetAxis("Vertical") >= 0.01f || Input.GetAxis("Horizontal") <= -0.01f || Input.GetAxis("Vertical") <= -0.01f)){
			controlCheck.transform.eulerAngles = (stickRotation + camRotation);
		}
		else{
			controlCheck.forward = this.transform.forward;
		}

		//*************************GAMEPLAY PART*************************//
		if(!isDead && !onPause){

			if(isKnocked){
				if(oneKnockSound){
					SoundManager.Instance.PlayAudio("Hit");
					oneKnockSound = false;
				}
			}
			else{
				oneKnockSound = true;
			}

			animator.speed = 1/Time.timeScale;

			//***iSGrounded Bool***//
			if(Physics.Raycast(groundCheck.position,-Vector3.up,out groundCheckHitC,0.25f) && groundCheckHitC.transform.gameObject.GetComponentInParent<Arms_Behavior>() == null && groundCheckHitC.transform.gameObject.GetComponentInParent<Bar>() == null && Vector3.Dot(groundCheckHitC.normal,Vector3.up) > 0.98f){
				isGrounded = true;
			}
			else{
				isGrounded = false;
			}
			//*********************//

			//*****Atterissage*****//
			if(isGrounded && impulSaut.y <= 0){
				impulSaut.z = 0f;
				impulSaut.x = 0f;
			}
			//*********************//
			


			//*****************OnGround*****************//
			if(isGrounded){
				animator.applyRootMotion = true;
				if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
					StickForce();
					if (stickForce > 0.1f) {
						this.transform.eulerAngles = (stickRotation + camRotation);		
					}
				}
				CancelInvoke("CanEdge");
				floating = true;
				canEdge = false;
				if(oneGroundSound){
					SoundManager.Instance.PlayAudio("GroundLanding");
					oneGroundSound = false;
				}
				isFromSprint = false;
				if(!isFromWall && animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAir") && oneTimeCross){
					if(stickForce > 0.01){
						animator.CrossFade("LandToRun",0.1f,-1);//TODO changer les Play() pour des CrossFade()
						oneTimeCross = false;
					}
					else{
						animator.CrossFade("LandToIdle",0.1f,-1);
						oneTimeCross = false;
					}
				}
				//***What if isGrounded?***//
				hasJumped = false;
				myBar = null;
				myOldBar = null;
				inTransit = false;
				isFromJump = false;
				push = Vector3.zero;
				inAir = false;
				CancelInvoke("InAir");
				canCeiling = true;
				CancelInvoke("WillCeiling");
				onWall = false;
				canSticky = true;
				onSticky = false;
				CancelInvoke("Unstick");
				fallDec = originalFallDec;
				isFromWall = false;
				CancelInvoke("IsFromWallNoMore");
				isWallFiring = false;
				isFromBar = false;

				SoundManager.Instance.StopAudio("WallSlide");
				if(Manager.Instance.InBackstage){
					avatarSFX.SetHeavySlideEffect(false,true);
					avatarSFX.SetHeavySlideEffect(false,false);
				}
				else{
					avatarSFX.SetSlideEffect(false,true);
					avatarSFX.SetSlideEffect(false,false);
				}
				//*************************//

				canStartMAC = true;
				impulSaut.y = gravMin;

				//*****SPRINT*****//
				if(!onBar && stickForce > 0.1f && (Input.GetButton ("Fire3") || Input.GetAxisRaw("TriggerR") < 0)){
					isSprinting = true;
				}
				else{
					if(inHandsFix){
						isSprinting = true;
					}
					else{
						isSprinting = false;
					}
				}
				if(isSprinting && stickForce > 0.1f){
					if(realSpeed < speedSprint){
						realSpeed += speedSprintInc *Time.deltaTime  *  (1/Time.timeScale);
					}
					else{
						realSpeed = speedSprint;
					}
				}
				else{
					if(realSpeed > speed){
						realSpeed -= speedSprintInc *Time.deltaTime  *  (1/Time.timeScale);
					}
					else{
						realSpeed = speed;
					}
				}
				//****************//

				if(Input.GetAxis("Horizontal") <= 0.01f && Input.GetAxis("Vertical") <= 0.01f && Input.GetAxis("Horizontal") >= -0.01f && Input.GetAxis("Vertical") >= -0.01f){
					stickForce = 0f;
				}
			}

			//***************NOT ON GROUND***************//
			else{
				this.transform.parent = null;
				//****ARMS****//
				if(inHandsFix){
					this.transform.forward = myBrutus.transform.forward;
				}
				//************//
					
				//************//
				oneTimeCross = true;
				Invoke ("CanEdge",0.3f);
				oneGroundSound = true;
					if(!onWall && !onBar && !onShoulderFix && !inHandsFix && !isFromBar && !goingFromABar){
						inAir = true;
						if(Mathf.Abs(impulSaut.x) + Mathf.Abs(impulSaut.z) < 4f){

							if(stickForce >0.1){
								impulSaut += this.transform.forward * stickForce;
							}
						}
					}
				#region Is on wall?
				//Debug.Log ("First "+Physics.Raycast(wallCheckOne.position,this.transform.forward,0.8f,wJLayer)+" "+frame);
				//Debug.Log ("Second "+Physics.Raycast(wallCheckTwo.position,this.transform.forward,0.8f,wJLayer)+" "+frame);
				//if(Physics.Raycast(wallCheckOne.position,this.transform.forward,0.8f,wJLayer)){
					//Debug.Log ("OnWall "+onWall);
					//Debug.Log ("onBar "+onBar);
					//Debug.Log ("inHandFix "+inHandsFix);
					//Debug.Log ("onShoulderFix "+onShoulderFix);
					//Debug.Log ("Dot "+Vector3.Dot (wallHitOne.normal,this.transform.forward));
				//}
				if(!onWall && !onBar && !inHandsFix && !OnShoulderFix && Physics.Raycast(wallCheckOne.position,this.transform.forward,out wallHitOne,0.8f,wJLayer) && Physics.Raycast(wallCheckTwo.position,this.transform.forward,out wallHitTwo,0.8f) && !Physics.Raycast(this.transform.position,-this.transform.up,1f) && Vector3.Dot (wallHitOne.normal,this.transform.forward) <=-0.75f && Vector3.Dot (wallHitTwo.normal,this.transform.forward) <=-0.75f){
						this.transform.forward = -wallHitOne.normal;
						onWall = true;
					oneSlideSound = true;
					myOldBar = null;
						if(onWall && canSticky){
							onSticky = true;
							canSticky = false;
							Invoke("Unstick",stickyTime);
							animator.Play ("IsOnWall");
						SoundManager.Instance.AvatarWallGripRandom();
						}
					}
					else{
						canSticky = true;
					}
					if(onWall && !Physics.Raycast(wallCheckOne.position,this.transform.forward,1f,wJLayer)){
						onWall = false;
						stickForce = 0f;
						canSticky = true;
						canFall = false;
					}
					else{
						if(onWall && !Physics.Raycast(wallCheckTwo.position,this.transform.forward,1f,wJLayer)){
							onWall = false;
							stickForce = 0f;
							canSticky = true;
							canFall = false;
						}
					}
				#endregion

				if(canStartMAC && !onWall){
					StartCoroutine("MidAirRotation");
					canStartMAC = false;
				}

				//**********ON WALL**********//
				if(onWall){
					if(onSticky){
						impulSaut.y = 0;
					}
					else{
						impulSaut.y -= wallSlide *Time.deltaTime  *  (1/Time.timeScale) /** Time.timeScale*/;
						
					}
					if(animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAir") || animator.GetCurrentAnimatorStateInfo(0).IsName("JumpTakeOff")){
						animator.Play("IsOnWall");
					}
					if(oneSlideSound && !onSticky){
						SoundManager.Instance.PlayAudio("WallSlide");
						oneSlideSound = false;
					}
					isFromJump = false;
					inAir = false;
					Invoke ("Baldness",0.05f);
					CancelInvoke("IsFromWallNoMore");
					canStartMAC = true;
					isSprinting = false;
					canCeiling = true;
					CancelInvoke("WillCeiling");
					stickForce = 0;
					realSpeed = speed;
					impulSaut.x = 0f;
					impulSaut.z = 0f;
					vecDep.z = 0f;
					vecDep.y = 0f;
					animator.applyRootMotion = false;

					//Version avec X
					if(Input.GetButtonDown("Fire1")){
							isFromJump = true;
							isFromWall = true;
							hasJumped = true;
							controlCheck.forward = this.transform.forward;
						}
						if(Input.GetButtonDown("Fire2")){
							this.transform.forward = -this.transform.forward;
							controlCheck.forward = this.transform.forward;
							willFall = true;
					}
				}
				else{
					SoundManager.Instance.StopAudio("WallSlide");
					if(Manager.Instance.InBackstage){
						avatarSFX.SetHeavySlideEffect(false,true);
						avatarSFX.SetHeavySlideEffect(false,false);
					}
					else{
						avatarSFX.SetSlideEffect(false,true);
						avatarSFX.SetSlideEffect(false,false);
					}
				}
				
				if(willFall){
					willFall = false;
					originalForce = stickForce;
					fallDec = 0f;
					onWall = false;
					onSticky = false;
					animator.applyRootMotion = false;
					canSticky = true;
					onSticky = false;
					CancelInvoke("Unstick");
					inAir = true;
					CancelInvoke("Baldness");
					animator.Play ("Jump From Wall");
				}
					//***********************//

					if(this.transform.position.y <= -25f){
						ThisIsKillingMeh();
					}
				}

			if(floating && canJump){
				//***********JUMP**********//
				if(Input.GetButtonDown("Fire1") && !hasJumped){
					floating = false;
					if(!isSprinting){
						hasJumped = true;
						isFromJump = true;
					}
					else{
						hasJumped = true;
						isFromSprint = true;
						isFromJump = true;
						if(hasLeftFoot && !hasRightFoot){
							sprintJumpFromLeft = true;
						}
						else{
							sprintJumpFromLeft = false;
						}
					}
				}
				//*************************//
			}
			//*****SAUT*****//
			if(hasJumped){
				if(!isFromWall && !hasBeenLaunched){
					if(!isSprinting){
						SoundManager.Instance.PlayAudio("Jump");
						impulSaut = Propulsion (highJump,distanceJump,jumpTime);
						stickForceOriginal = stickForce;
						animator.applyRootMotion = false;
						animator.Play("JumpTakeOff");
						hasJumped = false;
					}
					else
					{
						SoundManager.Instance.PlayAudio("Jump");
						stickForceOriginal = stickForce;
						impulSaut = Propulsion (sprintHighJump,sprintDistanceJump,sprintJumpTime);
						animator.applyRootMotion = false;
						hasJumped = false;
					}
				}
				else{
					if(isFromWall){
						//TODO
						yTemp = this.transform.position.y;
						//**WJ on BM**//
						if(bridgeMan != null && this.transform.forward == -bridgeMan.transform.forward){
							bridgeMan.Fall();
						}
						
						if (wallHitOne.transform.gameObject != null && wallHitOne.transform.tag == "TVScreen")
						{
							Transform parent = wallHitOne.transform;
							while(parent.parent != null)
								parent = parent.parent;
							if(parent.GetComponent<LinkedCam>() != null){
								parent.GetComponent<LinkedCam>().LinkedCamFlicker.BreakTV(wallHitOne.transform.gameObject);
							}
						}
						
						//************//
						impulSaut = WallPropulsion (highJump,180f,jumpTime);
						stickForceOriginal = stickForce;
						originalForce = stickForce;
						fallDec = 0f;
						Invoke("IsFromWallNoMore",1f);
						hasJumped = false;
						animator.applyRootMotion = false;
						canSticky = true;
						onSticky = false;
						CancelInvoke("Unstick");
						inAir = true;
						CancelInvoke("Baldness");
						controlCheck.transform.forward = wallHitOne.normal;
						animator.Play ("Jump From Wall");
						SoundManager.Instance.AvatarWallJumpRandom();
					}
				}
			

				if(isFromWall && this.transform.position.y < yTemp){
					IsFromWallNoMore();
				}
			}
			//***Ceiling***//
			if(Physics.Raycast(ceilingCheck.position,this.transform.up,out ceilingHit,0.5f)){
			}
			if(canCeiling && !onWall && !onBar && Physics.Raycast(ceilingCheck.position,this.transform.up,out ceilingHit,0.5f) && ceilingHit.transform.gameObject != this.gameObject){
				isCeiling = true;
			}
			else{
				isCeiling = false;
			}
			if(isCeiling){
				impulSaut.y = 0;
				canCeiling = false;
				isCeiling = false;
				Invoke("WillCeiling",1f);
			}
			//*************//

			SetAnim();
			if(isFromBar && !onBar){
					PostBarStuff();//TODO check that shiet
				}
			//***DEBUG***//
			if(trail){
				Instantiate(sphere,this.transform.position,Quaternion.identity);
			}
		//***********//

			//*******Deplacement**************//
			if(!onBar && !onShoulderFix && !inHandsFix){
				vecDep = ((this.transform.forward * stickForce) + impulSaut + push) *Time.deltaTime  *  (1/Time.timeScale);
			}
			else{
				vecDep = Vector3.zero;
			}



			//****************************BARRES**********************************//
			
			if(onBar && !inTransit && !willMove && Vector3.Dot (this.transform.forward,controlCheck.forward) <= 0f && !isBarTurning){
				willBarTurnInput = true;
			}

			if(onBar  && canBarTurn && !inTransit && !willMove && willBarTurnInput){
				willBarTurn = true;
				willBarTurnInput = false;
			}
			else{
				willBarTurn = false;
			}

			if(onBar && !inTransit){
				if(willBarTurn && canMove && !willMove && !isBarTurning){
					canBarTurn = false;
					isBarTurning = true;
					canTurnJump = false;
					animator.applyRootMotion = true;
					animator.Play("barTurn");
					isBarFront = ! isBarFront;
					controlCheck.forward = this.transform.forward;
					willBarTurn = false;
					willBarTurnInput = false;
				}
				
				barSucces = inTheRightZone;
				if(barSucces && canJump && canTurnJump){
					if(Input.GetButtonDown("Fire2")){
						//fireOnce = false;
						willBarTurnInput = false;
						controlCheck.forward = this.transform.forward;
						isFromBar = true;
						onBar = false;
						if(myBar != null && myBar.GetComponent<CapsuleCollider>() != null){
							myBar.GetComponent<CapsuleCollider>().isTrigger = false;
						}
						myBar = null;
						animator.CrossFade("JumpAir",0.4f);
						stickForce = 1f;
						originalForce = stickForce;
						controlCheck.forward = this.transform.forward;
						StartCoroutine("MidAirRotation");
					}
					
					if(Input.GetButtonDown("Fire1")){//TODO crÃ©er un bool qui empeche de sauter pendant le barTurn, mais pas au complet
						controlCheck.forward = this.transform.forward;
						willBarTurnInput = false;
						isFromBar = true;
						onBar = false;
						isBarTurning = false;
						if(myBar != null && myBar.GetComponent<CapsuleCollider>() != null){
							myBar.GetComponent<CapsuleCollider>().isTrigger = false;
						}
						myBar = null;
						if(isBarFront){
							animator.Play ("barCycle_T_jumpCycle");
							stickForce = 1f;
							impulSaut = new Vector3(0f,propVertical(3f,1f),0f);
							//barLerpThing = 0f;
							//yBar = this.transform.position.y;
							StartCoroutine("GoingFromTheBar");
							controlCheck.forward = this.transform.forward;
						}
						else{
							animator.Play ("barCycle_T_jumpCycle");
							stickForce = 1f;
							impulSaut = new Vector3(0f,propVertical(3f,1f),0f);
							//barLerpThing = 0f;
							//yBar = this.transform.position.y;
							StartCoroutine("GoingFromTheBar");
							controlCheck.forward = this.transform.forward;
						}
					}
				}
			}
			canBarTurn = false;
			//*******************************************************************//


			if(!isGrounded && !onWall && !onBar && !onShoulderFix && impulSaut.y <= 0f && !isFromBar && !isFromWall){
				Edging();
			}
			myCont.Move ((vecDep));

		}

		if(Input.GetButtonUp ("Fire1")){
			//Debug.Log ("I've been pressed "+compteur+" times! In the avatar and inTrigger "+inBarTrigger);
			compteur ++;
		}
	}//Fin update

	void OnTriggerEnter (Collider other){
		//****LETHAL**** //
		if(!isDead){
			if(other.tag == "Lethal" && !isInvincible){
				SoundManager.Instance.AvatarDeathRandom();
				animator.enabled = false;
				myCont.enabled = false;
				isDead = true;
				isInvincible = true;
				Billboard foo = Instantiate(myCensor,censor.position,Quaternion.identity) as Billboard;
				foo.MyMaster = censor;
				Invoke ("ThisIsKillingMeh",1.5f);
				vecDep = Vector3.zero;
				impulSaut = Vector3.zero;
				stickForce = 0f;
				myCont.enabled = false;
				Disparition ();
			}

			//*****BARS***** //
			if(other.GetComponentInParent<Bar>() != null && other.GetComponentInParent<Bar>().tag == "Bar"){
				emptyB = other.GetComponentInParent<Bar>().GetComponentInChildren<EmptyB>();
				emptyF = other.GetComponentInParent<Bar>().GetComponentInChildren<EmptyF>();
			}
		}
	}

	void OnTriggerExit(Collider other){
		if(!isDead){
			if(other.GetComponent<BarBigTrigger>() != null){
				//myBar = null;
				inTransit = false;
				inBarTrigger = false;
			}
			/*if(!isGrounded && !inBarTrigger && !hasBeenLaunched){
				if(other.gameObject.GetComponent<Denture>() == null){
					animator.Play("JumpAir");
				}

			}*/
		}
	}

	void OnTriggerStay(Collider other){
		//if(Input.GetButtonDown("Fire1")){
			//Debug.Log ("In a trigger "+compteur);
		//}
		if(!isDead){
			if(other.GetComponentInParent<Bar>() != null){
				//if(Input.GetButtonDown("Fire1")){
					//Debug.Log ("In a bar "+compteur);
				//}
				inBarTrigger = true;
				StickForce ();
				if(!isFromBar){
					if(!onBar && inTransit && barOneShot && myBar != null){
						SoundManager.Instance.BarGrabRandom();
						isFromWall = false;
						onBar = true;
						animator.CrossFade("barCycle",0.25f);
						inTheRightZone = true;
						canTurnJump = true;
						inTransit = false;
						inAir = false;
						animator.applyRootMotion = false;
						impulSaut = Vector3.zero;
						controlCheck.forward = this.transform.forward;
						if(myBar != null && myBar.GetComponentInChildren<CapsuleCollider>()){
							myBar.GetComponentInChildren<CapsuleCollider>().isTrigger = true;
						}
						if(Vector3.Angle(this.transform.forward,myBar.transform.forward) <= 46f && (Vector3.Distance (this.transform.position, emptyF.transform.position) >= Vector3.Distance(this.transform.position,emptyB.transform.position))){
							isBarFront = true;
						}
						if(Vector3.Angle(this.transform.forward,-myBar.transform.forward) <= 46f && (Vector3.Distance (this.transform.position, emptyF.transform.position) < Vector3.Distance(this.transform.position,emptyB.transform.position))){
							isBarFront = false;
					
						barOneShot = false;
					}
						if(isBarFront){
							myTarget = myBar.cibleGreen.GetComponent<BarFoo>();
							emptyBarF = myBar.GetComponentInChildren<PointBarF>();
							myTargetFoo = myTarget.TargetFoo;
							this.transform.forward = myBar.transform.forward;
						}
						else{
							myTarget = myBar.cibleRed.GetComponent<BarFoo>();
							myTargetFoo = myTarget.TargetFoo;
							emptyBarB = myBar.GetComponentInChildren<PointBarB>();
							this.transform.forward = -myBar.transform.forward;
						}
						if(isBarFront){
							this.transform.forward = myBar.transform.forward;
						}
						else{
							this.transform.forward = -myBar.transform.forward;
						}
					}
					if(myBar != null){
						this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(myBar.transform.position.x,myBar.transform.position.y - 2f,myBar.transform.position.z),10f*(Time.deltaTime / Time.timeScale));
					}





				}
			}
		}
	}




	//********************************MAC********************************//
	IEnumerator MidAirRotation(){
		while(onPause){
			yield return null;
		}
		while(!isGrounded && !onWall && /*!onPause*/ /*&&*/ !onBar && !inTransit && !isDead){
			//StickForce();
			if(onPause){
				yield return null;
			}
			else{
			//GRAVITÉ
			if(!onBar && !onShoulderFix && !inHandsFix){
				if(impulSaut.y > -gravMax){
					impulSaut.y -= gravity  * Time.deltaTime  *  (1/Time.timeScale);
				}
				else{impulSaut.y = -gravMax;
				}
			}
			else{
				impulSaut.y = 0f;
			}
			//Décrémentation du stickforce
			if(/*!onBar &&*/ (Input.GetAxis("Horizontal") >= 0.3f || Input.GetAxis("Vertical") >= 0.3f || Input.GetAxis("Horizontal") <= -0.3f || Input.GetAxis("Vertical") <= -0.3f)){
				StickForce();
			}
			else{
				if(originalForce > 0){
					originalForce -= fallDec *Time.deltaTime  *  (1/Time.timeScale);
				}
				else{
					originalForce = 0f;
				}
				stickForce = originalForce;
			}
			if(!isFromBar && stickForce < 0.6f && stickForce < stickForceOriginal){
				if(impulSaut.x >=1.75f){
					impulSaut.x -= 1.75f * (Time.deltaTime / Time.timeScale);
				}
				else{
					if(impulSaut.x <= -1.75f){
						impulSaut.x += 1.75f * (Time.deltaTime / Time.timeScale);
					}
					else{
						impulSaut.x = 0f;
					}
				}
				if(impulSaut.z >= 1.75f){
					impulSaut.z -= 1.75f * (Time.deltaTime / Time.timeScale);
				}
				else{
					if(impulSaut.z <= -1.75f){
						impulSaut.z += 1.75f * (Time.deltaTime / Time.timeScale);
					}
					else{
						impulSaut.z = 0f;
					}
				}
			}

			//MAC//
				if(!isFromWall){
				//else{//Fall ou Jump normal
					if(isFromSprint){
						MAC (macSpeed/4);
					}
					else{
						MAC (macSpeed);
					}
				}
			//}
			yield return null;
			}
		}
	}
	//*******************************************************************//

	IEnumerator GoingFromTheBar(){//'Cause I ain't having no money left
		Vector3 temp = myTargetFoo;

		//float speedB = 8f / Mathf.Clamp (Vector3.Distance (this.transform.position, myTargetFoo), 1f, 100f);
		float speedB =  Mathf.Clamp (Vector3.Distance (this.transform.position, myTargetFoo), 15f, 100f)/10f;
		//Debug.Log(speedB);
		//float speedB = 1.5f;
		float tempTime = 0f;
		Vector3 tempY = this.transform.position;

		while(!isGrounded /*&& !isCeiling*/ && !inTransit && !onBar && !onWall && Vector3.Distance(this.transform.position, myTargetFoo) >= 0.5f && !isDead){
			if(onPause){
				yield return null;
			}
			else{
			tempTime += speedB*(Time.deltaTime/ Time.timeScale);
			Vector3 fooV = new Vector3(Mathf.Lerp(tempY.x,myTargetFoo.x,tempTime),Mathfx.IsDouxLerp(tempY.y,myTargetFoo.y,tempTime),Mathf.Lerp(tempY.z,myTargetFoo.z,tempTime));
			this.transform.position = fooV;
			//this.transform.position = Vector3.Lerp (this.transform.position,myTargetFoo,Time.deltaTime);

			impulSaut.y = 0f;
			if(myTargetFoo != temp){
				yield break;
			}
			yield return null;
			}
								}/*
								isFromBar = false;*/
		//impulSaut.y = -7.5f;
		StartCoroutine("MidAirRotation");
		goingFromABar = false;
	}




	//**********************************************************************************************************************//
	//*************************************************FONCTIONS************************************************************//
	//**********************************************************************************************************************//


	private void StickForce(){
		stickForce = Mathfx.HighestAbsValue( new Vector3 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical"), 0f) );
		stickRotation = new Vector3 (this.transform.eulerAngles.x, Mathf.Atan2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * Mathf.Rad2Deg, this.transform.eulerAngles.z);
		camRotation = new Vector3(0f, Manager.Instance.MainCam.transform.rotation.eulerAngles.y, 0f);
	}

	#region FCT JUMP
	public float propVertical(float hight, float time){
		float vy = 2*(hight - ((-gravity * Mathf.Pow(time/2,2))/2));
		return vy;
	}
	public Vector3 Propulsion(float hight, float deplacement, float time){
		Vector3 propulsion = new Vector3 (0f,propVertical(hight,time),deplacement * stickForce);
		propulsion = OriVec(propulsion);
		inAir = true;//S'assure que l'avatar sait qu'elle se trouve dans les airs
		return propulsion;
	}
	public Vector3 WallPropulsion(float hight, float angle, float time){
		this.transform.forward = Quaternion.AngleAxis(angle,Vector3.up) * this.transform.forward; 
		Vector3 propulsion = new Vector3 (0f,propVertical(hight - 0.5f,time),distanceJump);
		propulsion = OriVec(propulsion);
		//propulsion = Quaternion.AngleAxis(angle,Vector3.up) * propulsion;
		inAir = true;
		return propulsion;
	}
	private void WallJump(){
		impulSaut = WallPropulsion (highJump,180f,jumpTime);
		stickForce = 1f;
		originalForce = stickForce;
		fallDec = 0f;
		isFromWall = true;
		Invoke("IsFromWallNoMore",1f);
		hasJumped = true;
		animator.applyRootMotion = false;
		isWallFiring = false;
	}
	#endregion

	//**Orientation de vecteur**//
	/// <summary>
	/// Oriente un vecteur créer par rapport au monde pour qu'il s'oriente dans le forward de l'Avatar
	/// </summary>
	/// vect = Vecteur que l'on veut orienter (exemple, le vecteur venant de la fct propulsion
	private Vector3 OriVec(Vector3 vect){
		float angle = Vector3.Angle(Vector3.forward,this.transform.forward);
		vect = Quaternion.AngleAxis(angle,Vector3.up) * vect;
		Vector3 vecTemp = new Vector3(vect.x,0,vect.z);
		vecTemp = Vector3.Normalize(vecTemp);
		if(vecTemp == this.transform.forward){
			return vect;
		}
		else{
			vect = Quaternion.AngleAxis(-angle * 2f,Vector3.up) * vect;
			return vect;
		}
	}
	//**************************//
	public void BarCheck(){
		if(isBarFront){
			if(onBar && Vector3.Dot (this.transform.forward,myBar.transform.forward) < 1){
				this.transform.forward = myBar.transform.forward;
			}
		}
		else{
			if(onBar && Vector3.Dot (this.transform.forward,-myBar.transform.forward) < 1){
				this.transform.forward = -myBar.transform.forward;
			}
		}
	}
	public void BarSwooch(){
		SoundManager.Instance.PlayAudio("BarRotation");
	}

	public void SetBar(Bar aBar){
		myBar = aBar;
		myOldBar = myBar;
	}

	public void SetBridgeMan(AnimationBridgeman other){
		bridgeMan = other;
	}
	public void ResetBridgeman(){
		bridgeMan = null;
	}
	public void CanBarTurn(){
		canBarTurn = true;
	}
	public void PostBarStuff(){
		onBar = false;
		//fireOnce = true;
		barOneShot = true;
		inAir = true;
		barSucces = false;
		canMove = false;
		willMove = false;
		willBarTurn = false;
		inBarTrigger = false;
		Invoke ("IsFromBarNoMore", 0.5f);
		//Debug.Log("A");
	}
	public void BarGrab(){
		SoundManager.Instance.BarGrabRandom();
		if(isBarTurning){
			canTurnJump = true;
		}
	}
	public void MyHand(){
			myBar.GetComponentInChildren<CapsuleCollider>().isTrigger = false;
	}

	public void NoMoreLaunched(){
		hasBeenLaunched = false;
	}

	public void Fall(){//Fait tomber du mur
		originalForce = stickForce;
		fallDec = 0f;
		onWall = false;
		onSticky = false;
		hasJumped = true;
		animator.applyRootMotion = false;
		CancelInvoke("Unstick");
	}

	private void MAC (float rotationSpeed){
		if(stickForce >0.01f){
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation,controlCheck.rotation, (rotationSpeed *Time.deltaTime  *  (1/Time.timeScale)));
			//Debug.Log("yaasd");
			Vector3 vectTempA = new Vector3(impulSaut.x,0f,impulSaut.z);
			float temp = Vector3.Angle(vectTempA,this.transform.forward);
			impulSaut = Quaternion.AngleAxis(temp,Vector3.up) * impulSaut;
			vectTempA = new Vector3(impulSaut.x,0f,impulSaut.z);
			vectTempA = Vector3.Normalize(vectTempA);
			if(vectTempA != this.transform.forward){
				impulSaut = Quaternion.AngleAxis(-temp * 2f,Vector3.up) * impulSaut;
			}
		}
	}
	public void CanMove(){
		canMove = true;
	}
	public void CannotMove(){
		canMove = false;
	}
	public void RightZone(){
		inTheRightZone = true;
	}
	public void WrongZone(){
		inTheRightZone = false;
	}
	private void ThisIsKillingMeh(){
		Manager.Instance.IsRespawning = true;
		Manager.Instance.LevelTransit();
	}
	public void SetInvincibility(){
		isInvincible = !isInvincible;
	}

	public void TargetChange(){
		if(myTarget.gameObject == myBar.cibleRed.gameObject){
			myTarget = myBar.cibleGreen.GetComponent<BarFoo>();
			myTargetFoo = myTarget.TargetFoo;
		}
		else{
			myTarget = myBar.cibleRed.GetComponent<BarFoo>();
			myTargetFoo = myTarget.TargetFoo;
		}
	}

	public void IsBarTurning(){
		isBarTurning = false;
	}
	public void Daze(){
		if(!isDazed){
			SoundManager.Instance.PlayAudio("Daze");
			SoundManager.Instance.PlayAudio("DazeBirds");
			isDazed = true;
			Invoke("UnDaze",3f);
			if(isGrounded){
				//KnockBack();
			}
			else{
				if(onWall){
					this.transform.forward = -this.transform.forward;
					onWall = false;
				}
				else{
					if(onBar){
					//	fireOnce = false;
						controlCheck.forward = this.transform.forward;
						isFromBar = true;
						onBar = false;
						if(myBar != null && myBar.GetComponent<CapsuleCollider>() != null){
							myBar.GetComponent<CapsuleCollider>().isTrigger = false;
						}
						myBar = null;
						animator.CrossFade("JumpAir",0.4f);
						stickForce = 1f;
						originalForce = stickForce;
						controlCheck.forward = this.transform.forward;
						StartCoroutine("MidAirRotation");
					}
					else{
						if(!onShoulderFix){
							impulSaut = Vector3.zero;
						}
					}
				}
			}
		}
		else{
			CancelInvoke("UnDaze");
			Invoke ("UnDaze",3f);
		}
	}
	public void UnDaze(){
		isDazed = false;
		SoundManager.Instance.StopAudio("DazeBirds");
	}

	public void KnockBack(Vector3 direction){
		Vector3 knockVec = new Vector3(direction.x,0f,direction.z);
		if(Vector3.Dot (this.transform.forward,knockVec)>= 0){
			isKnockedB = true;
		}
		else{
			isKnocked = true;
		}
		Invoke ("UnKnock",0.5f);
	}
	public void KnockBack(Transform nme){
		if(Vector3.Dot (this.transform.forward,nme.forward)>= 0){
			isKnockedB = true;
		}
		else{
			isKnocked = true;
		}
		Invoke ("UnKnock",0.5f);
	}

	public void Rolling (Vector3 roller){
	}

	public void NoClip(){
		myCont.enabled = !myCont.enabled;
	}
	private void CanEdge(){
		canEdge = true;
		floating = false;
		isSprinting = false;
	}
	public void Apparition(){
		myRender.enabled = true;
		myClothCollider.enabled = true;
		myCont.enabled = true;
		if(myShadow != null){
			myShadow.gameObject.SetActive(true);
		}
		myDenture.GetComponent<CapsuleCollider>().enabled = true;
		if(Manager.Instance.IsRespawning){
			animator.Play("spawn");
			Manager.Instance.IsRespawning = false;
		}
	}
	public void Disparition(){
		if(myClothCollider != null){
			myClothCollider.enabled = false;
		}
		if(myCont != null){
			myCont.enabled = false;
		}
		if(myDenture != null && myDenture.GetComponent<CapsuleCollider>()!=null){
			myDenture.GetComponent<CapsuleCollider>().enabled = false;
		}

		if(myShadow != null){
			myShadow.gameObject.SetActive(false);
		}
		if(myRender!=null){
			myRender.enabled = false;
		}
	}

	private void Edging(){
		if(canEdge && Physics.SphereCast(new Vector3 (this.transform.position.x,this.transform.position.y +1f,this.transform.position.z),0.6f,-Vector3.up, out edgeHit,0.4f)){
			stickForce = 0f;
			edgeDistance = this.transform.position - edgeHit.point;
			edgeDistance = new Vector3(edgeDistance.x,0f,edgeDistance.z);
			edgeDistance = edgeDistance.normalized;
			edgeDistance *= 0.25f;
			vecDep += edgeDistance;
		}
	}


	public void Transparency(){
		myRender.materials = matFade;
		isTransparent = true;
	}
	public void Opacity(){
		myRender.materials = matReal;
		isTransparent = false;
	}

	public void SetAnim(){
		animator.SetFloat("StickForce",stickForce);
		animator.SetFloat("StickForceDamp",stickForceDamp);
		animator.SetFloat("ImpulSautY",impulSaut.y);
		animator.SetBool ("IsGrounded",isGrounded);
		animator.SetBool ("IsSprinting",isSprinting);
		animator.SetBool ("IsOnWall",onWall);
		animator.SetBool ("InAir",inAir);
		animator.SetBool ("HasJumped",hasJumped);
		animator.SetBool ("IsFromWall",isFromWall);
		animator.SetBool ("IsKnocked",isKnocked);
		animator.SetBool ("IsKnockedB",isKnockedB);
		animator.SetBool ("IsOnBar",onBar);
		animator.SetBool ("IsFromBar",isFromBar);
		animator.SetBool ("WillBarTurn", willBarTurn);
		animator.SetBool ("InBarTrigger", inBarTrigger);
		animator.SetBool ("OnShoulderFix", onShoulderFix);
		animator.SetBool ("InHandsFix", inHandsFix);
		animator.SetBool ("HasBeenLaunched", hasBeenLaunched);
		animator.SetBool ("SprintJumpFromLeft", sprintJumpFromLeft);
		animator.SetBool ("IsFromJump", isFromJump);
		animator.SetBool ("IsFromSprint", isFromSprint);
	}

	#region Bool
	//********Invoke de Bool********//

	private void WillCeiling(){
		canCeiling = true;
	}
	private void CanJump(){
		canJump = true;
	}
	private void Unstick(){
		onSticky = false;
		if(Manager.Instance.InBackstage){
			avatarSFX.SetHeavySlideEffect(true,false);
			avatarSFX.SetHeavySlideEffect(true,true);
		}
		else{
			avatarSFX.SetSlideEffect(true,false);
			avatarSFX.SetSlideEffect(true,true);
		}
		if(onWall){
			Invoke ("WillFall",0.2f);
		}
	}
	private void WillFall(){
		canFall = true;
	}
	private void IsFromWallNoMore(){
		//Debug.Log("IsFromWallNoMore");
		isFromWall = false;
	}
	private void IsFromBarNoMore(){
		isFromBar = false;

	}
	private void IsFromJumpNoMore(){
		isFromJump = false;
	}
	private void Baldness(){
		inAir = false;
	}
	private void HasJumpedNoMore(){
		hasJumped = false;
	}
	private void GetInAir(){
		inAir = true;
	}
	private void UnKnock(){
		isKnocked = false;
		isKnockedB = false;
	}
	//******************************//
	#endregion


	#region Accesseurs
	//******ACCESSEUR******//
	public bool InHandsFix {
		get {
			return inHandsFix;
		}
		set {
			inHandsFix = value;
		}
	}
	public bool TowardBar {
		get {
			return towardBar;
		}
		set {
			towardBar = value;
		}
	}
	public Vector3 ImpulSaut {
		get {
			return impulSaut;
		}
		set {
			impulSaut = value;
		}
	}

	public bool InElevator {
		get {
			return inElevator;
		}
		set {
			inElevator = value;
		}
	}
	
	public bool OnShoulderFix {
		get {
			return onShoulderFix;
		}
		set {
			onShoulderFix = value;
		}
	}
	
	public bool WithArms {
		get {
			return withArms;
		}
		set {
			withArms = value;
		}
	}

	public bool IsRolling {
		get {
			return isRolling;
		}
		set {
			isRolling = value;
		}
	}
	public float RealDistanceJump {
		get {
			return realDistanceJump;
		}
	}

	public bool HasBeenLaunched {
		get {
			return hasBeenLaunched;
		}
		set {
			hasBeenLaunched = value;
		}
	}

	public CharacterController MyCont{
		get{return myCont;}
	}

	public float RealSprintDistanceJump {
		get {
			return realSprintDistanceJump;
		}
		set {
			realSprintDistanceJump = value;
		}
	}

	public bool IsInvincible {
		get {
			return isInvincible;
		}
		set {
			isInvincible = value;
		}
	}

	public bool IsGrounded {
		get {
			return isGrounded;
		}
		set {
			isGrounded = value;
		}
	}

	public bool InAir {
		get {
			return inAir;
		}
		set {
			inAir = value;
		}
	}

	public bool IsSprinting {
		get {
			return isSprinting;
		}
		set {
			isSprinting = value;
		}
	}

	public bool OnWall {
		get {
			return onWall;
		}
		set {
			onWall = value;
		}
	}

	public bool CanSticky {
		get {
			return canSticky;
		}
		set {
			canSticky = value;
		}
	}

	public bool OnSticky {
		get {
			return onSticky;
		}
		set {
			onSticky = value;
		}
	}

	public bool CanFall {
		get {
			return canFall;
		}
		set {
			canFall = value;
		}
	}

	public bool IsCeiling {
		get {
			return isCeiling;
		}
		set {
			isCeiling = value;
		}
	}

	public bool HasRightFoot {
		get {
			return hasRightFoot;
		}
		set {
			hasRightFoot = value;
		}
	}

	public bool CanCeiling {
		get {
			return canCeiling;
		}
		set {
			canCeiling = value;
		}
	}

	public bool CanStartMAC {
		get {
			return canStartMAC;
		}
		set {
			canStartMAC = value;
		}
	}

	public bool IsFromWall {
		get {
			return isFromWall;
		}
		set {
			isFromWall = value;
		}
	}

	public Light MyLight {
		get {
			return myLight;
		}
		set {
			myLight = value;
		}
	}

	public bool IsWallFiring {
		get {
			return isWallFiring;
		}
		set {
			isWallFiring = value;
		}
	}

	public bool HasJumped {
		get {
			return hasJumped;
		}
		set {
			hasJumped = value;
		}
	}

	public bool IsFromJump {
		get {
			return isFromJump;
		}
		set {
			isFromJump = value;
		}
	}

	public bool IsKnocked {
		get {
			return isKnocked;
		}
		set {
			isKnocked = value;
		}
	}

	public bool IsDazed {
		get {
			return isDazed;
		}
		set {
			isDazed = value;
		}
	}

	public bool OnBar {
		get {
			return onBar;
		}
		set {
			onBar = value;
		}
	}

	public bool IsFromBar {
		get {
			return isFromBar;
		}
		set {
			isFromBar = value;
		}
	}

	public bool InTransit {
		get {
			return inTransit;
		}
		set {
			inTransit = value;
		}
	}

	public bool OnShoulderOneTime {
		get {
			return onShoulderOneTime;
		}
		set {
			onShoulderOneTime = value;
		}
	}

	public bool OnPause {
		get {
			return onPause;
		}
		set {
			onPause = value;
		}
	}

	public bool IsDead {
		get {
			return isDead;
		}
		set {
			isDead = value;
		}
	}

	public Arms_Behavior MyBrutus {
		get {
			return myBrutus;
		}
		set {
			myBrutus = value;
		}
	}

	public bool InTheRightZone {
		get {
			return inTheRightZone;
		}
		set {
			inTheRightZone = value;
		}
	}

	public bool HasLeftFoot {
		get {
			return hasLeftFoot;
		}
		set {
			hasLeftFoot = value;
		}
	}

	public Vector3 MyTargetFoo {
		get {
			return myTargetFoo;
		}
		set {
			myTargetFoo = value;
		}
	}

	public Bar MyOldBar {
		get {
			return myOldBar;
		}
		set {
			myOldBar = value;
		}
	}

	public Vector3 Push
	{
		set{push = value;}
	}

	public AvatarSFX AvatarSFX
	{
		get{return avatarSFX;}
		set{avatarSFX = value;}
	}
	#endregion

}