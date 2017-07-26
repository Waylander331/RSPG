using UnityEngine;
using System.Collections;

public class SoundsPrefab : MonoBehaviour {

	void Awake(){
		DontDestroyOnLoad(this.gameObject);
	}
}
