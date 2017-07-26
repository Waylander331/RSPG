using UnityEngine;
using System.Collections;

public class FootR : MonoBehaviour {

	private bool oneTime;
	private RaycastHit footHit;
	private Avatar myParent;
	
	void Awake(){
		myParent = GetComponentInParent<Avatar>();
	}
	
	void Update () {
		if(myParent.IsGrounded && Physics.Raycast(this.transform.position,-this.transform.right,out footHit,0.2f) && footHit.transform.gameObject.GetComponentInParent<Avatar>() == null){
			myParent.HasRightFoot = true;
			if(oneTime){
				if(Manager.Instance.InBackstage){
					myParent.AvatarSFX.TriggerEffect_HeavyFootstepDust(false);
					myParent.AvatarSFX.Effect_SpawnFootprint_Right();
				}
				else{
					myParent.AvatarSFX.TriggerEffect_FootstepDust(false);
				}
				SoundManager.Instance.AvatarFootstepsRandom();
				oneTime = false;
			}
		}
		else{
			oneTime = true;
			myParent.HasRightFoot = false;
		}
	}
}