using UnityEngine;
using System.Collections;

public class SwitchAnimation : MonoBehaviour 
{
	Animator anim;

	// ControllerVariables
	int 
		walkAnimTriggerHash = Animator.StringToHash("walkAnimTrigger"),
		runAnimTriggerHash = Animator.StringToHash("runAnimTrigger"),
		sprintAnimTriggerHash = Animator.StringToHash("sprintAnimTrigger"),
		walkAnimTriggerReversedHash = Animator.StringToHash("walkAnimTriggerReversed"),
		runAnimTriggerReversedHash = Animator.StringToHash("runAnimTriggerReversed"),
		sprintAnimTriggerReversedHash = Animator.StringToHash("sprintAnimTriggerReversed");

	void Start()
	{
		anim = GetComponent<Animator> ();

	}

	public void SetTrigger(int triggerHash)
	{
		anim.SetTrigger (triggerHash);
	}

	public void SetAnimSpeed(float speed)
	{
		anim.speed = speed;
	}

	public int WalkAnimTriggerHash
	{
		get{return walkAnimTriggerHash;}
	}

	public int RunAnimTriggerHash
	{
		get{return runAnimTriggerHash;}
	}

	public int SprintAnimTriggerHash
	{
		get{return sprintAnimTriggerHash;}
	}

	public int WalkAnimTriggerReversedHash
	{
		get{return walkAnimTriggerReversedHash;}
	}

	public int RunAnimTriggerReversedHash
	{
		get{return runAnimTriggerReversedHash;}
	}

	public int SprintAnimTriggerReversedHash
	{
		get{return sprintAnimTriggerReversedHash;}
	}
}
