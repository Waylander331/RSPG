/*
 * Author : Olivier Reid
 * 
 * Desc :
 * 		GenericTrigger creates a trigger where you can decide :
 * 			- Which agent(s) affects the trigger
 * 			- Which object(s) are triggered.
 * 			- What action (Activate/Deactivate) to apply for each of these objects, for any trigger scenario (Enter/Stay/Exit).
 * 
 * 		The GenericTrigger can be triggered either when the agent(s) enters, stays, and/or exits it.
 * 		It has one of two modes :
 * 			- Once only : where the trigger is triggered only once.
 * 			- Multiple : where the trigger is triggered every time.
 * 
 * 		The GenericTrigger can also be "One way" (bool) where the side of which the agents enters/exit causes different results. 
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericTrigger : MonoBehaviour 
{
	public TriggerBehaviour behaviour; // Once Only / Multiple decided by the user

	public TriggerDirection direction;
	
	public bool
		front = true,
		back = true;
	
	public bool onlyPlayerAsAgent = true;
	
	public TriggerAgent triggerFor;
	public GameObject[] agents;
	
	public TriggeredObject[] triggeredObjects;
	
	public enum AgentResponse
	{
		single,
		multiple
	}
	
	private Side currentSide;
	
	public enum TriggerBehaviour
	{
		multiple,
		onceOnly
	}
	
	public enum TriggerDirection
	{
		Any,
		Specific
	}
	
	public enum State
	{
		enter,
		stay,
		exit
	}
	
	public enum Side
	{
		front,
		back
	}
	
	public enum TriggerAgent
	{
		firstAgentOnly,
		eachAgent
	}
	
	private ArrayList inTriggerList = new ArrayList ();
	
	private bool active; // For MultiTriggerSystem
	
	private TriggerDelay[] delays;
	
	public bool Active
	{
		get{return active;}
	}
	
	private bool triggerActive = true; // Pour onceOnly
	
	private bool hasExitAction;
	private bool didExit;

	private bool switchTrigger;
	
	void Start()
	{
		Transform parent = transform;
		while (parent.parent != null)
			parent = parent.parent;
		if (parent != null && parent.GetComponent<SwitchSystem>() != null) 
		{
			foreach(TriggeredObject trig in triggeredObjects)
			{
				trig.obj = GetComponent<IsTriggerable>();
				Transform systemEffects = transform;
				while(systemEffects.parent != null)
					systemEffects = systemEffects.parent;
				
				systemEffects = systemEffects.GetChild(0);
				if(GetComponent<Switch>().swBehaviour == Switch.SwitchBehaviour.toggleOnOff)
					trig.effect = systemEffects.GetChild(0).GetComponent<Effect_Default>();
				else
				{
					trig.effect = systemEffects.GetChild(1).GetComponent<Effect_ActivateSwitch>();
				}
			}
		}
		
		if (onlyPlayerAsAgent) 
		{
			agents = new GameObject[1];
			agents[0] = Manager.Instance.Avatar.gameObject;
		}
		if (direction == TriggerDirection.Any) 
		{
			front = true;
			back = true;
			
			foreach(TriggeredObject trig in triggeredObjects)
			{
				trig.actions.back.onEnter = trig.actions.front.onEnter;
				trig.actions.back.onStay = trig.actions.front.onStay;
				trig.actions.back.onExit = trig.actions.front.onExit;
			}
		}
		currentSide = Side.front; // Par défaut
		
		delays = new TriggerDelay[triggeredObjects.Length];
		for(int i = 0; i < delays.Length; i++)
		{
			delays[i] = gameObject.AddComponent<TriggerDelay>();
			delays[i].Trigger = this;
		}
		
		if(behaviour == TriggerBehaviour.onceOnly)
		{
			// check triggeredObjectsStates
			for(int i = 0; i < triggeredObjects.Length && !hasExitAction; i++)
			{
				if(triggeredObjects[i].actions.front.onExit != TriggerAction.Action.nothing && triggeredObjects[i].actions.back.onExit != TriggerAction.Action.nothing)
					hasExitAction = true;
			}
		}

		if (GetComponent<MoveStateInspector> () != null)
			switchTrigger = true;
	}

	void OnLevelWasLoaded(){
		if (Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits" && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit") {
			Invoke ("WaitForManager", 0.005f);
		}

	}

	void WaitForManager(){
		if (Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits" && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit") {
			DontDestroyOnReload temp = GetComponent<DontDestroyOnReload>();
			if (temp == null) temp = GetComponentInParent<DontDestroyOnReload>();
			
			if (temp != null && onlyPlayerAsAgent);
			{
				agents = new GameObject[1];
				agents[0] = Manager.Instance.Avatar.gameObject;
			}
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		if (IsAgent (col))
		{
			currentSide = ProduitScalaire(col, State.enter);
			
			if(triggerActive && CurrentActive())
			{
				if(triggerFor == TriggerAgent.eachAgent || triggerFor == TriggerAgent.firstAgentOnly && inTriggerList.Count == 0)
				{
					for(int i = 0; i < triggeredObjects.Length; i++)
						if(triggeredObjects[i].obj != null && triggeredObjects[i].effect != null)
							UpdateAction(triggeredObjects[i], i, State.enter);
				}
				
				if(behaviour == TriggerBehaviour.onceOnly && !hasExitAction)
				{
					triggerActive = false;
				}
				else if(hasExitAction)
					inTriggerList.Add (col.gameObject);

				if(switchTrigger)
					SoundManager.Instance.PlayAudio("SwitchActivation");
			}
		}
	}
	
	void OnTriggerStay(Collider col)
	{
		if (IsAgent(col) && CurrentActive()) 
		{
			currentSide = ProduitScalaire(col, State.enter);
			
			if(triggerActive && CurrentActive())
			{
				for(int i = 0; i < triggeredObjects.Length; i++)//(TriggeredObject trig in triggeredObjects)
					if(triggeredObjects[i].obj != null && triggeredObjects[i].effect != null)
						UpdateAction(triggeredObjects[i], i, State.stay);
			}
		}
	}
	
	void OnTriggerExit(Collider col)
	{
		if (IsAgent(col)) 
		{
			currentSide = ProduitScalaire(col, State.enter);
			
			inTriggerList.Remove (col.gameObject);
			
			if(triggerActive && CurrentActive())
			{
				if(triggerFor == TriggerAgent.eachAgent || triggerFor == TriggerAgent.firstAgentOnly && inTriggerList.Count == 0)
				{
					didExit = true;
					for(int i = 0; i < triggeredObjects.Length; i++)//(TriggeredObject trig in triggeredObjects)
						if(triggeredObjects[i].obj != null && triggeredObjects[i].effect != null)
							UpdateAction(triggeredObjects[i], i, State.exit);
					
					if (behaviour == TriggerBehaviour.onceOnly)
					{
						//Debug.Log ("Disabling on Exit");
						triggerActive = false;
					}
				}
			}
		}
	}
	
	private void UpdateAction(TriggeredObject trig, int index, State state)
	{
		if(triggerActive)
		{
			TriggerAction.Action action;
			bool noDelay = false;
			switch (state) 
			{
			case State.enter:
				noDelay = !trig.delayOnEnter ? true : false;
				action = currentSide == Side.front ? trig.actions.front.onEnter : trig.actions.back.onEnter;
				break;
			case State.stay:
				action = currentSide == Side.front ? trig.actions.front.onStay : trig.actions.back.onStay;
				break;
			case State.exit:
				noDelay = !trig.delayOnExit ? true : false;
				action = currentSide == Side.front ? trig.actions.front.onExit : trig.actions.back.onExit;
				break;
			default:
				Debug.LogWarning ("State is neither enter, stay, or exit");
				action = trig.actions.front.onEnter;
				break;
			}
			
			if(action != TriggerAction.Action.nothing) 
			{
				float tempDelay = noDelay ? 0f : trig.delay;
				delays [index].InvokeAction (trig, action, tempDelay, state);
			}
		}
	}


	//private void Activate(TriggeredObject trig, int index)
	//{
		//trig.effect.Activate (trig.obj);
		//delays [index].ActionToInvoke = TriggerAction.Action.activate;
		//delays [index].InvokeAction (trig.delay);
	//}
	
	//private void Deactivate(TriggeredObject trig, int index, State state)
	//{
		//trig.effect.Deactivate (trig.obj);
	//}
	
	// Compare the col.gameObject with all the gameObjects in agents[]
	bool IsAgent(Collider col)
	{
		bool isAgent = false;
		for(int i = 0; !isAgent && i < agents.Length; i++)
			if(agents[i] == col.gameObject)
				isAgent = true;		
		return isAgent;
	}
	
	bool CurrentActive()
	{
		return currentSide == Side.front && front || currentSide == Side.back && back;
	}
	
	Side ProduitScalaire(Collider col, State state)
	{	
		bool positive = Vector3.Angle(col.transform.forward, transform.forward) < 90f;
		switch (state) 
		{
		case State.enter:
			return positive ? Side.back : Side.front;
		case State.exit:
			return positive ? Side.front : Side.back;
		default:
			Debug.LogWarning("Erreur de produit scalaire");
			return Side.front;
		}
	}

	void OnDrawGizmosSelected()
	{
		if(triggeredObjects != null)
			foreach(TriggeredObject trig in triggeredObjects)
				if(trig != null && trig.obj != null)
					Gizmos.DrawLine(transform.position, trig.obj.transform.position);
	}
	
	public Side CurrentSide
	{
		get{return currentSide;}
	}
	
	public TriggerDelay[] Delays
	{
		get{return delays;}
	}
	
	public bool HasExitAction
	{
		get{return hasExitAction;}
	}
	
	public bool TriggerActive
	{
		set{triggerActive = value;}
	}
	
	public bool DidExit
	{
		get{return didExit;}
	}
}