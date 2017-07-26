using UnityEngine;
using System.Collections;

public class StepDustSFX : MonoBehaviour 
{
	private const float destroyDelay = 1f;

	void Start()
	{
		Invoke ("DestroySelf", destroyDelay);
	}

	void DestroySelf()
	{
		Destroy (gameObject);
	}
}
