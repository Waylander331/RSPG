using UnityEngine;
using System.Collections;

public class Fist : MonoBehaviour, ITriggerable {

	//private Avatar avatar;

	public float range;
	public float launchSpeed;
	public float rewindSpeed;
	public float launchDelay;
	public float rewindDelay;

	private bool isLaunching = false;
	private bool isRewinding = false;
	private bool isAtMax = false;
	private bool isAtMin = true;

	//private bool canLaunchTrap = true;

	private Vector3 positionInitiale;
	private Vector3 positionFinale;

	public enum Surface{
		wall,
		floor
	}

	public Surface surface;

	public FistTrap trap;

	// Use this for initialization
	void Start () {
		positionInitiale = transform.position;
		positionFinale = transform.position + transform.up * range;
	}
	
	// Update is called once per frame
	void Update () {
		if(isLaunching && !isAtMax){
			if(Vector3.Distance(positionFinale, transform.position + transform.up * launchSpeed * Time.deltaTime) < launchSpeed * Time.deltaTime){
				isAtMax = true;
				transform.position = positionFinale;
			}
			else this.transform.Translate(transform.up * launchSpeed * Time.deltaTime, Space.World);
		}
		if(isAtMax){
			Invoke ("Rewind", rewindDelay);
		}
		if(isRewinding && !isAtMin){
			if(Vector3.Distance(positionInitiale, transform.position - transform.up * rewindSpeed * Time.deltaTime) < rewindSpeed * Time.deltaTime){
				isAtMin = true;
				transform.position = positionInitiale;
			}
			else this.transform.Translate(-transform.up * rewindSpeed * Time.deltaTime, Space.World);
		}
	}

	/*void KnockNDaze(){
		GameObject temp = GameObject.FindGameObjectWithTag("Player");
		avatar = temp.GetComponent<Avatar>();

		switch (surface){
		case Surface.wall:
			//avatar.KnockSide();
			break;
		case Surface.floor:
			//avatar.KnockBack();
			break;
		}

		//avatar.Daze ();
	}*/

	void OnCollisionEnter(Collision other){
		if(other.collider.tag == "Player"){
			//KnockNDaze();
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere(positionFinale, 0.20f);
	}

	void OnDrawGizmosSelected(){
		for(int i = 0; i < trap.nextFist.Length; i++){
			if(trap.nextFist[i] != null){
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (transform.position,trap.nextFist[i].transform.position);
			}
		}
	}

	public void Triggered(EffectList effect){
		if(effect.GetType () == typeof(Effect_Default)){
			if(isAtMin){
				Invoke ("Launch", launchDelay);
			}
		}
	}

	public void UnTriggered(EffectList effect){
		if(effect.GetType () == typeof(Effect_Default)){
			CancelInvoke("Launch");
			for(int i = 0; i < trap.nextFist.Length; i++){
				if(trap.nextFist[i] != null){
					trap.nextFist[i].CancelInvoke("Launch");
				}
			}
		}
	}

	/*void RepeatLaunch(){
		if(trap.nextFist.Length != null){
			for(int i = 0; i < trap.nextFist.Length; i++){
				//trap.nextFist[i].Invoke("Launch", trap.delayBetweenFist);
				if(trap.nextFist[i].isAtMin == false){
					trap.nextFist[i].CancelInvoke("Launch");
				}
				else {
					if(trap.nextFist[i].IsInvoking("Launch")){
						trap.nextFist[i].CancelInvoke("Launch");
						trap.nextFist[i].Invoke("Launch", trap.delayBetweenFist);
					}
					else trap.nextFist[i].Invoke("Launch", trap.delayBetweenFist);
				}
			}			
		}
	}*/

	void Launch(){
		isAtMin = false;
		isLaunching = true;
		isRewinding = false;

		if(trap.nextFist.Length != 0){
			for(int i = 0; i < trap.nextFist.Length; i++){
				trap.nextFist[i].Invoke("Launch", trap.delayBetweenFist);		
			}
		}
	}
	
	void Rewind(){
		isAtMax = false;
		isLaunching = false;
		isRewinding = true;
	}
}

[System.Serializable]
public class FistTrap{
	public Fist[] nextFist;
	public float delayBetweenFist;
}
