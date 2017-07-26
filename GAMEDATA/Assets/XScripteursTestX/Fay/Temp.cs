using UnityEngine;
using System.Collections;

public class Temp : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		float rotation = 100 * Time.deltaTime;
		this.transform.Rotate(0, rotation, 0,Space.World);
	}
}
