using UnityEngine;
using System.Collections;

public class Effect_ChangeCamDistance : EffectList 
{
	public float newDistance;

	public override void Activate (IsTriggerable triggeredObject)
	{
		triggeredObject.Activate (this);
	}

	public override void Deactivate (IsTriggerable triggeredObject)
	{
		triggeredObject.Deactivate (this);
	}
}
