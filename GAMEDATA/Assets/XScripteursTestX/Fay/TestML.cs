using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(CharacterController))]
public class TestML : MonoBehaviour,IMovable, ITriggerable
{

	private Manager myMan;

	# region Platform Movement Var
	
	public enum Speed
	{
		VerySlow, Slow, Medium, Fast, VeryFast
	};
	
	public Speed platformSpeed;
	
	private Movable movable;
	//private CharacterController controller;
	private Transform myTransform;
	private Vector3 moveDirection;
	private float minTime = 0.1f;
	private float velocity;
	private float range = 0.2f;
	private float speed;
	public float maxRotationSpeed = 1000f;
	
	
	
	#endregion
	
	//public LayerMask groundLayer;
	
	#region Interface IMovable Var
	
	private float waitTime;
	private int index;
	
	private bool onHalfRot = false;
	private bool onFullRot = false;
	private Vector3 halfRotation = new Vector3(0,0,90);
	private Vector3 fullRotation =  new Vector3(0,0,180);
	private Quaternion resetRot;
	private float timerToRot;
	private bool rotated;
	private bool isRotating;
	
	
	#endregion
	
	#region Waypoint variable
	
	//private WaypointScript waypoint;
	/*int index;
	public string strTag;
	Dictionary<int, Transform> waypoint = new Dictionary<int, Transform>();*/
	#endregion
	//bool walking, waiting, victoring;
	//bool isWaiting, isVictoring;
	#region Delegate
	
	delegate void Method();
	delegate void Delfunc();
	delegate IEnumerator DelEnum();
	Delfunc delFunc;
	DelEnum delEnum;
	Method method;
	
	#endregion
	
	
	void Start()
	{
		myMan = FindObjectOfType<Manager>();
		//controller = GetComponent<CharacterController>();
		myTransform = GetComponent<Transform>();
		movable = GetComponent<Movable>();
		
		resetRot = new Quaternion (0,0,0,1);
		delFunc = this.Walk;
		delEnum = null;
		this.transform.position = movable.WpsList[movable.Courant].transform.position;
		//FindWaypoint()
		
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
		
		//waypoint = GameObject.Find("PathObject").GetComponent<PathScript>().GetPath(strTag);
		//walking = true; waiting =  false; victoring = false;
		//isWaiting = false; isVictoring = false;
		//target = GetTarget();
		//InvokeRepeating("NewTarget",0.01f, 2.0f);
	}
	
	void Update()
	{	
		if (delFunc != null)
		{
			delFunc();
		}
		else if ( delEnum != null)
		{
			StartCoroutine(delEnum());
			delEnum = null;
		}
		
		Rotation();
	}
	void OnTriggerEnter (Collider other) {
		Debug.Log("Zé tousser kkchose");
		if(other.gameObject.tag == "Player"){
			Debug.Log("CENSURE");
			ThisIsKillingMeh();
		}
	}
	private void ThisIsKillingMeh(){
		myMan.IsRespawning = true;
		myMan.LevelTransit(myMan.CurrentLevel);
	}
	/// <summary>
	/// Walk this instance.
	/// </summary>
	void Walk()
	{
		//if(movable.WpsList != null)
		{
			//Debug.Log (movable.WpsList.Count);
			if (Vector3.Distance(myTransform.position, movable.WpsList[movable.Next].transform.position) > range)
				//if((myTransform.position - movable.WpsList[movable.Next].transform.position).sqrMagnitude > range)
			{
				Move(movable.WpsList[movable.Next].transform);
			}
			
			else
			{
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
		if(isRotating)
		{
			moveDirection = myTransform.forward * speed;
			
			//controller.Move(moveDirection * Time.deltaTime);
			
			myTransform.Translate(moveDirection * Time.deltaTime, Space.World);
			
			var newRotation = Quaternion.LookRotation(target.position - myTransform.position).eulerAngles;
			var angles = myTransform.rotation.eulerAngles;
			myTransform.rotation = Quaternion.Euler(angles.x,Mathf.SmoothDampAngle(angles.y,newRotation.y, ref velocity, minTime, maxRotationSpeed),angles.z);
		}
		else if(!isRotating)
		{
			myTransform.position =  Vector3.MoveTowards(myTransform.position, movable.WpsList[movable.Next].transform.position, speed * Time.deltaTime);
		}
	}
	
	/*void OnTriggerStay(Collider other)
	{
		if (other.tag == ("Player"))
		{
			delFunc = null;
		}
	}*/
	/*	
	void OnTriggerExit(Collider other)
	{
		if ( other.tag == ("Player"))
		{
			delFunc = this.Walk;
		}
	}*/
	
	
	/// <summary>
	/// Triggered the specified effect.
	/// </summary>
	/// <param name="effect">Effect.</param>
	
	public void Triggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{
			delFunc = this.Walk;
		}
	}
	
	public void UnTriggered(EffectList effect)
	{
		Debug.Log ("Untriggered");
	}
	
	/// <summary>
	/// Wait the specified seconds.
	/// </summary>
	/// <param name="seconds">Seconds.</param>
	
	public void Wait(float seconds)
	{
		waitTime = seconds;
		
		if(index == movable.Next)
		{
			delFunc = null;
			delEnum = this.Timer;
			index++;
		}
	}
	
	/// <summary>
	/// Stop the specified isStopped.
	/// </summary>
	/// <param name="isStopped">If set to <c>true</c> is stopped.</param>
	
	public void Stop(bool isStopped)
	{
		if(isStopped)
		{
			delFunc = null;
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
		/*if(identity)
		{
			isRotating = false;
		}
		else if (!identity)
		{
			isRotating = true;
		}*/
	}
	
	/// <summary>
	/// Teleport the specified where.
	/// </summary>
	/// <param name="where">Where.</param>
	
	public void Teleport(WaypointScript where)
	{
		if(where)
		{
			myTransform.position = where.GetComponent<Transform>().position;
		}
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
		//if (++index == waypoint.Count) index = 0;
		movable.WpSetNext();
		index = movable.Next;
	}
	
	/// <summary>
	/// Rotate this instance.
	/// </summary>
	
	public void Rotation()
	{
		if (onHalfRot)
		{
			if(myTransform.rotation.eulerAngles.z < halfRotation.z)
			{
				myTransform.Rotate(0,0,180 * Time.smoothDeltaTime, Space.Self);
			}
		}
		
		else if (onFullRot)
		{
			if(myTransform.rotation.eulerAngles.z < fullRotation.z)
			{
				myTransform.Rotate(0,0,180 * Time.smoothDeltaTime, Space.Self);
			}
		}
		
		else if(!onFullRot && !onHalfRot)
		{
			if(myTransform.rotation.eulerAngles.z > 0)
			{
				myTransform.rotation = Quaternion.Lerp(myTransform.rotation, resetRot, 0.1f);
			}
		}
		
		if (rotated)
		{
			timerToRot -= Time.deltaTime;
			
			if(timerToRot <= 0)
			{
				rotated = false;
				onFullRot = false;
				onHalfRot = false;
				timerToRot = 0f;
			}
		}
	}
	
	/*IEnumerator Stop()
	{
		//if( animation)
		yield return new WaitForSeconds(2.0f);
		//walking = true; isVictoring = false;
		NextIndex();
		delFunc = this.Walk;
		Debug.Log ("Victory");
	}*/
	
	/// <summary>
	/// Wait this instance.
	/// </summary>
	
	IEnumerator Timer()
	{
		
		yield return new WaitForSeconds(waitTime);
		delFunc = Walk;
	}
}






