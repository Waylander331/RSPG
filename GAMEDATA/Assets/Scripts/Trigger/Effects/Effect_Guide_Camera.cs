using UnityEngine;
using System.Collections;

public class Effect_Guide_Camera : EffectList {
	void OnDrawGizmos() {
		DrawShape.DrawArrowForGizmo (this.transform.position, (this.transform.forward)*1f, Color.blue,0.5f,20f);
		
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
