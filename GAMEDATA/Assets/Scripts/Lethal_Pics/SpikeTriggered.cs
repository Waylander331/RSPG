using UnityEngine;
using System.Collections;
using UnityEngine;
using System.Collections;

public class SpikeTriggered : MonoBehaviour
{
	public bool active = true;
	private bool wasActive = true;

	public float
		launchSpeed,
		launchDelay,
		returnSpeed,
		returnDelay;

	private bool 
		launching = false,
		returning = false;

	private Spikes spikes;
	private SpikesTrigger detector;
	private BoxCollider lethal;

	void Start () 
	{
		spikes = transform.GetChild (0).GetComponent<Spikes> ();
		detector = transform.GetChild (1).GetComponent<SpikesTrigger> ();
		lethal = GetComponent<BoxCollider> ();
		SetLethal (false);
	}

	void Update () 
	{
		if(active != wasActive)
		{
			CancelInvoke("TriggerLaunch");
			CancelInvoke ("TriggerReturn");
			wasActive = active;
		}

		if(active)
		{
			if(launching)
			{
				if(spikes.Launch (launchSpeed) && !IsInvoking("TriggerReturn"))
				{
					//Debug.Log ("Invoking Return in " + returnSpeed);
					Invoke ("TriggerReturn", returnDelay);
					launching = false;
				}
			}
			else if(returning)
			{					
				if(spikes.Return (returnSpeed))
				{
					returning = false;
					SetLethal(false);
				}
			}

			if(detector.Detecting && !launching && !returning && !IsInvoking ("TriggerLaunch") && !IsInvoking ("TriggerReturn"))
			{
				Invoke ("TriggerLaunch", launchDelay);
				//Debug.Log ("Invoking Launch");
			}
		}
	}

	void TriggerLaunch()
	{
		SoundManager.Instance.PlayAudioInMyself (gameObject, "lethal_lethalspikein");
		launching = true;
		SetLethal (true);
	}

	void TriggerReturn()
	{
		SoundManager.Instance.PlayAudioInMyself (gameObject, "lethal_lethalspikeout");
		launching = false;
		returning = true;
	}

	void SetLethal(bool value)
	{
		lethal.enabled = value;
	}
}
