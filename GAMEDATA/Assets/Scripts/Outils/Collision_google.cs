using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Collision_google : MonoBehaviour {
	public GameObject[] myObjects;

	public GameObject cubeYellow;
	private List<CubeGizmo> myCubes = new List<CubeGizmo>();
	private List<BoxCollider> myBox = new List<BoxCollider>();

	// Use this for initialization
	void Start () {
		
		myObjects = (GameObject[]) GameObject.FindObjectsOfType (typeof(GameObject));

		foreach (GameObject go in myObjects) {
			if(go.transform.parent != null){
				go.transform.SetParent(null);
			}
			myBox.Clear();

			if(go.GetComponent<BoxCollider>() != null){

				foreach(BoxCollider box in go.GetComponents<BoxCollider>()){
					if(box.isTrigger == false){
						myBox.Add(box);
					}
				}
			}
					
			foreach(BoxCollider tempBox in myBox){
				Vector3 sTemp;
				Vector3 pTemp;
				sTemp.x = tempBox.GetComponent<BoxCollider>().size.x * tempBox.transform.localScale.x;
				sTemp.y = tempBox.GetComponent<BoxCollider>().size.y * tempBox.transform.localScale.y;
				sTemp.z = tempBox.GetComponent<BoxCollider>().size.z * tempBox.transform.localScale.z;

				pTemp.x = tempBox.GetComponent<BoxCollider>().center.x * tempBox.transform.localScale.x;
				pTemp.y = tempBox.GetComponent<BoxCollider>().center.y * tempBox.transform.localScale.y;
				pTemp.z = tempBox.GetComponent<BoxCollider>().center.z * tempBox.transform.localScale.z;

				myCubes.Add(new CubeGizmo( sTemp , tempBox.transform.position + (tempBox.transform.rotation * (pTemp - Vector3.zero)), tempBox.gameObject.transform.rotation));

			}

		
			if(go.GetComponent<Light>()==null){

				go.SetActive(false);
			}

		}

	

		foreach(CubeGizmo cube in myCubes){
			GameObject tempC = Instantiate(cubeYellow,cube.pos,cube.rot) as GameObject;
			tempC.transform.localScale = cube.size;
		}
	
		
		

		
	}


	
}
class CubeGizmo{
	public Vector3 size;
	public Vector3 pos;
	public Quaternion rot;
	
	public CubeGizmo(Vector3 s, Vector3 p, Quaternion r){
		this.size = s;
		this.pos = p;
		this.rot = r;
	}
}


