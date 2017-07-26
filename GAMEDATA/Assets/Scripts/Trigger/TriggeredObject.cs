using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggeredObject
{
	public float delay;
	public bool delayOnEnter;
	public bool delayOnExit;
	public IsTriggerable obj;
	public EffectList effect;

	public ActionPerSide actions;
}
