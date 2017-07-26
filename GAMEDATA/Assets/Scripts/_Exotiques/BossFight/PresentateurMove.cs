using UnityEngine;
using System.Collections;

public class PresentateurMove : MonoBehaviour {

	private Vector3 destination;
	private Presentateur pres;
	private bool animValue;

	public void PanRight (Presentateur presentateur, Animator anim){
		animValue = true;
		pres = presentateur;

		anim.SetBool ("goLeft", true);
		anim.SetBool ("neutre", false);

		destination = pres.transform.right * -4.28f + this.transform.position;
		StartCoroutine("Move");
	}

	public void PanLeft (Presentateur presentateur, Animator anim){

		animValue = true;
		pres = presentateur;

		anim.SetBool ("goRight", true);
		anim.SetBool ("neutre", false);

		destination = pres.transform.right * 2.14f + this.transform.position;
		StartCoroutine("Move");
	}

	IEnumerator Move(){
		while (animValue){
			if (Vector3.Distance(this.transform.position, destination) < 0.5f){
				this.transform.position = destination;

				animValue = false;
			}
			else{
				this.transform.position = Vector3.Lerp (this.transform.position, destination, 5f * Time.deltaTime);
			}
			yield return null;
		}
		pres.Player.IsMoving = false;
	}
}
