using UnityEngine;
using System.Collections;

public class NonStaticDetect : MonoBehaviour {
	public GameObject[] myStatics;
	// Use this for initialization
	void Start () {

		myStatics = (GameObject[]) GameObject.FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in myStatics) {
			if(go.isStatic){
				if(go.GetComponent<MeshRenderer>() != null){
					go.GetComponent<MeshRenderer>().enabled = false;
				}
			}
		}

	}
	

}
