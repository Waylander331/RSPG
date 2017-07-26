using UnityEngine;
using System.Collections;

public class AnimationBridgeman : MonoBehaviour 
{
	private const float shootDelay = 2f;
	private const float kickDelay = 1f;

	private float rotationSpeed; // TODO implements a rotation instead of lookAt.

	Animator anim;

	// Both detection trigger for the Bridgeman :
	// 		- one for it's vision where he'll start to shoot
	// 		- one for it's kicking range where he'll stop shooting and start kicking the player.
	private BridgeTrigger 
		vision,
		kick;

	private BridgemanLookAt lookAt;
	private BridgemanSpin spin;
	private bool changeDetected = true;

	private TomatoSpawn tomatoSpawn;
	private Transform bridgeCollisions;
	private Transform echasses;

	private bool isFalen;
	private LayerMask collision = 1 << 9;
	private LayerMask wallJumpable = 1 << 11;
	private bool kicked = false;

	// Variable Hashs
	int 
		seesPlayerHash = Animator.StringToHash("seesPlayer"),
		isHitHash = Animator.StringToHash("isHit"),
		spinDoneHash = Animator.StringToHash("spinDone"),
		fallDoneHash = Animator.StringToHash("fallDone"),
		shootHash = Animator.StringToHash("shoot"),
		kickHash = Animator.StringToHash("kick"),
		inKickingRangeHash = Animator.StringToHash("inKickingRange");

	// State Hashs
	int 
		idleStateHash = Animator.StringToHash("Blend Tree"),
		preKickStateHash = Animator.StringToHash("PreKick"),
		kickStateHash = Animator.StringToHash("Kick"),
		spinStateHash = Animator.StringToHash("Spin"),
		fallStateHash = Animator.StringToHash("Fall");

	// GunAnimator
	private Animator gunAnim;
	private Transform gunHandle;
	private Transform gunSnapPosition;
	private bool isSnapped = true;

	private Vector3 gunDirection;
	private const float gunForce = 200f;
	private bool applyingGunForce;
	private const float gunForceDuration = 0.1f;
	private Rigidbody gunRigidbody;
	private const float gunHeight = 0.12f;
	private const float gunLength = 0.9f;

	// GunStateHash
	int
		gunShootHash = Animator.StringToHash ("GunShoot");

	//private ScreenVisibleListener[] visibleListeners;

	private bool playingStepAudio;

	void Awake()
	{
		gunAnim = transform.GetChild (6).GetComponent<Animator> ();
		gunHandle = transform.GetChild (6).GetChild (2);
		gunSnapPosition = transform.GetChild (7).GetChild (0).GetChild (0).GetChild (0).GetChild (2).GetChild (0).GetChild (0).GetChild (2).
			GetChild (0).GetChild (0).GetChild (0).GetChild (0);

		bridgeCollisions = transform.GetChild (2);
		echasses = transform.GetChild (5);
	}

	void Start()
	{
		//visibleListeners = GetComponentsInChildren<ScreenVisibleListener> ();
		tomatoSpawn = GetComponent<TomatoSpawn>();
		anim = GetComponent<Animator> ();
		vision = transform.GetChild (0).GetComponent<BridgeTrigger>(); // Vision trigger MUST be the first child.
		kick = transform.GetChild (1).GetComponent<BridgeTrigger> (); // Kick trigger MUST be the second child
		kick.IsKickDetector = true;

		vision.AnimScript = kick.AnimScript = this;
		
		if (vision == null)
			Debug.LogWarning ("Child 0 doesn't have a script BridgeTrigger");
		if (kick == null)
			Debug.LogWarning ("Child 1 doesn't have a script BridgeTrigger");

		BridgemanLookAt lookAtComponent = GetComponent<BridgemanLookAt> ();
		if (lookAtComponent != null) 
		{
			lookAt = lookAtComponent;
		}
		else
		{
			lookAt = gameObject.AddComponent<BridgemanLookAt>();
		}
		spin = GetComponent<BridgemanSpin> ();

		gunAnim.transform.GetChild (0).GetComponent<BoxCollider> ().center = new Vector3 (0.35f, 0.21f, 7.37f);
		gunAnim.transform.GetChild (1).GetComponent<BoxCollider> ().center = new Vector3 (0.35f, -1.13f, 7.42f);

		gunAnim.transform.GetChild (0).GetComponent<CapsuleCollider> ().center = new Vector3 (0.49f, -0.12f, 7.07f);
		gunAnim.transform.GetChild (1).GetComponent<CapsuleCollider> ().center = new Vector3 (0.51f, -1.5f, 7.05f);
		gunAnim.transform.GetChild (3).GetComponent<BoxCollider> ().center = new Vector3 (0.52f, 0.2f, 7.05f);
		gunHandle.position = gunSnapPosition.position;
	}

	void FixedUpdate()
	{
		if(applyingGunForce)
		{
			gunRigidbody.AddForce(gunDirection.normalized * gunForce);
		}
	}

	void LateUpdate()
	{
		if(isSnapped)
		{
			gunHandle.position = gunSnapPosition.position;
		}
	}

	void Update () 
	{
		int current = anim.GetCurrentAnimatorStateInfo (0).shortNameHash;
		
		if(!isFalen)
		{
			// Update seesPlayer bool
			if (vision.IsDetecting != anim.GetBool(seesPlayerHash)) 
			{
				//lookAt.enabled = vision.IsDetectinlog;
				anim.SetBool (seesPlayerHash, vision.IsDetecting);
				if(!kick.IsDetecting && !IsInvoking("TriggerShootAnim"))
				{
					InvokeRepeating("TriggerShootAnim", shootDelay, shootDelay * 1.5f);
				}
			}
			else if(!vision.IsDetecting || kick.IsDetecting)
			{
				if(IsInvoking ("TriggerShootAnim"))
				{
					CancelInvoke("TriggerShootAnim");
				}
			}
		}
		// Invoke Shoot

		// Invoke Kick
		if (kick.IsDetecting)
		{
			anim.SetBool(inKickingRangeHash, true);
			if(!IsInvoking("InvokeKick") && current == idleStateHash/*current != kickStateHash && current != kickToIdleStateHash && current != preKickStateHash*/)
			{
				Invoke ("InvokeKick", kickDelay);
				changeDetected = false;
			}
		}
		else
		{
			anim.SetBool(inKickingRangeHash, false);
			if(IsInvoking("InvokeKick"))
			{
				CancelInvoke("InvokeKick");
				changeDetected = false;
			}
		}

		if (current == fallStateHash) 
		{
			bridgeCollisions.gameObject.SetActive(true);

			if(isSnapped)
			{
				isSnapped = false;
				gunRigidbody = gunAnim.gameObject.AddComponent<Rigidbody>();
				applyingGunForce = true;
				Invoke ("StopGunForce", gunForceDuration);
				gunAnim.enabled = false;

				gunDirection = transform.forward * gunLength;
				gunDirection.y = gunHeight;
			}
		}

		if(!kicked && current == kickStateHash && kick.IsDetecting)
		{
			kicked = true;
			Manager.Instance.Avatar.KnockBack(transform);
			SoundManager.Instance.PlayAudio("BMKnockBack");
			SoundManager.Instance.BMKickRandom();
			Invoke ("CanKick", kickDelay);
		}
		else if(kicked && !kick.IsDetecting)
		{
			kicked = false;
			if(IsInvoking ("CanKick"))
				CancelInvoke("CanKick");
		}
		lookAt.IsIdle = current == idleStateHash;

		if(!isFalen)
		{
			/*
			if(lookAt.enabled && !IsVisible())
				lookAt.enabled = false;
			else if(!lookAt.enabled && IsVisible())
				lookAt.enabled = true;
			*/

			if (kick.IsDetecting && lookAt.enabled && !Manager.Instance.Avatar.IsGrounded)
			{
				Ray ray = new Ray (Manager.Instance.Avatar.transform.position, Manager.Instance.Avatar.transform.forward);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, Vector3.Distance(transform.position, Manager.Instance.Avatar.transform.position), wallJumpable))
				{
					if(hit.collider.tag == "Bridgeman")
					{
						lookAt.StopAudio();
						lookAt.enabled = false;
					}
				}
			}
			else if (!lookAt.enabled && Manager.Instance.Avatar.IsGrounded)
			{
				lookAt.enabled = true;
			}
		}
	}

	void CanKick()
	{
		kicked = false;
	}

	void TriggerShootAnim()
	{
		if (vision.IsDetecting && !kick.IsDetecting && !anim.GetBool (isHitHash) && vision.IsSeeing()) 
		{
			if(tomatoSpawn.ShootTomato())
			{
				anim.SetTrigger (shootHash);
				gunAnim.SetTrigger (gunShootHash);
				SoundManager.Instance.BMFireTomatoRandom();
			}
		}
		else
		{
			CancelInvoke ("TriggerShootAnim");
		}
	}

	void InvokeKick()
	{
		anim.SetTrigger (kickHash);
	}
	
	// Triggers a hit on the bridgeman.
	public void Fall()
	{
		if(lookAt.enabled)
		{
			lookAt.StopAudio ();
			lookAt.enabled = false;
		}
		isFalen = true;
		anim.SetBool (isHitHash, true);
		anim.SetTrigger(kickHash);
		GetComponent<StopAtPos>().InitializeForSpin();
		spin.Rotating = true;
		CancelInvoke("InvokeKick");
		echasses.gameObject.SetActive(false);
		SoundManager.Instance.BMFallRandom ();
	}

	public TomatoSpawn TomatoScript
	{
		get{return tomatoSpawn;}
	}

	void StopGunForce()
	{
		applyingGunForce = false;
		Invoke ("DestroyGun", 3f);
	}

	void DestroyGun()
	{
		gunAnim.gameObject.SetActive (false);
	}

	/*
	bool IsVisible()
	{
		foreach (ScreenVisibleListener listener in visibleListeners)
			if (listener.IsVisible)
				return true;
		return false;
	}
	*/
}
