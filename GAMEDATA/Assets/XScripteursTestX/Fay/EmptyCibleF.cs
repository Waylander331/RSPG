﻿using UnityEngine;
using System.Collections;

public class EmptyCibleF : MonoBehaviour {

	//[HideInInspector]
	public GameObject myParent;

	//[HideInInspector]
	public EmptyB emptyB;
	//[HideInInspector]
	public EmptyF emptyF;


	void Start(){
		if(Vector3.Distance (this.transform.position, emptyF.transform.position) >= Vector3.Distance(this.transform.position,emptyB.transform.position)){
			Debug.LogError("Attention, vous devez placer la cibleF de <<" + myParent.gameObject.name + ">> devant la barre et non derriere!");
		}
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.green;
		Gizmos.DrawLine (this.transform.position,myParent.transform.position);
	}

}
