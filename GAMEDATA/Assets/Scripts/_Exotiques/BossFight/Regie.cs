using UnityEngine;
using System.Collections;

public class Regie : MonoBehaviour {

	MorceauRegie full;
	BrokenRegie broken;
	Animator anim;
	private ParticleSystem[] particles;

	private bool isBreaking;
	private Presentateur pres;

	void Start () {
		full = this.GetComponentInChildren<MorceauRegie>();
		broken = this.GetComponentInChildren<BrokenRegie>();
		anim = full.GetComponent<Animator>();
		pres = this.transform.parent.GetComponentInChildren<Presentateur>();

		anim.speed = 0;
		broken.gameObject.SetActive(false);

		particles = GetComponentsInChildren<ParticleSystem>();
		ParticleActive (false);
	}
	
	public void Break(){
		isBreaking = true;
		anim.speed = 1;
		ParticleActive(true);

		SoundManager.Instance.PlayAudio("BFFall");
		pres.Move();

		Invoke ("CamFlicker", 0.4f);
		Invoke ("ChangeObject", 1.5f);
	}

	void ParticleActive(bool active){
		foreach (ParticleSystem particle in particles){
			particle.gameObject.SetActive(active);
		}
	}

	void CamFlicker(){
		Manager.Instance.MainCam.Flicker (0.7f);
		Invoke ("CamOff", 0.7f);
	}

	void CamOff(){
		Manager.Instance.MainCam.FadeOut(3f);
	}

	void ChangeObject(){
		anim.speed = 0;
		isBreaking = false;
		full.gameObject.SetActive(false);
		broken.gameObject.SetActive(true);
		ParticleActive(false);
	}

	public bool IsBreaking{
		get {return isBreaking;}
		set {isBreaking = value;}
	}
}
