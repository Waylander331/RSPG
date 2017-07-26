using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GalleryManager : MonoBehaviour {
	
	/* * id menus gallery:
	 * 0 : Models
	 * 1 : Concetps
	 * 2 : Music
	 * 3 : Cinematic
	 * 4 : BotMenu
	 * 5 : TopMenuModels
	 * 6 : TopMenuConcepts
	 * */

	public GalleryModels galleryCam;

	public EventSystem eS;
	
	private Animator[] anims;

	private int[] galleryCountModels = new int[24]{/*Concepts*/2,4,5,5,6,6,7,7,8,11,12,14,/*Models*/1,1,3,3,4,5,8,9,10,12,13,15};
	private bool[] galleryBoolModels = new bool[24];
	public GameObject[] sousMenus;
	public GameObject[] buttonToSelected;
	private GameObject[] mainButtonModelsConcepts = new GameObject[29];
	public GameObject[] mainButtonModels;
	public GameObject[] mainButtonConcept;
	public MainButtonMusic[] mainButtonMusic;
	public MainButtonCinematic mainButtonCinematic;
	public GameObject[] mainButtonBotMenu;

	public GameObject[] arts;

	private int currentSelected = 0;
	private string controlName;

	private int courant = 0;

	private bool hide = false;
	private bool isPaused = false;
	private bool unlockConcept = false;
	private bool toTheRight = true;
	private bool toTheLeft = false;
	private Vector2 posInitTMM;
	private Vector2 posInitTMC;

	private GameObject currentSelectedGO;

	private AudioSource myAudio;

	void Start(){
		myAudio = transform.GetComponent<AudioSource>();
		//Manager.Instance.GalleryCount = 8;
		/*Manager.Instance.UnlockByLvl[0] = true;
		Manager.Instance.UnlockByLvl[1] = true;
		Manager.Instance.UnlockByLvl[2] = true;
		Manager.Instance.UnlockByLvl[3] = true;*/
		//Manager.Instance.UnlockByLvl[4] = true;

		posInitTMM = sousMenus[5].transform.GetChild(0).transform.position;
		posInitTMC = sousMenus[6].transform.GetChild(0).transform.position;

		for(int i = 0; i < arts.Length; i++){
			mainButtonModelsConcepts[i] = arts[i];
		}

		for(int i = 0; i < galleryCountModels.Length; i++){
			if(galleryCountModels[i] <= Manager.Instance.GalleryCount){
				galleryBoolModels[i] = true;
			}
		}
		for(int i = 0; i < mainButtonConcept.Length-4; i++){
			if(galleryBoolModels[i] == true){
				mainButtonConcept[i].transform.GetChild (0).gameObject.SetActive(false);
			}
		}
		for(int i = 0; i < mainButtonModels.Length-4; i++){
			if(galleryBoolModels[i+12] == true){
				mainButtonModels[i].transform.GetChild (0).gameObject.SetActive(false);
			}
		}
		for(int i = 0; i < Manager.Instance.UnlockByLvl.Length-1; i++){
			if(Manager.Instance.UnlockByLvl[i] == true){
				mainButtonConcept[i+12].transform.GetChild (0).gameObject.SetActive(false);
				mainButtonModels[i+12].transform.GetChild (0).gameObject.SetActive(false);
			}
		}

		currentSelectedGO = eS.currentSelectedGameObject;

	}

	void Update(){
		if(Input.GetButtonDown("Bumper")){
			hide = !hide;
		}
		if(hide){
			HideMenus();
		} else {		
			if(courant == 0){
				sousMenus[5].SetActive(true);
				sousMenus[10].SetActive(true);
				sousMenus[11].SetActive(true);
			} else if (courant == 1){
				sousMenus[6].SetActive(true);
			}
			sousMenus[4].SetActive(true);
		}
		if(Input.GetButtonDown("Fire3") && anims.Length > 0 && currentSelectedGO.name != "Model11"){
			foreach(Animator anim in anims){
				anim.Play("bonus");
			}
		}

		if(Input.GetButtonDown("Fire2")){
			Application.LoadLevel("EcranTitre");
		}

		if(eS.currentSelectedGameObject != currentSelectedGO){
			SoundManager.Instance.PlayAudio("MenuSwitch");
			currentSelectedGO = eS.currentSelectedGameObject;
		}

		if(eS.currentSelectedGameObject == mainButtonModels[8] && toTheRight){
			toTheRight = false;
			sousMenus[5].transform.GetChild(0).transform.GetComponent<RectTransform>().offsetMin = new Vector2(-922.879f,0);
			sousMenus[5].transform.GetChild(0).transform.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
			toTheLeft = true;
		}
		else if(eS.currentSelectedGameObject == mainButtonModels[7] && toTheLeft){
			toTheLeft = false;
			sousMenus[5].transform.GetChild(0).transform.position = posInitTMM;
			toTheRight = true;
		}

		if(eS.currentSelectedGameObject == mainButtonConcept[8] && toTheRight){
			toTheRight = false;
			sousMenus[6].transform.GetChild(0).transform.GetComponent<RectTransform>().offsetMin = new Vector2(-922.879f,0);
			sousMenus[6].transform.GetChild(0).transform.GetComponent<RectTransform>().offsetMax = new Vector2(0,0);
			toTheLeft = true;
		}
		else if(eS.currentSelectedGameObject == mainButtonConcept[7] && toTheLeft){
			toTheLeft = false;
			sousMenus[6].transform.GetChild(0).transform.position = posInitTMC;
			toTheRight = true;
		}
	}

	#region Screen
	public void ChangeScreen(int a){
		courant = a;
		mainButtonCinematic.plane.SetActive(false);
		galleryCam.GetComponent<Camera>().orthographic = false;
		galleryCam.HaveChoose = false;
		galleryCam.MyFocus = null;
		galleryCam.Destroyer(mainButtonModelsConcepts);
		galleryCam.transform.position = new Vector3(galleryCam.DefaultDistanceMax,2,0);
		galleryCam.transform.rotation = galleryCam.RotationStartFocus;
		HideArts();
		HideInfosMusic();
		HideAllBut0();
		sousMenus[a].SetActive(true);
		sousMenus[4].SetActive(true);
		sousMenus[5].transform.GetChild(0).transform.position = posInitTMM;
		sousMenus[6].transform.GetChild(0).transform.position = posInitTMC;
		StopCinematic();
		for(int i = 0; i < mainButtonMusic.Length; i++){
			StopMusic(i);
		}

		HideBotMenuButtons();

		if(a == 0){
			mainButtonBotMenu[0].SetActive(true);
			mainButtonBotMenu[1].SetActive(true);
			mainButtonBotMenu[8].SetActive(true);
			sousMenus[10].SetActive(true);
			sousMenus[11].SetActive(true);
		}
		else if(a == 1){
			mainButtonBotMenu[2].SetActive(true);
			mainButtonBotMenu[3].SetActive(true);
			mainButtonBotMenu[9].SetActive(true);
		}
		else if(a == 2){
			mainButtonBotMenu[4].SetActive(true);
			mainButtonBotMenu[5].SetActive(true);
			mainButtonBotMenu[10].SetActive(true);
			if(Manager.Instance.UnlockByLvl[4] == false){
				sousMenus[7].SetActive(false);
				sousMenus[8].SetActive(false);
				sousMenus[9].SetActive(false);
			} else {
				sousMenus[7].SetActive(true);
				sousMenus[8].SetActive(true);
				sousMenus[9].SetActive(true);
			}
		}
		else if(a == 3){
			galleryCam.GetComponent<Camera>().orthographic = true;
			galleryCam.GetComponent<Camera>().orthographicSize = 3.7f;
			mainButtonCinematic.plane.SetActive(true);
			if(!Manager.Instance.CinWasPlayed){
				foreach(GameObject b in mainButtonCinematic.button){
					b.SetActive(false);
				}
				mainButtonCinematic.plane.transform.GetChild(0).gameObject.SetActive(false);
			} else{
				foreach(GameObject b in mainButtonCinematic.button){
					b.SetActive(true);
				}
				mainButtonCinematic.plane.transform.GetChild(0).gameObject.SetActive(true);
			}
			mainButtonBotMenu[6].SetActive(true);
			mainButtonBotMenu[7].SetActive(true);
			mainButtonBotMenu[11].SetActive(true);
		}

		eS.SetSelectedGameObject(buttonToSelected[a]);
	}

	public void ReturnToMain(){
		Application.LoadLevel("EcranTitre");
	}
	#endregion


	#region Model
	public void ChooseModel (int a){
		if(galleryBoolModels[a] == false){
			galleryCam.Destroyer(mainButtonModelsConcepts);
			galleryCam.InstanceCreate(mainButtonModelsConcepts[mainButtonModelsConcepts.Length-1]);
			galleryCam.MyFocus = mainButtonModelsConcepts[mainButtonModelsConcepts.Length-1].transform.GetChild(0).transform;
		} else {
			galleryCam.Destroyer(mainButtonModelsConcepts);
			galleryCam.InstanceCreate(mainButtonModelsConcepts[a]);
			galleryCam.MyFocus = mainButtonModelsConcepts[a].transform.GetChild(0).transform;

		}
		if(mainButtonModelsConcepts[a].GetComponentInChildren<Animator>() != null){
			anims = mainButtonModelsConcepts[a].GetComponentsInChildren<Animator>();
		} else anims = new Animator[0];
		galleryCam.transform.position = new Vector3(galleryCam.DefaultDistanceMax,galleryCam.MyFocus.position.y,galleryCam.MyFocus.position.z);
		galleryCam.CamDistanceMax = galleryCam.DefaultDistanceMax;
		galleryCam.transform.rotation = galleryCam.RotationStartFocus;
		galleryCam.HaveChoose = true;
		galleryCam.IsZooming = false;
	}
	
	public void ChooseModelLvl(int a){
		if(Manager.Instance.UnlockByLvl[a] == false){
			galleryCam.Destroyer(mainButtonModelsConcepts);
			galleryCam.InstanceCreate(mainButtonModelsConcepts[mainButtonModelsConcepts.Length-1]);
			galleryCam.MyFocus = mainButtonModelsConcepts[mainButtonModelsConcepts.Length-1].transform.GetChild(0).transform;
		} else {
			galleryCam.Destroyer(mainButtonModelsConcepts);
			galleryCam.InstanceCreate(mainButtonModelsConcepts[a+24]);
			galleryCam.MyFocus = mainButtonModelsConcepts[a+24].transform.GetChild(0).transform;

		}
		if(mainButtonModelsConcepts[a+24].GetComponentInChildren<Animator>() != null){
			anims = mainButtonModelsConcepts[a+24].GetComponentsInChildren<Animator>();
		} else anims = new Animator[0];
		galleryCam.transform.position = new Vector3(galleryCam.DefaultDistanceMax,galleryCam.MyFocus.position.y,galleryCam.MyFocus.position.z);
		galleryCam.CamDistanceMax = galleryCam.DefaultDistanceMax;
		galleryCam.transform.rotation = galleryCam.RotationStartFocus;
		galleryCam.HaveChoose = true;
		galleryCam.IsZooming = false;
	}
	#endregion


	#region Concept
	public void ChooseConcept (int a){
		HideArts();
		if(galleryBoolModels[a] == false){
			arts[arts.Length-1].SetActive(true);
		} else {
			mainButtonModelsConcepts[a].SetActive(true);
		}
	}

	public void ChooseConceptLvl (int a){
		HideArts();
		if(Manager.Instance.UnlockByLvl[a] == false){
			arts[arts.Length-1].SetActive(true);
		} else arts[a+12].SetActive(true);
	}
	#endregion


	#region Music
	public void PlayMusic(int a){
			myAudio.clip = mainButtonMusic[a].music;
		if(!myAudio.isPlaying){
				myAudio.Play();
			}
	}

	public void StopMusic(int a){
		if(myAudio.clip == mainButtonMusic[a].music){
				myAudio.Stop();
			}
	}

	public void PauseMusic(int a){
		if(myAudio.clip == mainButtonMusic[a].music){
			if(myAudio.isPlaying){
			myAudio.Pause();
			} else myAudio.UnPause();
		}
	}

	public void ShowInfos(int a){
		HideInfosMusic();
		mainButtonMusic[a].header.SetActive(true);
		mainButtonMusic[a].corps.SetActive(true);
	}
	#endregion


	#region Cinematic
	public void PlayCinematic(){
		myAudio.clip = mainButtonCinematic.movie.audioClip;
		mainButtonCinematic.movie.Play();
		myAudio.Play();
	}

	public void StopCinematic(){
		mainButtonCinematic.movie.Stop();
		myAudio.Stop();
	}

	public void PauseCinematic(){
		mainButtonCinematic.movie.Pause();
		myAudio.Pause();	
	}
	#endregion
	

	#region Hide
	public void HideAllBut0(){
		for(int i=0; i < sousMenus.Length ; i++){
			sousMenus[i].SetActive(false);			
		}
	}

	public void HideMenus(){
		sousMenus[4].SetActive(false);
		sousMenus[5].SetActive(false);
		sousMenus[6].SetActive(false);
		sousMenus[10].SetActive(false);
		sousMenus[11].SetActive(false);
	}

	public void HideBotMenuButtons(){
		for(int i=0; i < mainButtonBotMenu.Length ; i++){
			mainButtonBotMenu[i].SetActive(false);
		}
	}

	public void HideArts(){
		for(int i=0; i < arts.Length; i++){
			arts[i].SetActive(false);
		}
	}

	public void HideInfosMusic(){
		for(int i = 0; i < mainButtonMusic.Length; i++){
			mainButtonMusic[i].header.SetActive(false);
			mainButtonMusic[i].corps.SetActive(false);
		}
	}
	#endregion


	//Accesseurs
	public GameObject[] MainButtonModelsConcepts {
		get {return mainButtonModelsConcepts;}
		set {mainButtonModelsConcepts = value;}
	}
}

[System.Serializable]
public class MainButtonMusic{
	public AudioClip music;
	public GameObject header;
	public GameObject corps;
}

[System.Serializable]
public class MainButtonCinematic{
	public GameObject plane;
	public MovieTexture movie;
	public GameObject[] button;
}

