using UnityEngine;
using System.Collections;

public class Curtains : MonoBehaviour {

	private Cloth myCloth;

	// Use this for initialization
	void Start () {
		CapsuleCollider[] foo = new CapsuleCollider[1];
		foo[0] = Manager.Instance.Avatar.myClothCollider;
		myCloth = GetComponent<Cloth>();
		myCloth.capsuleColliders = foo;
	}
	void OnBecameInvisible(){
		if(myCloth != null){
			myCloth.enabled = false;
		}
	}
	void OnBecameVisible(){
		if(myCloth != null){
			myCloth.enabled = true;
		}
	}
}
