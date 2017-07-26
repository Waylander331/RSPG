using UnityEngine;
using System.Collections;

public class CinManager : MonoBehaviour 
{
	public GameObject[] maskDusts;
	private int soundIndex;
	private int mcIndex;

	void Start()
	{
		if (Manager.Instance.LevelName == "CINEMATIQUE_FIN"){ 
			Manager.Instance.CinWasPlayed = true;
			Manager.Instance.IsFromCin = true;
			Debug.Log (Manager.Instance.IsFromCin);

			soundIndex = 0;
			mcIndex = 0;
			Invoke ("MC", 0.5f);
			
			foreach (GameObject dust in maskDusts)
				dust.SetActive (false);
		}
	}

	void RollCredits(){
		Application.LoadLevel("Credits");
	}

	void ToMenu(){
		Application.LoadLevel("EcranTitre");
	}

	void PlaySound(){
		switch (soundIndex){
		case 0:
			SoundManager.Instance.PlayAudio("CinPhotoGrab");
			break;
		case 1:
			SoundManager.Instance.PlayAudio("CinPhotoMan");
			break;
		case 2:
			SoundManager.Instance.PlayAudio("CinMaskClean");
			break;
		case 3:
			SoundManager.Instance.PlayAudio("CinCarpetFall");
			break;
		case 4: 
			SoundManager.Instance.PlayAudio("CinAvatarFall");
			break;
		case 5:
			SoundManager.Instance.PlayAudio("CinMaskBreak");
			break;
		}
		soundIndex ++;
	}

	void MC(){
		switch (mcIndex){
		case 0:
			SoundManager.Instance.PlayAudio ("CinAie");
			Invoke ("MC", 3f);
			break;
		case 1: 
			SoundManager.Instance.PlayAudio ("CinCeQueTuVoulais");
			Invoke ("MC", 2.5f);
			break;
		case 2: 
			SoundManager.Instance.PlayAudio ("CinToutDetruit");
			Invoke ("MC", 2.5f);
			break;
		case 3:
			SoundManager.Instance.PlayAudio ("CinTesHeureuse");
			Invoke ("MC", 4f);
			break;
		case 4:
			SoundManager.Instance.PlayAudio ("CinMasque");
			break;
		}
		mcIndex ++;
	}

	void MissingFootstep(){
		SoundManager.Instance.AvatarFootstepsRandom();
	}

	void CinCourteSound(){
		SoundManager.Instance.PlayAudio("CinPhotoMan");
	}

	void TriggerDustEvent()
	{
		foreach (GameObject dust in maskDusts){
			dust.SetActive (true);
		}
	}
}
