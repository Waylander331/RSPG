using UnityEngine;
using System.Collections;

public class TriggerDelay : MonoBehaviour 
{
	private TriggeredObject trig;
	private TriggerAction.Action action;
	private GenericTrigger.State state;

	private GenericTrigger trigger;

	public void InvokeAction(TriggeredObject trig, TriggerAction.Action action, float delay, GenericTrigger.State state)
	{
		this.trig = trig;
		this.action = action;
		this.state = state;

		if(!IsInvoking("StartAction"))
			Invoke ("StartAction", delay);
	}

	void StartAction()
	{
		if (action == TriggerAction.Action.activate)
			trig.effect.Activate (trig.obj);
		else if(action == TriggerAction.Action.deactivate)
			trig.effect.Deactivate (trig.obj);
		
		if(trigger.behaviour == GenericTrigger.TriggerBehaviour.onceOnly && ((state == GenericTrigger.State.enter && (!trigger.HasExitAction || trigger.HasExitAction && trigger.DidExit)) || (state == GenericTrigger.State.exit)))
		{
			bool queuedJobs = false;
			for(int i = 0; i < trigger.Delays.Length && !queuedJobs; i++)
			{
				if(trigger.Delays[i].IsInvoking("InvokeAction"))
					queuedJobs = true;
			}
			if(!queuedJobs)
				trigger.TriggerActive = false;
		}
	}

	public GenericTrigger Trigger
	{
		set{trigger = value;}
	}
}
