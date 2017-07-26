using UnityEngine;
using System.Collections;

public class TutoStar : Stars {

	private Animator anim;
	private Renderer[] myRenderers;
	private Vector3 avPosition;
	private bool isTaken = false; //Call Liam Neeson!
	
	// StarSFX
	private StarSFX starSFX;

	void Start(){
		if (anim == null) anim = this.GetComponent<Animator>();
		anim.speed = 0;
		
		myRenderers = GetComponentsInChildren<Renderer>();
	}

	void Update(){
		if(!isTaken){
			avPosition = Manager.Instance.Avatar.transform.position;
			float distance = Vector3.Distance(avPosition, this.transform.position);
			
			if(distance < animationStartDistance && distance >= 0.5f){
				anim.speed = 2f - (distance / animationStartDistance * 2f);
			}
			else if (distance <= 0.5f) anim.speed = 2f;
		}
	}

	void OnTriggerEnter(Collider other){
		
		if(other.tag == "Player" && !isTaken){

			if(starSFX == null) starSFX = GetComponentInChildren<StarSFX>(); 
			
			starSFX.TriggerStarGetEffect();
			starSFX.TriggerStarConfettisEffect();

			Manager.Instance.IsHyped = true;
			
			SoundManager.Instance.PlayAudio ("MonkeyTake");

			Taken();
		}
	}	
	
	/// <summary>
	/// Set l'etoile comme etant prise
	/// </summary>
	public void Taken(){
		anim.speed = 2.5f * (1f/0.75f);

		isTaken = true;
		Invoke ("DisableAnimator", 2f);
		
		this.GetComponent<BoxCollider>().enabled = false;
	}
	
	void DisableAnimator(){
		
		if (anim == null) anim = this.GetComponent<Animator>();
		anim.enabled = false;
		
		if (myRenderers == null) this.GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in myRenderers)
			rend.enabled = false;
	}
	
	//ACCESSEURS
	public bool IsTaken{
		get {return isTaken;}
		set {isTaken = value;}
	}
	public StarSFX StarSFX
	{
		set{starSFX = value;}
	}
}
