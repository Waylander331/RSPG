using UnityEngine;
using System.Collections;

public class ElevatorTrigger : MonoBehaviour 
{
	private bool isInside;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
			isInside = true;
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
			isInside = false;
	}

	public bool IsInside
	{
		get{return isInside;}
		set{isInside = value;}
	}
}
