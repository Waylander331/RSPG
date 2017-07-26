using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GizmoRegleFixe : MonoBehaviour {

	public Vector3 endPosition1;
	public Vector3 startTangent1;
	public Vector3 endTangent1;

	public Vector3 endPosition2;
	public Vector3 startTangent2;
	public Vector3 endTangent2;

	public float lenght = 4f;
	public float jumpHeight = 2.6f;

	public float lenght2 = 6.3f;
	public float jumpHeight2 = 2f;

	public bool showMeasur1 = false;
	public bool canShow = false;
	public bool showMeasur2 = false;
	public bool canShow2 = false;

	public int numberOfSubdivisions = 10;

	public void Update(){
		endPosition1 = transform.position + transform.right * lenght;
		startTangent1 = transform.position + transform.up * jumpHeight;
		endTangent1 = endPosition1 + transform.up * jumpHeight;

		endPosition2 = transform.position + transform.right * lenght2;
		startTangent2 = transform.position + transform.up * jumpHeight2;
		endTangent2 = endPosition2 + transform.up * jumpHeight2;
	}
	
	void OnDrawGizmos(){
		Gizmos.color = Color.blue;
		numberOfSubdivisions = Mathf.Max(1,numberOfSubdivisions);
		Vector3[] array = new Vector3[numberOfSubdivisions+1];
		if(canShow){
			for (int i=0;i<=numberOfSubdivisions;i++){
				float t = i/(float)numberOfSubdivisions;
				float omt = 1.0f-t; // One minus t = omt
				array[i] = transform.position*(omt*omt*omt) + startTangent1*(3*omt*omt*t) + endTangent1*(3*omt*t*t)+ endPosition1*(t*t*t);
				if (i>0){
					Gizmos.DrawLine(array[i-1],array[i]);
				}
			}
		}
		/*Gizmos.color = Color.blue;
		numberOfSubdivisions = Mathf.Max(1,numberOfSubdivisions);
		Vector3[] array2 = new Vector3[numberOfSubdivisions+1];*/
		if(canShow2){
			for (int i=0;i<=numberOfSubdivisions;i++){
				float t = i/(float)numberOfSubdivisions;
				float omt = 1.0f-t; // One minus t = omt
				array[i] = transform.position*(omt*omt*omt) + startTangent2*(3*omt*omt*t) + endTangent2*(3*omt*t*t)+ endPosition2*(t*t*t);
				if (i>0){
					Gizmos.DrawLine(array[i-1],array[i]);
				}
			}
		}
	}
}
