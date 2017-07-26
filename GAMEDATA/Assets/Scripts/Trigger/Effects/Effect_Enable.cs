using UnityEngine;
using System.Collections;

public class Effect_Enable : EffectList 
{
	public override void Activate(IsTriggerable triggeredObject)
	{
		triggeredObject.gameObject.SetActive(true);
	}

	public override void Deactivate(IsTriggerable triggeredObject)
	{
		triggeredObject.gameObject.SetActive(false);
	}
}
