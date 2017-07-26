using UnityEngine;
using System.Collections;

public class Effect_SetRotationValues : EffectList 
{
	[System.Serializable]
	public class ApplyOn
	{
		public bool
			changeSpeed,
			changeRotationDegree,
			changeAxis,
			changeDelay,
			changeLoop;
	}

	public ApplyOn changes;
	public float newRotationSpeed;
	public float newRotationDegree;
	public RotationAxis newAxis;
	public float newDelay;
	public bool newLoop;

	public enum RotationAxis
	{
		X_Axis,
		Y_Axis,
		Z_Axis
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


