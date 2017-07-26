using UnityEngine;
using System.Collections;

public class Scr_Peluche : MonoBehaviour {
	public GameObject peluche;
	public float spawnTimer;
	// Use this for initialization
	void Start () {
		//peluche = GameObject.FindGameObjectWithTag ("Peluche");
		Invoke ("SpawnPeluche", spawnTimer);
	}
	
	// Update is called once per frame
	void Update () {

	}
	void SpawnPeluche (){
		Instantiate (peluche, this.transform.position, this.transform.rotation);
		Invoke ("SpawnPeluche", spawnTimer);
	}
}
