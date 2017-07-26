using UnityEngine;
using System.Collections;

//[System.Serializable]
public class DontDestroyOnReload : MonoBehaviour {

	//[SerializeField]
	private int myLevel;

	void Awake(){
		myLevel = -1;
	}

	void Start () {
		Invoke ("SetUp", 0.01f);
	}

	void OnLevelWasLoaded(){
		Invoke ("LoadUp", 0.001f);
	}

	void SetUp(){
		myLevel = Manager.Instance.CurrentLevel;
		DontDestroyOnLoad(this);
	}

	void LoadUp(){
		if(Manager.Instance.CurrentLevel == myLevel){
			DontDestroyOnReload[] foo = FindObjectsOfType<DontDestroyOnReload>();

			foreach (DontDestroyOnReload other in foo){
				if (other.transform.position == this.transform.position && other.name == this.name && other.MyLevel < 0){
					if (other.IsInvoking())other.CancelInvoke();
					Destroy (other.gameObject);
				}
			}
		}
		else if (myLevel > 0){
			Destroy (this.gameObject);
		}
	}

	public int MyLevel{
		get {return myLevel;}
	}
}
