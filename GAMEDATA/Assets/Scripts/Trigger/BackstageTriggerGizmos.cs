using UnityEngine;
using System.Collections;

public class BackstageTriggerGizmos : MonoBehaviour 
{
	private const float initPosOffset = 0.5f;
	private const float absoluteLength = 0.75f;

	void OnDrawGizmos()
	{
		Vector3 initPos = transform.position + transform.forward * transform.localScale.z * initPosOffset;
		DrawShape.DrawArrowForGizmo (initPos, transform.forward * absoluteLength, Color.blue);

		initPos = transform.position - transform.forward * transform.localScale.z * initPosOffset;
		DrawShape.DrawArrowForGizmo (initPos, -transform.forward * absoluteLength, Color.red);
	}
}
