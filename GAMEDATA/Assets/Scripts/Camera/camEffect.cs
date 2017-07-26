using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class camEffect : MonoBehaviour, ITriggerable {


	private camBehavior myCam;

	// POUR ENLEVER UN BUG AVEC L'INTERFACE ITRIGGERABLE
	private EffectList effect;

	void Awake () {
		myCam = Camera.main.GetComponent<camBehavior> ();
	}
	
	
	public void Triggered(EffectList effect){
		//Debug.Log ("Cam Trigger");
		if(effect.GetType() == typeof(Effect_Camera_Shake)){
			myCam.IsShaking = true;
		}

		if(effect.GetType() == typeof(Effect_Camera_Height)){
			myCam.LowestAutomaticPoint = effect.GetComponent<Effect_Camera_Height>().lowestAutomaticPoint;
		}

		if(effect.GetType() == typeof(Effect_Guide_Camera)){
			myCam.SetPos = true;
			myCam.CamEffectPos = effect.transform.forward*-1f;
		}

		if(effect.GetType() == typeof(Effect_Guide_Camera)){
			myCam.SetPos = true;
			myCam.CamEffectPos = effect.transform.forward*-1f;
		}

		if(effect.GetType() == typeof(Effect_Fix_Camera)){
			TriggerFixCam (effect.GetComponent<Effect_Fix_Camera>());
//			Camera.main.transform.position = effect.transform.position;
//			Camera.main.transform.rotation = effect.transform.rotation;
		}

		if(effect.GetType() == typeof(Effect_ChangeCamDistance))
		{
			float newDistance = effect.GetComponent<Effect_ChangeCamDistance>().newDistance;
			myCam.CamDistanceMax = newDistance;
		}
	}

	public void UnTriggered(EffectList effect){
		//Debug.Log ("Cam Untriggered");
		if(effect.GetType() == typeof(Effect_Camera_Shake)){
			myCam.IsShaking = false;
		}
		if(effect.GetType() == typeof(Effect_Guide_Camera)){
			myCam.SetPos = false;
			myCam.CamEffectPos = effect.transform.forward*-1f;
		}

		if(effect.GetType() == typeof(Effect_Fix_Camera)){
			UnTriggerFixCam();
		}
		if(effect.GetType() == typeof(Effect_Camera_Height)){
			myCam.LowestAutomaticPoint = 0.3f;
		}
		if(effect.GetType() == typeof(Effect_ChangeCamDistance))
		{
			myCam.CamDistanceMax = myCam.DefaultDistanceMax;
		}
	}

	// for TV destruction Feature
	// ==============================
	public void TempCamShake(float duration)
	{
		myCam.IsShaking = true;
		Invoke ("StopCamShake", duration);
	}

	public void StopCamShake()
	{
		myCam.IsShaking = false;
	}
	// ==============================


	// for Elevator loading
	// ==============================
	public void TriggerFixCam(Effect_Fix_Camera fixCamEffect)
	{
		myCam.IsMoving = false;
		myCam.IsPlayerControlled = false;
		myCam.IsLookAting = fixCamEffect.isLookAting;
		myCam.FixedView = fixCamEffect.transform.rotation.eulerAngles;
		myCam.FixedPos = fixCamEffect.transform.position;
	}

	public void UnTriggerFixCam()
	{
		myCam.IsMoving = true;
		myCam.IsPlayerControlled = true;
		myCam.IsLookAting = true;
		myCam.FixedView = Vector3.zero;
	}
	// ==============================
}
