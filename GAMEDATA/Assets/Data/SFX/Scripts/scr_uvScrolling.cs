using UnityEngine;
using System.Collections;

public class scr_uvScrolling : MonoBehaviour {

	public Vector2 scrollSpeed = new Vector2(10f,10f);
	
	public bool animNormale = false;
	
	//contient le decalage voulu
	private Vector2 uvOffset = Vector2.zero;
	private Renderer myRend;
	void LateUpdate () {
	 
	
	 	uvOffset += (scrollSpeed*Time.deltaTime);
		
		myRend.material.SetTextureOffset ("_MainTex", uvOffset);
		
		if(animNormale){
			myRend.material.SetTextureOffset ("_BumpMap", uvOffset);

		}
	}
}
