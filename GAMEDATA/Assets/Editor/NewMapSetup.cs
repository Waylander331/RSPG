using UnityEngine;
using System.Collections;
using UnityEditor;

public class NewMapSetup : Editor {

	private static GameObject manager;
	private static spawnpoint spawnP;
	private static GameObject avatar;
	private static GameObject cheatManager;
	private static Vector3 avPosition;
	private static GameObject mainCam;
	private static GameObject console;
	private static GameObject proj;
	private static GameObject pause;

	[MenuItem("Tools/NewMapSetup %m")]
	static void MapSetup(){

		manager = GameObject.Find("Manager");
		if(manager != null) 
			DestroyImmediate(manager); //supprimer le vieux pour le remplacer par le nouveau

		cheatManager = GameObject.Find("CheatManager");
		if(cheatManager != null)
			DestroyImmediate(cheatManager);

		avatar = GameObject.FindGameObjectWithTag("Player");
		if(avatar != null){
			avPosition = avatar.transform.position;
			DestroyImmediate(avatar);
		}
		else avPosition = Vector3.zero;

		//spawnP = GameObject.Find("SpawnPosition");
		spawnP = GameObject.FindObjectOfType<spawnpoint>();
		if(spawnP == null)
			InstSpawnPoint(); 

		mainCam = GameObject.Find ("GameCamera");
		GameObject temp = GameObject.FindGameObjectWithTag("MainCamera");
		if(mainCam != temp) 
			DestroyImmediate(temp); //Destroys the default camera
		if(mainCam != null)
			DestroyImmediate (mainCam);

		proj = GameObject.Find ("ProjecteurAvatar");
		if (proj != null)
			DestroyImmediate(proj);

		console = GameObject.Find ("ConsoleInGame");
		if(console != null)
			DestroyImmediate(console);

		pause = GameObject.Find ("PauseMenu");
		if(pause != null)
			DestroyImmediate(pause);

		InstAvatar();
		InstGameCam();
		InstManager();
		InstCheatMan();
		//InstConsole();
		InstProjector();
		InstPause();
		
		pause.transform.SetAsFirstSibling();
		//console.transform.SetAsFirstSibling();
		cheatManager.transform.SetAsFirstSibling();
		mainCam.transform.SetAsFirstSibling();
		proj.transform.SetAsFirstSibling();
		avatar.transform.SetAsFirstSibling();
		spawnP.transform.SetAsFirstSibling();
		manager.transform.SetAsFirstSibling();
	}

	[MenuItem("Tools/NewMapSetupPlus %#m")]
	static void NoAvatarSetup(){
		
		manager = GameObject.Find("Manager");
		if(manager != null) 
			DestroyImmediate(manager); //supprimer le vieux pour le remplacer par le nouveau
		
		cheatManager = GameObject.Find("CheatManager");
		if(cheatManager != null)
			DestroyImmediate(cheatManager);
		
		avatar = GameObject.FindGameObjectWithTag("Player");
		if(avatar != null){
			avPosition = avatar.transform.position;
		}
		else {
			Debug.LogError ("No Avatar in Scene. Use Ctrl+M.");
			return;
		}

		proj = GameObject.Find ("ProjecteurAvatar");
		if (proj != null)
			DestroyImmediate(proj);

		spawnP = GameObject.Find("SpawnPoint_0").GetComponent<spawnpoint>();
		if(spawnP != null){
			DestroyImmediate(spawnP.gameObject); 
		}			
		
		mainCam = GameObject.Find ("GameCamera");
		GameObject temp = GameObject.FindGameObjectWithTag("MainCamera");
		if(mainCam != temp) 
			DestroyImmediate(temp); //Destroys the default camera
		if(mainCam != null)
			DestroyImmediate (mainCam);		

		console = GameObject.Find ("ConsoleInGame");
		if(console != null)
			DestroyImmediate(console);

		pause = GameObject.Find ("PauseMenu");
		if(pause != null)
			DestroyImmediate(pause);

		InstSpawnPoint(); 
		InstGameCam();
		InstManager();
		InstCheatMan();	
		//InstConsole();
		InstProjector();
		InstPause();

		pause.transform.SetAsFirstSibling();
		//console.transform.SetAsFirstSibling();
		cheatManager.transform.SetAsFirstSibling();
		mainCam.transform.SetAsFirstSibling();
		proj.transform.SetAsFirstSibling();
		avatar.transform.SetAsFirstSibling();
		spawnP.transform.SetAsFirstSibling();
		manager.transform.SetAsFirstSibling();
	}

	static void InstManager(){
		manager = AssetDatabase.LoadAssetAtPath("Assets/_Obligatoire/Manager.prefab", typeof(GameObject)) as GameObject;
		manager = Instantiate(manager) as GameObject; 
		manager.name = "Manager"; //or else it adds (Clone) at the end & will not work properly.

		Manager man = manager.GetComponent<Manager>();
		man.starPrefab = (Stars)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Manager/Star.prefab", typeof(Stars));
	}
	static void InstSpawnPoint(){
		spawnP = AssetDatabase.LoadAssetAtPath("Assets/_Ingredients/SpawnPoint.prefab", typeof(spawnpoint)) as spawnpoint;
		spawnP = Instantiate (spawnP, avPosition, Quaternion.identity) as spawnpoint;
		spawnP.name = "SpawnPoint";
	}
	static void InstAvatar(){
		avatar = AssetDatabase.LoadAssetAtPath("Assets/_Obligatoire/Avatar.prefab", typeof(GameObject)) as GameObject;
		avatar = Instantiate(avatar, avPosition, Quaternion.identity) as GameObject;
		//avatar.name = "Avatar";
		avatar.tag = "Player";
	}
	static void InstGameCam(){
		mainCam = AssetDatabase.LoadAssetAtPath("Assets/_Obligatoire/GameCamera.prefab", typeof(GameObject)) as GameObject;
		mainCam = Instantiate(mainCam, spawnP.transform.position - (Vector3.forward * 5 + Vector3.up * -2), Quaternion.identity) as GameObject;
		mainCam.name = "GameCamera";

		mainCam.GetComponent<camBehavior>().myFocus = avatar.transform; //set the camera target
	}
	static void InstCheatMan(){
		cheatManager = AssetDatabase.LoadAssetAtPath("Assets/_Obligatoire/CheatManager.prefab", typeof(GameObject)) as GameObject;
		cheatManager = Instantiate(cheatManager) as GameObject;
		cheatManager.name = "CheatManager";
	}
	static void InstConsole(){
		console = AssetDatabase.LoadAssetAtPath("Assets/Outils/ConsoleInGame.prefab", typeof(GameObject)) as GameObject;
		console = Instantiate(console) as GameObject;
		console.name = "ConsoleInGame";
	}
	static void InstProjector(){
		proj = AssetDatabase.LoadAssetAtPath("Assets/_Obligatoire/ProjecteurAvatar.prefab", typeof (GameObject)) as GameObject;
		proj = Instantiate(proj, new Vector3(avatar.transform.position.x,avatar.transform.position.y + 8f,avatar.transform.position.z), Quaternion.identity) as GameObject;
		proj.transform.forward = -avatar.transform.up;
		proj.name = "ProjecteurAvatar";
	}
	static void InstPause(){
		pause = AssetDatabase.LoadAssetAtPath("Assets/_Obligatoire/Menus/PauseMenu.prefab", typeof (GameObject)) as GameObject;
		pause = Instantiate(pause) as GameObject;
		pause.name = "PauseMenu";
	}

}
