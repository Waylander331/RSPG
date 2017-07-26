using UnityEngine;
using System.Collections;

public class BridgemanStepAudioPlayer : MonoBehaviour 
{
	public void Play()
	{
		int random = Random.Range (0, 2);
		switch(random)
		{
		case 0:
			SoundManager.Instance.PlayAudioInMyself(gameObject, "BM_BMSidesteps");
			break;
		case 1:
			SoundManager.Instance.PlayAudioInMyself(gameObject, "BM_BMSidesteps_1");
			break;
		}
	}
}
