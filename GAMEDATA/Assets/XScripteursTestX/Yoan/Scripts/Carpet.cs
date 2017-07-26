using UnityEngine;
using System.Collections;

public class Carpet : MonoBehaviour 
{
	public float minSpeed;
	public float maxSpeed;
	public float carpetSpeed;
	public bool carpetDirection;
	public float speedControl;

	//private CharacterController controller;



	void Update()
	{
		if ( Input.GetKeyDown("r"))
		{
			carpetSpeed = -carpetSpeed;
		}
		
		if(Input.GetKey("q") && carpetSpeed < maxSpeed)
		{
			carpetSpeed += speedControl;
		}
		if(Input.GetKey("e")&& carpetSpeed > minSpeed)
		{
			carpetSpeed -= speedControl;
		}
	}


	void OnTriggerColliderHit(ControllerColliderHit Hit)
	{
		if(Hit.gameObject.tag == ("Player"))
		{
			CharacterController controller = Hit.gameObject.GetComponent<CharacterController>() as CharacterController;
			controller.Move(Vector3.forward * carpetSpeed * Time.deltaTime);
		}
	}
}
