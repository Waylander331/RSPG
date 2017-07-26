using UnityEngine;
using System.Collections;

public class NoSlipParent : MonoBehaviour 
{

	private Platform platform;
	private LiftingPlatform lift;
	private Avatar player;
	private LayerMask collWall;
	private bool canUnparent = false;
	private bool asHitWall = false;
	// Use this for initialization
	void Start ()
	{
		//collWall += 1 << 9;
		collWall += 1 << 11;
		lift = GetComponentInParent<LiftingPlatform>() as LiftingPlatform;
		platform = GetComponentInParent<Platform>() as Platform;
		player = Manager.Instance.Avatar;
	}

	//Rajoute par Arianne pour quand les plateformes sont DontDestroyOnReload
	void OnLevelWasLoaded()
	{
		Invoke ("WaitForManager", 0.001f);
	}
	void WaitForManager()
	{
		lift = GetComponentInParent<LiftingPlatform>() as LiftingPlatform;
		platform = GetComponentInParent<Platform>() as Platform;
		player = Manager.Instance.Avatar;
	}
	//--------

	// Update is called once per frame
	void Update ()
	{
		if(platform != null && canUnparent)
		{
			if(!platform.Unrotated)
			{
				player.transform.parent = null;
			}
		}

		if(asHitWall && canUnparent)
		{
			player.transform.parent = null;
		}
	}

	void LateUpdate()
	{
		if(platform != null && canUnparent)		
		{
			if(!platform.Unrotated)
			{
				player.transform.parent = null;
			}
		}
	}

	void FixedUpdate()
	{
		if(platform != null && canUnparent)		
		{
			if(!platform.Unrotated)
			{
				player.transform.parent = null;
			}
		}
	}

	void OnTriggerStay(Collider other)
	{
		if(platform != null && !asHitWall)
		{
			if(other.tag == ("Player") && platform.Unrotated)
			{
				player.transform.parent = this.transform;
				canUnparent = true;
			}
		}
		if((1 << other.gameObject.layer) == collWall && player.transform.parent != null)
		{
			asHitWall = true;
		}

		if (lift != null)
		{
			if(other.tag == ("Player"))
		   	{
				player.transform.parent = this.transform;
			}
		}
	} 

	void OnTriggerExit(Collider other)
	{
		if(other.tag == ("Player"))
		{
			canUnparent = false;
			player.transform.parent = null;
		}
		if((collWall << other.gameObject.layer)!= 0)
		{
			asHitWall = false;
		}
	}
}
