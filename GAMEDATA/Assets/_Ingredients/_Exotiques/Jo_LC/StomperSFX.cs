using UnityEngine;
using System.Collections;

public class StomperSFX : MonoBehaviour 
{
	private Transform sparks01;
	private Transform sparks02;

	private const float effectDuration = 3f;
	private LayerMask collision = 1 << 9;

	void Start()
	{
		sparks01 = transform.GetChild (0);
		sparks02 = transform.GetChild (1);

		sparks01.gameObject.SetActive (false);
		sparks02.gameObject.SetActive (false);
	}

	void FixedUpdate()
	{
		Ray ray = new Ray (transform.position, -Vector3.up);
		if (!sparks01.gameObject.activeSelf && Physics.Raycast (ray, 1f, collision))
			TriggerSparks ();
	}

	void TriggerSparks()
	{
		sparks01.gameObject.SetActive (true);
		sparks02.gameObject.SetActive (true);
		Invoke ("DisableSparks", effectDuration);
	}

	void DisableSparks()
	{
		sparks01.gameObject.SetActive (false);
		sparks02.gameObject.SetActive (false);
	}
}
