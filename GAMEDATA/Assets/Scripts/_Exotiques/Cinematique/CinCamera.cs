using UnityEngine;
using System.Collections;

public class CinCamera : MonoBehaviour {

	public Texture solidTexture;
	private Color guiColor;
	private float fade;
	private Transform dummy;
	private Transform group;

	void Start(){
		fade = -1.5f;
		//Fade ();
		guiColor = Color.black;

		dummy = this.transform.parent;
		group = dummy.transform.parent;
		group.GetComponent<CinDummies>().Cam = this;

		//ReParent();
	}

	void LateUpdate(){
		guiColor.a = Mathf.Lerp(guiColor.a, fade, 5*(Time.deltaTime*(1/Time.timeScale)));
		guiColor.a = Mathf.Clamp(guiColor.a,0f,1f);
	}

	void OnGUI(){
		GUI.color = guiColor;
		GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height), solidTexture);
	}

	public void Fade(){
		fade = -fade;
	}
	
	public void ReParent(){
		if(this.transform.parent == group){ 
			this.transform.SetParent (dummy);
		}
		else this.transform.parent = group;
	}

}
