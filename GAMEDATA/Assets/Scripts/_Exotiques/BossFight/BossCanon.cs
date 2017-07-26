using UnityEngine;
using System.Collections;

public class BossCanon : TomatoSpawn, ITriggerable {

	public float callTime;
	private bool isUnTriggered;
	private int trigIndex;
	public static int counter;

	void Start () {
		isUnTriggered = true;
		player = Manager.Instance.Avatar;
		spawnAt = transform.GetChild(0);
		if(!Manager.Instance.IsRespawning)counter = 0;
		PoolTomatoes();
	}

	void Update(){
		if(Input.GetMouseButtonDown(2)) AltShot();
	}

	void OnLevelWasLoaded(){

		Invoke ("WaitForManager", 0.001f);
	}

	void WaitForManager(){
		player = Manager.Instance.Avatar;
		spawnAt = transform.GetChild(0);
		
		PoolTomatoes();
	}

	public void Triggered(EffectList effect){
		if(isUnTriggered) AltShot();//Invoke ("AltShot", callTime);
		isUnTriggered = false;
	}

	public void UnTriggered(EffectList effect){
	}

	void AltShot(){

		ShootTomato (BossManager.Instance.Colonnes[counter].transform);
		BossManager.Instance.Colonnes[counter].Invoke("Break", callTime); 
		counter ++;

		SoundManager.Instance.BFShakeRandom();
		BossManager.Instance.TriggedCanon[trigIndex] = true;
		Invoke("Disable", 1f);
	}

	void Disable(){
		this.enabled = false;
		//this.GetComponent<CanonLookAt>().enabled = false;
	}

	public int TriggerIndex{
		get {return trigIndex;}
		set {trigIndex = value;}
	}
	public bool IsUnTriggered{
		get {return isUnTriggered;}
		set {isUnTriggered = true;}
	}

}
