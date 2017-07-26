using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GizmoRapporteur : MonoBehaviour {

	public Transform r0;
	public Transform r1;

	public bool showRapporteur;
	public bool canShow;

	public float angle;

	// Update is called once per frame
	void Update () {

		Vector3 temp = r0.position;
		temp.y = transform.position.y;
		r0.position = temp;

		temp = r1.position;
		temp.y = transform.position.y;
		r1.position = temp;

	}

	public void CalculAngle(){

		angle = Vector3.Angle((r0.position - transform.position), (r1.position - transform.position));

	}

	void OnDrawGizmos(){
		if(canShow){
			Gizmos.color = Color.black;
			Gizmos.DrawLine(transform.position, r0.position);
			Gizmos.DrawLine(transform.position, r1.position);
			Gizmos.DrawLine(r0.position, r1.position);

			Gizmos.DrawSphere(r0.position, 0.1f);
			Gizmos.DrawSphere(r1.position, 0.1f);
		}
	}
}
