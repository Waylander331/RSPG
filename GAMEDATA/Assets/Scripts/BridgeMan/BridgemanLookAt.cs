/*
 * Author : Olivier Reid
 * 
 * Desc : Simple lookAt only on the X axis.
 * 
 */

using UnityEngine;
using System.Collections;

public class BridgemanLookAt : MonoBehaviour 
{
	private float turningValueInc = 0.075f;
	private float recoverValueInc = 0.1f;
	//private bool active;
	private Transform player;
	
	private bool didMove; // detects whether the bridgeman moved or not
	private bool movedLeft; // detects whether the bridgeman moved left (true) or right (false)

	private float lastRotation; // euler.y
	private float angle;
	private float newAngle;
	private Animator anim;
	private int turnValueHash = Animator.StringToHash("turnValue");
	private float turningValue = 0f;

	private BridgemanStepAudioPlayer stepAudio;
	private bool playingStepAudio;
	private const float initStepAudioDelay = 0.2f;
	private const float repeatStepAudioDelay = 0.5f;

	private bool isIdle;

	void Start()
	{
		stepAudio = transform.GetChild (8).GetComponent<BridgemanStepAudioPlayer> ();
		player = Manager.Instance.Avatar.transform;
		if (player == null)
			Debug.LogWarning ("Didn't find player on start");
		else 
		{
			lastRotation = transform.rotation.eulerAngles.y;
		}

		anim = GetComponent<Animator> ();
		if(anim == null)
		{
			Debug.LogWarning("No Animator in bridgeman");
		}
	}

	void FixedUpdate()
	{
		CheckDidMove (); // Update didMove and movedLeft
		UpdateRotationDirection ();
		UpdateTurningValue (); // change turnValue toward extremities
	}

	void Update () 
	{
		if (player == null) 
		{
			player = Manager.Instance.Avatar.transform;
		}
		else //if (active)
		{
			transform.LookAt (player.transform.position);
			Vector3 eulers = transform.eulerAngles;
			transform.rotation = Quaternion.Euler (0f, eulers.y, 0f);
		}
	}

	void LateUpdate()
	{
		if (IsTurning () && !playingStepAudio)
			playingStepAudio = true;
		else if (!IsTurning () && playingStepAudio)
			playingStepAudio = false;
		
		if (playingStepAudio && !IsInvoking ("PlayStepAudio"))
		{
			InvokeRepeating ("PlayStepAudio", initStepAudioDelay, repeatStepAudioDelay);
		}
		else if (!playingStepAudio && IsInvoking ("PlayStepAudio"))
		{
			CancelInvoke ("PlayStepAudio");
		}
	}

	public void StopAudio()
	{
		if (IsInvoking ("PlayStepAudio"))
			CancelInvoke ("PlayStepAudio");
	}

	public void PlayStepAudio()
	{
		stepAudio.Play ();
	}

	public bool DidMove
	{
		get{return didMove;}
	}

	void CheckDidMove()
	{
		didMove = lastRotation != transform.rotation.eulerAngles.y;
	}

	private void UpdateRotationDirection()
	{
		float currentAngle = transform.rotation.eulerAngles.y;
		movedLeft = Mathf.DeltaAngle(currentAngle, lastRotation) > 0f;
	
		lastRotation = transform.rotation.eulerAngles.y;
	}

	private void UpdateTurningValue()
	{
		if (didMove) 
		{
			if(movedLeft)
			{
				if(turningValue - turningValueInc > -1f)
					turningValue -= turningValueInc;
				else
					turningValue = -1f;
			}
			else
			{
				if(turningValue + turningValueInc < 1f)
					turningValue += turningValueInc;
				else
					turningValue = 1f;
			}
		}
		// change turnValue toward 0
		else
		{
			if(turningValue < 0f && turningValue + recoverValueInc < 0f)
				turningValue += recoverValueInc;
			else if(turningValue > 0f && turningValue - recoverValueInc > 0f)
				turningValue -= recoverValueInc;
			else
				turningValue = 0f;
		}
		anim.SetFloat(turnValueHash, turningValue);
	}

	bool IsTurning()
	{
		return isIdle && turningValue != 0;
	}

	public bool IsIdle
	{
		set{isIdle = value;}
	}
}
