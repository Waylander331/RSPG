using UnityEngine;
using System.Collections;

public class StayHyped : MonoBehaviour {

	private bool trigged;

	void Update(){
		if (trigged && !Manager.Instance.IsHyped && !Manager.Instance.IsSlow)
			Manager.Instance.IsHyped = true;
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player")
			trigged = true;
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Player")
			trigged = false;
	}

}
