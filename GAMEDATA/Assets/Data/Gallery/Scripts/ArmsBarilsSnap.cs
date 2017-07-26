using UnityEngine;
using System.Collections;

public class ArmsBarilsSnap : MonoBehaviour {

	private Animator anim;

	private MeshRenderer baril1;
	private MeshRenderer baril2;
	private MeshRenderer baril3;

	// Use this for initialization
	void Start () {
		baril1 = transform.GetChild(0).GetChild (0).GetComponent<MeshRenderer>();
		baril1.enabled = false;
		baril2 = transform.GetChild(1).GetChild (0).GetComponent<MeshRenderer>();
		baril2.enabled = false;
		baril3 = transform.GetChild(2).GetChild (0).GetComponent<MeshRenderer>();
		baril3.enabled = false;
		anim = transform.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(anim.GetCurrentAnimatorStateInfo(0).IsName("bonus")){
			baril1.enabled = true;
			baril2.enabled = true;
			baril3.enabled = true;
		}
		else{
			baril1.enabled = false;
			baril2.enabled = false;
			baril3.enabled = false;
		}
	}
}
