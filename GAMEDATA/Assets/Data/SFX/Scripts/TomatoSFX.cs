using UnityEngine;
using System.Collections;

public class TomatoSFX : MonoBehaviour 
{
	private Transform tomatoSplash;

	private const float deactivationDelay = 0.5f;

	void Awake()
	{
		tomatoSplash = transform.GetChild (0);
		tomatoSplash.gameObject.SetActive (false);
		transform.parent.GetComponent<Tomato> ().TomatoSFX = this;
	}

	public void TriggerTomatoSplash()
	{
		tomatoSplash.gameObject.SetActive (true);
		Invoke ("DeactivateTomatoSplash", deactivationDelay);
	}

	void DeactivateTomatoSplash()
	{
		tomatoSplash.gameObject.SetActive (false);
	}
}
