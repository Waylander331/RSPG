using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MenuPause : MonoBehaviour {

	/*
	 * id menus principal:
	 * 0 : Principal
	 * 1 : Options
	 * 2 : Controles...
	 * 3 : Retour au Hub
	 * 4 : Quitter
	 * 5 : Objectifs
	 * */
	public EventSystem eS;
	public GameObject[] sousMenus;
	public GameObject[] mainButtonParSousMenu;
	public Text[] objectifs;
	public GameObject[] collectable;

	public Text collectibleText;
	public Text starText;

	private bool doItOnce = true;
	private bool doItOnce2 = false;

	private int collectible1CountChange = 0;
	private int star1CountChange = 0;

	private AudioSource[] sonsPrefab;
	
	private List<AudioSource> musics = new List<AudioSource>();
	private List<AudioSource> sons = new List<AudioSource>();

	private GameObject currentSelected;
	
	void Awake (){
		sonsPrefab = GameObject.FindObjectsOfType<AudioSource>() as AudioSource[];
	}
	
	void Start (){
		if(sonsPrefab.Length > 0){
			foreach(AudioSource son in sonsPrefab){
				if(son.tag == "Music"){
					musics.Add (son);
				} else sons.Add (son);
			}
			if(musics.Count > 0){
				sousMenus[1].transform.GetChild (3).GetComponent<Slider>().value = musics[0].volume;
			}
			if(sons.Count > 0){
				sousMenus[1].transform.GetChild (4).GetComponent<Slider>().value = sons[0].volume;
			}
		}

		star1CountChange = Manager.Instance.TotalStarsTaken;
		collectible1CountChange = Manager.Instance.CollectibleCount;

		currentSelected = eS.currentSelectedGameObject;
	}
	
	void Update(){
		if(Manager.IsPaused){
			if(doItOnce){
				transform.GetChild(0).gameObject.SetActive(true);
				ChangeScreen(0);
				doItOnce = false;
			}

			if(!sousMenus[0].activeSelf){
				if((sousMenus[3].activeSelf || sousMenus[4].activeSelf || sousMenus[1].activeSelf || sousMenus[5].activeSelf) && Input.GetButtonDown("Fire2")){
					ChangeScreen(0);
				}
				if(sousMenus[2].activeSelf && Input.GetButtonDown("Fire2")){
					ChangeScreen(1);
				}
			}

			if(eS.currentSelectedGameObject != currentSelected){
				SoundManager.Instance.PlayAudio("MenuSwitch");
				currentSelected = eS.currentSelectedGameObject;
			}

			if(Manager.Instance.LevelName == "0_TUTO_P3_YL"){
				collectable[0].SetActive(false);
				collectable[1].SetActive(false);
			} else {
				collectable[0].SetActive(true);
				collectable[1].SetActive(true);
			}

			objectifs[0].text = (Manager.Instance.StarLvl1 + " / 3");
			objectifs[1].text = (Manager.Instance.CollLvl1 + " / 9");
			objectifs[2].text = (Manager.Instance.StarLvl2 + " / 3");
			objectifs[3].text = (Manager.Instance.CollLvl2 + " / 9");
			objectifs[4].text = (Manager.Instance.StarLvl3 + " / 3");
			objectifs[5].text = (Manager.Instance.CollLvl3 + " / 9");
			objectifs[6].text = (Manager.Instance.StarLvl4 + " / 3");
			objectifs[7].text = (Manager.Instance.CollLvl4 + " / 9");
			objectifs[8].text = (Manager.Instance.StarLvl5 + " / 3");
			objectifs[9].text = (Manager.Instance.CollLvl5 + " / 9");

			if(Manager.Instance.HaveAllStars1 && Manager.Instance.StarBin[1] == 0){
				Manager.Instance.HaveAllStars1 = false;
				HideAllBut0();
				ChangeScreen(3);
			}
			if(Manager.Instance.HaveAllStars2 && Manager.Instance.StarBin[2] == 0){
				Manager.Instance.HaveAllStars2 = false;
				HideAllBut0();
				ChangeScreen(3);
			}
			if(Manager.Instance.HaveAllStars3 && Manager.Instance.StarBin[3] == 0){
				Manager.Instance.HaveAllStars3 = false;
				HideAllBut0();
				ChangeScreen(3);
			}
			if(Manager.Instance.HaveAllStars4 && Manager.Instance.StarBin[4] == 0){
				Manager.Instance.HaveAllStars4 = false;
				HideAllBut0();
				ChangeScreen(3);
			}
			if(Manager.Instance.HaveAllStars5 && Manager.Instance.StarBin[5] == 0){
				Manager.Instance.HaveAllStars5 = false;
				HideAllBut0();
				ChangeScreen(3);
			}

			foreach(Button button in transform.GetComponentsInChildren<Button>()){
				if(button.gameObject.transform.GetChild (0).GetComponent<Shadow>() != null){
					if(eS.currentSelectedGameObject == button.gameObject){
						button.gameObject.transform.GetChild (0).GetComponent<Shadow>().enabled = true;
					} else button.gameObject.transform.GetChild (0).GetComponent<Shadow>().enabled = false;
				}
			}

			doItOnce2 = true;

		} else if(doItOnce2){
			collectable[0].SetActive(false);
			collectable[1].SetActive(false);
			HideAllBut0();
			transform.GetChild(0).gameObject.SetActive(false);
			doItOnce = true;
			doItOnce2 = false;
		}

		if(Manager.Instance.LevelName != "0_HUB_YL" && Manager.Instance.LevelName != "0_HUB_GDT" && Manager.Instance.LevelName != "0_TUTO_P3_YL" && Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits" && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit"){
			if(Manager.Instance.ShowMenuOnce1 && Manager.Instance.StarBin[1] == 0){
				Manager.Instance.ShowMenuOnce1 = false;
				Manager.Instance.Invoke("Pause",1);
				Manager.Instance.HaveAllStars1 = true;
			}
			if(Manager.Instance.ShowMenuOnce2 && Manager.Instance.StarBin[2] == 0){
				Manager.Instance.ShowMenuOnce2 = false;
				Manager.Instance.Invoke("Pause",1);
				Manager.Instance.HaveAllStars2 = true;
			}
			if(Manager.Instance.ShowMenuOnce3 && Manager.Instance.StarBin[3] == 0){
				Manager.Instance.ShowMenuOnce3 = false;
				Manager.Instance.Invoke("Pause",1);
				Manager.Instance.HaveAllStars3 = true;
			}
			if(Manager.Instance.ShowMenuOnce4 && Manager.Instance.StarBin[4] == 0){
				Manager.Instance.ShowMenuOnce4 = false;
				Manager.Instance.Invoke("Pause",1);
				Manager.Instance.HaveAllStars4 = true;
			}
			if(Manager.Instance.ShowMenuOnce5 && Manager.Instance.StarBin[5] == 0){
				Manager.Instance.ShowMenuOnce5 = false;
				Manager.Instance.Invoke("Pause",1);
				Manager.Instance.HaveAllStars5 = true;
			}
		}

		if(Manager.Instance.LevelName == "0_HUB_YL"){
			collectibleText.text = (Manager.Instance.CollectibleCount + " / 45");
			starText.text = (Manager.Instance.TotalStarsTaken + " / 15");
			sousMenus[0].transform.GetChild (0).GetChild (1).GetComponent<Button>().interactable = false;
		} else if(Manager.Instance.LevelName == "0_HUB_GDT"){
			collectibleText.text = (Manager.Instance.CollectibleCount + " / 45");
			starText.text = (Manager.Instance.TotalStarsTaken + " / 15");
		} else if(Manager.Instance.GameLevel == 1){
			starText.text = (Manager.Instance.StarLvl1 + " / 3");
			collectibleText.text = (Manager.Instance.CollLvl1 + " / 9");
		} else if (Manager.Instance.GameLevel == 2){
			starText.text = (Manager.Instance.StarLvl2 + " / 3");
			collectibleText.text = (Manager.Instance.CollLvl2 + " / 9");
		} else if (Manager.Instance.GameLevel == 3){
			starText.text = (Manager.Instance.StarLvl3 + " / 3");
			collectibleText.text = (Manager.Instance.CollLvl3 + " / 9");
		} else if (Manager.Instance.GameLevel == 4){
			starText.text = (Manager.Instance.StarLvl4 + " / 3");
			collectibleText.text = (Manager.Instance.CollLvl4 + " / 9");
		} else if (Manager.Instance.GameLevel == 5){
			starText.text = (Manager.Instance.StarLvl5 + " / 3");
			collectibleText.text = (Manager.Instance.CollLvl5 + " / 9");
		}
		
		if(collectible1CountChange != Manager.Instance.CollectibleCount){
			collectable[1].SetActive(true);
			Invoke ("StopToShow", 3f);
			collectible1CountChange = Manager.Instance.CollectibleCount;
		}
		if(star1CountChange != Manager.Instance.TotalStarsTaken){
			collectable[0].SetActive(true);
			Invoke ("StopToShow", 3f);
			star1CountChange = Manager.Instance.TotalStarsTaken;
		}
	}

	void OnLevelWasLoaded(){
		Invoke ("ResetSons", 0.05f);
	}

	void ResetSons(){
		if(sonsPrefab.Length != GameObject.FindObjectsOfType<AudioSource>().Length){
			Array.Clear(sonsPrefab, 0, sonsPrefab.Length);
			sonsPrefab = new AudioSource[0];
			sonsPrefab = GameObject.FindObjectsOfType<AudioSource>() as AudioSource[];
			sons.Clear();
			musics.Clear();
			foreach(AudioSource son in sonsPrefab){
				if(son.tag == "Music"){
					musics.Add (son);
				} else sons.Add (son);
			}
			if(musics.Count > 0){
				sousMenus[1].transform.GetChild (3).GetComponent<Slider>().value = musics[0].volume;
			}
			if(sons.Count > 0){
				sousMenus[1].transform.GetChild (4).GetComponent<Slider>().value = sons[0].volume;
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

	public void SetVolumeMusique(){
		if(musics.Count != 0){
			for(int i = 0; i < musics.Count; i++){
				musics[i].volume = sousMenus[1].transform.GetChild (3).GetComponent<Slider>().value;
			}
		}
	}
	
	public void SetVolumeSon(){
		if(sons.Count != 0){
			for(int i = 0; i < sons.Count; i++){
				sons[i].volume = sousMenus[1].transform.GetChild (4).GetComponent<Slider>().value;
			}
		}
	}

	public void RetourAuJeu(){
		Manager.Instance.UnPause();
	}

	public void RetourRegie(){
		Manager.Instance.Save ();
		//LoadingScreen.Instance.mainButton[3].SetActive(false);
		Application.LoadLevel("0_HUB_YL");
		Manager.Instance.UnPause();
	}

	public void RetourMainMenu(){
		Manager.Instance.Save ();
		//LoadingScreen.Instance.mainButton[3].SetActive(false);
		Application.LoadLevel("Save_And_Quit");
		Manager.Instance.UnPause();
	}

	void StopToShow(){
		collectable[0].SetActive(false);
		collectable[1].SetActive(false);
	}

}

	
