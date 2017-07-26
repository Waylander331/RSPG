using UnityEngine;
using System.Collections;

public class FootL : MonoBehaviour {

	private bool oneTime;
	private RaycastHit footHit;
	private Avatar myParent;
	
	void Awake(){
		myParent = GetComponentInParent<Avatar>();
	}
	
	void Update () {
		if(myParent.IsGrounded && Physics.Raycast(this.transform.position,-this.transform.right,out footHit,0.2f) && footHit.transform.gameObject.GetComponentInParent<Avatar>() == null){
			myParent.HasLeftFoot = true;
			if(oneTime){
				if(Manager.Instance.InBackstage){
					myParent.AvatarSFX.TriggerEffect_HeavyFootstepDust(true);
					myParent.AvatarSFX.Effect_SpawnFootprint_Left();
				}
				else{
					myParent.AvatarSFX.TriggerEffect_FootstepDust(true);
				}
				myParent.AvatarSFX.TriggerEffect_FootstepDust(true);
				SoundManager.Instance.AvatarFootstepsRandom();
				oneTime = false;
			}
		}
		else{
			oneTime = true;
			myParent.HasLeftFoot = false;
		}
	}
}
