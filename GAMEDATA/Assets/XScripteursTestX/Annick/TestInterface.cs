using UnityEngine;
using System.Collections;

public class TestInterface : MonoBehaviour, ITriggerable 
{
	/// <summary>
	/// Activates the object.
	/// </summary>
	public void Triggered(EffectList effect)
	{
		/*
		switch (effect.GetType()) 
		{
		case typeof(AnEffect):
			Debug.Log ("Does an effect on " + name);
			break;
		case typeof(Effect_Enable):
			gameObject.SetActive(true);
			break;
		}*/
		Debug.Log (gameObject.name + " is Triggered!");
		if(effect.GetType() == typeof(AnEffect))
		{
			//Debug.Log ("Does an effect on " + name);
		}
		else if(effect.GetType() == typeof(Effect_Enable))
		{
			gameObject.SetActive(true);
		}
	}

	/// <summary>
	/// Deactivates the object.
	/// </summary>
	public void UnTriggered(EffectList effect)
	{
		Debug.Log (gameObject.name + " is UnTriggered!");
		if(effect.GetType() == typeof(AnEffect))
		{
			//Debug.Log ("Stop doing an effect on " + name);
		}
		else if(effect.GetType() == typeof(Effect_Enable))
		{
			gameObject.SetActive(false);
		}
	}
}
