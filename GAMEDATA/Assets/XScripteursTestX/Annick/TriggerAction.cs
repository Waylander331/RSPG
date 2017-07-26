/*
 * Author : Olivier Reid
 * 
 * Desc : 
 * 		TriggerAction is used with a GenericTrigger.
 * 		It includes an object and his action (activate or deactivate) when an agent enters,
 * 		stays or exit the trigger.
 */

using UnityEngine;
using System.Collections;

[System.Serializable]
public class TriggerAction
{
	//public IsTriggerable obj; // The object that is triggered

	// The action that'll be done for each specific trigger scenario (enter, stay, exit)
	// Can either "Activate" or "Deactivate" the object.
	public Action 
		onEnter,
		onStay,
		onExit;

	public enum Action
	{
		nothing,
		activate,
		deactivate
	}
	// A delegate function that'll be either Activate or Deactivate
	// It is updated when an agent enter, stays, or exit the trigger
	//public delegate void ActionFunction(TriggeredObject obj, int index, GenericTrigger.State state);
	/*
	private ActionFunction function;

	public ActionFunction Function
	{
		get{return function;}
		set{function = value;}
	}*/
}
