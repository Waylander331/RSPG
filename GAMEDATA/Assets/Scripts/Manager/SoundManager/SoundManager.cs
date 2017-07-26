using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SoundManager : MonoBehaviour {

	public GameObject prefab;

	[SerializeField] private bool avatar;
	[SerializeField] private bool switchPrefab;
	[SerializeField] private bool arms;
	[SerializeField] private bool bridgeman;
	[SerializeField] private bool bars;
	[SerializeField] private bool door;
	[SerializeField] private bool scissorLift;
	[SerializeField] private bool collectible;
	[SerializeField] private bool chariot;
	[SerializeField] private bool sFX;
	[SerializeField] private bool bossFight;
	[SerializeField] private bool cinematique;
	[SerializeField] private bool menu;
	[SerializeField] private bool credits;

	[SerializeField] private bool music;

	[SerializeField] private TextAsset soundsFile;

	[SerializeField] private AudioDico[] soundsEffects;

	[SerializeField] private Dictionary<string, AudioDico> sounds =  new Dictionary<string, AudioDico>();

	private static SoundManager instance;

	private GameObject[] musics;
	private AudioSource[] sonsPrefab;

	private string[] lignes;
	private int index = 0;
	private bool canPlay = true;
	private bool fromHub = false;
	private bool fromMain = false;
	private bool fromOther = false;
	private bool fromElevator = false;

	
	void Awake(){
		EssenceMemeDuSingleton();
		if(SoundManager.Instance == this){	
			DontDestroyOnLoad(this.gameObject);
			musics = GameObject.FindGameObjectsWithTag("Music");
			if(GameObject.Find ("SoundsPrefabs") != null){
				sonsPrefab = GameObject.Find ("SoundsPrefabs").GetComponentsInChildren<AudioSource>() as  AudioSource[];
			} else sonsPrefab = GameObject.FindObjectsOfType<AudioSource>() as AudioSource[];
		}
	}

	/*void Start(){
		if(SoundManager.Instance == this){
			if(GameObject.Find ("SoundsPrefabs") != null){
				sonsPrefab = GameObject.Find ("SoundsPrefabs").GetComponentsInChildren<AudioSource>() as  AudioSource[];
			} else sonsPrefab = GameObject.FindObjectsOfType<AudioSource>() as AudioSource[];
		}
	}*/

	void Update(){

		if(/*Manager.Instance.LevelName == "Splash" || */Manager.Instance.LevelName == "EcranTitre"){
			if(canPlay || fromOther || fromHub){
				StopMusic();
				PlayAudio("Main");
				canPlay = false;
				fromOther = false;
				fromHub = false;
				fromMain = true;
			}
		} else if(Manager.Instance.LevelName == "0_TUTO_P3_YL" || Manager.Instance.LevelName == "0_HUB_YL"){
			if(fromOther || fromMain || fromElevator){
				StopMusic();
				PlayAudio("Hub");
				fromHub = true;
				fromOther = false;
				fromMain = false;
				fromElevator = false;
			}
		} else if(Manager.Instance.LevelName == "1_MANEGE_MAG"){
			if(fromHub || fromMain || fromElevator){
				StopMusic();
				PlayAudio("Niveau1");
				PlayAudio("RailChariot");
				fromHub = false;
				fromOther = true;
				fromMain = false;
				fromElevator = false;
			}
		} else if(Manager.Instance.LevelName == "2_FETEFORAINE_JLC"){
			if(fromHub || fromMain || fromElevator){
				StopMusic();
				PlayAudio("Niveau2");
				fromHub = false;
				fromOther = true;
				fromMain = false;
				fromElevator = false;
			}
		} else if(Manager.Instance.LevelName == "3_CARROUSEL_PD"){
			if(fromHub || fromMain || fromElevator){
				StopMusic();
				PlayAudio("Niveau3");
				fromHub = false;
				fromOther = true;
				fromMain = false;
				fromElevator = false;
			}
		} else if(Manager.Instance.LevelName == "4_CHUCKY_PC"){
			if(fromHub || fromMain || fromElevator){
				StopMusic();
				PlayAudio("Niveau4");
				fromHub = false;
				fromOther = true;
				fromMain = false;
				fromElevator = false;
			}
		} else if(Manager.Instance.LevelName == "5_FREAKSHOW_JLem"){
			if(fromHub || fromMain || fromElevator){
				StopMusic();
				PlayAudio("Niveau5");
				fromHub = false;
				fromOther = true;
				fromMain = false;
				fromElevator = false;
			}
		} else if(Manager.Instance.LevelName == "0_HUB_GDT"){
			if(fromHub || fromMain || fromElevator){
				StopMusic();
				PlayAudio("BossFightSong");
				fromHub = false;
				fromOther = true;
				fromMain = false;
				fromElevator = false;
			}
		} else if(Manager.Instance.LevelName == "Credits"){
			if(fromHub || fromMain || fromElevator){
				StopMusic();
				PlayAudio("BossFightSong");
				fromHub = false;
				fromOther = true;
				fromMain = false;
				fromElevator = false;
			}
		}

		if(Manager.Instance.LevelName == "GalleryModels"){
			StopMusic();
			canPlay = true;
		}

		/*if(Manager.Instance.LevelName == "1_MANEGE_MAG"){
			PlayAudio("RailChariot");
		} else StopAudio("RailChariot");*/

		/*if(Input.GetButton("Jump")){
			PlayAudio("Niveau2");
			//StopAudio("Niveau2");
		}*/

		if(Manager.Instance.InBackstage){
			foreach(GameObject music in musics){
				if(music.name != "ElevatorSong"){
					music.GetComponent<AudioReverbFilter>().dryLevel = -800f;
				}
			}
		} else {
			foreach(GameObject music in musics){
				music.GetComponent<AudioReverbFilter>().dryLevel = 0f;
			}
		}

		if(Manager.Instance.IsSlow){
			if(sonsPrefab.Length > 0){
				foreach(AudioSource sons in sonsPrefab){
					sons.pitch = 0.6f;
				}
			}
		} else {
			if(sonsPrefab.Length > 0){
				foreach(AudioSource sons in sonsPrefab){
					sons.pitch = 1f;
				}
			}
		}
	}

	void EssenceMemeDuSingleton(){		
		if(SoundManager.Instance == null){
			instance = this;
		}
		else{
			if(SoundManager.Instance!=this){
				Destroy(this.gameObject);
			}			
		}
	}

	public void ReadFile(){
		string tempContenu= soundsFile.text ;

		lignes = tempContenu.Split("\n"[0]);

		soundsEffects = new AudioDico[lignes.Length];

		foreach(string ligne in lignes){
			Split (ligne);
		}
		index = 0;
	}
	
	void Split(string ligne){
		string[] temp = ligne.Split("#"[0]);
		soundsEffects[index] = new AudioDico(temp[0],temp[1],float.Parse(temp[2]),int.Parse(temp[3]));
		index++;
		
	}

	public void PlayAudio(string name){
		if(IndexOfAudioSource(sonsPrefab, name) > -1){
			sonsPrefab[IndexOfAudioSource(sonsPrefab, name)].Play();
		}
	}

	public void PlayAudioInMyself(GameObject owner, string name){
		if(owner != null){
			AudioSource[] temp = owner.GetComponentsInChildren<AudioSource>() as AudioSource[];
			if(IndexOfAudioSourceMyself(temp, name) > -1){
				temp[IndexOfAudioSourceMyself(temp, name)].Play();
			}
		}
	}

	public void StopAudio(string name){
		if(IndexOfAudioSource(sonsPrefab, name) > -1){
			if(sonsPrefab[IndexOfAudioSource(sonsPrefab, name)].isPlaying){
				sonsPrefab[IndexOfAudioSource(sonsPrefab, name)].Stop();
			}
		}
	}

	public void StopAudioInMyself(GameObject owner, string name){
		if(owner != null){
			AudioSource[] temp = owner.GetComponentsInChildren<AudioSource>() as AudioSource[];
			if(IndexOfAudioSourceMyself(temp, name) > -1){
				if(temp[IndexOfAudioSourceMyself(temp, name)].isPlaying){
					temp[IndexOfAudioSourceMyself(temp, name)].Stop();
				}
			}
		}
	}

	static int IndexOfAudioSource(AudioSource[] arr, string value){
		for (int i = 0; i < arr.Length; i++){
			if (arr[i].gameObject.name == value){
				return i;
			}
		} 
		return -1;
	}

	static int IndexOfAudioSourceMyself(AudioSource[] arr, string value){
		for (int i = 0; i < arr.Length; i++){
			if (arr[i].clip.name == value){
				return i;
			}
		}
		return -1;
	}

	public void StopMusic(){
		StopAudio ("Main");
		StopAudio ("Hub");
		StopAudio ("Niveau1");
		StopAudio ("Niveau2");
		StopAudio ("Niveau3");
		StopAudio ("Niveau4");
		StopAudio ("Niveau5");
		StopAudio ("ElevatorSong");
		StopAudio ("BossFightSong");
		StopAudio ("RailChariot");
	}

	void OnLevelWasLoaded(){
		if(SoundManager.Instance == this){
			Invoke ("ResetSons", 0.05f);
			//Debug.Log("OnLevelWasLoaded");
		}
	}

	void ResetSons (){
		if(GameObject.Find ("SoundsPrefabs") == null){
			Array.Clear (sonsPrefab, 0, sonsPrefab.Length);
			sonsPrefab = new AudioSource[0];
			sonsPrefab = GameObject.FindObjectsOfType<AudioSource>() as AudioSource[];
		}
	}

	//************AVATAR_RANDOM**************//
	public void AvatarFootstepsRandom(){
		if(!Manager.Instance.InBackstage){
			int number = UnityEngine.Random.Range(0, 5);

			switch(number){
			case 0:
				PlayAudio("Footsteps1");
				break;
			case 1:
				PlayAudio("Footsteps2");
				break;
			case 2:
				PlayAudio("Footsteps3");
				break;
			case 3:
				PlayAudio("Footsteps4");
				break;
			case 4:
				PlayAudio("Footsteps5");
				break;
			}
		} else {
			int number = UnityEngine.Random.Range(0, 4);
			
			switch(number){
			case 0:
				PlayAudio("FootstepsDust1");
				break;
			case 1:
				PlayAudio("FootstepsDust2");
				break;
			case 2:
				PlayAudio("FootstepsDust3");
				break;
			case 3:
				PlayAudio("FootstepsDust1");
				break;
			}
		}
	}

	public void AvatarWallGripRandom(){
		int number = UnityEngine.Random.Range(0, 2);
		
		switch(number){
		case 0:
			PlayAudio("WallJump1");
			break;
		case 1:
			PlayAudio("WallJump2");
			break;
		}
	}

	public void AvatarWallJumpRandom(){
		int number = UnityEngine.Random.Range(0, 2);
		
		switch(number){
		case 0:
			PlayAudio("WallGrip1");
			break;
		case 1:
			PlayAudio("WallGrip2");
			break;
		}
	}

	public void AvatarDeathRandom(){
		int number = UnityEngine.Random.Range(0, 6);
		
		switch(number){
		case 0:
			PlayAudio("Death1");
			break;
		case 1:
			PlayAudio("Death2");
			break;
		case 2:
			PlayAudio("Death3");
			break;
		case 3:
			PlayAudio("Death4");
			break;
		case 4:
			PlayAudio("Death5");
			break;
		case 5:
			PlayAudio("Death6");
			break;
		}
	}

	//************ARMS_RANDOM**************//
	public void ArmsFootstepsRandom(){
		int number = UnityEngine.Random.Range(0, 5);
		
		switch(number){
		case 0:
			PlayAudio("AFootsteps1");
			break;
		case 1:
			PlayAudio("AFootsteps2");
			break;
		case 2:
			PlayAudio("AFootsteps3");
			break;
		case 3:
			PlayAudio("AFootsteps4");
			break;
		case 4:
			PlayAudio("AFootsteps5");
			break;
		}
	}

	public void ArmsLaunchRandom(){
		int number = UnityEngine.Random.Range(0, 3);
		
		switch(number){
		case 0:
			PlayAudio("ALaunch1");
			break;
		case 1:
			PlayAudio("ALaunch2");
			break;
		case 2:
			PlayAudio("ALaunch3");
			break;
		}
	}

	public void ArmsPushRandom(){
		int number = UnityEngine.Random.Range(0, 2);
		
		switch(number){
		case 0:
			PlayAudio("APush1");
			break;
		case 1:
			PlayAudio("APush2");
			break;
		}
	}

	//************BRIDGEMAN_RANDOM**************//
	public void BMFireTomatoRandom(){
		int number = UnityEngine.Random.Range(0, 2);
		
		switch(number){
		case 0:
			PlayAudio("BMFireTomato1");
			break;
		case 1:
			PlayAudio("BMFireTomato2");
			break;
		}
	}

	public void BMSideStepsRandom(){
		int number = UnityEngine.Random.Range(0, 2);
		
		switch(number){
		case 0:
			PlayAudio("BMSideSteps1");
			break;
		case 1:
			PlayAudio("BMSideSteps2");
			break;
		}
	}

	public void BMFallRandom(){
		int number = UnityEngine.Random.Range(0, 4);
		
		switch(number){
		case 0:
			PlayAudio("BMFalling1");
			break;
		case 1:
			PlayAudio("BMFalling2");
			break;
		case 2:
			PlayAudio("BMFalling3");
			break;
		case 3:
			PlayAudio("BMFalling4");
			break;
		}
	}

	public void BMKickRandom(){
		int number = UnityEngine.Random.Range(0, 6);
		
		switch(number){
		case 0:
			PlayAudio("BMKick1");
			break;
		case 1:
			PlayAudio("BMKick2");
			break;
		case 2:
			PlayAudio("BMKick3");
			break;
		case 3:
			PlayAudio("BMKick4");
			break;
		case 4:
			PlayAudio("BMKick5");
			break;
		case 5:
			PlayAudio("BMKick6");
			break;
		}
	}

	public void TomatoSplatRandom(){
		int number = UnityEngine.Random.Range(0, 2);
		
		switch(number){
		case 0:
			PlayAudio("TomatoSplat1");
			break;
		case 1:
			PlayAudio("TomatoSplat2");
			break;
		}
	}

	//************BAR_RANDOM**************//
	public void BarGrabRandom(){
		int number = UnityEngine.Random.Range(0, 2);
		
		switch(number){
		case 0:
			PlayAudio("BarGrab1");
			break;
		case 1:
			PlayAudio("BarGrab2");
			break;
		}
	}

	//************SFX_RANDOM**************//
	public void TVBreakRandom(){
		int number = UnityEngine.Random.Range(0, 4);
		
		switch(number){
		case 0:
			PlayAudio("TVBreak1");
			break;
		case 1:
			PlayAudio("TVBreak2");
			break;
		case 2:
			PlayAudio("TVBreak3");
			break;
		case 3:
			PlayAudio("TVBreak4");
			break;
		}
	}

	//************BOSSFIGHT_RANDOM**************//
	public void BFShakeRandom(){
		int number = UnityEngine.Random.Range(0, 3);
		
		switch(number){
		case 0:
			PlayAudio("BFShake1");
			break;
		case 1:
			PlayAudio("BFShake2");
			break;
		case 2:
			PlayAudio("BFShake3");
			break;
		}
	}


	//Accesseurs
	public static SoundManager Instance{
		get{return instance;}
	}

	public Dictionary<string, AudioDico> Sounds {
		get {return sounds;}
	}

	public AudioDico[] SoundsEffects {
		get {return soundsEffects;}
		set {soundsEffects = value;}
	}

	public bool FromElevator {
		get {return fromElevator;}
		set {fromElevator = value;}
	}
}

[System.Serializable]
public class AudioDico{
	public string name;
	public string path;
	public float volume;
	public int priority;

	public AudioDico(string name, string path, float volume, int priority){
		this.name = name;
		this.path = path;
		this.volume = volume;
		this.priority = priority;
	}
}
