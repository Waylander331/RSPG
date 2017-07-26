using UnityEngine;
using System.Collections;

public class HypeEffect : MonoBehaviour {

	private ParticleSystem myParticles;

	// Use this for initialization
	void Start () {
		myParticles = GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Manager.Instance.Avatar.InElevator){
			myParticles.enableEmission = false;
		}
		else{
			if(Manager.Instance.IsHyped){
				myParticles.enableEmission = true;
			}
			else{
				myParticles.enableEmission = false;
			}
		}
	}
}
