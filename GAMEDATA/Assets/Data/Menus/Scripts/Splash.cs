using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Splash : MonoBehaviour {
	public float dureeFadeIn1 = 1f;
	public Sprite image1;
	public float dureeAffiche1;
	public float dureeFadeOut1 = 1f;

	public RawImage myMovie;

	private MovieTexture mt;

	private bool canPlay = true;
	private float incAlpha;
	private bool isImage1 = true;
	private bool isGoingForward = true;
	private Color tempColor = Color.white;
	private Image myImageRenderer;

	// Use this for initialization
	void Start () {
		myImageRenderer = this.GetComponent<Image>();

		incAlpha = (1.0f/dureeFadeIn1)*Time.deltaTime;
		tempColor.a = 0;
		myImageRenderer.sprite = image1;

		mt = (MovieTexture)myMovie.mainTexture;
		mt.Stop();
		mt.Play();
		mt.Stop();
		myMovie.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//FadeIn 01

		if(isImage1 && isGoingForward){

			if(myImageRenderer.color.a <1f){
				tempColor.a +=incAlpha;
				myImageRenderer.color = tempColor;

			}
			else {
				if(!IsInvoking("StandBy")){
					Invoke("StandBy",dureeAffiche1);
				}
			}
		}
		//FadeOut
		if(isImage1 && !isGoingForward){

			if(myImageRenderer.color.a >0f){
				tempColor.a -=incAlpha;
				myImageRenderer.color = tempColor;
			}
			else {
				ChangeImage();
			}
		}

		if(!isImage1 && isGoingForward){
			if(!mt.isPlaying && !canPlay){
				ChangeImage();
			} else {
				mt.Play ();
				canPlay = false;
			}
		}

		if(Input.GetButtonDown ("Fire1")){
			Application.LoadLevel("EcranTitre");
		}
	}


	void StandBy(){
		incAlpha = (1.0f/dureeFadeOut1)*Time.deltaTime;
		isGoingForward = false;

	}

	void ChangeImage(){
		if(isImage1){
			myMovie.enabled = true;
			isImage1 = false;
			isGoingForward = true;
		}
		else {
			Application.LoadLevel("EcranTitre");
		}

	}


}
