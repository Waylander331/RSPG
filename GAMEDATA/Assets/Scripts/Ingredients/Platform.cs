using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(CharacterController))]
public class Platform : MonoBehaviour,IMovable, ITriggerable
{
	public bool canFall;
	public float fallTimer;

	private bool fall = false;

	private bool noHit = false;

	public enum Speed
	{
		VerySlow, Slow, Medium, Fast, VeryFast
	};

	private bool youDied = false;

	#region platform variable
	public Speed platformSpeed;
	private Movable movable;
	private WaypointScript waypoint;
	private LayerMask groundLayer;
	private Transform myTransform;
	private Vector3 moveDirection;
	private float minTime = 0.1f;
	private float velocity;
	private float range = 0.2f;
	private float speed;
	private float gravity = 0f;
	private float maxRotationSpeed;
	private float rotationSpeed = 2.5f;
	#endregion

	#region Interface IMovable Var
	private float waitTime;
	private int index;
	public bool isTrigger = false;
	public MovableRotation trap;
	private bool onHalfRot = false;
	private bool onFullRot = false;
	private float totalRotation = 0;
	private float totalUnrotate;
	private float timerToRot;
	//private bool rotated;
	private bool unrotated = true;
	private int myMovement = 0;
	private Transform pivotZ;
	private bool canStart = false;
	private bool doItOnce = false;
	#endregion
	
	#region Delegate
	delegate void DelRot(MovableRotation trap);
	delegate void Delfunc();
	delegate IEnumerator DelEnum();
	Delfunc delFunc;
	DelEnum delEnum;
	DelRot delRot;
	#endregion
		
	//===================
	// EFFECT_ROTATE
	private float rotateEffectAngle;
	private float rotateEffectSpeed;
	private float rotateEffectDelay;
	private Effect_Rotate.RotationAxis rotateEffectAxis;

	private bool rotateEffectActive;
	private bool rotateEffectRotating;
	private float rotateEffectCount = 0f;
	//===================

	void Start()
	{
		if(Application.isEditor)
		{
			pivotZ = transform.GetChild(0).GetComponent<Transform>();
			myTransform = GetComponent<Transform>();
			movable = GetComponent<Movable>();
			index = movable.Next;

			if(movable.WpsList.Count > 1)
			{
				if(movable.WpStart != null)
				{
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
	}

	void OnLevelWasLoaded()
	{
		if(!Application.isEditor)
		{
			pivotZ = transform.GetChild(0).GetComponent<Transform>();
			myTransform = GetComponent<Transform>();
			movable = GetComponent<Movable>();
			index = movable.Next;

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
	}
		
	void Update()
	{
		//waypoint = GetComponent<WaypointScript>();
		/*if(canFall) // enclenche le timer pour faire tomber la plateforme
		{
			if(fallTimer <= 0)
			{
				if(!noHit)
				{
					moveDirection.y -= gravity;
					myTransform.Translate(moveDirection * Time.deltaTime, Space.World);
				}
				else
				{
					gravity = 0;
				}
			}
		}*/

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

		//===================
		// EFFECT_ROTATE
		if(rotateEffectRotating)
		{
			float rotationValue = 0f;
			if(rotateEffectCount + rotationValue < rotateEffectAngle)
			{
				rotationValue = rotateEffectSpeed * Time.deltaTime;
			}
			else
			{
				rotationValue = rotateEffectAngle - rotateEffectCount;
			}

			rotateEffectCount += rotationValue;

			Vector3 rotation = Vector3.zero;
			switch(rotateEffectAxis)
			{
			case Effect_Rotate.RotationAxis.x_Axis:
				rotation.x = rotationValue;
				break;
			case Effect_Rotate.RotationAxis.y_Axis:
				rotation.y = rotationValue;
				break;
			case Effect_Rotate.RotationAxis.z_Axis:
				rotation.z = rotationValue;
				break;
			default:
				Debug.LogWarning("Erreur : RotationAxis is Invalid. Putting x as the axis.");
				rotation.x = rotationValue;
				break;
			}

			transform.Rotate(rotation, Space.Self);
			if(rotateEffectCount == rotateEffectAngle)
			{
				rotateEffectRotating = false;
				rotateEffectCount = 0f;
				if(rotateEffectActive)
				{
					Invoke ("TriggerEffectRotation", rotateEffectDelay);
				}
			}
		}
	}
		

	void OnTriggerEnter(Collider other) 
	{
		/*if(other.gameObject.layer == groundLayer)
		{
			noHit = true;
		}*/
		
		if (other.tag == ("Player"))
		{
			if(canFall)
			{
				delFunc += TimerFall;
			}
		}
	}

	/// <summary>
	/// Triggered the specified effect.
	/// </summary>
	/// <param name="effect">Effect.</param>

	public void Triggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{
			if(trap.isRotating)
			{
				if(trap.rotateHalf)
				{
					delRot += RotateHalf;
				}
				else if(trap.rotateFull)
				{
					delRot += RotateFull;
				}
			}
			else
			{
				if(doItOnce)
				{
					canStart = true;
					delFunc += Walk;
					doItOnce = false;
				}
			}
		}
		if(effect.GetType() == typeof (Effect_Move))
		{
			if(trap.isRotating)
			{
				if(trap.rotateHalf)
				{
					delRot += RotateHalf;
				}
				else if(trap.rotateFull)
				{
					delRot += RotateFull;
				}
			}
			else
			{
				if(doItOnce)
				{
					canStart = true;
					delFunc += Walk;
					doItOnce = false;
				}
			}
		}
		//===================
		// EFFECT_ROTATE
		if(effect.GetType() == typeof(Effect_Rotate))
		{
			Effect_Rotate effectVar = effect as Effect_Rotate;
			rotateEffectAngle = effectVar.angle;
			rotateEffectSpeed = effectVar.rotationSpeed;
			rotateEffectDelay = effectVar.delay;
			rotateEffectAxis = effectVar.rotationAxis;

			rotateEffectActive = true;
			TriggerEffectRotation();
		}
		//===================
	}

	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{
			delRot -= RotateHalf;
			delRot -= RotateFull;
		}

		//===================
		// EFFECT_ROTATE
		if(effect.GetType() == typeof(Effect_Rotate))
		{
			rotateEffectActive = false;
			CancelInvoke ("TriggerEffectRotation");
		}
		//===================
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

		switch(isTrigger)
		{
		case false:
			if(rotation.seconds != 0 && !onHalfRot)
			{
				unrotated = false;
				timerToRot = rotation.seconds;
				delEnum = RotateTimer;
				onHalfRot = true;
			}
			else if(rotation.isRotating && !onHalfRot)
			{
				unrotated = false;
				onHalfRot = true;
				Rotation();
			}
			else if(!rotation.isRotating && onHalfRot)
			{
				unrotated = false;
				onHalfRot = false;
				Rotation();
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
				unrotated = false;
				timerToRot = rotation.seconds;
				delEnum = RotateTimer;
				onFullRot = true;
			}
			else if(rotation.isRotating && !onFullRot )
			{
				unrotated = false;
				onFullRot = true;
				Rotation();
			}
			else if(!rotation.isRotating && onFullRot)
			{
				unrotated = false;
				onFullRot = false;
				Rotation();
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
			pivotZ.Rotate(rotationAmt * Time.timeScale,0,0);
			totalRotation += rotationAmt * Time.timeScale;
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
		if(OnHalfRot)
		{
			totalRotation = 90;
		}

		float rotationAmt = rotationSpeed;
		if (totalRotation < 180)
		{
			pivotZ.Rotate(rotationAmt * Time.timeScale,0,0);
			totalRotation += rotationAmt * Time.timeScale;
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
			pivotZ.Rotate(-rotationAmt * Time.timeScale,0,0);
			totalUnrotate -= rotationAmt * Time.timeScale;
		}
		else
		{
			onHalfRot = false;
			onFullRot = false;
			delFunc -= Unrotate;
			totalUnrotate = 0;
			totalRotation = 0;
			unrotated = true;
		}
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
				Move(movable.WpsList[movable.Next].transform);
			}
			else
			{
				canStart = false;
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
			maxRotationSpeed = 5;
			moveDirection = myTransform.forward * speed;
			//moveDirection.y -= gravity;

			myTransform.Translate(moveDirection * Time.deltaTime, Space.World);
			//myTransform.position =  Vector3.MoveTowards(myTransform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			Vector3 TargetDir = (target.position - myTransform.position);
			Vector3 newDir = Vector3.RotateTowards(myTransform.forward, TargetDir, maxRotationSpeed * Time.deltaTime, 0.0f );
			myTransform.rotation = Quaternion.LookRotation(newDir);
			break;

		case 2:
			maxRotationSpeed = 400;
			myTransform.position =  Vector3.MoveTowards(myTransform.position, movable.WpsList[movable.Next].transform.position, speed * Time.smoothDeltaTime);
			var newRotation = Quaternion.LookRotation(target.position - myTransform.position).eulerAngles;
			var angles = myTransform.rotation.eulerAngles;
			myTransform.rotation = Quaternion.Euler(angles.x,Mathf.SmoothDampAngle(angles.y,newRotation.y, ref velocity, minTime, maxRotationSpeed),newRotation.z);
			break;

		}

		// The Drunken Platform!!!!!!
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
	/// Wait this instance.
	/// </summary>

	IEnumerator TimerDelay()
	{
		yield return new WaitForSeconds(waitTime);
		delFunc += Walk;
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
	/// Timers the fall.
	/// </summary>

	public void TimerFall() // Fonction timer falling plateforme
	{
		fallTimer -= Time.deltaTime;
		
		if(fallTimer <=0f)
		{
			//gravity = 0.25f;
			this.GetComponent<Rigidbody>().isKinematic = false;
			this.GetComponent<Rigidbody>().useGravity = true;
			delFunc -= Walk;
		}

		if (fallTimer <= -3f)
		{
			Destroy(this.gameObject);
			delFunc -= TimerFall;
		}
	}

	// ==============
	// EFFECT_ROTATE
	void TriggerEffectRotation()
	{
		rotateEffectRotating = true;
	}
	// ==============

	/*public bool IsRotating{
		get{
			return trap.isRotating;
		}
		set{
			trap.isRotating = value;
		}
	}

	public bool RotHalf{
		get{
			return trap.rotateHalf;
		}
		set{
			trap.rotateHalf = value;
		}
	}

	public bool RotFull{
		get{
			return trap.rotateFull;
		}
		set{
			trap.rotateFull = value;
		}
	}

	public float Seconds{
		get{
			return trap.seconds;
		}
		set{
			trap.seconds = value;
		}
	}
*/
	public bool OnHalfRot {
		get {
			return onHalfRot;
		}
		set {
			onHalfRot = value;
		}
	}

	public bool RotateEffectRotating {
		get {
			return rotateEffectRotating;
		}
		set {
			rotateEffectRotating = value;
		}
	}

	public bool OnFullRot {
		get {
			return onFullRot;
		}
		set {
			onFullRot = value;
		}
	}

	public bool Unrotated {
		get {
			return unrotated;
		}
		set {
			unrotated = value;
		}
	}

	#region Waypoint kinda obsolete....
	/*public void FindWaypoint()
	{
		if (string.IsNullOrEmpty(strTag))
		{
			Debug.LogError("No Waypoint Given!");
		}
		
		GameObject[] gos = GameObject.FindGameObjectsWithTag(strTag);
		foreach (GameObject go in gos)
		{
			WaypointScript script = go.GetComponent<WaypointScript>();
			waypoint.Add(script.index, go.transform);
		}
		range = 0.1f;
	}*/
	#endregion
}






