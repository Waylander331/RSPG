using UnityEngine;
using System.Collections;

public class CinFoot : MonoBehaviour { 

	private bool oneTime;
	//private RaycastHit footHit; 

	void Update(){

		if(Physics.Raycast(this.transform.position, -this.transform.right,/* out footHit,*/0.3f)){

			if(oneTime){
				SoundManager.Instance.AvatarFootstepsRandom(); 
				oneTime = false; 
			}
		}
		else oneTime = true; 
	}

}
