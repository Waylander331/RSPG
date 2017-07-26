using UnityEngine;
using System.Collections;

public class Presentateur : MonoBehaviour {

	private Animator animator;
	private PresentateurMove move;
	private MCPlayer player;

	private bool isBoss;
	private bool isRight;
	private bool isLeft;

	void Start(){
		animator = this.GetComponent<Animator>();

		Invoke ("Init", 0.001f);
	}

	void OnLevelWasLoaded(){
		if (!isBoss) Invoke ("Init", 0.001f);
	}

	void Init(){
		if(Manager.Instance.LevelName == "0_HUB_GDT" || Manager.Instance.LevelName == "gymmi"){
			isBoss = true;
			isRight = false;
			isLeft = true;
			move = this.GetComponentInParent<PresentateurMove>(); 
		}
		else {
			isRight = false;
			isLeft = false;
			isBoss = false;
		}

		Defaults (); 
	}

	public void Move(){
		if (isLeft){ 

			move.PanRight (this, animator);
			isLeft = false;
			isRight = true;
			player.IsMoving = true;
		}
		else if (isRight){

			move.PanLeft (this, animator);
			isRight = false;
			isLeft = false;
			player.IsMoving = true;
		}
		else {
			animator.SetBool("panique", true);
			player.IsMoving = false;
		
			animator.SetBool ("neutre", false);
			Invoke ("Deactivate", 1f);
		}
	}

	void Deactivate(){
		move.gameObject.SetActive (false);
	}

	public void Defaults(){
		animator.SetBool("blase", false);
		animator.SetBool("colerique", false);
		animator.SetBool("enjoue", false);
		animator.SetBool("impressionne", false);
		animator.SetBool("inquiet", false);
		animator.SetBool("nargueur", false);
		animator.SetBool("panique", false);
		animator.SetBool("goLeft", false);
		animator.SetBool("goRight", false);
		Idles ();
	}

	void Idles(){
		if(isBoss && BossManager.Instance.Started) {
			animator.SetBool("neutre", true);
			animator.SetBool("BossFight", true);
		}
		else{ 
			animator.SetBool("neutre", true);
			animator.SetBool("BossFight", false);
		}
	}

	public Animator Animateur{
		get {return animator;}
	}
	public MCPlayer Player{
		get {return player;}
		set {player = value;}
	}
}
