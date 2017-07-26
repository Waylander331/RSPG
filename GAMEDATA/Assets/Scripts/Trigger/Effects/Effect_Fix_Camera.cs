using UnityEngine;
using System.Collections;

public class Effect_Fix_Camera : EffectList {
	public bool isLookAting;
	void OnDrawGizmos() {
		DrawShape.DrawArrowForGizmo (this.transform.position, (this.transform.forward)*1f, Color.blue,0.5f,20f);
		DrawShape.DrawXForGizmo (this.transform.position, Color.red, 0.5f);
		
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
