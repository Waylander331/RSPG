using UnityEngine;
using System.Collections;

public class IsTriggerable : MonoBehaviour 
{

	public void Activate(EffectList effect)
	{
		SendMessage("Triggered", effect);
	}

	public void Deactivate(EffectList effect)
	{
		SendMessage ("UnTriggered", effect);
	}
}
