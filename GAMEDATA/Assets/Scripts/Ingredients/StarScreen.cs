using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class StarScreen : MonoBehaviour {

	private Manager man;

	public int levelID; 
	public int starsInLevel = 3;

	[Range(0f,20f)]
	public float transitionSpeed;
	//private int imgToShow;
	private int minRange;
	private int maxRange;
	private int index;
	
	public static Texture2D baseTexture;
	private Renderer myRenderer; 

	void Start(){
		man = GameObject.Find ("Manager").GetComponent<Manager>();

		myRenderer = this.GetComponent<Renderer>();
		baseTexture = myRenderer.material.mainTexture as Texture2D;
		Invoke("TextureSetup", 0.0001f); 

		//imgToShow = Random.Range(0,starsInLevel);
		InvokeRepeating("Slideshow", 0.002f, transitionSpeed);
	}

	void Slideshow(){
		index ++;
		if (index >= maxRange)
			index = minRange;
		myRenderer.material.SetTexture("_MainTex", man.Images[index]);
	}

	void TextureSetup(){
		minRange = (levelID - 1) * starsInLevel; 
		maxRange = levelID * starsInLevel;
		index = minRange;

		while(index < maxRange){
			if(man.Images[index] == null) man.Images[index] = baseTexture;
			index ++;
		}


	}

}

