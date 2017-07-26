using UnityEngine;
using System.Collections;
using System.IO;

public class Stars : MonoBehaviour {

	//Liens Gameplay
	private Camera cam;
	private int myLevel;
	private Animator anim;
	private Renderer[] myRenderers;
	private Vector3 avPosition;
	public float animationStartDistance = 10;

	//Mes valeurs
	private bool isTaken = false; //Call Liam Neeson!

	// StarSFX
	private StarSFX starSFX;

	void Start(){
		if (anim == null) anim = this.GetComponent<Animator>();
		anim.speed = 0;

		myRenderers = GetComponentsInChildren<Renderer>();
	}

	void Update(){
		if(!isTaken){
			avPosition = Manager.Instance.Avatar.transform.position;
			float distance = Vector3.Distance(avPosition, this.transform.position);

			if(distance < animationStartDistance && distance >= 0.5f){
				anim.speed = 2f - (distance / animationStartDistance * 2f);
			}
			else if (distance <= 0.5f) anim.speed = 2f;
		}
	}
	
	void OnTriggerEnter(Collider other){

		if(other.tag == "Player" && !isTaken){

			starSFX.TriggerStarGetEffect();
			starSFX.TriggerStarConfettisEffect();

			Manager.Instance.IsHyped = true;
			Manager.Instance.IsHyped = true;

			SoundManager.Instance.PlayAudio ("MonkeyTake");

			anim.speed = 2.5f * (1f/0.75f);

			Manager.Instance.GotStar(this);

			if(Manager.Instance.GameLevel <= 5 && Manager.Instance.GameLevel > 0)
				StartCoroutine ("TakePic");

			Taken(false);
		}
	}


	/// <summary>
	/// Set l'etoile comme etant prise
	/// </summary>
	public void Taken(bool isImmediate){
		isTaken = true;
		if(!IsInvoking("DisableAnimator")){	
			if (isImmediate) Invoke ("DisableAnimator", 0.001f);
			else Invoke ("DisableAnimator", 2f);		
		}

		this.GetComponent<BoxCollider>().enabled = false;
	}


	void DisableAnimator(){

		if (anim == null) anim = this.GetComponent<Animator>();
		anim.enabled = false;

		/*if (myRenderers == null)*/ this.GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in myRenderers)
			rend.enabled = false;
	}

	/// <summary>
	/// Set l'etoile comme n'etant pas prise.
	/// </summary>
	public void ResetTaken(){
		isTaken = false;
		anim.enabled = true;

		foreach (Renderer rend in myRenderers)
			rend.enabled = false;

		this.GetComponent<BoxCollider>().enabled = true;
	}

	void Clang(){
		int temp = Random.Range (1,3);
		switch (temp){
		case 1:
			SoundManager.Instance.PlayAudioInMyself(this.gameObject, "collectibles_monkeynear1");
			break;
		case 2:
			SoundManager.Instance.PlayAudioInMyself(this.gameObject, "collectibles_monkeynear2");
			break;
		}
	}

	#region Photos
	IEnumerator TakePic(){
		yield return new WaitForEndOfFrame();

		cam.enabled = true;

		cam.Render();
		int height = cam.pixelHeight;
		int width = cam.pixelWidth;
		Rect screenSize = new Rect(0,0,width,height);

		Texture2D tex = new Texture2D (width,height, TextureFormat.RGB24, false);
		tex.ReadPixels (screenSize,0,0); 
		tex.Apply();
		Manager.Instance.AddScreenshot(tex, myLevel, this);

		cam.enabled = false;

	}
	#endregion

//ACCESSEURS
	public bool IsTaken{
		get {return isTaken;}
		set {isTaken = value;}
	}
	public int MyLevel{
		get {return myLevel;}
		set {myLevel = value;}
	}
	public Camera Cam{
		get {return cam;}
		set {cam = value;}
	}
	public StarSFX StarSFX
	{
		set{starSFX = value;}
	}
}
