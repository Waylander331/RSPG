 using UnityEngine;
using System.Collections;

public class AvatarSFX : MonoBehaviour 
{
	private Transform footprint_Left;
	private Transform footprint_Right;
	private Transform footstepDust_Left;
	private Transform footstepDust_Left2;
	private Transform footstepDust_Right;
	private Transform footstepDust_Right2;
	private Transform jumpLandingDust;
	private Transform jumpLandingDust2;
	private Transform wallSlideDust_Left;
	private Transform wallSlideDust_Left2;
	private Transform wallSlideDust_Right;
	private Transform wallSlideDust_Right2;
	private Transform knockbackDust;
	private Transform knockbackDust2;

	private const float deactivateDelay = 0.25f;

	private const float effectRate = 0.2f;
	private bool canLeftFootprint = true;
	private bool canRightFootprint = true;
	private bool canDustLeftFoot = true;
	private bool canDustRightFoot = true;
	private bool canFallDust = true;

	void Awake()
	{
		footprint_Left = transform.GetChild (0);
		footprint_Right = transform.GetChild (1);
		footstepDust_Left = transform.GetChild (2);
		footstepDust_Left2 = transform.GetChild (3);
		footstepDust_Right = transform.GetChild (4);
		footstepDust_Right2 = transform.GetChild (5);
		jumpLandingDust = transform.GetChild (6);
		jumpLandingDust2 = transform.GetChild (7);
		wallSlideDust_Left = transform.GetChild (8);
		wallSlideDust_Left2 = transform.GetChild (9);
		wallSlideDust_Right = transform.GetChild (10);
		wallSlideDust_Right2 = transform.GetChild (11);
		knockbackDust = transform.GetChild (12);
		knockbackDust2 = transform.GetChild (13);

		footprint_Left.gameObject.SetActive (false);
		footprint_Right.gameObject.SetActive (false);
		footstepDust_Left.gameObject.SetActive (false);
		footstepDust_Left2.gameObject.SetActive (false);
		footstepDust_Right.gameObject.SetActive (false);
		footstepDust_Right2.gameObject.SetActive (false);
		jumpLandingDust.gameObject.SetActive (false);
		jumpLandingDust2.gameObject.SetActive (false);
		wallSlideDust_Left.gameObject.SetActive (false);
		wallSlideDust_Left2.gameObject.SetActive (false);
		wallSlideDust_Right.gameObject.SetActive (false);
		wallSlideDust_Right2.gameObject.SetActive (false);
		knockbackDust.gameObject.SetActive (false);
		knockbackDust2.gameObject.SetActive (false);
	}

	void Start()
	{
		Manager.Instance.Avatar.AvatarSFX = this;
	}

	public void Effect_SpawnFootprint_Left()
	{
		if (canLeftFootprint)
		{
			GameObject footprint = (GameObject) Instantiate (footprint_Left.gameObject, footprint_Left.position, footprint_Left.rotation);
			footprint.SetActive (true);
			footprint.transform.parent = null;
			canLeftFootprint = false;
			Invoke ("CanLeftFootprint", effectRate);
		}
	}

	void CanLeftFootprint()
	{
		canLeftFootprint = true;
	}

	public void Effect_SpawnFootprint_Right()
	{
		if(canRightFootprint)
		{
			GameObject footprint = (GameObject) Instantiate (footprint_Right.gameObject, footprint_Right.position, footprint_Right.rotation);
			footprint.SetActive (true);
			footprint.transform.parent = null;
			canRightFootprint = false;
			Invoke ("CanRightFootprint", effectRate);
		}
	}

	void CanRightFootprint()
	{
		canRightFootprint = true;
	}
	
	/// <summary>
	/// Triggers a dust effect from footsteps. leftFoot(bool) value determines whether the dust comes from
	/// the left foot (true) or the right foot (false).
	/// </summary>
	/// <param name="leftFoot">If set to <c>true</c> left foot.</param>
	public bool TriggerEffect_FootstepDust(bool leftFoot)
	{
		if(leftFoot && canDustLeftFoot)
		{
			GameObject dust = (GameObject) Instantiate (footstepDust_Left.gameObject, footstepDust_Left.transform.position, footstepDust_Left.transform.rotation);
			dust.SetActive(true);
			canDustLeftFoot = false;
			Invoke ("CanDustLeftFoot", effectRate);
			return true;
		}
		else if (!leftFoot && canDustRightFoot)
		{
			GameObject dust = (GameObject) Instantiate (footstepDust_Right.gameObject, footstepDust_Right.transform.position, footstepDust_Right.transform.rotation);
			dust.SetActive(true);
			canDustRightFoot = false;
			Invoke ("CanDustRightFoot", effectRate);
			return true;
		}
		return false;
	}

	void CanDustLeftFoot()
	{
		canDustLeftFoot = true;
	}

	void CanDustRightFoot()
	{
		canDustRightFoot = true;
	}

	public void TriggerEffect_HeavyFootstepDust(bool leftFoot)
	{
		if(TriggerEffect_FootstepDust(leftFoot))
		{
			if(leftFoot)
			{
				GameObject dust = (GameObject) Instantiate (footstepDust_Left2.gameObject, footstepDust_Left2.transform.position, footstepDust_Left2.transform.rotation);
				dust.SetActive(true);
			}
			else
			{
				TriggerEffect_FootstepDust (leftFoot);
				GameObject dust = (GameObject) Instantiate (footstepDust_Right2.gameObject, footstepDust_Right2.transform.position, footstepDust_Right2.transform.rotation);
				dust.SetActive(true);
			}
		}
	}

	void DeactivateFootstepDust_Left()
	{
		footstepDust_Left.gameObject.SetActive (false);
		footstepDust_Left2.gameObject.SetActive (false);
	}

	void DeactivateFootstepDust_Right()
	{
		footstepDust_Right.gameObject.SetActive (false);
		footstepDust_Right2.gameObject.SetActive (false);
	}

	public bool TriggerEffect_JumpLandingDust()
	{
		if(canFallDust)
		{
			GameObject dust = (GameObject) Instantiate (jumpLandingDust.gameObject, jumpLandingDust.transform.position, jumpLandingDust.transform.rotation);
			dust.SetActive(true);
			canFallDust = false;
			Invoke ("CanFallDust", effectRate);
			return true;
		}
		return false;
	}

	void CanFallDust()
	{
		canFallDust = true;
	}

	public void TriggerEffect_HeavyJumpLandingDust()
	{
		if(TriggerEffect_JumpLandingDust ())
		{
			GameObject dust = (GameObject) Instantiate (jumpLandingDust2.gameObject, jumpLandingDust2.transform.position, jumpLandingDust2.transform.rotation);
			jumpLandingDust2.gameObject.SetActive (true);
		}
	}

	public void SetKnockbackDustEffect(bool value)
	{
		knockbackDust.gameObject.SetActive (value);
	}

	public void SetHeavyKnockbackDustEffect(bool value)
	{
		SetKnockbackDustEffect (value);
		knockbackDust2.gameObject.SetActive (value);
	}

	/// <summary>
	/// Activate or deactivate slide effect with the value(bool) value. 
	/// Set leftHand(bool) to true if it's for the leftHand, or false for the right hand.
	/// </summary>
	/// <param name="value">If set to <c>true</c> value.</param>
	/// <param name="left">If set to <c>true</c> left.</param>
	public void SetSlideEffect(bool value, bool leftHand)
	{
		GameObject hand = leftHand ? wallSlideDust_Left.gameObject : wallSlideDust_Right.gameObject;
		hand.SetActive (value);
	}

	public void SetHeavySlideEffect(bool value, bool leftHand)
	{
		SetSlideEffect (value, leftHand);
		GameObject hand = leftHand ? wallSlideDust_Left2.gameObject : wallSlideDust_Right2.gameObject;
		hand.SetActive (value);
	}
}
