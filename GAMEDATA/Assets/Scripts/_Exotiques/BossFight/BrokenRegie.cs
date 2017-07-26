using UnityEngine;
using System.Collections;

public class BrokenRegie : MonoBehaviour {

	void OnEnable(){
		Invoke ("CamOn", 0.2f);
		//TODO particules
	}

	void CamOn(){
		Manager.Instance.MainCam.FadeIn (3f);
	}

}
