using UnityEngine;
using System.Collections;

public class SawAudioSource : MonoBehaviour 
{
	void Start()
	{
		SoundManager.Instance.PlayAudioInMyself (gameObject, "lethal_lethalsawmechanized");
	}
}
