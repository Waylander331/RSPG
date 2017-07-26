using UnityEngine;
using System.Collections;

public class MagnetSFXTrigger : MonoBehaviour 
{
	private MagnetSFX sfx;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "RailPlatform")
			sfx.AddOne ();
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "RailPlatform")
			sfx.RemoveOne ();
	}

	public MagnetSFX SFX
	{
		set{sfx = value;}
	}
}
