using UnityEngine;
using System.Collections;

public class BarWall : MonoBehaviour {

	//[HideInInspector]
	public EmptyCibleB cibleRed;
	//[HideInInspector]
	public EmptyCibleF cibleGreen;

	public bool isRetractable;

	void OnDrawGizmosSelected(){
		Gizmos.color=Color.green;
		Gizmos.DrawLine (this.transform.position,cibleGreen.transform.position);
		
		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position,cibleRed.transform.position);
	}
	
	
}
