using UnityEngine;
using System.Collections;

public class Scr_Colonne : MonoBehaviour 
{

	//*****DECLARATION VARIABLES*****
	private bool hasSmashedColonne = false; //Variable pour vérifier si la Colonne a été touchée par la Poney
	public Scr_Morceau1 morceau1;
	public Scr_Morceau2 morceau2;
	public Scr_Morceau3 morceau3;
	public Scr_Morceau4 morceau4;
	public GameObject explosion;
	public Vector3 offset1 = Vector3.one;
	public Vector3 offset2 = Vector3.one;
	public Vector3 offset3 = Vector3.one;
	public Vector3 offset4 = Vector3.one;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	void OnTriggerEnter (Collider other) //QUAND L'OBJET ENTRE EN COLLISION AVEC UN AUTRE //"other" est une variable créé pour l'occasion
	{
		if (other.tag == "Poney")
		{
			Debug.Log ("Le poney a touché la colonne");
			hasSmashedColonne = true;

			Instantiate (morceau1,this.transform.position+offset1,morceau1.transform.rotation);
			Instantiate (morceau2,this.transform.position+offset2,morceau2.transform.rotation);
			Instantiate (morceau3,this.transform.position+offset3,morceau3.transform.rotation);
			Instantiate (morceau4,this.transform.position+offset4,morceau4.transform.rotation);
			Instantiate (explosion, this.transform.position, Quaternion.identity);
			Destroy (this.gameObject);

			//OFFSET--- Y=HAUT/BAS, X=AVANT/ARRIERE, Z=DROIT/GAUCHE

			
		}
	}


	//*****ACCESSEURS POUR ENVOYER L'INFORMATION DANS UN AUTRE SCRIPT
	public bool HasSmashedColonne
	{
		get {return hasSmashedColonne;}
		set {hasSmashedColonne = value;}
	}



}
