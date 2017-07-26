using UnityEngine;
using System.Collections;

public class Birdies : MonoBehaviour {

	private Avatar myParent;
	private BirdsParticles birdHolder;
	private bool oneShot;

	void Start(){
		birdHolder = GetComponentInChildren<BirdsParticles>();
		oneShot = false;
	}
	void Update () {
	
		if(myParent.IsDazed){
			if(oneShot){
				birdHolder.SpawnBirds();
				birdHolder.Spawned = true;
				oneShot = false;
			}
		}
		else{
			birdHolder.DestroyBirds();
			birdHolder.Spawned = false;
			oneShot = true;
		}
	}
	
	public Avatar MyParent {
		get {
			return myParent;
		}
		set {
			myParent = value;
		}
	}
}
