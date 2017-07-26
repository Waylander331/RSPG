using UnityEngine;
using System.Collections;

public class Effect_Set_Checkpoint : EffectList 
{

	void OnDrawGizmos() {
		DrawShape.DrawXForGizmo (this.transform.position, Color.green, 0.5f);	
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
