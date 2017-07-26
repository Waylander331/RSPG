using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour , ITriggerable
{
	public SwitchBehaviour swBehaviour = SwitchBehaviour.activateOnly;
	private bool active;

	public enum SwitchBehaviour
	{
		activateOnly,
		toggleOnOff
	}

	public bool Active 
	{
		get{return active;}
	}

	public void Triggered(EffectList effect)
	{
		if (effect.GetType () == typeof(Effect_Default)) 
		{
			active = !active;
		}
		if(effect.GetType() == typeof(Effect_ActivateSwitch))
		{
			active = true;
		}
	}

	public void UnTriggered(EffectList effect)
	{
		if (effect.GetType () == typeof(Effect_Default)) 
		{
			active = !active;
		}
		if (effect.GetType () == typeof(Effect_ActivateSwitch)) 
		{
			active = false;
		}
	}
}
