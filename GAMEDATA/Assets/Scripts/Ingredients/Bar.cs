using UnityEngine;
using System.Collections;

public class Bar : MonoBehaviour {

	//[HideInInspector]
	public EmptyCibleB cibleRed;
	//[HideInInspector]
	public EmptyCibleF cibleGreen;
	//[HideInInspector]
	public Transform ancrage;
	//[HideInInspector]
	public Transform ancrage2;
	private float distance;

	[HideInInspector]
	public GameObject cables;
	private GameObject myCable;
	[HideInInspector]
	public int priority;

	[HideInInspector]
	public bool isOnWall;
	public bool isRetractable;

	void Start(){
		if(!isOnWall && ancrage != null && ancrage2 != null){
			Vector3 temp = new Vector3(this.transform.position.x,ancrage.position.y,this.transform.position.z);
			ancrage.position = temp;
			distance = ancrage.position.y - ancrage2.position.y;
			if(distance < 1.5f){
				Debug.LogError("La <<"+this.gameObject.name+">> a un point d'ancrage mal placé.  Il ne peut pas etre sous la barre et doit etre au moins a 2.5m de la barre.");
			}
			Vector3 foo = new Vector3(this.transform.position.x, ancrage2.position.y + (distance/2f),this.transform.position.z);
			myCable = Instantiate(cables,foo,Quaternion.identity) as GameObject;
			myCable.transform.forward = this.transform.forward;
			myCable.transform.localScale = new Vector3(1,distance * 1.5f,1);
			myCable.GetComponent<Renderer>().material.SetTextureScale("_MainTex", new Vector2(1f,1.5f * distance));
		}
	}

	void OnDrawGizmosSelected(){
		Gizmos.color=Color.green;
		Gizmos.DrawLine (this.transform.position,cibleGreen.transform.position);

		Gizmos.color = Color.red;
		Gizmos.DrawLine(this.transform.position,cibleRed.transform.position);
	}


}
