using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GizmoRegle : MonoBehaviour {

	public Transform g0;
	public Transform g1;
	public Transform g2;
	public Transform g3;
	public float width = 10;
	public float height = 5;

	void Update(){
		width = Mathf.Round(width*4f)/4f;
		height = Mathf.Round(height*4f)/4f;

		g1.position = transform.position + transform.right * width;

		g2.position = g1.position + transform.up * height;

		g3.position = transform.position + transform.up * height;

		g0.position = transform.position;

	}

	void OnDrawGizmos(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine(g0.position, g1.position);
		Gizmos.DrawLine(g1.position, g2.position);
		Gizmos.DrawLine(g2.position, g3.position);
		Gizmos.DrawLine(g3.position, g0.position);
	}
}
