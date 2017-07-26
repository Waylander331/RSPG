using UnityEngine;
using System.Collections;

public class BarFoo : MonoBehaviour {

	private Vector3 targetFoo;

	// Use this for initialization
	void Start () {
		targetFoo = new Vector3(this.transform.position.x,this.transform.position.y -1.6f,this.transform.position.z);
	}
	public Vector3 TargetFoo {
		get {
			return targetFoo;
		}
		set {
			targetFoo = value;
		}
	}
}
