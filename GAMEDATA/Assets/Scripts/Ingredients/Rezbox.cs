using UnityEngine;
using System.Collections;

public class Rezbox : MonoBehaviour {

	private Rigidbody myBod;
	private Animator myMoves;
	private bool oneTime;
	private RezboxSFX sfx;

	// Use this for initialization
	void Start () {
		oneTime = true;
		myBod = GetComponent<Rigidbody>();
		myMoves = GetComponentInChildren<Animator>();
		Manager.Instance.Avatar.OnPause = true;
		Manager.Instance.Avatar.Disparition();
	}
	
	// Update is called once per frame
	void Update () {
		if(myBod.velocity.y <= 0.1f && oneTime){
			Invoke ("Shake",1f);
			oneTime = false;
		}
	}
	public void Destruction(){
		GetComponent<BoxCollider>().enabled = false;
		Manager.Instance.Avatar.OnPause = false;
		Manager.Instance.Avatar.Apparition();
		sfx.ActivateEffect ();
		SoundManager.Instance.PlayAudio("RezBox2");
		Destroy (this.gameObject);
	}
	public void Shake(){
		if(myMoves != null){
			myMoves.Play ("shake");
			SoundManager.Instance.PlayAudio("RezBox1");
		}
		else{
			if(GetComponentInChildren<Animator>() != null){
				myMoves = GetComponentInChildren<Animator>();
				Shake ();
			}
		}
	}

	public RezboxSFX SFX
	{
		set{sfx = value;}
	}
}
