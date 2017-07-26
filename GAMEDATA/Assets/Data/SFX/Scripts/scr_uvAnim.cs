using UnityEngine;
using System.Collections;

public class scr_uvAnim : MonoBehaviour {

	public int uvAnimationTileX = 5; //Here you can place the number of columns of your sheet. 
	                           //The above sheet has 24
	 
	public int uvAnimationTileY = 3; //Here you can place the number of rows of your sheet. 
	                          //The above sheet has 1
	public float framesPerSecond = 10.0f;
	
	public bool animNormale = false;
	
	private int nbFrames ;
	
	private Vector2 size;

	private Renderer myRend;

	void Start(){
		nbFrames = uvAnimationTileX * uvAnimationTileY;
		// Size of every tile
		size = new Vector2 (1.0f / (float)uvAnimationTileX, 1.0f /(float) uvAnimationTileY);
		myRend = GetComponent<Renderer>();
	}
	void Update () {
	 
		// Calculate index
		int index  = (int)(Time.time * framesPerSecond);
		// repeat when exhausting all frames
		index = index % nbFrames;
	 
		

	 
		// split into horizontal and vertical index
		int uIndex = index % uvAnimationTileX;
		int vIndex = index / uvAnimationTileX;
	 
		// build offset
		// v coordinate is the bottom of the image in opengl so we need to invert.
		Vector2 offset = new Vector2 ((float)uIndex * size.x, 1.0f - size.y - vIndex * size.y);
	 
		myRend.material.SetTextureOffset ("_MainTex", offset);
		myRend.material.SetTextureScale ("_MainTex", size);
		
		if(animNormale){
			myRend.material.SetTextureOffset ("_BumpMap", offset);
			myRend.material.SetTextureScale ("_BumpMap", size);			
		}
	}
}
