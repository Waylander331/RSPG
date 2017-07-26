using UnityEngine;
using System.Collections;

public class Effect_Rotate : EffectList 
{
	public float angle;
	public float rotationSpeed;
	public float delay;
	public RotationAxis rotationAxis;

	public enum RotationAxis
	{
		x_Axis,
		y_Axis,
		z_Axis
	}

	public override void Activate (IsTriggerable triggeredObject)
	{
		triggeredObject.Activate (this);
	}
	
	public override void Deactivate (IsTriggerable triggeredObject)
	{
		triggeredObject.Deactivate (this);
	}
}
