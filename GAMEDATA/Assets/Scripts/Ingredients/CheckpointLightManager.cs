using UnityEngine;
using System.Collections;

public class CheckpointLightManager : MonoBehaviour 
{
	public Material bulbLightOff;
	public Material bulbLightOn;

	public Material ringLightOff;
	public Material ringLightOn;

	private MeshRenderer bulbs;
	private MeshRenderer ring;

	private bool onTexture;
	private const float shiftDelay = 0.8f;

	void Awake()
	{
		if(transform.parent.childCount > 2)
		{

			bulbs = transform.parent.GetChild (2).GetComponent<MeshRenderer> ();
			ring = transform.parent.GetChild (0).GetChild (2).GetComponent<MeshRenderer>();


			if(bulbs != null) 
				bulbs.material = bulbLightOff;
			if(ring != null)
				ring.material = ringLightOff;
		}
		enabled = false;
	}

	void Update()
	{
		if(bulbs != null)
		{
			if(!onTexture && !IsInvoking ("OnTexture"))
			{
				Invoke ("OnTexture", shiftDelay);
			}
			else if (onTexture && !IsInvoking ("OffTexture"))
			{
				Invoke ("OffTexture", shiftDelay);
			}
		}
	}

	void OnTexture()
	{
		bulbs.material = bulbLightOn;
		onTexture = true;
	}

	void OffTexture()
	{
		bulbs.material = bulbLightOff;
		onTexture = false;
	}

	public void TurnOn()
	{
		if(bulbs != null)
			bulbs.material = bulbLightOn;
		if (ring != null)
			ring.material = ringLightOn;
		//if(mFilter != null)
			//mFilter.sharedMesh = cylinder;
		onTexture = true;
		enabled = true;
		CancelInvokes ();
	}

	public void TurnOff()
	{
		if(bulbs != null)
			bulbs.material = bulbLightOff;
		if (ring != null)
			ring.material = ringLightOff;
		//if(mFilter != null)
			//mFilter.sharedMesh = quad;
		onTexture = false;
		enabled = false;
		CancelInvokes ();
	}

	void CancelInvokes()
	{
		if (IsInvoking ("OnTexture"))
			CancelInvoke ("OnTexture");
		if (IsInvoking ("OffTexture"))
			CancelInvoke ("OffTexture");
	}
}
