using UnityEngine;
using System.Collections;

public class BackstageTrigger : MonoBehaviour 
{
	private GenericTrigger trigger;

	void Awake()
	{
		trigger = GetComponent<GenericTrigger> ();
		trigger.behaviour = GenericTrigger.TriggerBehaviour.multiple;
		trigger.direction = GenericTrigger.TriggerDirection.Specific;
		trigger.front = true;
		trigger.back = true;
		trigger.onlyPlayerAsAgent = true;
		trigger.triggeredObjects = new TriggeredObject[1];
		trigger.triggeredObjects [0] = new TriggeredObject ();
		trigger.triggeredObjects [0].obj = new IsTriggerable ();
		trigger.triggeredObjects [0].obj = transform.GetChild (0).GetComponent<IsTriggerable> ();
		trigger.triggeredObjects [0].effect = new Effect_ChangeInBackstageState ();
		trigger.triggeredObjects [0].effect = transform.GetChild (1).GetComponent<Effect_ChangeInBackstageState> ();
		trigger.triggeredObjects [0].actions = new ActionPerSide ();
		trigger.triggeredObjects [0].actions.front = new TriggerAction ();
		trigger.triggeredObjects [0].actions.front.onExit = new TriggerAction.Action ();
		trigger.triggeredObjects [0].actions.front.onExit = TriggerAction.Action.activate;
		trigger.triggeredObjects [0].actions.back = new TriggerAction ();
		trigger.triggeredObjects [0].actions.back.onExit = TriggerAction.Action.deactivate;
	}
}
