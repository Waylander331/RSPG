using UnityEngine;
using System.Collections;

public class scr_DestroyPeluche : MonoBehaviour {
	public float thrust = 0.2f;
	public Rigidbody rb;
	public float speed = 6;
	void Start() {
		rb = GetComponent<Rigidbody>();
		Invoke ("DestroyPeluche", 10);
	}
	void Update() {
		rb.AddForce(transform.forward * thrust);
		transform.position += Vector3.forward * Time.deltaTime * speed;
	}
	void DestroyPeluche (){
		if (this.gameObject != null) {
			DestroyObject (this.gameObject);
		}
	}
}
