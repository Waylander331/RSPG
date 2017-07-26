using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scr_Interface : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	//SI LA TOUCHE "ESCAPE" EST APPUYÉ, L'APPLICATION QUITTE
	if (Input.GetKey (KeyCode.Escape))
		{
			Application.Quit ();
		}


	}

	//SI LE BOUTON EST APPUYÉ, L'APPLICATION REDÉMARRE (FONCTION)
	public void ClickTest()
	{
		Debug.Log("Bouton Cliqué");
		Application.LoadLevel (Application.loadedLevelName);
	}

}
