using UnityEngine;
using System.Collections;

public class BMTomatoSnap : MonoBehaviour {

	private Animator anim;

	private MeshRenderer tomato;

	// Use this for initialization
	void Start () {
		tomato = transform.GetChild(0).GetChild (0).GetComponent<MeshRenderer>();
		tomato.enabled = false;
		anim = transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("bonus")){
			tomato.enabled = true;
		} else tomato.enabled = false;
	}
}
