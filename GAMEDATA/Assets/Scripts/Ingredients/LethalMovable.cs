using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(CharacterController))]
public class LethalMovable : MonoBehaviour,IMovable
{
	// FOR MOVABLE
	// ====================
	private float waitTime;
	private int index;
	public bool isTrigger = false;
	public MovableRotation trap;
	private bool onHalfRot = false;
	private bool onFullRot = false;
	public float totalRotation = 0;
	public float totalUnrotate;
	//private Quaternion halfRotation = new Quaternion(0,0,0.5f,0.5f);
	//private Vector3 halfRotation = new Vector3(0,0,90);
	private Quaternion fullRotation =  new Quaternion(0,0,1,0);
	private Quaternion resetRot = new Quaternion (0,0,0,1);
	private float timerToRot;
	private bool rotated;
	private int myMovement = 0;
	private bool canStart = false;
	delegate void DelRot(MovableRotation trap);
	delegate void Delfunc();
	delegate IEnumerator DelEnum();
	Delfunc delFunc;
	DelEnum delEnum;
	DelRot delRot;
	Movable movable;
	private float range = 0.2f;
	private float speed;
	public float maxRotationSpeed;
	private Vector3 moveDirection;
	private float velocity;
	private float minTime = 0.1f;
	public float rotationSpeed;
	public enum Speed
	{
		VerySlow, Slow, Medium, Fast, VeryFast
	};
	
	public Speed platformSpeed;
	// ====================
	
	void Start()
	{
		//controller = GetComponent<CharacterController>();
		movable = GetComponent<Movable>();
		if(movable != null)
		{
			if(movable.Group != null)
			{
				movable.Group.transform.parent = transform.parent;
				if(movable.WpsList.Count > 1)
				{
					if(movable.WpStart != null){
						movable.Courant = movable.WpStart.Id;
						movable.Next = movable.WpStart.Id + 1;
					}
					delFunc += Walk;
				}
				this.transform.position = movable.WpsList[movable.Courant].transform.position;
				
				//delEnum = null;
				
				switch(platformSpeed)
				{
				case Speed.VerySlow:
					speed = 1;
					break;
				case Speed.Slow:
					speed = 3;
					break;
				case Speed.Medium:
					speed = 5;
					break;
				case Speed.Fast:
					speed = 7;
					break;
				case Speed.VeryFast:
					speed = 10;
					break;
				default:
					speed = 5;
					break;
				}
			}
			
		}
		// =============
	}
	
	
	void Update()
	{
		if (delRot != null)
		{
			delRot(trap);
		}
		if (delFunc != null)
		{
			delFunc();
		}
		
		if ( delEnum != null)
		{
			StartCoroutine(delEnum());
			delEnum = null;
		}
	}
	
	//============
	// MOVABLE
	//============
	
	/// <summary>
	/// Wait the specified seconds.
	/// </summary>
	/// <param name="seconds">Seconds.</param>
	
	public void Wait(float seconds)
	{
		delFunc -= Walk;
		
		if(index == movable.Next)
		{
			waitTime = seconds;
			delEnum = TimerDelay;
			index++;
		}
	}
	
	/// <summary>
	/// Walk this instance.
	/// </summary>
	void Walk()
	{
		if(movable.WpsList.Count > 1)
		{
			if (Vector3.Distance(transform.position, movable.WpsList[movable.Next].transform.position) > range)
			{
				Move(movable.WpsList[movable.Next].transform);
			}
			else
			{
				canStart = false;
				transform.position = movable.WpsList[movable.Next].transform.position;
				NextIndex();
			}
		}
	}
	
	/// <summary>
	/// Stop the specified isStopped.
	/// </summary>
	/// <param name="isStopped">If set to <c>true</c> is stopped.</param>
	
	public void Stop(bool isStopped)
	{
		if(isStopped && !canStart)
		{
			delFunc -= Walk;
		}
		
	}
	
	/// <summary>
	/// News the speed.
	/// </summary>
	/// <param name="newSpeed">New speed.</param>
	
	public void NewSpeed(float newSpeed)
	{
		speed = newSpeed;
	}
	
	/// <summary>
	/// Changes the rotation.
	/// </summary>
	/// <param name="identity">If set to <c>true</c> identity.</param>
	
	public void	ChangeRotation(int orientation)
	{
		switch(orientation)
		{
		case 0:
			myMovement = orientation;
			break;
			
		case 1:
			myMovement = orientation;
			break;
			
		case 2:
			myMovement = orientation;
			break;
		}
	}
	
	/// <summary>
	/// Teleport the specified where.
	/// </summary>
	/// <param name="where">Where.</param>
	
	public void Teleport(WaypointScript where)
	{
		transform.position = where.GetComponent<Transform>().position;
		movable.Next = where.Id +1;
	}
	
	/// <summary>
	/// Rotates the half.
	/// </summary>
	/// <param name="half">If set to <c>true</c> half.</param>
	
	public void RotateHalf(MovableRotation rotation)
	{
		switch(isTrigger)
		{
		case false:
			if(rotation.seconds != 0 && !onHalfRot)
			{
				timerToRot = rotation.seconds;
				delEnum = RotateTimer;
				onHalfRot = true;
			}
			else if(rotation.isRotating && !onHalfRot )
			{
				onHalfRot = true;
				Rotation();
			}
			else if(!rotation.isRotating && onHalfRot)
			{
				onHalfRot = false;
				Rotation ();
			}
			break;
			
		case true:
			if(rotation.seconds != 0 && !onHalfRot)
			{
				timerToRot = rotation.seconds;
				delEnum = RotateTimer;
				onHalfRot = true;
			}
			break;
		}
	}
	/// <summary>
	/// Rotates the full.
	/// </summary>
	/// <param name="full">If set to <c>true</c> full.</param>
	
	public void RotateFull(MovableRotation rotation)
	{
		switch(isTrigger)
		{
		case false:
			if(rotation.seconds != 0 && !onFullRot)
			{
				timerToRot = rotation.seconds;
				delEnum = RotateTimer;
				onFullRot = true;
			}
			else if(rotation.isRotating && !onFullRot )
			{
				onFullRot = true;
				Rotation();
			}
			else if(!rotation.isRotating && onFullRot)
			{
				onFullRot = false;
				Rotation ();
			}
			break;
			
		case true:
			if(rotation.seconds != 0 && !onFullRot)
			{
				timerToRot = rotation.seconds;
				delEnum = RotateTimer;
				onFullRot = true;
			}
			break;
		}
	}
	
	/// <summary>
	/// Wait this instance.
	/// </summary>
	
	IEnumerator TimerDelay()
	{
		yield return new WaitForSeconds(waitTime);
		delFunc += Walk;
	}
	
	/// <summary>
	/// Move the specified target.
	/// </summary>
	/// <param name="target">Target.</param>
	void Move(Transform target)
	{
		switch(myMovement)
		{
		case 0:
			transform.position =  Vector3.MoveTowards(transform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			//transform.rotation = Quaternion.identity;
			
			break;
			
		case 1:
			maxRotationSpeed = 5;
			moveDirection = transform.forward * speed;
			//moveDirection.y -= gravity;
			
			transform.Translate(moveDirection * Time.deltaTime, Space.World);
			//myTransform.position =  Vector3.MoveTowards(myTransform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			Vector3 TargetDir = (target.position - transform.position);
			Vector3 newDir = Vector3.RotateTowards(transform.forward, TargetDir, maxRotationSpeed * Time.deltaTime, 0.0f );
			transform.rotation = Quaternion.LookRotation(newDir);
			break;
			
		case 2:
			maxRotationSpeed = 400;
			transform.position =  Vector3.MoveTowards(transform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			var newRotation = Quaternion.LookRotation(target.position - transform.position).eulerAngles;
			var angles = transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler(angles.x,Mathf.SmoothDampAngle(angles.y,newRotation.y, ref velocity, minTime, maxRotationSpeed),newRotation.z);
			break;
			
		}
		
		
		//controller.Move(moveDirection * Time.deltaTime);
		/*moveDirection = myTransform.forward * speed;

			myTransform.Translate(moveDirection * Time.deltaTime, Space.World);

			var newRotation = Quaternion.LookRotation(target.position - myTransform.position).eulerAngles;
			var angles = myTransform.rotation.eulerAngles;
			//myTransform.rotation = Quaternion.Euler(angles.x,Mathf.SmoothDampAngle(angles.y,newRotation.y, ref velocity, minTime, maxRotationSpeed),newRotation.z);
			myTransform.rotation = Quaternion.Euler(Mathf.SmoothDampAngle(angles.x,newRotation.x, ref velocity, minTime, maxRotationSpeed),
			                                        Mathf.SmoothDampAngle(angles.y,newRotation.y, ref velocity, minTime, maxRotationSpeed),
			                                        Mathf.SmoothDampAngle(angles.z,newRotation.z, ref velocity, minTime, maxRotationSpeed));*/
	}
	
	/// <summary>
	/// Nexts the index.
	/// </summary>
	
	void NextIndex()
	{
		//if (++index == waypoint.Count) index = 0;
		movable.WpSetNext();
		index = movable.Next;
	}
	
	IEnumerator UnrotateTimer()
	{
		yield return new WaitForSeconds(timerToRot);
		delFunc += Unrotate;
		delFunc -= HalfRotation;
		delFunc -= FullRotation;
	}
	
	IEnumerator RotateTimer()
	{
		yield return new WaitForSeconds(timerToRot);
		Rotation();
		delEnum = UnrotateTimer;
	}
	
	/// <summary>
	/// Rotate this instance.
	/// </summary>
	
	public void Rotation()
	{
		if (onHalfRot)
		{
			delFunc += HalfRotation;
		}
		else if (onFullRot)
		{
			delFunc += FullRotation;
		}
		
		else if(!onFullRot && !onHalfRot)
		{
			totalRotation = 0;
			delFunc -= HalfRotation;
			delFunc -= FullRotation;
			delFunc += Unrotate;
		}
	}
	
	/// <summary>
	/// Halfs the rotation.
	/// </summary>
	public void HalfRotation()
	{
		/*if(myTransform.rotation.eulerAngles.y < halfRotation.y)
		{
			myTransform.Rotate(0,180,0 * Time.smoothDeltaTime, Space.Self);
		}*/
		//myTransform.rotation = Quaternion.Lerp(myTransform.rotation, halfRotation, Time.smoothDeltaTime);
		//pivotZ.rotation = Quaternion.RotateTowards(pivotZ.rotation,halfRotation, rotationSpeed * 2);
		//Vector3 temp =  Vector3.RotateTowards(pivotZ.forward, halfRotation, rotationSpeed * Time.deltaTime, 0.0f);
		float rotationAmt = rotationSpeed;
		if (totalRotation < 90)
		{
			transform.Rotate(rotationAmt,0,0);
			totalRotation += rotationAmt;
		}
		else
		{
			totalUnrotate = totalRotation;
			delFunc -= HalfRotation;
		}
	}
	
	/// <summary>
	/// Fulls the rotation.
	/// </summary>
	public void FullRotation()
	{
		/*if(myTransform.rotation.eulerAngles.y < fullRotation.y)
		{
			myTransform.Rotate(0,180,0 * Time.smoothDeltaTime, Space.Self);
		}*/
		//myTransform.rotation = Quaternion.Lerp(myTransform.rotation, fullRotation, Time.smoothDeltaTime);
		
		//pivotZ.rotation = Quaternion.RotateTowards(pivotZ.rotation,fullRotation, rotationSpeed);
		if(onHalfRot)
		{
			totalRotation = 90;
		}
		
		float rotationAmt = rotationSpeed;
		if (totalRotation < 180)
		{
			transform.Rotate(rotationAmt,0,0);
			totalRotation += rotationAmt;
		}
		else
		{
			totalUnrotate = totalRotation;
			delFunc -= FullRotation;
		}
	}
	
	/// <summary>
	/// Unrotate this instance.
	/// </summary>
	public void Unrotate()
	{
		/*if(myTransform.rotation.eulerAngles.y > 0)
		{
			myTransform.rotation = Quaternion.Lerp(myTransform.rotation, resetRot, 0.1f);
		}*/
		//myTransform.rotation = Quaternion.Lerp(myTransform.rotation, resetRot,  Time.smoothDeltaTime);
		float rotationAmt = rotationSpeed;
		if (totalUnrotate > 0)
		{
			transform.Rotate(-rotationAmt,0,0);
			totalUnrotate -= rotationAmt;
		}
		else
		{
			onHalfRot = false;
			onFullRot = false;
			delFunc -= Unrotate;
			totalUnrotate = 0;
			totalRotation = 0;
		}
	}
}






