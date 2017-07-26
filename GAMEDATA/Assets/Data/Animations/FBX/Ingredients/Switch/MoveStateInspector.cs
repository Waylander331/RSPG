/*
 * Author : Olivier Reid
 * 
 * Desc : 
 * Used with StateAnimation Script.
 * 
 * MoveStateInspector detects the state of the Avatar when it enters the trigger and calls the correct animation
 * to StateAnimation script. 
 * 
 * 
 */
using UnityEngine;
using System.Collections;

public class MoveStateInspector : MonoBehaviour 
{
	SwitchAnimation animScript; // 2nd child
	GenericTrigger trigger;

	bool 
		walkReversed,
		runReversed,
		sprintReversed;

	void Start()
	{
		animScript = transform.parent.GetChild (1).GetComponent<SwitchAnimation> ();
		trigger = GetComponent<GenericTrigger> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			Avatar av = Manager.Instance.Avatar;

			if(av.IsSprinting)
			{
				animScript.SetTrigger
					(trigger.CurrentSide == GenericTrigger.Side.back ? animScript.SprintAnimTriggerHash : animScript.SprintAnimTriggerReversedHash);
			}
			else if(av.stickForce > 0.6f) // Running
			{
				animScript.SetTrigger
					(trigger.CurrentSide == GenericTrigger.Side.back ? animScript.RunAnimTriggerHash : animScript.RunAnimTriggerReversedHash);
			}
			else if(av.stickForce > 0f)
			{
				animScript.SetTrigger 
					(trigger.CurrentSide == GenericTrigger.Side.back ? animScript.WalkAnimTriggerHash : animScript.WalkAnimTriggerReversedHash);
			}
			SoundManager.Instance.PlayAudio ("SwitchTourniquet");
		}
	}
}