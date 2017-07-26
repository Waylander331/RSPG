using UnityEngine;
using System.Collections;

public class SpikesAudioPlayer : MonoBehaviour 
{
	public void PlayAudioOut()
	{
		SoundManager.Instance.PlayAudioInMyself (gameObject, "lethal_lethalspikein");
	}

	public void PlayAudioIn()
	{
		SoundManager.Instance.PlayAudioInMyself (gameObject, "lethal_lethalspikeout");
	}
}
