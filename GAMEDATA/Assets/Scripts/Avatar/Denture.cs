using UnityEngine;
using System.Collections;

public class Denture : MonoBehaviour {
	private Avatar myParent;
	private CapsuleCollider myBar;
	
	void Awake(){
		myParent = GetComponentInParent<Avatar>() as Avatar;
	}
	
	void OnTriggerStay(Collider other){
		if(!myParent.OnBar && !myParent.IsFromBar && other.gameObject.tag == "Bar"){
			myParent.InTransit = true;
			if(myParent.MyOldBar != other.gameObject.GetComponentInParent<Bar>()){
				myParent.SetBar(other.gameObject.GetComponentInParent<Bar>());
			}
		}
	}
	public CapsuleCollider MyBar{
		get{return myBar;}
		set{myBar = value;}
	}
}