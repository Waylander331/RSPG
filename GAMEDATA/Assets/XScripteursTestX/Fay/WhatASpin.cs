using UnityEngine;
using System.Collections;

public class WhatASpin : MonoBehaviour {

	public float vRotation;
	public GameObject one;
	public GameObject two;
	public GameObject three;
	public GameObject four;

	private MeshRenderer unos;
	private MeshRenderer dos;
	private MeshRenderer thres;
	private MeshRenderer quatro;

	void Start(){
		unos = one.GetComponent<MeshRenderer>();
		dos = two.GetComponent<MeshRenderer>();
		thres = three.GetComponent<MeshRenderer>();
		quatro = four.GetComponent<MeshRenderer>();

		unos.enabled = true;
		dos.enabled = true;
		thres.enabled = true;
		quatro.enabled = true;

		unos.material.color = Color.red;
		dos.material.color = Color.red;
		thres.material.color = Color.red;
		quatro.material.color = Color.red;
	}

	// Update is called once per frame
	void Update () {
		float rotation = vRotation * Time.deltaTime;
		this.transform.Rotate(rotation,0, 0,Space.Self);
	}
}
