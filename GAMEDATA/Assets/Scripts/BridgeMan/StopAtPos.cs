using UnityEngine;
using System.Collections;

public class StopAtPos : MonoBehaviour 
{
	private int numberOfTurn = 1;
	private float minRotation;
	
	private BridgemanSpin script;
	private float rotSpeed;
	private Vector3 initialForward;
	
	private Vector3 stopAtVector;
	
	private float currentRotation = 0f;
	private float targetRotation;
	private Transform stopAt;

	void Start()
	{
		script = GetComponent<BridgemanSpin> ();
		stopAt = transform.GetChild (4);
		stopAt.SetParent (null);
	}
	
	void OnDrawGizmos()
	{
		if(stopAt == null) stopAt = transform.GetChild (4);
		Vector3 arrowPos = transform.position;
		arrowPos.y += 0.2f;
		Vector3 arrowDir = stopAt.position - transform.position;
		arrowDir.y = 0f;
		arrowDir = arrowDir.normalized * 8.25f;
		
		DrawShape.DrawArrowForGizmo (arrowPos, arrowDir, Color.blue, 0.7f, 35);
		Vector3 lineInitPos = transform.position;
		lineInitPos.y += 4f;
		Gizmos.DrawLine (lineInitPos, stopAt.position);
	}
	
	void LateUpdate () 
	{
		if (script.Rotating) 
		{
			float absRotSpeed = Mathf.Abs (rotSpeed);
			if(currentRotation + absRotSpeed * Time.deltaTime <= targetRotation)
			{
				currentRotation += absRotSpeed * Time.deltaTime;
			}
			else // Finish spin
			{
				script.Rotating = false;
			}
		}
	}

	public void InitializeForSpin()
	{
		rotSpeed = script.RotationSpeed;

		Vector2 vec2Forward = new Vector2 (transform.forward.x, transform.forward.z);
		Vector2 vec2stopAtPos = new Vector2(stopAt.position.x - transform.position.x, stopAt.position.z - transform.position.z);

		float shortTarget = Vector2.Angle (vec2Forward, vec2stopAtPos);
		float longTarget = 360f - shortTarget;
		
		if (targetRotation < minRotation && numberOfTurn == 0) 
		{
			targetRotation += 360f;
			rotSpeed *= 2f;
			script.RotationSpeed *= 2;
		}
		
		transform.Rotate (0f, rotSpeed * Time.deltaTime, 0f); // fake rotation to see if targetPos is short or long

		vec2Forward = new Vector2 (transform.forward.x, transform.forward.z);

		float newAngle = Vector2.Angle (vec2Forward, vec2stopAtPos);
		
		// if increase, goes to shortTarget, else goes to long.
		if(shortTarget >= rotSpeed && shortTarget <= 180f - rotSpeed) 
			targetRotation = newAngle > shortTarget ? shortTarget : longTarget;
		// if increase, goes to longTarget, else goes to short
		else 
		{
			targetRotation = newAngle > shortTarget ? longTarget : shortTarget;
		}
		
		targetRotation += 360f * numberOfTurn;
		
		transform.Rotate (0f, -rotSpeed * Time.deltaTime, 0f); // Put it back to original pos.
	}
}
