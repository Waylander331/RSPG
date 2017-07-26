using UnityEngine;
using System.Collections;

public class NonCollisionDetec : MonoBehaviour {
	public GameObject[] myObjects;
//	public bool checkForCollision;
//	public bool checkForWallJumpable;
	// Use this for initialization
	void Start () {
		
		myObjects = (GameObject[]) GameObject.FindObjectsOfType (typeof(GameObject));
		//if(checkForCollision){
			foreach (GameObject go in myObjects) {

				if(go.layer != 0){
					if(go.GetComponent<MeshRenderer>() != null){
						go.GetComponent<MeshRenderer>().enabled = false;
					}
					if(go.GetComponent<SkinnedMeshRenderer>() != null){
						go.GetComponent<SkinnedMeshRenderer>().enabled = false;
					}
				}



			}
		//}



		
	}
	
	
}
