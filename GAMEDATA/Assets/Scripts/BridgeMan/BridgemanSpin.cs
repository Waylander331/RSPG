using UnityEngine;
using System.Collections;

public class BridgemanSpin : MonoBehaviour 
{
	private bool counterClockwise;
	private bool rotating;
	private float rotationSpeed = 450f;
	
	private bool lastRotation;
	private bool lastRotationDone;

	public bool CounterClockwise
	{
		get{return counterClockwise;}
	}

	public float RotationSpeed
	{
		get{return rotationSpeed;}
		set{rotationSpeed = value;}
	}

	void Start()
	{
		if (counterClockwise && rotationSpeed > 0f || !counterClockwise && rotationSpeed < 0f)
			rotationSpeed = -rotationSpeed;
	}

	void LateUpdate()
	{
		if (rotating) 
		{
			transform.Rotate (0f, rotationSpeed * Time.deltaTime, 0f);
			if(lastRotationDone) rotating = false;
			if(lastRotation) lastRotationDone = true;
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawLine (transform.position, transform.forward * 3f);
	}
	
	public bool Rotating
	{
		get{return rotating;}
		set{rotating = value;}
	}
	
	public bool LastRotation
	{
		set{lastRotation = value;}
	}
}
