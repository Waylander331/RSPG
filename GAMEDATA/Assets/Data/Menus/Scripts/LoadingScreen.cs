using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {
	
	public GameObject[] mainButton;
	public Text level;

	private static LoadingScreen instance;

	void Awake(){
		DontDestroyOnLoad(this.gameObject);
		EssenceMemeDuSingleton();
	}

	void Update(){

		if(!Manager.Instance.IsNewGame && !Manager.Instance.HaveContinue && Application.isLoadingLevel){
			mainButton[1].SetActive(true);
			mainButton[0].SetActive(false);
		} else if((Manager.Instance.IsNewGame || Manager.Instance.HaveContinue) && Application.isLoadingLevel){
			mainButton[1].SetActive(true);
			mainButton[0].SetActive(true);
		} else if(!Application.isLoadingLevel){
			mainButton[1].SetActive(false);
			mainButton[0].SetActive(false);
		}

		if(Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits" 
		   && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit"){
			if(CheatManager.Instance.CheatEnabled){
				mainButton[2].SetActive(true);

				/*if(Application.isLoadingLevel && !Manager.Instance.Avatar.InElevator){
					mainButton[3].SetActive(true);
				} else if(Application.isLoadingLevel && Manager.Instance.Avatar.InElevator) mainButton[3].SetActive(false);*/
				if(!Application.isLoadingLevel){
					mainButton[3].SetActive(false);
				}

			} else {
				mainButton[2].SetActive(false);
				mainButton[3].SetActive(false);
			}
		} else {
			mainButton[2].SetActive(false);
			mainButton[3].SetActive(false);
		}
	}

	void EssenceMemeDuSingleton(){		
		if(LoadingScreen.Instance == null){
			instance = this;
		}
		else{
			if(LoadingScreen.Instance != this){
				Destroy(this.gameObject);
			}			
		}
	}

	public static LoadingScreen Instance {
		get {return instance;}
	}
}
