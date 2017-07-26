using UnityEngine;
using System.Collections;

public class Scr_Poney : MonoBehaviour 
{

	//*****DÉCLARATIONS VARIABLES*****
	private float vitesse = 0.0f;

	public Scr_Morceau_Poney1 morceau_poney1;
	public Scr_Morceau_Poney2 morceau_poney2;
	public Vector3 offset1 = Vector3.one;
	public Vector3 offset2 = Vector3.one;




	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{

		this.transform.Translate (-vitesse, 0, 0, Space.World);


		if (Input.GetKeyDown ("space"))
		{
		    Debug.Log("space key was pressed");
			vitesse = 0.5f;
		}
	}

	void OnTriggerEnter (Collider other) //QUAND L'OBJET ENTRE EN COLLISION AVEC UN AUTRE //"other" est une variable créé pour l'occasion
	{
		if (other.tag == "Colonne")
		{
			Debug.Log ("Le poney a touché la colonne");
			Instantiate (morceau_poney1,this.transform.position+offset1,morceau_poney1.transform.rotation);
			Instantiate (morceau_poney2,this.transform.position+offset2,morceau_poney2.transform.rotation);

			Destroy (this.gameObject);

		}
	}




}
