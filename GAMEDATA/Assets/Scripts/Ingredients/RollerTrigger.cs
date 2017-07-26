using UnityEngine;
using System.Collections;

public class RollerTrigger : MonoBehaviour {

	private CharacterController controller;	
	private bool isFinding;
	private Roller papaRoller;
	private float jumpTemp;
	private float sprintJumpTemp;

	private bool haveToFall = false;

	void Start(){
		papaRoller = transform.GetComponentInParent<Roller>();
		isFinding = true;
		jumpTemp = Manager.Instance.Avatar.distanceJump;
		sprintJumpTemp = Manager.Instance.Avatar.sprintDistanceJump;
	}

	void Update (){
		if(isFinding){
			Invoke ("FindController",0.1f);
		}
	}
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){

			if(papaRoller.IsReversed){
				papaRoller.Sens = -papaRoller.Sens;
			}
			//haveToFall = false;
			if(!papaRoller.IsFloor){
				if(Manager.Instance.Avatar.OnWall){
					if(Manager.Instance.Avatar.transform.forward == transform.up){
						papaRoller.Sens = - transform.forward;
					}
					else if(Manager.Instance.Avatar.transform.forward == -transform.up){
						papaRoller.Sens = transform.forward;
					}
					controller.Move(papaRoller.Sens * papaRoller.CarpetSpeed * Time.deltaTime);
				}
				if(Manager.Instance.Avatar.IsFromWall){
					Manager.Instance.Avatar.IsFromWall = false;
				}
			}
		}
	}
	
	void OnTriggerStay(Collider other){
		if(other.tag == "Player"){
			if(!papaRoller.IsFloor){
				if(Manager.Instance.Avatar.OnWall){
					haveToFall = true;
					if(Manager.Instance.Avatar.transform.forward == transform.up){
						papaRoller.Sens = - transform.forward;
					}
					else if(Manager.Instance.Avatar.transform.forward == -transform.up){
						papaRoller.Sens = transform.forward;
					}
					controller.Move(papaRoller.Sens * papaRoller.CarpetSpeed * Time.deltaTime);
					if(Input.GetButton("Fire2")){
						Manager.Instance.Avatar.transform.forward = -Manager.Instance.Avatar.transform.forward;
						Manager.Instance.Avatar.ImpulSaut = new Vector3(0,Manager.Instance.Avatar.ImpulSaut.y,0);
						Manager.Instance.Avatar.stickForce = 0f;
						Manager.Instance.Avatar.fallDec = 0f;
						Manager.Instance.Avatar.OnWall = false;
						Manager.Instance.Avatar.OnSticky = false;
						Manager.Instance.Avatar.HasJumped = true;
						haveToFall = false;
					}
				}
				if (!Manager.Instance.Avatar.OnWall && !Manager.Instance.Avatar.IsFromWall && !Manager.Instance.Avatar.IsGrounded && Manager.Instance.Avatar.InAir && haveToFall){
					Manager.Instance.Avatar.transform.forward = -Manager.Instance.Avatar.transform.forward;
					Manager.Instance.Avatar.ImpulSaut = new Vector3(0,Manager.Instance.Avatar.ImpulSaut.y,0);
					Manager.Instance.Avatar.stickForce = 0f;
					Manager.Instance.Avatar.fallDec = 0f;
					Manager.Instance.Avatar.OnWall = false;
					Manager.Instance.Avatar.OnSticky = false;
					Manager.Instance.Avatar.HasJumped = true;
					haveToFall = false;
				}
				if(Manager.Instance.Avatar.IsGrounded){
					haveToFall = false;
				}
			} 
			else{ 
				papaRoller.Sens = transform.forward;

				if(papaRoller.IsReversed){
					papaRoller.Sens = -papaRoller.Sens;
				}
				if(!papaRoller.IsReversed && Vector3.Dot (Manager.Instance.Avatar.transform.forward, transform.forward) > 0.4f){
					Manager.Instance.Avatar.distanceJump = jumpTemp + (float)papaRoller.CarpetSpeed/2;
					Manager.Instance.Avatar.sprintDistanceJump = sprintJumpTemp + (float)papaRoller.CarpetSpeed/2;
				}
				if(!papaRoller.IsReversed && Vector3.Dot(Manager.Instance.Avatar.transform.forward, transform.forward) < -0.4f){
					Manager.Instance.Avatar.distanceJump = jumpTemp - (float)papaRoller.CarpetSpeed/2;
					Manager.Instance.Avatar.sprintDistanceJump = sprintJumpTemp - (float)papaRoller.CarpetSpeed/2;
				}
				if(papaRoller.IsReversed && Vector3.Dot (Manager.Instance.Avatar.transform.forward, transform.forward) > 0.4f){
					Manager.Instance.Avatar.distanceJump = jumpTemp - (float)papaRoller.CarpetSpeed/2;
					Manager.Instance.Avatar.sprintDistanceJump = sprintJumpTemp - (float)papaRoller.CarpetSpeed/2;
				}
				if(papaRoller.IsReversed && Vector3.Dot (Manager.Instance.Avatar.transform.forward, transform.forward) < -0.4f){
					Manager.Instance.Avatar.distanceJump = jumpTemp + (float)papaRoller.CarpetSpeed/2;
					Manager.Instance.Avatar.sprintDistanceJump = sprintJumpTemp + (float)papaRoller.CarpetSpeed/2;
				}

				if(Vector3.Dot (Manager.Instance.Avatar.transform.forward, transform.forward) < 0.4f && Vector3.Dot (Manager.Instance.Avatar.transform.forward, transform.forward) > -0.4f){
					Manager.Instance.Avatar.distanceJump = jumpTemp + (float)papaRoller.CarpetSpeed/4;
					Manager.Instance.Avatar.sprintDistanceJump = sprintJumpTemp + (float)papaRoller.CarpetSpeed/4;
				}

				controller.Move(papaRoller.Sens * papaRoller.CarpetSpeed * Time.deltaTime);
			}
		}
	}
	
	void OnTriggerExit(Collider other){
		if(other.tag == "Player"){
			Manager.Instance.Avatar.distanceJump = Manager.Instance.Avatar.RealDistanceJump;
			Manager.Instance.Avatar.sprintDistanceJump = Manager.Instance.Avatar.RealSprintDistanceJump;
			//Manager.Instance.Avatar.IsFromWall = false;
			haveToFall = false;
		}
	}

	void FindController(){
		isFinding = false;
		controller = Manager.Instance.Avatar.gameObject.GetComponent<CharacterController>();
	}

}
