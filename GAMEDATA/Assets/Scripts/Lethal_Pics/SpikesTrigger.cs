using UnityEngine;
using System.Collections;

public class SpikesTrigger : MonoBehaviour 
{
	bool detecting;

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
			detecting = true;
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
			detecting = false;
	}

	public bool Detecting
	{
		get{return detecting;}
	}
}
