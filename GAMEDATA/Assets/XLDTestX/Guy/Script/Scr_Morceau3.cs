using UnityEngine;
using System.Collections;

public class Scr_Morceau3 : MonoBehaviour 
{

	//*****DÉCLARATION VARIABLES
	private Rigidbody myBody;
	
	// Use this for initialization
	void Start () 
	{
		//*****VA DONNER UNE FORCE D'IMPULSION INITIALE AU RIGIDBODY
		myBody = this.GetComponent <Rigidbody>();
		myBody.AddForce (this.transform.right*1500f); //CHOIX DISPONIBLES = forward, up, right (ou inverser avec négatif)
		myBody.AddForce (-this.transform.forward*500f);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

}
