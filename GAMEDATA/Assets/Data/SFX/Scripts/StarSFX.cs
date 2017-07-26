using UnityEngine;
using System.Collections;

public class StarSFX : MonoBehaviour 
{
	private Transform starGet;
	private Transform starConfettis;

	private const float effectDuration = 1f;

	void Awake()
	{
		starGet = transform.GetChild (0);
		starConfettis = transform.GetChild (1);
	}

	void Start()
	{
		transform.parent.GetComponent<Stars> ().StarSFX = this;
	}

	public void TriggerStarGetEffect()
	{
		if(!starGet.gameObject.activeSelf)
		{
			starGet.gameObject.SetActive(true);
			Invoke ("StopStarGetEffect", effectDuration);
		}
	}

	void StopStarGetEffect()
	{
		starGet.gameObject.SetActive (false);
	}

	public void TriggerStarConfettisEffect()
	{
		if(!starConfettis.gameObject.activeSelf)
		{
			starConfettis.gameObject.SetActive(true);
			Invoke ("StopStarConfettisEffect", effectDuration);
		}
	}

	void StopStarConfettisEffect()
	{
		starConfettis.gameObject.SetActive (false);
	}
}
