using UnityEngine;
using System.Collections;
using System.IO;

public class Collectible : MonoBehaviour {
	
	private Manager manager;
	//private MenuPause pause;
	private int myLevel;

	private float rotation = 100f;
	
	private bool isTaken = false;

	/*void Start (){
		pause = GameObject.Find ("PauseMenu").GetComponent<MenuPause>();
	}*/

	void Update(){
		transform.Rotate(0, rotation * Time.deltaTime, 0, Space.World);
	}
	
	
	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			Taken ();
			/*pause.transform.GetChild(0).gameObject.SetActive(true);
			pause.collectable[0].SetActive(false);
			pause.collectable[1].SetActive(true);
			Invoke ("StopShowing", 2);*/
			SoundManager.Instance.PlayAudio("CollectibleTake");
			Manager.Instance.GotCollectible(this);
			/*pause.transform.GetChild(0).gameObject.SetActive(true);
			pause.collectable[0].SetActive(false);
			pause.collectable[1].SetActive(true);
			Invoke ("StopShowing", 2);*/
		}
	}
	
	public void Taken(){
		isTaken = true;
		//pause.b ++;
		this.GetComponent<SphereCollider>().enabled = false;
		if(GetComponentInChildren<MeshRenderer>() != null){
			GetComponentInChildren<MeshRenderer>().enabled = false;
		}
		if(GetComponentInChildren<ParticleSystem>() != null){
			GetComponentInChildren<ParticleSystem>().Stop ();
		}
	}

	/*private void StopShowing(){
		pause.transform.GetChild(0).gameObject.SetActive(false);
		pause.collectable[0].SetActive(true);
		pause.collectable[1].SetActive(true);
	}
*/
	//Accesseurs
	public Manager Man{
		get{return manager;}
		set{manager = value;}
	}
	public int MyLevel{
		get {return myLevel;}
		set {myLevel = value;}
	}
	public bool IsTaken{
		get {return isTaken;}
		set {isTaken = value;}
	}
	
}