using UnityEngine;
using System.Collections;

// Le test d'un effet (dérive de EffectList)
public class AnEffect : EffectList 
{
	// Test de variables
	// Par exemple, pour camShake, sera duree, vitesseShake, forceShake, etc.
	public float classFloat;
	public int classInt;

	public override void Activate(IsTriggerable triggeredObject)
	{
		//Debug.Log ("Activating " + triggeredObject.gameObject.name);
		triggeredObject.Activate (this);
		//Exemple pour camShake avec la Main Camera comme triggeredObject

		/*
		CamScript camScript = triggeredObject.GetComponent<CamScript>();
		if(camScript != null)
		{
			camScript.duree = classFloat;
			camScript.speed = classInt;
			etc...
			camScript.Activate();
		}
		// Dans Activate() : fait le camShake avec les parametres updatés
		*/

	}

	public override void Deactivate(IsTriggerable triggeredObject)
	{
		triggeredObject.Deactivate (this);
		//Debug.Log ("Deactivating " + triggeredObject.gameObject.name);
	}
}
