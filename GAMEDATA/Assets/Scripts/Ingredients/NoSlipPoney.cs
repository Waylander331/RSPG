using UnityEngine;
using System.Collections;

public class NoSlipPoney : MonoBehaviour {

	private Poney poney;
	private Avatar player;
	private LayerMask collWall;
	private bool canUnparent = false;
	private bool asHitWall = false;

	void Start(){
		collWall += 1 << 11;
		poney = GetComponentInParent<Poney>() as Poney;
		player = Manager.Instance.Avatar;
	}

	void Update(){
		if(asHitWall && canUnparent){
			player.transform.parent = null;
		}
	}

	void OnTriggerEnter(Collider other){
		if(!asHitWall){
			if (other.tag == ("Player")){
				player.transform.parent = this.transform;
				canUnparent = true;
			}
		}
		if((1 << other.gameObject.layer) == collWall && player.transform.parent != null){
			asHitWall = true;
		}

	}

	void OnTriggerStay(Collider other){
		if (other.tag == ("Player")){
			other.transform.parent = this.transform;
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag == ("Player")){
			player.transform.parent = null;
			canUnparent = false;
		}
		if((collWall << other.gameObject.layer)!= 0){
			asHitWall = false;
		}
	}
}
