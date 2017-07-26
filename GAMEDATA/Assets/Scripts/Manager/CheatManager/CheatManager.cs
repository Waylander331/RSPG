using UnityEngine;
using System.Collections;

public class CheatManager : Singleton<CheatManager>
{
	private bool cheatEnabled;

	// Invincibility
	private bool invincible;
	private bool toggledOnFreeCam;

	// BulletTime
	private bool bulletTime;
	private bool infiniteBulletTime;
	
	// Pause
	private bool paused;

	private bool holdingHKey;

	// GhostMode Variables (cam)
	private bool camBehaviourEnabled;
	private bool modCamEnabled;
	private float mouseSpeed = 95;
	private float moveSpeed = 15;

	// NoClip
	private bool noClip;

	// Fix Avatar
	private bool fixedAvatar;
	private float fixedAvatarDistance = 4f;

	private bool showBubble;

	// Avatar State
	private bool wasOnBar;
	private bool wasInAir;

	private FrameRate myFR;

	private static CheatManager instance;

	void Start()
	{
		if(Manager.Instance.LevelName == "Splash" || Manager.Instance.LevelName == "EcranTitre" || Manager.Instance.LevelName == "Credits" 
		   || Manager.Instance.LevelName == "GalleryModels" || Manager.Instance.LevelName == "CINEMATIQUE_FIN" || Manager.Instance.LevelName == "Save_And_Quit"){
			cheatEnabled = false;
		}

		myFR = GetComponent<FrameRate>();
		myFR.enabled = false;
		
		// set camBehaviourEnabled and modCamEnabled to current value
		Camera cam = Camera.main;
		if (cam == null) 
			Debug.LogWarning ("No main camera detected");
		else
		{
			modCam mod_Cam = cam.GetComponent<modCam>();
			camBehavior cam_behaviour = cam.GetComponent<camBehavior>();

			if(mod_Cam == null) 
			{
				Debug.LogWarning ("No modCam Script on Main Cam");
				mod_Cam = cam.gameObject.AddComponent<modCam>();
				mod_Cam.enabled = false;
			}
			modCamEnabled = mod_Cam.enabled;

			if(cam_behaviour == null) 
			{
				Debug.LogWarning("No camBehaviour script on Main Cam");
				cam_behaviour = cam.gameObject.AddComponent<camBehavior>();
				cam_behaviour.enabled = true;
			}
			camBehaviourEnabled = cam_behaviour.enabled;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		EssenceMemeDuSingleton();
		
			Camera cam = Camera.main;
			if (cam == null) 
				Debug.LogWarning ("No main camera detected");
			else
			{
				modCam mod_Cam = cam.GetComponent<modCam>();
				camBehavior cam_behaviour = cam.GetComponent<camBehavior>();
				
				if(mod_Cam == null) 
				{
					mod_Cam = cam.gameObject.AddComponent<modCam>();
					mod_Cam.enabled = false;
				}
				modCamEnabled = mod_Cam.enabled;
				
				if(cam_behaviour == null) 
				{
					cam_behaviour = cam.gameObject.AddComponent<camBehavior>();
					cam_behaviour.enabled = true;
				}
				camBehaviourEnabled = cam_behaviour.enabled;
			}
	}

	void LateUpdate()
	{
		if (Input.GetKeyDown (KeyCode.F12))
		{
			if(Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits"
			   && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit"){
				SetCheatsActive (!cheatEnabled);
				//Debug.Log (cheatEnabled ? "Cheats are active" : "Cheats are unactive");
				if (Camera.main.GetComponent<modCam> () == null) 
				{
					//Debug.Log ("No modCam");
					Camera.main.gameObject.AddComponent<modCam> ();
				}
			}
		}

		if (cheatEnabled) 
		{	
			// ================
			// INVINCIBILITY
			// ================
			
			// Toogle Invincibility
			if (Input.GetKeyDown (KeyCode.I) && IsDefault()) 
			{
 				Invincible();
			}

			// ================
			// SHOW BUBBLES
			// ================
			if(Input.GetKeyDown (KeyCode.T))
				FeBubble();

			// ================
			// GHOST MODE
			// ================

			if(Input.GetKeyDown (KeyCode.F)){
				if(myFR.isActiveAndEnabled){
					myFR.enabled = false;
				}
				else{
					myFR.enabled = true;
				}
			}

			// Toggle Free cam
			if (Input.GetKeyDown (KeyCode.G)) 
			{
				if(noClip) NoClip();
				if (IsDefault () || IsFixed ())
				{
					MakeFree ();
				}
				else
				{
					MakeDefault ();
				}
			}
			
			// Return
			if (Input.GetKeyDown (KeyCode.Escape) && (IsFixed () || IsFree ()))
			{
				bool tempPause = false;
				if(paused) {UnPause (); tempPause = true;}
				MakeDefault ();
				if(noClip) NoClip ();
				if(tempPause) Pause ();

			}
			
			// Respawn player at Cam pos
			if (Input.GetKeyDown (KeyCode.Return) && (IsFree () || IsFixed ())) 
			{
				bool tempPause = false;

				if(paused) {UnPause(); tempPause = true;}
				if(!noClip)
					SpawnAtPos ();
				else
					NoClip();

				MakeDefault ();
				if(tempPause) Pause ();
			}
			
			// Fix Cam
			if (Input.GetKeyDown (KeyCode.Space) && IsFree ())
				MakeFixed ();
			
			// ================
			// GET STARS
			// ================
			
			if (Input.GetKeyDown (KeyCode.Equals)) {
				Manager.Instance.GotStar ();
				Manager.Instance.ShowStars ();
			}
			
			if (Input.GetKeyDown (KeyCode.Minus)) {
				Manager.Instance.RevertStar ();
				Manager.Instance.ShowStars ();
			}
			
			
			// ================
			// LOAD LEVEL
			// ================
			
			// Load specific level
			if (Input.GetKeyDown (KeyCode.Keypad0)){
				LoadingScreen.Instance.level.text = "Ecran_Titre";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (1);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad1)){
				LoadingScreen.Instance.level.text = "00_Hub";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (2);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad2)){
				LoadingScreen.Instance.level.text = "01_Manege";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (3);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad3)){
				LoadingScreen.Instance.level.text = "02_Fete_Foraine";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (4);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad4)){
				LoadingScreen.Instance.level.text = "03_Carrousel";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (5);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad5)){
				LoadingScreen.Instance.level.text = "04_Ronny";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (6);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad6)){
				LoadingScreen.Instance.level.text = "05_FreakShow";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (7);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad7)){
				LoadingScreen.Instance.level.text = "Tuto";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (8);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad8)){
				LoadingScreen.Instance.level.text = "BossFight";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (9);
				Manager.Instance.Avatar.OnPause = true;
			}
			if (Input.GetKeyDown (KeyCode.Keypad9)){
				LoadingScreen.Instance.level.text = "";
				LoadingScreen.Instance.mainButton[3].SetActive(true);
				Manager.Instance.LevelTransit (10);
				Manager.Instance.Avatar.OnPause = true;
			}

			
			// Reload level
			if (Input.GetKeyDown (KeyCode.R)) {
				ReloadLevel ();
				Manager.Instance.Avatar.OnPause = true;
			}
			
			// ================
			// HYPE
			// ================
			
			if (holdingHKey && !Input.GetKey (KeyCode.H))
				holdingHKey = false;
			
			// Toggle bulletTime
			if (Input.GetKeyDown (KeyCode.H)) {
				bulletTime = !bulletTime;
				if (bulletTime) {
					CheatBulletTime ();
				} else {
					Manager.Instance.BulletTimeStop ();
					infiniteBulletTime = false;
					holdingHKey = false;
					CancelInvoke ("InfiniteBulletTime");
				}
			}
			
			// ================
			// PAUSE
			// ================
			
			if (Input.GetKeyDown (KeyCode.P)) 
			{
				if(paused) UnPause ();
				else Pause();

				if(IsFree()) Manager.Instance.Avatar.OnPause = true;
			}

			// ================
			// NO CLIP
			// ================

			if (Input.GetKeyDown (KeyCode.N)) 
				NoClip();

			if(fixedAvatar)
			{
				Transform mainCam = Camera.main.transform;
				Manager.Instance.Avatar.transform.position = mainCam.position + mainCam.forward * fixedAvatarDistance;
			}
		}
	}

	// ================
	// GHOSTMODE FUNCTIONS
	// ================


	// Enable or disable Behaviour
	void SetCamBehaviour (bool set)
	{
		Camera.main.GetComponent<camBehavior>().enabled = camBehaviourEnabled = set;
	}

	// Make Mod Cam movable or not
	void SetModCam(bool set)
	{
		modCam mod_Cam = Camera.main.GetComponent<modCam>();
		mod_Cam.enabled = modCamEnabled = set;;
		mod_Cam.MouseSensitivity = mouseSpeed;
		mod_Cam.MoveSensitivity = moveSpeed;
	}
	
	void SpawnAtPos()
	{
		Manager.Instance.Avatar.transform.position = Camera.main.transform.position;
		Camera.main.transform.Translate(0f, 0f, -Manager.Instance.Avatar.transform.forward.z, Space.World);
		SetModCam (false);
	}

	// Default Cam
	void MakeDefault()
	{
		if (wasInAir || wasOnBar)
			ResetAvatarVariables ();

		wasInAir = wasOnBar = false;
		SetCamBehaviour (true);
		SetModCam (false);
		if (toggledOnFreeCam) 
		{
			toggledOnFreeCam = false;
			invincible = false;
			Manager.Instance.Avatar.SetInvincibility();
		}
		Manager.Instance.Avatar.OnPause = false;
	}

	void MakeFixed()
	{
		SetCamBehaviour (false);
		SetModCam (false);
		if (toggledOnFreeCam) 
		{
			toggledOnFreeCam = false;
			invincible = false;
			Manager.Instance.Avatar.SetInvincibility();
		}
		Manager.Instance.Avatar.OnPause = false;
	}

	// Make a free Cam
	void MakeFree()
	{
		// Check avatar state to prevent glitches
		wasOnBar = Manager.Instance.Avatar.OnBar;
		wasInAir = Manager.Instance.Avatar.InAir;

		Manager.Instance.Avatar.OnPause = true;
		SetCamBehaviour (false);
		SetModCam(true);
		if (!invincible) 
		{
			toggledOnFreeCam = true;
			invincible = true;
			Manager.Instance.Avatar.SetInvincibility();
		}
	}

	bool IsDefault(){return camBehaviourEnabled && !modCamEnabled ? true : false;}
	bool IsFixed(){return !camBehaviourEnabled && !modCamEnabled ? true : false;}
	bool IsFree(){return !camBehaviourEnabled && modCamEnabled ? true : false;}

	// ================
	// LOAD FUNCTIONS
	// ================
	void ReloadLevel()
	{
		Manager.Instance.LevelTransit (Manager.Instance.CurrentLevel);
	}

	// ================
	// HYPE FUNCTIONS
	// ================

	// Alternate bullet time for cheat mode
	void CheatBulletTime()
	{
		Manager.Instance.BulletTime(); // Bullet time lasts
		Manager.Instance.CancelInvoke("BulletTimeStop");
		holdingHKey = true;
		Invoke ("InfiniteBulletTime", 0.25f * Time.timeScale);
		Invoke ("CheatBulletTimeStop", 3f * Time.timeScale);

	}

	// Alternate bullet time stop for cheat mode
	void CheatBulletTimeStop()
	{
		if (!infiniteBulletTime) 
		{
			Manager.Instance.BulletTimeStop ();
			bulletTime = false;
			CancelInvoke ("InfiniteBulletTime");
		}
	}

	void InfiniteBulletTime()
	{
		if (holdingHKey)
			infiniteBulletTime = true;
	}

	void SetCheatsActive(bool set)
	{
		cheatEnabled = set;
		if (!set) 
		{
			// Turn off all cheats.
			MakeDefault ();
			if (invincible)
			{
				Manager.Instance.Avatar.SetInvincibility ();
				invincible = false;
			}
			CancelInvoke ("InfiniteBulletTime");
			CancelInvoke ("CheatBulletTimeStop");
			holdingHKey = false;
			infiniteBulletTime = false;
			if (bulletTime) 
			{
				CheatBulletTimeStop ();
				bulletTime = false;
			}
			if(paused) UnPause();
			if(noClip) NoClip();
		}
	}

	void FixAvatar(bool set)
	{
		fixedAvatar = set;

		bool tempPause = false;
		if(paused) {UnPause (); tempPause = true;}

		// Set noClip
		if (noClip != set)
		{
			Manager.Instance.Avatar.NoClip ();
			noClip = !noClip;
		}

		// Fix or release Avatar
		if (set) 
		{
			MakeFree ();
			Transform mainCam = Camera.main.transform;
			Manager.Instance.Avatar.transform.position = mainCam.position + mainCam.forward * fixedAvatarDistance;
		}
		else
			MakeDefault();

		if(tempPause) Pause ();
	}

	// ================
	// INVINCIBILITY FUNCTIONS
	// ================
	void Invincible()
	{
		Manager.Instance.Avatar.SetInvincibility ();
		invincible = !invincible;
	}

	// ================
	// NOCLIP FUNCTIONS
	// ================

	void NoClip()
	{
		noClip = !noClip;
		Manager.Instance.Avatar.NoClip ();
		FixAvatar (noClip);
	}

	// ================
	// PAUSE FUNCTIONS
	// ================

	void Pause()
	{
		Manager.Instance.Avatar.OnPause = true;
		Manager.Instance.Pause ();
		paused = true;
	}

	void UnPause()
	{
		Manager.Instance.Avatar.OnPause = false;
		Manager.Instance.UnPause ();
		paused = false;
	}

	// ================
	// FÉ FUNCTIONS
	// ================
	void FeBubble()
	{
		showBubble = !showBubble;
		int foo = 0;
		Manager.Instance.Avatar.trail = !Manager.Instance.Avatar.trail;
		if (!Manager.Instance.Avatar.trail) {
			Debug.Log ("kekou");
			WhatAmI[] trail = FindObjectsOfType (typeof(WhatAmI)) as WhatAmI[];
			foreach (WhatAmI temp in trail) {
				Destroy (temp.gameObject);
				Debug.Log (foo++);
			}
		}
	}

	// ================
	// FIX FUNCTIONS
	// ================
	
	void ResetAvatarVariables()
	{
		Manager.Instance.Avatar.ImpulSaut = Vector3.zero;
		if (wasOnBar) Manager.Instance.Avatar.MyHand ();
		Manager.Instance.Avatar.PostBarStuff ();
		Manager.Instance.Avatar.Push = Vector3.down * Manager.Instance.Avatar.speed * 1.2f;
		Manager.Instance.Avatar.HasBeenLaunched = false;
	}

	void EssenceMemeDuSingleton(){		
		if(CheatManager.Instance == null){
			instance = this;
		}
		else{
			if(CheatManager.Instance != this){
				Destroy(this.gameObject);
			}			
		}
	}

	public bool CheatEnabled {
		get {return cheatEnabled;}
	}

	public static CheatManager Instance {
		get {return instance;}
	}
}
