﻿using UnityEngine;
using System.Collections;

public class Effect_TurnOnTv : EffectList 
{
	public override void Activate (IsTriggerable triggeredObject)
	{
		triggeredObject.Activate (this);
	}

	public override void Deactivate (IsTriggerable triggeredObject)
	{
		triggeredObject.Deactivate (this);
	}
}
