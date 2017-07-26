using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;

public class Manager : MonoBehaviour {

	#region Variables

	//Game Info
	private Avatar av;
	private camBehavior mc;
	private int collectibleCount = 0;
	private int currentLevel = 0;
	private int gameLevel;
	private string levelName;
	private bool isFromStart;
	private bool isFromCin;
	private bool cinWasPlayed;
	private bool isNewGame = true;
	private bool haveContinue = false;

	//Level loads
	private AsyncOperation ao;
	private List<Transform> spawnPoints = new List<Transform>(); 
	private Transform position;
	private Vector3 camPos;
	private Vector3 camRot;
	private int levelToLoad;
	private int spawnID = 0;
	private bool isRespawning;
	private bool loadingScreen;
	private bool lookForCam;
	private bool canLoad;

	//Saves 
	private string saveFile = "/Save_Info.dat";
	private GameData[] data = new GameData[3];
	private int saveNumber = 0;
	
	//Star Screens
	private Texture2D[] images;
	private List<byte[]> imageStock = new List<byte[]>(); 

	//Star Info
	public Stars starPrefab;
	private int[] starBin; //contient infos sur etoiles prises ou non sur 3 bits
	private Stars[] stars = new Stars[3];
	private bool haveAllStars1 = false;
	private bool haveAllStars2 = false;
	private bool haveAllStars3 = false;
	private bool haveAllStars4 = false;
	private bool haveAllStars5 = false;
	private bool showMenuOnce1 = true;
	private bool showMenuOnce2 = true;
	private bool showMenuOnce3 = true;
	private bool showMenuOnce4 = true;
	private bool showMenuOnce5 = true;
	private int starLvl1 = 0;
	private int starLvl2 = 0;
	private int starLvl3 = 0;
	private int starLvl4 = 0;
	private int starLvl5 = 0;

	//Collectible Info
	public Collectible collectiblePrefab;
	private int[] collectibleBin; //contient infos sur collectibles prises ou non sur 3 bits
	private Collectible[] collectibles = new Collectible[9];
	private int collectibleCountChange = 0;
	private int collLvl1 = 0;
	private int collLvl2 = 0;
	private int collLvl3 = 0;
	private int collLvl4 = 0;
	private int collLvl5 = 0;

	//Gallery
	private int collectibleCountBonus = 0;
	private int galleryCount = 0;
	private bool[] unlockByCount;
	private bool[] unlockByLvl;

	//Hub States
	[Range(0,16)]
	private int totalStarsTaken = 0;
	private bool tutorialDone = false;

	//Select
	[Range (0,3)]
	public float selectPressTime = 1f; //temps qu'on le garde enfonce pour retourner au hub
	private float selectCounter = 0f;
	private bool selectIsPressed;

	//Pause
	static bool isPaused;
	private float prePauseTimeScale;
	private float prePauseFixedDelta;
	private float musicVolume;
	private float soundVolume;

	//Bullet Time
	[Range(0,1)]
	public float slowFactor = 0.4f;
	[Range (0,5)]
	public float slowTimeMax = 3.0f;
	private bool isSlow;
	private bool isHyped;

	//Voodoo Singletonnien
	public static Manager instance;

	//Checkpoints
	private CheckPoint[] checkpoints;
	private int currentCheckpoint = -1;

	public Rezbox rezBox;

	// For Elevator
	// to prevent cutscene with respawn
	private bool reloading;

	// Backstage
	private bool inBackstage;

	#endregion	

	#region Unity game functions
	void Awake(){

		EssenceMemeDuSingleton();

		if(Manager.Instance==this){	
			DontDestroyOnLoad(this.gameObject);
			if(Application.loadedLevelName != "EcranTitre"){ 
				isFromStart = true;
			}

			saveNumber = 0;
			spawnID = 0;

			SetMyBools();	
			Setup ();

			starBin = new int[7] {0,7,7,7,7,7,7}; //6 levels (=index) et 7 est representation binaire des etoiles.
			collectibleBin = new int[6] {0,511,511,511,511,511};
			unlockByLvl = new bool[5];
			unlockByCount = new bool[15];

			images = new Texture2D[15];
			for(int i = 0; i < images.Length; i++) images[i] = null;

			currentLevel = Application.loadedLevel;
			levelToLoad = Application.loadedLevel;
			levelName = Application.loadedLevelName;			
			if (currentLevel >= 2) gameLevel = currentLevel - 2;
			
			if(levelName != "Splash" && levelName != "EcranTitre" && levelName != "Credits" && levelName != "GalleryModels" && levelName != "CINEMATIQUE_FIN" && levelName != "Save_And_Quit") { //LEVELS
				InstantiateStars();
				SetUpStars();
				InstantiateCollectibles();
				SetUpCollectibles();
				SpawnAvatar();
			}

			RefreshCheckpointArray ();
		}
	}

	void Start()
	{
		Invoke ("Started", 1f);
	}

	void Started(){
		canLoad = true;
		isFromStart = false;
	}

	void Update(){

		if((Input.GetButtonDown("HypeY")|| Input.GetButtonDown("Bumper")) && isHyped){
			BulletTime();
		}

		if(Input.GetButtonDown("Cancel") && !isRespawning && canLoad && !CheatManager.Instance.CheatEnabled && (levelName != "Splash" && levelName != "EcranTitre" && levelName != "Credits" && levelName != "GalleryModels" && levelName != "CINEMATIQUE_FIN" && levelName != "Save_And_Quit")) Pause();

		if(Input.GetButtonDown ("Pause")){
			if(isPaused) UnPause();
			else if (!isRespawning && !av.InElevator && levelName != "Splash" && levelName != "EcranTitre" && levelName != "Credits" && levelName != "GalleryModels" && levelName != "CINEMATIQUE_FIN" && levelName != "Save_And_Quit") Pause();
		}

		if(Input.GetButtonDown ("Back") && currentLevel != 0){
			selectCounter = 0f;
			selectIsPressed = true;
		}
		if(Input.GetButtonUp ("Back")) selectIsPressed = false;
		if (selectIsPressed && !isPaused){
			selectCounter += Time.deltaTime * (1/Time.timeScale); 
			if (selectCounter >= selectPressTime) {
				//LevelTransit(2); ******
				Save ();
				Application.LoadLevel("0_HUB_YL");
				selectIsPressed = false;
			}
		}

	//Gestion des evenements du HUB

		//Regarde que si le collectibleCount change
		if(collectibleCountChange != galleryCount){
			UnlockGallery();
			collectibleCountChange = galleryCount;
		}

		if(ao != null){

			if(!ao.isDone && lookForCam/* && loadingScreen*/){ 

				camPos = GetCamDistance();
				camRot = mc.transform.forward;
			}
		}
		else if (Application.isEditor && lookForCam && loadingScreen){
			av.transform.position = spawnPoints[0].position; //lock l'avatar
			camPos = GetCamDistance(); //memorise la position de la cam
			camRot = mc.transform.forward;
		}
	}
	
	void OnLevelWasLoaded(int level){

		if(Manager.Instance == this){ //Singleton check. Evite d'avoir plein d'etoiles

			if (level != currentLevel || isFromStart)
			{
				reloading = false;
				isRespawning = false;
			}
			else
			{
				reloading = true;
			}

			currentLevel = level;
			if(currentLevel != levelToLoad) levelToLoad = currentLevel;

			if(!reloading)
			{
				RefreshCheckpointArray();
				currentCheckpoint = -1;
				Setup();
				inBackstage = false;
			}
			else
			{
				if(currentCheckpoint != -1)
				{
					inBackstage = checkpoints[currentCheckpoint].isInBackstage;
				}
			}

			canLoad = true;

			InitializeLevel();

			if(lookForCam && loadingScreen){
				mc.transform.position = SetCamDistance();
				mc.transform.forward = camRot;
				lookForCam = false;
			}
			isNewGame = false;
			haveContinue = false;
		}

	}
	#endregion

	#region Level Loads
	/// <summary>
	/// Transition de niveau
	/// </summary>
	/// <param name="levelToLoad">Level to load.</param>
	public void LevelTransit(int levelToLoad){
		if(isSlow) { //cancels bullet time
			CancelInvoke("BulletTimeStop");
			BulletTimeStop();
		}

		if(canLoad){
			lookForCam = true;
			this.levelToLoad = levelToLoad;
		
			if(Application.isEditor) Application.LoadLevel(levelToLoad);
			else {
				StartCoroutine("LevelLoad");
			}
		}
	}

	public void LevelTransit(int levelToLoad, int spawn){

		if(isSlow) { //cancels bullet time
			CancelInvoke("BulletTimeStop");
			BulletTimeStop();
		}

		if(canLoad){

			Save ();

			lookForCam = true;
			this.levelToLoad = levelToLoad;

			if(Application.isEditor) Application.LoadLevel(levelToLoad);
			else {
				StartCoroutine("LevelLoad");
			}
		}
	}

	//Pour Respawn
	public void LevelTransit(){
		if(isSlow) { 
			CancelInvoke("BulletTimeStop");
			BulletTimeStop();
		}

		if(canLoad){
			lookForCam = true;
			
			if(Application.isEditor) Application.LoadLevel(levelToLoad);
			else {
				StartCoroutine("LevelLoad");
			}
		}
		else if (Application.isEditor) Debug.Log("Je ne peux pas loader en ce moment. Probablement parce que je load deja le niveau " + levelToLoad);
	} 

	IEnumerator LevelLoad(){

		ao = Application.LoadLevelAsync(levelToLoad);
		while(!ao.isDone){
			yield return null;
		}
	}

	#endregion

	#region Level Init
	/// <summary>
	/// Initializes the level.
	/// </summary>
	/// <param name="level">Level.</param>
	public void InitializeLevel(){

		levelName = Application.loadedLevelName;

		if (currentLevel >= 2) gameLevel = currentLevel - 2;
		isHyped = false;

		if(levelName != "Splash" && levelName != "EcranTitre" && levelName != "Credits" && levelName != "GalleryModels" && levelName != "CINEMATIQUE_FIN" && levelName != "Save_And_Quit") { //LEVELS
			InstantiateStars();
			SetUpStars();
			InstantiateCollectibles();
			SetUpCollectibles();
			SpawnAvatar();
		}
	}

	public void InitializeLevel(int spawn){

		levelName = Application.loadedLevelName;
		
		if (currentLevel >= 2) gameLevel = currentLevel - 2;
		isHyped = false;
		
		if(levelName != "Splash" && levelName != "EcranTitre" && levelName != "Credits" && levelName != "GalleryModels" && levelName != "CINEMATIQUE_FIN" && levelName != "Save_And_Quit") { //LEVELS
			InstantiateStars();
			SetUpStars();
			InstantiateCollectibles();
			SetUpCollectibles();
			SpawnAvatar();
		}
	}

	public void SpawnAvatar(){
 
		av = GameObject.FindGameObjectWithTag ("Player").GetComponentInChildren<Avatar>();

		if(currentCheckpoint != -1)
		{
			position = checkpoints [currentCheckpoint].RespawnPoint;
		}
		else if(spawnPoints.Count > 0 && spawnID < spawnPoints.Count){

			if(spawnPoints[spawnID] != null)
				position = spawnPoints[spawnID];
			else
			{
				Setup();
				if(spawnPoints[spawnID] != null)
					position = spawnPoints[spawnID];
				else
				{
					position = GameObject.Find ("SpawnPoint_0").transform;
				}
			}
			position = GameObject.Find ("SpawnPoint_0").transform;
		}
		else {
			position = GameObject.Find ("SpawnPoint_0").transform;
		}

		av.transform.position = position.position;
		av.transform.rotation = position.rotation;

		mc = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<camBehavior>();

		Vector3 tempPos = av.transform.position;

		if(mc != null){
			if(mc.myFocus == null) mc.myFocus = av.transform;
			tempPos.z -= 4f;
			tempPos.y += 3f;
			mc.transform.position = tempPos;

		}
		else if (Application.isEditor)Debug.LogError ("Il n'y a pas de GameCamera dans la scene. Allez dans Assets/_Obligatoire ou faites ctrl + M");

		tempPos = av.transform.position;

		ProjectorAv proj = FindObjectOfType<ProjectorAv>();
		tempPos.y += 8f;
		proj.transform.position = av.transform.position;

		if(isRespawning){
			Instantiate(rezBox, new Vector3(position.transform.position.x, position.transform.position.y + 2f, position.transform.position.z), Quaternion.identity);
		}
	}

	Vector3 GetCamDistance(){
		float camX = av.transform.position.x - mc.transform.position.x;
		float camY = av.transform.position.y - mc.transform.position.y;
		float camZ = av.transform.position.z - mc.transform.position.z;
		return new Vector3 (camX, camY, camZ);
	}

	Vector3 SetCamDistance(){
		return new Vector3 (av.transform.position.x + camPos.x, av.transform.position.y + camPos.y, av.transform.position.z + camPos.z);
	}

	void SetMyBools(){
		//tutorialDone = false; //TODO mettre false pour version finale, pour commencer le jeu par le tutoriel
		selectIsPressed = false;
		isHyped = false;
		isSlow = false;
		isPaused = false;
		isFromStart = true;
		//isFromCin = false;
		lookForCam = false;
	}
	#endregion

	#region Star init

	void InstantiateStars(){ //TODO pourrait etre condense

		GameObject[] starPositions = GameObject.FindGameObjectsWithTag("Star");
		string temp;

		for(int i = 0; i < starPositions.Length; i++){
			stars[i] = null;
		}

		if(starPositions.Length > 0){
			for(int i = 0; i < starPositions.Length; i++){
				if(Application.isEditor)starPositions[i].transform.SetAsFirstSibling();
				temp = starPositions[i].name; 
				temp.TrimEnd(); //Au cas ou il y aurait des espaces a la fin 

				if (temp.EndsWith("1")) {
					stars[0] = Instantiate(starPrefab, starPositions[i].transform.position,  starPositions[i].transform.rotation) as Stars;
					stars[0].name = "1";
					stars[0].MyLevel = gameLevel; //ID du niveau et non du build
					stars[0].Cam = starPositions[i].GetComponentInChildren<Camera>();
					stars[0].Cam.enabled = false;
					stars[0].transform.parent = starPositions[i].transform;
				}
				else if(temp.EndsWith("2")) {
					stars[1] = Instantiate(starPrefab, starPositions[i].transform.position, starPositions[i].transform.rotation) as Stars;
					stars[1].name = "2";
					stars[1].MyLevel = gameLevel;
					stars[1].Cam = starPositions[i].GetComponentInChildren<Camera>();
					stars[1].Cam.enabled = false;
					stars[1].transform.parent = starPositions[i].transform;
				}
				else if(temp.EndsWith("3")) {
					stars[2] = Instantiate(starPrefab, starPositions[i].transform.position, starPositions[i].transform.rotation) as Stars;
					stars[2].name = "4";
					stars[2].MyLevel = gameLevel;
					stars[2].Cam = starPositions[i].GetComponentInChildren<Camera>();
					stars[2].Cam.enabled = false;
					stars[2].transform.parent = starPositions[i].transform;
				}
				else print("SVP revoir les nomenclatures d'etoile");
			}
		}
	}

	/// <summary>
	/// Sets up stars.
	/// </summary>
	public void SetUpStars(){

		if(gameLevel < 6 && stars.Length > 1){

			int temp = starBin[gameLevel];

			for (int i = (stars.Length-1); i >= 0; i--){
				if(stars[i] !=null){

					if (temp - int.Parse(stars[i].name) >=0) 
						temp -= int.Parse(stars[i].name); 

					else stars[i].Taken(true);
				}
			}
		}
	}

	#endregion

	#region Star info
	public void ShowStars() //Olivier Reid
	{
		for (int i = 0; i < starBin.Length; i++) 
		{
			int binNumber = starBin[i];

			string message = "Niveau : " + i + "\nEtoiles : ";

			if(starBin[i] == 0) message += "Aucune";
			else
			{
				bool 
					firstExists = false,
					secondExists = false,
					thirdExists = false,
					firstNumber = true;

				// Check which stars exists
				if(binNumber - 4 >= 0)
				{
					thirdExists = true;
					binNumber -= 4;
				}
				if(binNumber - 2 >= 0)
				{
					secondExists = true;
					binNumber -= 2;
				}
				if(binNumber - 1 >= 0)
				{
					firstExists = true;
				}

				// Update Message
				if(firstExists)
				{
					message += "1";
					if(firstNumber) firstNumber = false;
				}
				if(secondExists)
				{
					if(!firstNumber) message += ", ";
					else firstNumber = false;
					message += "2";
				}
				if(thirdExists)
				{
					if(!firstNumber) message += ", ";
					else firstNumber = false;
					message += "3";
				}
			}
		}
	}

	/// <summary>
	/// Get all Stars. For cheat manager.
	/// </summary>
	public void GotStar() //Olivier Reid
	{
		for(int i = 0; i < starBin.Length; i++) starBin[i] = 0;
		totalStarsTaken = 15;
		//Save ();
	}

	/// <summary>
	/// Note que l'etoile est prise en changeant la valeur binaire.
	/// </summary>
	/// <param name="whichOne">Which Star.</param>
	public void GotStar(Stars whichOne){

		if(currentLevel < 8){
			int myValue = int.Parse (whichOne.name);
			starBin[gameLevel] -= myValue;
			totalStarsTaken ++;

			if(gameLevel == 1){
				starLvl1 ++;
			} else if(gameLevel == 2){
				starLvl2 ++;
			} else if(gameLevel == 3){
				starLvl3 ++;
			} else if(gameLevel == 4){
				starLvl4 ++;
			} else if(gameLevel == 5){
				starLvl5 ++;
			}
		}
	}

	/// <summary>
	/// Revert All Stars.
	/// </summary>
	public void RevertStar() //Olivier Reid
	{
		starBin [0] = 0;
		for (int i = 1; i < starBin.Length; i++) {
			starBin [i] = 7;
			images[i-1] = null;
		}
		totalStarsTaken = 0;
	}

	/// <summary>
	/// Revert one star
	/// </summary>
	/// <param name="whichOne">Which Star.</param>
	public void RevertStar(Stars whichOne){

		int myValue = int.Parse (whichOne.name);
		starBin[gameLevel] += myValue;
		totalStarsTaken --;

		int temp = (gameLevel - 1) * 3;
		int temp2 = int.Parse (whichOne.name);
		if(temp2 == 2) temp ++;
		else if (temp2 == 4) temp += 2;
		
		images[temp] = null;
	}

	public void AddScreenshot(Texture2D img, int level, Stars star){
		int temp = (level - 1) * 3;
		int temp2 = int.Parse(star.name);

		if(temp2 == 2) temp ++;
		else if (temp2 == 4) temp += 2;

		images[temp] = img;
	}
	#endregion

	#region Collectibles Init
	void InstantiateCollectibles(){ //TODO pourrait etre condense
		
		GameObject[] collectiblesPositions = GameObject.FindGameObjectsWithTag("Collectible");
		string temp;
		
		for(int i = 0; i < collectiblesPositions.Length; i++){
			collectibles[i] = null;
		}
		
		if(collectiblesPositions.Length > 0){
			for(int i = 0; i < collectiblesPositions.Length; i++){
				collectiblesPositions[i].transform.SetAsFirstSibling();//TODO
				temp = collectiblesPositions[i].name;
				temp.TrimEnd(); //Au cas ou il y aurait des espaces a la fin 
				
				if (temp.EndsWith("1")) {
					collectibles[0] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[0].Man = this;
					collectibles[0].name = "1";
					collectibles[0].MyLevel = gameLevel; //ID du niveau et non du build
					collectibles[0].transform.parent = collectiblesPositions[i].transform;
				}
				else if(temp.EndsWith("2")) {
					collectibles[1] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[1].Man = this;
					collectibles[1].name = "2";
					collectibles[1].MyLevel = gameLevel;
					collectibles[1].transform.parent = collectiblesPositions[i].transform;
				}
				else if(temp.EndsWith("3")) {
					collectibles[2] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[2].Man = this;
					collectibles[2].name = "4";
					collectibles[2].MyLevel = gameLevel;
					collectibles[2].transform.parent = collectiblesPositions[i].transform;
				}
				else if (temp.EndsWith("4")) {
					collectibles[3] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[3].Man = this;
					collectibles[3].name = "8";
					collectibles[3].MyLevel = gameLevel; //ID du niveau et non du build
					collectibles[3].transform.parent = collectiblesPositions[i].transform;
				}
				else if(temp.EndsWith("5")) {
					collectibles[4] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[4].Man = this;
					collectibles[4].name = "16";
					collectibles[4].MyLevel = gameLevel;
					collectibles[4].transform.parent = collectiblesPositions[i].transform;
				}
				else if(temp.EndsWith("6")) {
					collectibles[5] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[5].Man = this;
					collectibles[5].name = "32";
					collectibles[5].MyLevel = gameLevel;
					collectibles[5].transform.parent = collectiblesPositions[i].transform;
				}
				else if (temp.EndsWith("7")) {
					collectibles[6] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[6].Man = this;
					collectibles[6].name = "64";
					collectibles[6].MyLevel = gameLevel; //ID du niveau et non du build
					collectibles[6].transform.parent = collectiblesPositions[i].transform;
				}
				else if(temp.EndsWith("8")) {
					collectibles[7] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[7].Man = this;
					collectibles[7].name = "128";
					collectibles[7].MyLevel = gameLevel;
					collectibles[7].transform.parent = collectiblesPositions[i].transform;
				}
				else if(temp.EndsWith("9")) {
					collectibles[8] = Instantiate(collectiblePrefab, collectiblesPositions[i].transform.position, collectiblesPositions[i].transform.localRotation) as Collectible;
					collectibles[8].Man = this;
					collectibles[8].name = "256";
					collectibles[8].MyLevel = gameLevel;
					collectibles[8].transform.parent = collectiblesPositions[i].transform;
				}
				else print("SVP revoir les nomenclatures de collectibles!");
			}
			
		}
	}

	public void SetUpCollectibles(){
		if(gameLevel < 6 && gameLevel > 0){
			
			int temp = collectibleBin[gameLevel];
			
			for(int i = (collectibles.Length-1); i >= 0; i--){
				if(collectibles[i] !=null){					
					if(temp - int.Parse(collectibles[i].name) >= 0){
						temp -= int.Parse(collectibles[i].name);
					} else collectibles[i].Taken();
				}
			}
		}
	}

	#endregion

	#region Collectibles Info
	public void GotCollectible(Collectible whichOne){
		
		if(currentLevel < 8){
			int myValue = int.Parse (whichOne.name);
			collectibleBin[gameLevel] -= myValue;
			collectibleCount ++;
			collectibleCountBonus ++;
			if(collectibleCountBonus >= 3){
				collectibleCountBonus -= 3;
				galleryCount += 1;
			}
		
			if(gameLevel == 1){
				collLvl1 ++;
			} else if(gameLevel == 2){
				collLvl2 ++;
			}
			else if(gameLevel == 3){
				collLvl3 ++;
			}
			else if(gameLevel == 4){
				collLvl4 ++;
			}
			else if(gameLevel == 5){
				collLvl5 ++;
			}
		}
	}

	void UnlockGallery(){
		unlockByCount[galleryCount-1] = true;
		if(collectibleBin[1] == 0){
			unlockByLvl[0] = true;
		} else if (collectibleBin[2] == 0){
			unlockByLvl[1] = true;
		} else if (collectibleBin[3] == 0){
			unlockByLvl[2] = true;
		} else if (collectibleBin[4] == 0){
			unlockByLvl[3] = true;
		} else if (collectibleBin[5] == 0){
			unlockByLvl[4] = true;
		}
	}

	#endregion

	#region Pause
	public void Pause(){

		isPaused = true;
		av.OnPause = true;
		prePauseTimeScale = Time.timeScale;
		prePauseFixedDelta = Time.fixedDeltaTime;
		Time.timeScale = 0f;
	}
	public void UnPause(){

		isPaused = false;
		av.OnPause = false;
		Time.timeScale = prePauseTimeScale;
		Time.fixedDeltaTime = prePauseFixedDelta;
	}
	#endregion

	#region Sauvegardes
	public void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + saveFile);

		if(data[saveNumber] == null) data[saveNumber] = new GameData();

		data[saveNumber].totalStarsTaken = totalStarsTaken;
		data[saveNumber].collectibleCount = collectibleCount;
		data[saveNumber].starBin = starBin;
		data[saveNumber].collectibleBin = collectibleBin;
		data[saveNumber].tutorialDone = tutorialDone;
		data[saveNumber].cinWasPlayed = cinWasPlayed;

		data[saveNumber].haveAllStars1 = haveAllStars1;
		data[saveNumber].haveAllStars2 = haveAllStars2;
		data[saveNumber].haveAllStars3 = haveAllStars3;
		data[saveNumber].haveAllStars4 = haveAllStars4;
		data[saveNumber].haveAllStars5 = haveAllStars5;
		data[saveNumber].showMenuOnce1 = showMenuOnce1;
		data[saveNumber].showMenuOnce2 = showMenuOnce2;
		data[saveNumber].showMenuOnce3 = showMenuOnce3;
		data[saveNumber].showMenuOnce4 = showMenuOnce4;
		data[saveNumber].showMenuOnce5 = showMenuOnce5;

		data[saveNumber].unlockByLvl = unlockByLvl;
		data[saveNumber].galleryCount = galleryCount;
		data[saveNumber].collectibleCountBonus = collectibleCountBonus; 

		data[saveNumber].collLvl1 = collLvl1;
		data[saveNumber].collLvl2 = collLvl2;
		data[saveNumber].collLvl3 = collLvl3;
		data[saveNumber].collLvl4 = collLvl4;
		data[saveNumber].collLvl5 = collLvl5;
		data[saveNumber].starLvl1 = starLvl1;
		data[saveNumber].starLvl2 = starLvl2;
		data[saveNumber].starLvl3 = starLvl3;
		data[saveNumber].starLvl4 = starLvl4;
		data[saveNumber].starLvl5 = starLvl5;

		if(images != null){

			imageStock.Clear();

			for (int i = 0; i < images.Length; i++){
				if(images[i] != null && images[i].name != "Interrogation"){
					byte[] temp = images[i].EncodeToPNG();
					imageStock.Add(temp);
				}
				else imageStock.Add (null);
			}		
			data[saveNumber].images = imageStock;
		}
		else data[saveNumber].images = null;

		bf.Serialize(file, data);
		file.Close ();
	}

	public void Load(){
		if(File.Exists (Application.persistentDataPath + saveFile)){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + saveFile, FileMode.Open); 

			data = (GameData[]) bf.Deserialize(file);
			file.Close ();

			totalStarsTaken = data[saveNumber].totalStarsTaken;
			collectibleCount = data[saveNumber].collectibleCount;
			starBin = data[saveNumber].starBin;
			collectibleBin = data[saveNumber].collectibleBin;
			tutorialDone = data[saveNumber].tutorialDone;
			cinWasPlayed = data[saveNumber].cinWasPlayed;

			haveAllStars1 = data[saveNumber].haveAllStars1;
			haveAllStars2 = data[saveNumber].haveAllStars2;
			haveAllStars3 = data[saveNumber].haveAllStars3;
			haveAllStars4 = data[saveNumber].haveAllStars4;
			haveAllStars5 = data[saveNumber].haveAllStars5;
			showMenuOnce1 = data[saveNumber].showMenuOnce1;
			showMenuOnce2 = data[saveNumber].showMenuOnce2;
			showMenuOnce3 = data[saveNumber].showMenuOnce3;
			showMenuOnce4 = data[saveNumber].showMenuOnce4;
			showMenuOnce5 = data[saveNumber].showMenuOnce5;

			unlockByLvl = data[saveNumber].unlockByLvl;
			galleryCount = data[saveNumber].galleryCount;
			collectibleCountBonus = data[saveNumber].collectibleCountBonus; 
			
			collLvl1 = data[saveNumber].collLvl1;
			collLvl2 = data[saveNumber].collLvl2;
			collLvl3 = data[saveNumber].collLvl3;
			collLvl4 = data[saveNumber].collLvl4;
			collLvl5 = data[saveNumber].collLvl5;
			starLvl1 = data[saveNumber].starLvl1;
			starLvl2 = data[saveNumber].starLvl2;
			starLvl3 = data[saveNumber].starLvl3;
			starLvl4 = data[saveNumber].starLvl4;
			starLvl5 = data[saveNumber].starLvl5;

			if(data[saveNumber].images != null){

				for (int i = 0; i < data[saveNumber].images.Count; i++){
					if(data[saveNumber].images[i] != null){

						images[i] = new Texture2D(2,2);
						images[i].LoadImage(data[saveNumber].images[i]);
					}
					else images[i] = null;
				}
			}
			else images = null;

			if(currentLevel != 1) LevelTransit(2);
		}
	}

	public void ResetSaveFile(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + saveFile);
		
		for(int i = 0; i < 3; i++){
			if(data[i] != null){
				data[i].totalStarsTaken = 0;
				data[i].collectibleCount = 0;
				data[i].starBin = new int[7] {1,7,7,7,7,7,7};
				data[i].collectibleBin = new int[6] {0,511,511,511,511,511};
				data[i].tutorialDone = false;
				data[i].cinWasPlayed = false;

				data[i].haveAllStars1 = false;
				data[i].haveAllStars2 = false;
				data[i].haveAllStars3 = false;
				data[i].haveAllStars4 = false;
				data[i].haveAllStars5 = false;
				data[i].showMenuOnce1 = false;
				data[i].showMenuOnce2 = false;
				data[i].showMenuOnce3 = false;
				data[i].showMenuOnce4 = false;
				data[i].showMenuOnce5 = false;

				if(data[i].unlockByLvl !=null){
					for (int j = 0; j < data[i].unlockByLvl.Length; j++){
						data[i].unlockByLvl[i] = false;
					}
				}
				data[i].galleryCount = 0;				
				data[i].collectibleCountBonus = 0; 
				
				data[i].collLvl1 = 0;
				data[i].collLvl2 = 0;
				data[i].collLvl3 = 0;
				data[i].collLvl4 = 0;
				data[i].collLvl5 = 0;
				data[i].starLvl1 = 0;
				data[i].starLvl2 = 0;
				data[i].starLvl3 = 0;
				data[i].starLvl4 = 0;
				data[i].starLvl5 = 0;

				for (int j = 0; j < data[saveNumber].images.Count; j++){
					if(data[saveNumber].images[j] != null)
						images[j] = null;
				}
			}
		}		
		bf.Serialize(file, data);
		file.Close ();
	}

	public void ResetSaveFile(int fileNumber){
		if(data[fileNumber] != null){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create (Application.persistentDataPath + saveFile);
			
			data[fileNumber].totalStarsTaken = 0;
			data[fileNumber].collectibleCount = 0;
			data[fileNumber].starBin = new int[7] {1,7,7,7,7,7,7};
			data[fileNumber].collectibleBin = new int[6] {0,511,511,511,511,511};
			data[fileNumber].tutorialDone = false;
			data[fileNumber].cinWasPlayed = false;

			data[fileNumber].haveAllStars1 = false;
			data[fileNumber].haveAllStars2 = false;
			data[fileNumber].haveAllStars3 = false;
			data[fileNumber].haveAllStars4 = false;
			data[fileNumber].haveAllStars5 = false;
			data[fileNumber].showMenuOnce1 = false;
			data[fileNumber].showMenuOnce2 = false;
			data[fileNumber].showMenuOnce3 = false;
			data[fileNumber].showMenuOnce4 = false;
			data[fileNumber].showMenuOnce5 = false;

			if(data[fileNumber].unlockByLvl !=null){
				for (int i = 0; i < data[fileNumber].unlockByLvl.Length; i++){
					data[fileNumber].unlockByLvl[i] = false;
				}
			}
			data[fileNumber].galleryCount = 0;
			data[fileNumber].collectibleCountBonus = 0; 
			
			data[fileNumber].collLvl1 = 0;
			data[fileNumber].collLvl2 = 0;
			data[fileNumber].collLvl3 = 0;
			data[fileNumber].collLvl4 = 0;
			data[fileNumber].collLvl5 = 0;
			data[fileNumber].starLvl1 = 0;
			data[fileNumber].starLvl2 = 0;
			data[fileNumber].starLvl3 = 0;
			data[fileNumber].starLvl4 = 0;
			data[fileNumber].starLvl5 = 0;
			
			for (int i = 0; i < data[saveNumber].images.Count; i++){
				if(data[saveNumber].images[i] != null)
					images[i] = null;
			}
			bf.Serialize(file, data);
			file.Close ();
		}
	}
	#endregion

	#region Hype
	public void BulletTime(){ // protected for cheat Manager

		isSlow = true;
		isHyped = false;
		Time.timeScale = slowFactor;
		Time.fixedDeltaTime = slowFactor * 0.02f;

		SoundManager.Instance.PlayAudio ("Hype");

		Invoke ("BulletTimeStop", slowTimeMax * Time.timeScale);
	}

	public void BulletTimeStop(){
		isSlow = false;
		Time.timeScale = 1;
		Time.fixedDeltaTime = 1f * 0.02f;
	}
	#endregion

	#region Singleton
	//Essence meme du singleton
	void EssenceMemeDuSingleton(){

		if(Manager.Instance == null){
			instance = this;
		}
		else{
			if(Manager.Instance!=this){
				DestroyImmediate(this.gameObject);
			}			
		}
	}
	#endregion

	#region Checkpoints
	//=====================
	// CHECKPOINT FUNCTIONS
	//=====================
	public void SetCheckpoint(CheckPoint newCheckpoint)
	{
		int index = GetIndex (newCheckpoint);
		if(index != -1 && (currentCheckpoint == -1 || newCheckpoint != checkpoints[currentCheckpoint]))
		{
			if(currentCheckpoint != -1) 
			{
				checkpoints[currentCheckpoint].Active = true;
				checkpoints[currentCheckpoint].CheckpointLightManager.TurnOff();
			}
			currentCheckpoint = index;
			newCheckpoint.Active = false;
			checkpoints[currentCheckpoint].CheckpointLightManager.TurnOn();
			if(!newCheckpoint.IsElevatorCheckpoint)
				SoundManager.Instance.PlayAudio("CheckPointSound");
		}
	}
	
	private int GetIndex(CheckPoint obj)
	{
		for (int i = 0; i < checkpoints.Length; i++)
			if (checkpoints [i] == obj)
				return i;
		return -1;
	}
	
	public int CurrentCheckpoint
	{
		get{return currentCheckpoint;}
		set{currentCheckpoint = value;}
	}
	
	public void RefreshCheckpointArray()
	{
		if(checkpoints != null)
		{
			for(int i = 0; i < checkpoints.Length; i++)
			{
				if(checkpoints[i] != null)
				{
					checkpoints[i].gameObject.SetActive(true);
					DestroyImmediate (checkpoints[i].gameObject);
				}
			}
		}
		GameObject[] checkpointGameObjects = GameObject.FindGameObjectsWithTag("Checkpoint");
		if(checkpointGameObjects.Length > 0)
		{
			checkpoints = new CheckPoint[checkpointGameObjects.Length];
			for (int i = 0; i < checkpointGameObjects.Length; i++)
			{
				checkpoints [i] = checkpointGameObjects [i].GetComponent<CheckPoint> ();
				DontDestroyOnLoad(checkpoints[i].gameObject);
			}
		}
	}

	// FOR SPAWN POINTS
	// ====================


	void Setup()
	{
		spawnPoints.Clear ();

		GameObject[] spawnPointsObj = GameObject.FindGameObjectsWithTag ("SpawnPoint");
		if(spawnPoints.Count == 0 || spawnPoints[0] == null){
			spawnPoints.Add (null);
			spawnPoints.Add (null);
			spawnPoints.Add (null);
			spawnPoints.Add (null);
			spawnPoints.Add (null);
			spawnPoints.Add (null);
			spawnPoints.Add (null);
			}
			
		for(int i = 0; i < spawnPointsObj.Length; i++)
		{
			if(spawnPointsObj[i] != null) 
			{
			spawnPoints.RemoveAt(spawnPointsObj[i].GetComponent<spawnpoint>().Id);
			spawnPoints.Insert(spawnPointsObj[i].GetComponent<spawnpoint>().Id, spawnPointsObj[i].transform);
			}
		}
	}

	// BACKSTAGE
	// ======================

	public void SetInBackstage(bool value)
	{
		inBackstage = value;
	}
	// ======================

	// Have allallall stars

	public bool AllStarsTaken()
	{
		return totalStarsTaken == 15;
	}

	#endregion

	#region Accesseurs
	public string SaveFile{
		get {return saveFile;}
	}
	public string LevelName{
		get {return levelName;}
	}
	public int SaveNumber{ 
		get {return saveNumber;}
		set {saveNumber = value;}
	}
	public int CurrentLevel{
		get {return currentLevel;}
	}
	public int GameLevel{
		get {return gameLevel;}
	}
	public int LevelToLoad{
		get {return levelToLoad;}
	}
	public int TotalStarsTaken{
		get {return totalStarsTaken;}
	}
	public int CollectibleCount{
		get {return collectibleCount;}
		set {collectibleCount = value;}
	}
	public float PrePauseTimeScale{
		get {return prePauseTimeScale;}
	}
	public float PrePauseFixedD{
		get {return prePauseFixedDelta;}
	}
	public float MusicVolume{
		get {return musicVolume;}
		set {musicVolume = value;}
	}
	public float SoundVolume{
		get {return soundVolume;}
		set {soundVolume = value;}
	}
	public bool TutorialDone{
		get {return tutorialDone;}
		set {tutorialDone = value;}
	}
	public bool IsHyped{
		get {return isHyped;}
		set {isHyped = value;}
	}
	public bool IsSlow{
		get {return isSlow;}
	}
	public static bool IsPaused
	{
		get{return isPaused;}
		set{isPaused = value;}
	}
	public bool IsRespawning{
		get {return isRespawning;}
		set {isRespawning = value;}
	}
	public bool LoadingScreen{
		get {return loadingScreen;}
		set {loadingScreen = value;}
	}
	public bool CanLoad{
		get {return canLoad;}
		//set {canLoad = value;}
	}
	public bool IsFromStart{
		get {return isFromStart;}
		set {isFromStart = value;}
	}
	public bool IsFromCin{
		get {return isFromCin;}
		set {isFromCin = value;}
	}
	public bool CinWasPlayed{
		get {return cinWasPlayed;}
		set {cinWasPlayed = value;}
	}
	public List<Transform> SpawnPoints{
		get {return spawnPoints;}
		set {spawnPoints = value;}
	}
	public Texture2D[] Images{
		get {return images;}
		set {images = value;}
	}
	public Avatar Avatar{ 
		get {return av;}
		set {av = value;}
	}
	public camBehavior MainCam{
		get {return mc;}
		set {mc = value;}
	}
	public Stars StarPrefab{
		get {return starPrefab;}
		set {starPrefab = value;}
	}
	public int[] StarBin{
		get {return starBin;}
	}
	public bool[] UnlockByCount {
		get {return unlockByCount;}
	}
	public bool[] UnlockByLvl {
		get {return unlockByLvl;}
		set {unlockByLvl = value;}
	}
	public static Manager Instance{
		get{return instance;}
	}
	public int GalleryCount {
		get {return galleryCount;}
		set {galleryCount = value;}
	}
	public AsyncOperation AO {
		get { return ao;}
	}
	public int SpawnID{
		get{return spawnID;}
		set{spawnID = value;}
	}
	public bool Reloading{
		get{return reloading;}
	}
	public bool InBackstage{
		get{return inBackstage;}
	}
	public bool IsNewGame {
		get {return isNewGame;}
		set {isNewGame = value;}
	}
	public bool HaveContinue {
		get {return haveContinue;}
		set {haveContinue = value;}
	}
	public bool HaveAllStars1 {
		get {return haveAllStars1;}
		set {haveAllStars1 = value;}
	}
	public bool HaveAllStars2 {
		get {return haveAllStars2;}
		set {haveAllStars2 = value;}
	}
	public bool HaveAllStars3 {
		get {return haveAllStars3;}
		set {haveAllStars3 = value;}
	}
	public bool HaveAllStars4 {
		get {return haveAllStars4;}
		set {haveAllStars4 = value;}
	}
	public bool HaveAllStars5 {
		get {return haveAllStars5;}
		set {haveAllStars5 = value;}
	}
	public bool ShowMenuOnce1 {
		get {return showMenuOnce1;}
		set {showMenuOnce1 = value;}
	}
	public bool ShowMenuOnce2 {
		get {return showMenuOnce2;}
		set {showMenuOnce2 = value;}
	}
	public bool ShowMenuOnce3 {
		get {return showMenuOnce3;}
		set {showMenuOnce3 = value;}
	}
	public bool ShowMenuOnce4 {
		get {return showMenuOnce4;}
		set {showMenuOnce4 = value;}
	}
	public bool ShowMenuOnce5 {
		get {return showMenuOnce5;}
		set {showMenuOnce5 = value;}
	}

	public int StarLvl1 {
		get {return starLvl1;}
		set {starLvl1 = value;}
	}
	public int StarLvl2 {
		get {return starLvl2;}
		set {starLvl2 = value;}
	}
	public int StarLvl3 {
		get {return starLvl3;}
		set {starLvl3 = value;}
	}
	public int StarLvl4 {
		get {return starLvl4;}
		set {starLvl4 = value;}
	}
	public int StarLvl5 {
		get {return starLvl5;}
		set {starLvl5 = value;}
	}

	public int CollLvl1 {
		get {return collLvl1;}
		set {collLvl1 = value;}
	}
	public int CollLvl2 {
		get {return collLvl2;}
		set {collLvl2 = value;}
	}
	public int CollLvl3 {
		get {return collLvl3;}
		set {collLvl3 = value;}
	}
	public int CollLvl4 {
		get {return collLvl4;}
		set {collLvl4 = value;}
	}
	public int CollLvl5 {
		get {return collLvl5;}
		set {collLvl5 = value;}
	}
	#endregion
}

#region GameData
[Serializable]
class GameData{
	public int totalStarsTaken;
	public int collectibleCount;
	public int[] starBin;
	public int[] collectibleBin;
	public bool tutorialDone;
	public bool cinWasPlayed;
	public bool haveAllStars1;
	public bool haveAllStars2;
	public bool haveAllStars3;
	public bool haveAllStars4;
	public bool haveAllStars5;
	public bool showMenuOnce1;
	public bool showMenuOnce2;
	public bool showMenuOnce3;
	public bool showMenuOnce4;
	public bool showMenuOnce5;
	public int galleryCount;
	public bool[] unlockByLvl;
	public int collectibleCountBonus = 0; 
	public int collLvl1 = 0;
	public int collLvl2 = 0;
	public int collLvl3 = 0;
	public int collLvl4 = 0;
	public int collLvl5 = 0;
	public int starLvl1 = 0;
	public int starLvl2 = 0;
	public int starLvl3 = 0;
	public int starLvl4 = 0;
	public int starLvl5 = 0;
	public List<byte[]> images = new List<byte[]>();
}
#endregion
