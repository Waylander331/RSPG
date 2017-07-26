using UnityEngine;
using System.Collections;

public class ScissorLiftPlatform : MonoBehaviour
{
	private LiftingPlatform lift;
	// Use this for initialization
	void Start () 
	{
		lift = GetComponentInParent<LiftingPlatform>();
	}
	
	// Gage the animation in function of the Trigger.
	void OnTriggerEnter(Collider other)
	{
		if ( other.tag == ("UpperTrigger"))
		{
			lift.myAnime["vibrateUp"].speed = 0;
			lift.CanRewind = true;
			if(!lift.PlayerIn)
			{
				//lift.myAnime.Play();
				lift.myAnime["vibrateUp"].speed = -0.4f;
			}
		}

		if(lift.CanRewind && other.tag == ("LowerTrigger"))
		{
			lift.myAnime["vibrateUp"].speed = 0;
			lift.CanRewind = false;
			if(lift.PlayerIn)
			{
				//lift.myAnime.Play();
				lift.myAnime["vibrateUp"].speed = 0.8f;
			}
		}
	}
}
