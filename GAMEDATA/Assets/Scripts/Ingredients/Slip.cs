using UnityEngine;
using System.Collections;

public class Slip : MonoBehaviour 
{

	private Platform platform;
	private CharacterController controler;

	private Vector3 moveDirection;

	private float slipForce = 100;

	void Start()
	{
		platform = GetComponentInParent<Platform>() as Platform;
	}
	void OnLevelWasLoaded()
	{
		Invoke ("WaitForManager", 0.001f);
	}
	void WaitForManager()
	{
		platform = GetComponentInParent<Platform>() as Platform;
	}

	void OnTriggerStay(Collider other)
	{
	 	if(other.tag == ("Player"))
		{
			controler = other.GetComponent<CharacterController>();
			moveDirection = new Vector3(Input.GetAxis("Horizontal"),-slipForce, Input.GetAxis("Vertical"));

			if(platform.RotateEffectRotating)
			{
				controler.Move(moveDirection * Time.deltaTime);
			}
		}
	}



}
