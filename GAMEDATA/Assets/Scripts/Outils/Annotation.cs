using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Annotation : MonoBehaviour {

	void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, 0.25f);
	}
}
