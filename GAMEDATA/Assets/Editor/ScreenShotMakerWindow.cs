//this script makes the ScreenShotMaker Window, and Creates the ScreenShotMaker GameObject on Play

using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class ScreenShotMakerWindow : EditorWindow
{

	static string myFolder = Application.loadedLevelName; // create a folder called screenshots

	public static bool Enable = false; //weather or not this tool is turned on

	private ScreenShotMaker obj; 

	public static bool GameObjectMade = false; //weather or not the ScreenShotMaker GameObject has been made yet
	public static bool camModEnable;
	public static float mouseS = 15f;
	public static float moveS = 10f;

	[MenuItem("Tools/ScreenShotMaker %k")] //creates option in menu to bring up window
	static void SSMW()
	{
		EditorWindow.GetWindow(typeof(ScreenShotMakerWindow)); //creates window
	}
	
	void OnGUI () 
	{
		//Get Playerprefs
		Enable = GetScreenShotEnable(); 
		camModEnable = GetFreeCamEnable();
		mouseS = GetFreeCamMouseS ();
		moveS = GetFreeCamMoveS ();

		// Title Label
		GUILayout.Label ("Screen Shot Maker Settings", EditorStyles.boldLabel); 

		//Screenshots
		Enable = EditorGUILayout.BeginToggleGroup ("Enable Screenshot", Enable);
		GUILayout.Label ("Manually take a screenshot with F12", EditorStyles.miniLabel);
		GUILayout.Label (" ", EditorStyles.boldLabel);
			
		EditorGUILayout.EndToggleGroup ();

		//FreeCam
		camModEnable = EditorGUILayout.BeginToggleGroup ("Enable Freecam Mode", camModEnable);
			mouseS = EditorGUILayout.Slider ("Mouse Sensitivity",mouseS, 0f, 100f);
			moveS = EditorGUILayout.Slider ("Move Sensitivity",moveS, 0f, 100f);
		EditorGUILayout.EndToggleGroup ();	

		if (Enable){PlayerPrefs.SetString("ScreenShotMakerEnabled","true");}
		else{PlayerPrefs.SetString("ScreenShotMakerEnabled","false");}

		//EditorGUILayout.TextField ("Folder Name: ", myFolder); 

		if (camModEnable){PlayerPrefs.SetString("FreeCamEnabled","true");}
		else{PlayerPrefs.SetString("FreeCamEnabled","false");}
		PlayerPrefs.SetString("MyFolderName", myFolder);

		PlayerPrefs.SetFloat ("FreeCamMouseS", mouseS);
		PlayerPrefs.SetFloat ("FreeCamMoveS", moveS);
	}
	
	void Update()
	{
		//get enable and playerprefs 
		Enable = GetScreenShotEnable();
		camModEnable = GetFreeCamEnable();
		myFolder = GetMyFolderName();
		//obj.MyFolder = GetMyFolderName();
		mouseS = GetFreeCamMouseS();
		moveS = GetFreeCamMoveS();

		//if editor is playing
		if (EditorApplication.isPlaying)
		{
			if(Camera.main.GetComponent<modCam> () == null){
				/*modCam md = */Camera.main.gameObject.AddComponent<modCam>();
			}
			if(Camera.main.GetComponent<modCam> ().MoveSensitivity != moveS || Camera.main.GetComponent<modCam> ().MouseSensitivity != mouseS || !Camera.main.GetComponent<camBehavior> ().enabled != !camModEnable || Camera.main.GetComponent<modCam> ().enabled != camModEnable){
				Camera.main.GetComponent<camBehavior> ().enabled = !camModEnable;
				Camera.main.GetComponent<modCam> ().enabled = camModEnable;
				Camera.main.GetComponent<modCam> ().MouseSensitivity = mouseS;
				Camera.main.GetComponent<modCam> ().MoveSensitivity = moveS;
			}
			if (Enable && !GameObjectMade) //Create the game object, add its components
			{
				GameObject gameObject = new GameObject("ScreenShotMaker");
				gameObject.AddComponent<ScreenShotMaker>();
				gameObject.GetComponent<ScreenShotMaker>().myFolder = myFolder;
		      	gameObject.tag = "EditorOnly";
				gameObject.name = "ScreenShotMaker";

				obj = gameObject.GetComponent<ScreenShotMaker>();
				GameObjectMade = true;
			}
			if(obj.MyFolder != myFolder) obj.myFolder = myFolder;
		}
	}

	//method for getting the Enable from PlayerPrefs 
	private bool GetScreenShotEnable()
	{
		if (PlayerPrefs.HasKey("ScreenShotMakerEnabled"))
		{
			if (PlayerPrefs.GetString("ScreenShotMakerEnabled") == "true")
			{
				return true;
			}
			else
			{
				return false;
			}
			
		}
		else
		{
			return false;
		}
	}

	private string GetMyFolderName()
	{
		if(PlayerPrefs.HasKey("MyFolderName"))
		{
			return PlayerPrefs.GetString("MyFolderName");
		}
		else
		{
			return "default";
		}
	}

	private bool GetFreeCamEnable()
	{
		if (PlayerPrefs.HasKey("FreeCamEnabled"))
		{
			if (PlayerPrefs.GetString("FreeCamEnabled") == "true")
			{
				return true;
			}
			else
			{
				return false;
			}
			
		}
		else
		{
			return false;
		}
	}

	private float GetFreeCamMouseS()
	{
		if (PlayerPrefs.HasKey("FreeCamMouseS"))
		{
			return PlayerPrefs.GetFloat("FreeCamMouseS");	
		}
		else
		{
			return 0f;
		}
	}

	private float GetFreeCamMoveS()
	{
		if (PlayerPrefs.HasKey("FreeCamMoveS"))
		{
			return PlayerPrefs.GetFloat("FreeCamMoveS");	
		}
		else
		{
			return 0f;
		}
	}


	
}
