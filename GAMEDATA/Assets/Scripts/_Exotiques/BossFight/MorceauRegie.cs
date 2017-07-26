using UnityEngine;
using System.Collections;

public class MorceauRegie : MonoBehaviour {

	public void DestructionSound(){
		SoundManager.Instance.PlayAudio ("BFDropMic");
		SoundManager.Instance.PlayAudio ("BFCamShutDown");
		SoundManager.Instance.PlayAudio ("BossWin");
	}
}
