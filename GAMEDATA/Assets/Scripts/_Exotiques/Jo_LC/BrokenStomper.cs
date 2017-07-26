using UnityEngine;
using System.Collections;

public class BrokenStomper: MonoBehaviour 
{
	public float descentDistance;
	public float goSpeed;
	public float returnSpeed;
	
	private float traveledDistance = 0f;
	private BrokenStomperTrigger trigger;
	private bool going;
	private bool reachedPos;

	private bool reversed;
	private bool gotValue = false;

	void Start()
	{
		trigger = transform.GetChild (0).GetChild (0).GetComponent<BrokenStomperTrigger> ();
		reversed = descentDistance < 0f;

		BrokenStomperGizmo gizmo = transform.GetChild (1).GetComponent<BrokenStomperGizmo> ();
		gizmo.OnEditor = false;
		gizmo.InitPos = gizmo.transform.position;

		descentDistance = Mathf.Abs (descentDistance);
		goSpeed = Mathf.Abs (goSpeed);
		returnSpeed = Mathf.Abs (returnSpeed);
	}

	void LateUpdate()
	{
		if(!going && trigger.IsDetecting)
		{
			going = true;
		}
		else if (going && !trigger.IsDetecting)
		{
			going = false;
		}

		if(going && traveledDistance != descentDistance || !going && traveledDistance != 0f)
		{
			reachedPos = false;
		}

		if(!reachedPos)
		{
			going = trigger.IsDetecting;
			if(going && traveledDistance != descentDistance)
			{
				float toTravel = 0f;
				if(traveledDistance + goSpeed * Time.deltaTime < descentDistance)
				{
					toTravel = goSpeed * Time.deltaTime;
				}
				else
				{
					toTravel = descentDistance - traveledDistance;
					reachedPos = true;
				}
		
				traveledDistance += toTravel;
				if(reversed) toTravel = -toTravel;
				transform.Translate (Vector3.down * toTravel, Space.World);
			}
			else if(!going && traveledDistance != 0f)
			{
				float toTravel = 0f;
				if(traveledDistance - returnSpeed * Time.deltaTime > 0f)
				{
					toTravel = returnSpeed * Time.deltaTime;
				}
				else
				{
					toTravel = traveledDistance;
					reachedPos = true;
				}

				traveledDistance -= toTravel;
				if(reversed) toTravel = -toTravel;
				transform.Translate (Vector3.up * toTravel, Space.World);

			}
		}
	}

	public bool Reversed
	{
		get{return reversed;}
	}
}
