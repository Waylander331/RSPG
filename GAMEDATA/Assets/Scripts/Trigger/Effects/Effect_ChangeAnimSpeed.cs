using UnityEngine;
using System.Collections;

public class Effect_ChangeAnimSpeed : EffectList 
{
	public bool isOrnithorynque = true;
	public float targetSpeed;
	public bool instantSpeedChange;
	public float speedIncrement;

	public override void Activate (IsTriggerable triggeredObject)
	{
		triggeredObject.Activate (this);
	}

	public override void Deactivate (IsTriggerable triggeredObject)
	{
		triggeredObject.Deactivate (this);
	}
}
