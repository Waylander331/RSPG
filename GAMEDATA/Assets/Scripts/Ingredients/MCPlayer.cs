using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class MCPlayer : MonoBehaviour, ITriggerable {

	private Effect_MC effect;
	private AudioSource player;
	private Presentateur presentateur;
	public Camera cam;
	private ScreenOverlay overlay;

	private bool isMoving;
	private bool isButtonPressing;

	void Start(){

		isMoving = false;
		isButtonPressing = false;

		player = this.GetComponent<AudioSource>();
		presentateur = GetComponent<Presentateur>();
		presentateur.Player = this;

		overlay = cam.GetComponent<ScreenOverlay>();
		Invoke ("TvOff", 0.05f);
	}

	void OnLevelWasLoaded(){
		if (player == null) player = this.GetComponent<AudioSource>();
		if (presentateur == null) presentateur = GetComponent<Presentateur>();
		presentateur.Player = this;

		/*if (overlay == null)*/ overlay = cam.GetComponent<ScreenOverlay>();
		overlay.enabled = false; 
		cam.enabled = true; //doivent avoir ces settings une fraction de seconde pour que le overlay se fasse bien

		//Invoke ("TvOff", 0.05f);
	}

	void Update(){
		if(player.isPlaying){
			if (!isButtonPressing)
				ShowAnim();
			if(IsInvoking("TvOff")) CancelInvoke("TvOff");
		}
		else if (!isMoving && !isButtonPressing){
			presentateur.Defaults();
		}
		if(!player.isPlaying && effect != null && !IsInvoking("TvOff") && cam.enabled == true) Invoke ("TvOff", 1f);
	}

	public void Triggered(EffectList eff){
		if(eff.GetType() == typeof(Effect_MC)){

			if(!IsInvoking("SayLine") && !player.isPlaying){
				effect = eff.GetComponent<Effect_MC>();
				TvOn();
				Invoke ("SayLine", 0.5f);
			}
			else if (player.isPlaying && eff.GetComponent<Effect_MC>().line != effect.line){
				CancelInvoke ("TvOff");
				CancelInvoke ("TvDisable");
				TvOn (); 

				effect = eff.GetComponent<Effect_MC>();
				Invoke ("SayLine", player.clip.length);
			}
		}
	}

	public void UnTriggered(EffectList effect){		

	}

	public void SayLine(){
		player.clip = effect.line;
		player.Play();

		if(player.clip != null && player.clip.name == "BossStart"){
			presentateur.Animateur.SetBool("buttonPress", true);

			Invoke ("BossButton", 0.2f);
		}
	}

	void BossButton(){
		isButtonPressing = false;
		presentateur.Animateur.SetBool("buttonPress", false);
	}

	public void ShowAnim(){
		presentateur.Animateur.SetBool("neutre", effect.Neutre);
		presentateur.Animateur.SetBool("blase", effect.Blase);
		presentateur.Animateur.SetBool("colerique", effect.Colerique);
		presentateur.Animateur.SetBool("enjoue", effect.Enjoue);
		presentateur.Animateur.SetBool("impressionne", effect.Impressionne);
		presentateur.Animateur.SetBool("inquiet", effect.Inquiet);
		presentateur.Animateur.SetBool("nargueur", effect.Nargueur);
		presentateur.Animateur.SetBool("panique", effect.Panique);
		presentateur.Animateur.SetBool("BossFight", effect.bossFight);
	}

	public void TvOn(){
		if (IsInvoking("TvOff")) CancelInvoke("TvOff");
		if (!cam.enabled) cam.enabled = true;
		if (overlay.enabled) overlay.enabled = false;
	}

	public void TvOff(){
		if(Manager.Instance.LevelName != "0_HUB_YL") {
			overlay.enabled = true;
			Invoke ("TvDisable", 0.1f);
		}
		else{
			overlay.enabled = false;
			cam.enabled = true;
		}
	}

	void TvDisable(){
		cam.enabled = false;
	}

	public bool IsMoving{
		get {return isMoving;}
		set {isMoving = value;}
	}
}
