using UnityEngine;
using System.Collections;

public class Character_Cont : MonoBehaviour, ITriggerable {

	private Avatar myAvatar;
	// POUR ENLEVER UN BUG AVEC L'INTERFACE ITRIGGERABLE
	private EffectList effect;
	
	void Start () {
		Invoke ("GetAvatar", 0.1f);
	}

	void GetAvatar(){
		myAvatar = Manager.Instance.Avatar;
	}
	
	public void Triggered(EffectList effect){

		if(effect.GetType() == typeof(Effect_Change_Level)){		
			if(Manager.Instance.CanLoad) {

				if(Manager.Instance.LevelName == "0_TUTO_P3_YL" && effect.GetComponent<Effect_Change_Level>().levelId == Effect_Change_Level.enum1.Hub_YL) {
					Manager.Instance.TutorialDone = true;
}
				Manager.Instance.LevelTransit((int)effect.GetComponent<Effect_Change_Level>().levelId, effect.GetComponent<Effect_Change_Level>().SpawnPointId);
			}
		}

		if(effect.GetType() == typeof(Effect_toDaze)){
			Manager.Instance.Avatar.Daze(); 
		}

		if(effect.GetType () == typeof(Effect_ChangeInBackstageState))
		{
			Manager.Instance.SetInBackstage(true);
		}
	}
	
	public void UnTriggered(EffectList effect){
		if(effect.GetType() == typeof(Effect_Change_Level)){
			Manager.Instance.LoadingScreen = false;
			Manager.Instance.IsFromStart = false;
		}

		if(effect.GetType () == typeof(Effect_ChangeInBackstageState))
		{
			Manager.Instance.SetInBackstage(false);
		}
	}
	
	
}
