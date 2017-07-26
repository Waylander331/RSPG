using UnityEngine;
using System.Collections;

public class WhatAmI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("Kill",5f);
	}
	
	private void Kill(){
		Destroy (this.gameObject);
	}
}
