using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Poney : MonoBehaviour, IMovable, ITriggerable {

	private NavMeshAgent navOwner;

	# region Platform Movement Var
	
	public enum Speed
	{
		VerySlow, Slow, Medium, Fast, VeryFast
	};
	
	public Speed platformSpeed;
	
	private Movable movable;
	private WaypointScript waypoint;
	private Transform myTransform;
	private Vector3 moveDirection;
	private float minTime = 0.1f;
	private float velocity;
	private float range = 0.2f;
	private float speed;
	private float gravity = 0f;
	private float maxRotationSpeed;
	private float rotationSpeed;
	
	
	
	#endregion
	
	#region Interface IMovable Var
	
	private float waitTime;
	private int index;

	private int myMovement = 0;
	private Transform pivotZ;
	private Transform pivotF;
	private bool doItOnce = false;
	private bool canStart = false;
	
	#endregion
	
	#region Delegate
	delegate void Delfunc();
	delegate IEnumerator DelEnum();
	Delfunc delFunc;
	DelEnum delEnum;
	#endregion

	
	void Start()
	{
		pivotZ = transform.GetChild(0).GetComponent<Transform>();
		pivotF = transform.GetChild(1).GetComponent<Transform>();
		myTransform = GetComponent<Transform>();
		movable = GetComponent<Movable>();
		navOwner = GetComponent<NavMeshAgent>();
		
		if(movable.WpsList.Count > 1)
		{
			if(movable.WpStart != null){
				movable.Courant = movable.WpStart.Id;
				movable.Next = movable.WpStart.Id + 1;
			}
			delFunc += Walk;
		}
		this.transform.position = movable.WpsList[movable.Courant].transform.position;

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
	
	void Update()
	{
		if (delFunc != null)
		{
			delFunc();
		}
		
		if ( delEnum != null)
		{
			StartCoroutine(delEnum());
			delEnum = null;
		}

		navOwner.speed = speed;
		pivotZ.rotation = Quaternion.identity;

	}

	/// <summary>
	/// Triggered the specified effect.
	/// </summary>
	/// <param name="effect">Effect.</param>
	
	public void Triggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{				
			if(doItOnce){
				canStart = true;
				delFunc += Walk;
				doItOnce = false;
			}
		}
		if(effect.GetType() == typeof (Effect_Move))
		{
			if(doItOnce){
				canStart = true;
				delFunc += Walk;
				doItOnce = false;
			}
		}
	}
	
	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{
			if(doItOnce){
				canStart = true;
				delFunc += Walk;
				doItOnce = false;
			}
		}

		if(effect.GetType() == typeof (Effect_Move))
		{
			if(doItOnce){
				canStart = true;
				delFunc += Walk;
				doItOnce = false;
			}
		}
	}
	
	/// <summary>
	/// Wait the specified seconds.
	/// </summary>
	/// <param name="seconds">Seconds.</param>
	
	public void Wait(float seconds)
	{
		if(index == movable.Next)
		{
			delFunc -= Walk;
			waitTime = seconds;
			delEnum = TimerDelay;
			index++;
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
			doItOnce = true;
			//canStart = true;
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
		myTransform.position = where.GetComponent<Transform>().position;
		movable.Next = where.Id +1;
	}
	
	/// <summary>
	/// Rotates the half.
	/// </summary>
	/// <param name="half">If set to <c>true</c> half.</param>
	
	public void RotateHalf(MovableRotation rotation)
	{

	}
	/// <summary>
	/// Rotates the full.
	/// </summary>
	/// <param name="full">If set to <c>true</c> full.</param>
	
	public void RotateFull(MovableRotation rotation)
	{

	}
	
	/// <summary>
	/// Nexts the index.
	/// </summary>
	
	void NextIndex()
	{
		movable.WpSetNext();
		index = movable.Next;
	}
	
		
	/// <summary>
	/// Walk this instance.
	/// </summary>
	void Walk()
	{
		if(movable.WpsList.Count > 1)
		{
			if (Vector3.Distance(myTransform.position, movable.WpsList[movable.Next].transform.position) > range)
			{
				navOwner.enabled = true;
				Move(movable.WpsList[movable.Next].transform);
				pivotF.forward = movable.WpsList[movable.Next].transform.position - myTransform.position;
			}
			else
			{
				canStart = false;
				navOwner.enabled = false;
				myTransform.position = movable.WpsList[movable.Next].transform.position;
				NextIndex();
			}
		}
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
			myTransform.position =  Vector3.MoveTowards(myTransform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			myTransform.rotation = Quaternion.identity;
			
			break;
			
		case 1:
			myTransform.position =  Vector3.MoveTowards(myTransform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			pivotF.forward = movable.WpsList[movable.Next].transform.position - myTransform.position;
			break;
			
		case 2:
			maxRotationSpeed = 400;
			myTransform.position =  Vector3.MoveTowards(myTransform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			var newRotation = Quaternion.LookRotation(target.position - myTransform.position).eulerAngles;
			var angles = myTransform.rotation.eulerAngles;
			myTransform.rotation = Quaternion.Euler(angles.x,Mathf.SmoothDampAngle(angles.y,newRotation.y, ref velocity, minTime, maxRotationSpeed),newRotation.z);
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
}
