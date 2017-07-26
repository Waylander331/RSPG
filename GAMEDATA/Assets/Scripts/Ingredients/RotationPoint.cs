using UnityEngine;
using System.Collections;

public class RotationPoint : MonoBehaviour, ITriggerable
{
	public bool active = true;
	public float rotationSpeed = 360f;

	public enum RotationAxis
	{
		X_Axis,
		Y_Axis,
		Z_Axis
	}

	public RotationAxis axis;
	private Vector3 rotationVector;
	private float initialSpeed;

	void Start()
	{
		rotationVector = new Vector3
			(axis == RotationAxis.X_Axis ? 1f : 0f, axis == RotationAxis.Y_Axis ? 1f : 0f, axis == RotationAxis.Z_Axis ? 1f : 0f);
		initialSpeed = rotationSpeed;
	}

	void Update()
	{
		if (active)
			transform.Rotate (rotationVector * rotationSpeed * Time.deltaTime, Space.Self);
	}

	public void Triggered(EffectList effect)
	{
		if(effect.GetType() == typeof(Effect_Default))
		{
			active = !active;
		}

		if(effect.GetType() == typeof(Effect_ActivateRotationGroup))
		{
			active = true;
		}

		if(effect.GetType() == typeof(Effect_SetRotationValues))
		{
			Effect_SetRotationValues values = (Effect_SetRotationValues) effect;
			if(values.changes.changeSpeed) rotationSpeed = values.newRotationSpeed;
		}
	}

	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType() == typeof(Effect_Default))
		{
			active = !active;
		}
		
		if(effect.GetType() == typeof(Effect_ActivateRotationGroup))
		{
			active = false;
		}

		if(effect.GetType() == typeof(Effect_SetRotationValues))
		{
			rotationSpeed = initialSpeed;
		}
	}
}
