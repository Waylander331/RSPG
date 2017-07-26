using UnityEngine;
using System.Collections;

public class scr_textureAnim : MonoBehaviour {
	public Material[] materiauxAAnimer;
	public float vitesseAnimation;
	private int nbMat;
	private int index;
	private Renderer myRend;
	// Use this for initialization
	void Start () {
		nbMat = materiauxAAnimer.Length; // sera une constante, alors on la calcule qu'une fois
		myRend = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		index = (int)(Time.time*vitesseAnimation);
		
		index %=nbMat; //on veut pas depasser les limites du tableau
		
		myRend.material = materiauxAAnimer[index];
		
	}
}
