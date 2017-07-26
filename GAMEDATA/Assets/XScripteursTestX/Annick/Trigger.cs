using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {
	public IsTriggerable[] aTrigger;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		foreach(IsTriggerable temp in aTrigger){
			Gizmos.DrawLine(this.transform.position, temp.transform.position);
		}
		Color tempColor = Color.blue;
		tempColor.a = 0.75f;
		
		Gizmos.color = tempColor;
		Gizmos.DrawCube(this.transform.position,this.transform.localScale);
	}
	void OnDrawGizmos(){

		Color tempColor = Color.magenta;
		tempColor.a = 0.25f;

		Gizmos.color = tempColor;

		Gizmos.DrawCube(this.transform.position,this.transform.localScale);
	}

}
