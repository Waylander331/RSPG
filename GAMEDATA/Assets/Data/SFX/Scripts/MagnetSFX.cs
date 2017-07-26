using UnityEngine;
using System.Collections;

public class MagnetSFX : MonoBehaviour 
{
	private int platformCount = 0;

	private Transform[] sparksSFX;
	private MagnetSFXTrigger trigger;

	void Start ()
	{
		sparksSFX = new Transform[transform.childCount - 1];
		for(int i = 0; i < sparksSFX.Length; i++)
		{
			sparksSFX[i] = transform.GetChild(i);
		}

		trigger = transform.GetChild (transform.childCount - 1).GetComponent<MagnetSFXTrigger> ();
		trigger.SFX = this;
		foreach (Transform t in sparksSFX)
			t.gameObject.SetActive (false);
	}

	void ActivateSparks()
	{
		foreach (Transform t in sparksSFX)
			t.gameObject.SetActive (true);
	}

	void DeactivateSparks()
	{
		foreach (Transform t in sparksSFX)
			t.gameObject.SetActive (false);
	}

	public void AddOne()
	{
		platformCount++;
		ActivateSparks();
	}

	public void RemoveOne()
	{
		platformCount--;
		if (platformCount == 0)
			DeactivateSparks ();
	}
}
