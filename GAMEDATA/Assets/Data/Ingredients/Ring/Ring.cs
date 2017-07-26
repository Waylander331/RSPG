using UnityEngine;
using System.Collections;

public class Ring : MonoBehaviour {

	private Color myColor;
	private ParticleSystem mySparkles;
	private float rotation = 100f;

	// Use this for initialization

	private void Start(){
		mySparkles = GetComponentInChildren<ParticleSystem>();
		mySparkles.startColor = myColor;
	}

	void Update(){
		transform.Rotate(0, rotation * Time.deltaTime, 0, Space.World);
	}

	private void OnTriggerEnter(Collider other){
		if(other.tag =="Player"){
			SoundManager.Instance.PlayAudio("RingTake");
			transform.parent.GetComponent<RingMaster>().AddOneRing();
			this.gameObject.SetActive(false);
				}
			}

	public Color MyColor {
		get {
			return myColor;
		}
		set {
			myColor = value;
		}
	}
}

