using UnityEngine;
using System.Collections;

public class RotationPointInterval : MonoBehaviour, ITriggerable
{
	public bool active = true;
	private bool realActive;
	public bool loop;

	public float rotationSpeed = 360f;
	public float rotationDegree = 90f;
	public float delay = 1f;
	
	private float currentRotation;
	private bool rotating;
	private bool reversed;

	private float initialSpeed;
	private float initialRotationDegree;
	private float initialDelay;
	private bool initialLoop;
	private RotationAxis initialAxis;

	private float currentRotationSpeed;
	private float currentRotationDegree;
	private RotationAxis currentAxis;

	public enum RotationAxis
	{
		X_Axis,
		Y_Axis,
		Z_Axis
	}

	public RotationAxis axis;

	void Start()
	{
		initialSpeed = rotationSpeed;
		initialRotationDegree = rotationDegree;
		initialDelay = delay;
		initialLoop = loop;
		initialAxis = axis;

		if (rotationDegree * rotationSpeed < 0f)
		{
			reversed = true;
		}
		rotationDegree = Mathf.Abs (rotationDegree);
		rotationSpeed = Mathf.Abs (rotationSpeed);
		realActive = active;
		if (active)
			TriggerRotation ();
	}

	void Update()
	{
		if(rotating)
		{
			float targetRotation = currentRotationSpeed * Time.deltaTime;
			if(currentRotation + targetRotation < currentRotationDegree)
			{
				currentRotation += targetRotation;
			}
			else
			{
				targetRotation = currentRotationDegree - currentRotation;
				currentRotation = 0f;
				rotating = false;
				if(!loop) active = false;
			}
			if(reversed) targetRotation = -targetRotation;

			Vector3 rotationVector = new Vector3
				(axis == RotationAxis.X_Axis ? targetRotation : 0f, axis == RotationAxis.Y_Axis ? targetRotation : 0f, axis == RotationAxis.Z_Axis ? targetRotation : 0f);
			transform.Rotate(rotationVector, Space.Self);
		}
		else
		{
			if(realActive != active)
				realActive = !realActive;
			if(realActive && !IsInvoking("StartRotating"))
				TriggerRotation();
			else if(!realActive && IsInvoking("StartRotating"))
				CancelRotation();
		}
	}

	void StartRotating()
	{
		UpdateRotationProperties ();
		rotating = true;
	}

	void TriggerRotation()
	{
		active = true;
		Invoke ("StartRotating", delay);
	}

	void CancelRotation()
	{
		active = false;
		CancelInvoke ("TriggerRotation");
	}

	void UpdateRotationProperties()
	{
		currentRotationSpeed = rotationSpeed;
		currentRotationDegree = rotationDegree;
		currentAxis = axis;
	}

	public void Triggered(EffectList effect)
	{	
		if(effect.GetType() == typeof(Effect_ActivateRotationGroup))
		{
			active = true;
		}

		if(effect.GetType() == typeof(Effect_SetRotationValues))
		{
			Effect_SetRotationValues values = (Effect_SetRotationValues) effect;
			if(values.changes.changeSpeed) rotationSpeed = values.newRotationSpeed;
			if(values.changes.changeRotationDegree) rotationDegree = values.newRotationDegree;
			if(values.changes.changeLoop) loop = values.newLoop;
			if(values.changes.changeDelay) delay = values.newDelay;

			if(values.changes.changeAxis) 
			{
				switch(values.newAxis)
				{
				case Effect_SetRotationValues.RotationAxis.X_Axis:
					axis = RotationAxis.X_Axis;
					break;
				case Effect_SetRotationValues.RotationAxis.Y_Axis:
					axis = RotationAxis.Y_Axis;
					break;
				case Effect_SetRotationValues.RotationAxis.Z_Axis:
					axis = RotationAxis.Z_Axis;
					break;
				}
			}

		}
	}
	
	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType() == typeof(Effect_ActivateRotationGroup))
		{
			active = false;
		}

		if(effect.GetType() == typeof(Effect_SetRotationValues))
		{
			rotationSpeed = initialSpeed;
			rotationDegree = initialRotationDegree;
			loop = initialLoop;
			delay = initialDelay;
			axis = initialAxis;
		}
	}
}
