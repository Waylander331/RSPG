using UnityEngine;
using System.Collections;

public class DisableCollider : MonoBehaviour 
{
	public void InvokeDisable(float delay)
	{
		Invoke ("Disable", delay);
	}
	
	void Disable()
	{
		GetComponent<SphereCollider>().enabled = false;
	}
}
