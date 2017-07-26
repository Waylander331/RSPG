using UnityEngine;
using System.Collections;

public class NoSlip : MonoBehaviour {
	
	private CharacterController controller;
	private Platform platform;
	private Avatar player;
	private Poney poney;
	//private Transform playerT;

	private LayerMask collision;

	//private bool hideNseek = true;
	private bool onMovable = false;
	

	//private CollisionFlags collisionFlags;
	private Vector3 moveDirection = Vector3.zero;
	private Transform activePlatform;
	private Vector3 activeLocalPlatformPoint;
	private Vector3 activeGlobalPlatformPoint;
	private Vector3 newGlobalPlatformPoint;
	//private Vector3 lastPlatformVelocity;
	private Vector3 calculatedMovement;
	private Quaternion activeLocalPlatformRotation;
	private Quaternion activeGlobalPlatformRotation;

	private RaycastHit hit;
	//private Ray capsuleRay;

	void Start ()
	{
		collision = 1 << 9;
		player = Manager.Instance.Avatar;
		//moveDirection = transform.TransformDirection (transform.forward);
		controller = GetComponent<CharacterController>();
	}

	void OnLevelWasLoaded()
	{
		//controller = GetComponent<CharacterController>();
	}

	void FixedUpdate()
	{
		if(player == null)
		{
			player = Manager.Instance.Avatar;
		}
		if(controller == null)
		{
			controller = GetComponent<CharacterController>();
		}
		SurfaceCheck();
	}

	void Update()
	{
		CalculatePositionRotation();
	}

	void LateUpdate()
	{
		ApplyDisplacement();
	}
	



	/// <summary>
	/// Surfaces the check.
	/// </summary>
	public void SurfaceCheck()
	{
		Vector3 startP = transform.position + controller.center;
		//Vector3 endP = startP + Vector3.up * controller.height;
		Vector3 endP = startP * controller.height;
		//collision = 1 << 9;
		
		// Check if we are on a platform via a CapsuleCast. 
		//if(Physics.CapsuleCast(startP,endP,controller.radius,Vector3.down,out hit, 0.80f, collision))
		if(Physics.Raycast(transform.position, Vector3.down, out hit, 0.15f, collision))
		{
			activePlatform = hit.collider.transform;

			if(hit.transform.tag == ("Platform"))
			{
				platform = hit.transform.GetComponent<Platform>();
				onMovable = true;
			}
			else
			{
				onMovable = false;
			}
			/*if(hit.transform == null)
			{
				activePlatform = null;
				lastPlatformVelocity = Vector3.zero;
			}*/
		}
	}

	/// <summary>
	/// Calculates the position rotation.
	/// </summary>
	public void CalculatePositionRotation()
	{
		if (!onMovable)
		{ 
			activePlatform = null;
			//lastPlatformVelocity = Vector3.zero;
		} 
		if (onMovable)
		{
			//if (player.IsGrounded)
			{
				
				//moveDirection = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
				//moveDirection = player.transform.TransformDirection(moveDirection);
				//collisionFlags = controller.Move(moveDirection * Time.deltaTime); 
				//player.IsGrounded = (collisionFlags & CollisionFlags.CollidedBelow) != 0;
			
			
			//calculatedMovement = moveDirection * Time.deltaTime;
		 	//controller.Move(calculatedMovement);
			
			if (activePlatform != null)
				{
					activeGlobalPlatformPoint = transform.position;
					activeLocalPlatformPoint = activePlatform.InverseTransformPoint (transform.position);
					if( platform != null)
					{
						if(platform.Unrotated /*|| poney.Unrotated*/)
						{
							activeGlobalPlatformRotation = transform.rotation;
							activeLocalPlatformRotation = Quaternion.Inverse(activePlatform.rotation) * transform.rotation;
						}
					}
				}
			}
		}
	}
	
	/// <summary>
	/// Applies the displacement.
	/// </summary>
	public void ApplyDisplacement()
	{
		if(player.IsGrounded)
		{
			if (activePlatform != null)
			{
				newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
				calculatedMovement = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
				transform.position = (transform.position + calculatedMovement);
				//lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;

				if(platform != null)
				{
					if(platform.Unrotated)
					{
						Quaternion newGlobalPlatformRotation = activePlatform.rotation * activeLocalPlatformRotation;
						Quaternion rotationDiff = newGlobalPlatformRotation * Quaternion.Inverse(activeGlobalPlatformRotation);
						transform.rotation = rotationDiff * transform.rotation;
					}
				}
			}
		//	else
			{
				//lastPlatformVelocity = Vector3.zero;
			}
		}
	}


}






