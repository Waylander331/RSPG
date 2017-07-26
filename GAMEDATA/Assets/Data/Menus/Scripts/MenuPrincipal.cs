using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPrincipal : MonoBehaviour {

	/*
	 * id menus principal:
	 * 0 : Principal
	 * 1 : Nouvelle partie
	 * 2 : Options
	 * 3 : Controles
	 * */
	public EventSystem eS;
	public GameObject[] sousMenus;
	public GameObject[] mainButtonParSousMenu;

	private AudioSource[] sonsPrefab;

	private List<AudioSource> musics = new List<AudioSource>();
	private List<AudioSource> sons = new List<AudioSource>();

	private GameObject currentSelected;

	void Awake (){
		sonsPrefab = GameObject.FindObjectsOfType<AudioSource>() as AudioSource[];

		Manager.Instance.IsFromCin = false;
	}

	void Start (){
		if(sonsPrefab.Length != 0){
			foreach(AudioSource son in sonsPrefab){
				if(son.tag == "Music"){
					musics.Add (son);
				} else sons.Add (son);
			}
			if(musics.Count > 0){
				sousMenus[2].transform.GetChild (3).GetComponent<Slider>().value = musics[0].volume;
			}
			if(sons.Count > 0){
				sousMenus[2].transform.GetChild (4).GetComponent<Slider>().value = sons[0].volume;
			}
		}

		currentSelected = eS.currentSelectedGameObject;
	}

	void Update(){
		if(!sousMenus[0].activeSelf){
			if((sousMenus[1].activeSelf || sousMenus[2].activeSelf) && Input.GetButtonDown("Fire2")){
				ChangeScreen(0);
			}
			if(sousMenus[3].activeSelf && Input.GetButtonDown("Fire2")){
				ChangeScreen(2);
			}
		}
		if(eS.currentSelectedGameObject != currentSelected){
			SoundManager.Instance.PlayAudio("MenuSwitch");
			currentSelected = eS.currentSelectedGameObject;
		}

		foreach(Button button in transform.GetComponentsInChildren<Button>()){
			if(button.gameObject.transform.GetChild (0).GetComponent<Shadow>() != null){
				if(eS.currentSelectedGameObject == button.gameObject){
					button.gameObject.transform.GetChild (0).GetComponent<Shadow>().enabled = true;
				} else button.gameObject.transform.GetChild (0).GetComponent<Shadow>().enabled = false;
			}
		}
	}
	
	public void ChangeScreen(int a){

		HideAllBut0();
		sousMenus[a].SetActive(true);
		eS.SetSelectedGameObject(mainButtonParSousMenu[a]);

	}

	public void HideAllBut0(){
		for(int i=0; i < sousMenus.Length ; i++){
			sousMenus[i].SetActive(false);			
		}
	}

	public void CreditRoll(){
		Application.LoadLevel("Credits");
	}

	public void Quit(){
		Application.Quit();
	}

	public void ContinuerPartie(){
		Manager.Instance.Load();
		Manager.Instance.HaveContinue = true;
		HideAllBut0();
		if(Manager.Instance.TutorialDone) Manager.Instance.LevelTransit(2);
		else Application.LoadLevel("0_TUTO_P3_YL");
	}

	public void SetVolumeMusique(){
		if(musics.Count != 0){
			for(int i = 0; i < musics.Count; i++){
				musics[i].volume = sousMenus[2].transform.GetChild (3).GetComponent<Slider>().value;
			}
		}
	}

	public void SetVolumeSon(){
		if(sons.Count != 0){
			for(int i = 0; i < sons.Count; i++){
				sons[i].volume = sousMenus[2].transform.GetChild (4).GetComponent<Slider>().value;
			}
		}
	}

	public void StartNewGame(){
		Manager.Instance.ResetSaveFile();
		Manager.Instance.IsNewGame = true;
		HideAllBut0();
		Application.LoadLevel("0_TUTO_P3_YL");
		//Manager.Instance.LevelTransit(2);
	}

	public void StartGallery(){
		Application.LoadLevel("GalleryModels");
	}
}

	
