using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class spawnpoint : MonoBehaviour {

	public int Id;
	//private Manager myMag;

	void Awake(){
		//Setup(); 
	}

	void OnLevelWasLoaded(int level){
		//Setup ();
	}

	void Update(){
		this.gameObject.name = "SpawnPoint_" + Id;
	}

	/*
	void Setup(){
		if (Manager.Instance != null) {
			if(Manager.Instance.SpawnPoints.Count == 0 || Manager.Instance.SpawnPoints[0] == null){
				Manager.Instance.SpawnPoints.Add (null);
				Manager.Instance.SpawnPoints.Add (null);
				Manager.Instance.SpawnPoints.Add (null);
				Manager.Instance.SpawnPoints.Add (null);
				Manager.Instance.SpawnPoints.Add (null);
				Manager.Instance.SpawnPoints.Add (null);
			}
			Manager.Instance.SpawnPoints.RemoveAt(Id);
			Manager.Instance.SpawnPoints.Insert(Id, this.transform);
		}
	}*/

}
