using UnityEngine;
using System.Collections;

public class MCCredits : MonoBehaviour {

	private int index;

	void Start(){
		if (Manager.Instance.CinWasPlayed) Invoke ("Play", 1.5f);
	}

	void Play(){
		switch (index){
		case 0:
			SoundManager.Instance.PlayAudio ("CreditsQuoiCa");
			Invoke ("Play", 5f);
			break;
		case 1: 
			SoundManager.Instance.PlayAudio ("CreditsQuoiCeBruit");
			Invoke ("Play", 5f);
			break;
		case 2: 
			SoundManager.Instance.PlayAudio ("CreditsExcuse");
			Invoke ("Play", 8f);
			break;
		case 3: 
			SoundManager.Instance.PlayAudio ("CreditsSortir");
			Invoke ("Play", 4.5f);
			break;
		case 4:
			SoundManager.Instance.PlayAudio ("CreditsQqn");
			Invoke ("Play", 4f);
			break;
		case 5:
			SoundManager.Instance.PlayAudio ("CreditsCandidate749");
			Invoke ("Play", 4.5f);
			break;
		case 6: 
			SoundManager.Instance.PlayAudio ("CreditsAnya");
			break;
		}
		index ++;
	}
}
