using UnityEngine;
using System.Collections;

[System.Serializable]
public class Effect_ActivateSwitch : EffectList 
{
	public Effect_ActivateSwitch(){}
	public override void Activate (IsTriggerable triggeredObject)
	{
		triggeredObject.Activate (this);
	}
	
	public override void Deactivate (IsTriggerable triggeredObject)
	{
		triggeredObject.Deactivate (this);
	}
}
