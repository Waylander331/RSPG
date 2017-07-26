using UnityEngine;
using System.Collections;

public class BossManager : MonoBehaviour {

	[Range(0,16)]
	public int starsForBossAccess;
	private bool canBoss; 

	public BossCanon[] canons;
	public Regie[] colonnes = new Regie[3];
	[Range(3,10)]
	public float transitionTime = 5f;

	private bool[] triggedCanon = new bool[3];

	private Presentateur pres;
	private Camera[] presCams;

	private bool started;

	private static BossManager instance;

	void Start () {

		EssenceMemeDuSingleton();

		if (instance == this){
			if(Manager.Instance.TotalStarsTaken >= starsForBossAccess) canBoss = true;
			else canBoss = false;

			started = true; 

			for(int i = 0; i <triggedCanon.Length; i++) {
				triggedCanon[i] = false;
				canons[i].TriggerIndex = i;
			}
		}
	}

	void Update(){

		//if(lift.CanRewind && !started) started = true;

		if(TrueCheck(triggedCanon)){
			Invoke ("TheEnd", transitionTime);
		}
	}

	bool TrueCheck (bool[] array){
		for(int i = 0; i < array.Length; i++){
			if (array[i] == false){ 
				return false;
			}
		}
		return true;
	}

	void TheEnd(){
		Application.LoadLevel("CINEMATIQUE_FIN");
	}

	void EssenceMemeDuSingleton(){
		
		if(BossManager.Instance == null){
			instance = this;
		}
		else{
			if(BossManager.Instance!=this){
				Destroy(this.gameObject);
			}			
		}
	}

	public bool[] TriggedCanon{
		get {return triggedCanon;}
		set {triggedCanon = value;}
	}
	public Regie[] Colonnes{
		get {return colonnes;}
	}
	public bool CanBoss{
		get {return canBoss;}
	}
	public bool Started{
		get {return started;}
	}
	public static BossManager Instance{
		get {return instance;}
	}

}
