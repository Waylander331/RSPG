using UnityEngine;
using System.Collections;

public class Scr_Morceau_Poney2 : MonoBehaviour 

{
	
	//*****DÉCLARATION VARIABLES
	private Rigidbody myBody;
	
	// Use this for initialization
	void Start () 
	{
		//*****VA DONNER UNE FORCE D'IMPULSION INITIALE AU RIGIDBODY
		myBody = this.GetComponent <Rigidbody>();
		myBody.AddForce (-this.transform.up*1500f); //CHOIX DISPONIBLES = forward, up, right (ou inverser avec négatif)
		myBody.AddForce (this.transform.forward*500f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
