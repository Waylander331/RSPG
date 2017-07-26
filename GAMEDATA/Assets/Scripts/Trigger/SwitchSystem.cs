using UnityEngine;
using System.Collections;

public class SwitchSystem : MonoBehaviour
{
	public bool onlyOnce;

	private Switch[] switches;
	public SimpleTriggeredObject[] triggeredObjects;

	private bool wasActive = false;

	void Start()
	{
		switches = new Switch[transform.childCount - 1];
		int j = 0;
		for(int i = 1; i < transform.childCount; i++)
		{
			Transform child = transform.GetChild (i);
			bool isASwitch = child.transform.childCount != 0 && child.GetComponent<Switch>() == null;
			if(isASwitch)
				child = child.GetChild(0);
			if(child != null)
			{
				Switch s = child.GetComponent<Switch>();
				if(s != null)
				{
					switches[j] = s;
					j++;
				}
				else
				{
					Debug.LogWarning ("A child in SwitchSystem isn's a trigger.");
				}
			}
		}
	}

	void Update()
	{
		bool allActive = true;
		for (int i = 0; allActive && i < switches.Length; i++) 
		{
			if(switches[i] != null)
			{
				if(!switches[i].Active) allActive = false;
			}
		}

		if (allActive && !wasActive) 
		{
			Trigger();
			wasActive = true;
		}
		else if (!allActive && wasActive)
		{
			UnTrigger();
			wasActive = false;
		}

	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;

		for(int i = 0; i < transform.childCount; i++)
		{
			Transform s = transform.GetChild (i);
			bool isASwitch = s.transform.childCount != 0 && s.GetComponent<Switch>() == null;
			if(isASwitch)
				s = s.GetChild(0);
			if(s != null)
			{
				if(s.GetComponent<Switch>() != null)
					Gizmos.DrawLine (transform.position, s.transform.position);
				else
				{
					Debug.LogWarning ("A child in SwitchSystem isn's a trigger.");
				}
			}
		}
		Gizmos.color = Color.blue;
		foreach (SimpleTriggeredObject trig in triggeredObjects) 
		{
			if(trig.obj != null)
				Gizmos.DrawLine (transform.position, trig.obj.transform.position);
		}
	}

	// When all switches are active
	void Trigger()
	{
		foreach (SimpleTriggeredObject trig in triggeredObjects) 
		{
			if(trig.obj != null)
			{
				if(trig.action == TriggerAction.Action.activate)
				{
					trig.obj.Activate(trig.effect);
				}
				else if(trig.action == TriggerAction.Action.deactivate)
				{
					trig.obj.Deactivate(trig.effect);
				}
			}
		}
		if(onlyOnce) enabled = false;
	}

	// When at least one switch is unactive
	void UnTrigger()
	{
		foreach (SimpleTriggeredObject trig in triggeredObjects) 
		{
			if(trig.obj != null)
			{
				if(trig.action == TriggerAction.Action.activate)
				{
					trig.obj.Deactivate(trig.effect);
				}
				else if (trig.action == TriggerAction.Action.deactivate)
				{
					trig.obj.Activate(trig.effect);
				}
			}
		}
	}
}
