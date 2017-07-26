using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class TempAnn : MonoBehaviour {
	public Vector3 offset = Vector3.one;
	public Transform zeX;
	public Transform zeY;
	public Transform zeZ;
	
	// Update is called once per frame
	void Update () {
		zeX.position = this.transform.position + this.transform.right * offset.x;
		zeY.position = this.transform.position + this.transform.up * offset.y;
		zeZ.position = this.transform.position + this.transform.forward* offset.z;
	}
}
