using UnityEngine;
using System.Collections;

public class Kid : MonoBehaviour {

	public MeshCollider myFunnel;
	public Knives[] myKnives; //Knife 0 = gauche, knife 1 = droite!!!

	public void Funnel(int on){
		if(on == 1){
			myFunnel.enabled = true;
		}
		else{
			myFunnel.enabled = false;
		}
	}

	public void KnifeDown(){
		//SoundManager.Instance.PlayAudio("KidKnifeDown");
		SoundManager.Instance.PlayAudioInMyself(this.gameObject,"KidKnifedown");
	}
	public void KnifeDown2(){
		SoundManager.Instance.PlayAudioInMyself(this.gameObject,"KidKnifedown2");
	}


	public void Closing(){
		SoundManager.Instance.PlayAudioInMyself(this.gameObject,"KidRotationDown_close");
	}
	public void Opening(){
		SoundManager.Instance.PlayAudioInMyself(this.gameObject,"KidRotationDown_open");
	}

	public void KnifeInMotion(int index){
		myKnives[index - 1].tag = "Lethal";
		myKnives[index - 1].GetComponent<BoxCollider>().isTrigger = true;
	}
	public void KnifeNoMotion(int index){
		myKnives[index - 1].tag = "Untagged";
		myKnives[index - 1].GetComponent<BoxCollider>().isTrigger = false;
	}

}
