using UnityEngine;
using System.Collections;

public class Foot : MonoBehaviour {

	private bool oneTime;
	private RaycastHit footHit;
	private Avatar myParent;

	void Awake(){
		myParent = GetComponentInParent<Avatar>();
	}

	void Update () {
	if(myParent.IsGrounded && Physics.Raycast(this.transform.position,-this.transform.right,out footHit,0.2f) && footHit.transform.gameObject.GetComponentInParent<Avatar>() == null){
			if(oneTime){
				if(Random.Range(0,2) == 1){
					SoundManager.Instance.PlayAudio("FootstepL");
				}
				else{
					SoundManager.Instance.PlayAudio("FootstepR");
				}
				oneTime = false;
			}
		}
		else{
			oneTime = true;
		}
	}
}
