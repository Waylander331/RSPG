using UnityEngine;
using System.Collections;

public class CheckPointGizmos : MonoBehaviour 
{
	private Color defaultColor = Color.yellow;
	private const float defaultAlpha = 0.25f;
	private const float onSelectedAlpha = 0.5f;

	void OnDrawGizmos()
	{
		Color color = defaultColor;
		color.a = defaultAlpha;
		
		Gizmos.color = color;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);

		color.a = 1f;
		Gizmos.color = color;
		Gizmos.DrawLine (transform.position, transform.parent.GetChild (1).position);
	}

	void OnDrawGizmosSelected()
	{
		Color color = defaultColor;
		color.a = onSelectedAlpha;
		
		Gizmos.color = color;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);

		color.a = 1f;
		Gizmos.color = color;
		Gizmos.DrawLine (transform.position, transform.parent.GetChild (1).position);
	}
}
