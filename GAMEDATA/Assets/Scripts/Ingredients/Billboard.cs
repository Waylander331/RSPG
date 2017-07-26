using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour {

	private Transform myMaster;

	void Update () {
		this.transform.LookAt(Camera.main.transform);
		this.transform.position = myMaster.position;
	}

	public Transform MyMaster {
		get {
			return myMaster;
		}
		set {
			myMaster = value;
		}
	}
}
