using UnityEngine;
using System.Collections;

public class Effect_Change_Level : EffectList 
{

	public enum enum1 {Menu = 1, Hub_YL = 2, Manege_MAG = 3, FeteForaine_JLC = 4, Carrousel_PD = 5, Chucky_PC = 6, FreakShow_Jlem = 7, Tuto = 8, BossFight_GDT = 9};

	public enum1 levelId;
	private int spawnPointId;

	void Start()
	{
		if(Manager.Instance.GameLevel >= 0 && Manager.Instance.GameLevel <= 7)
			spawnPointId = Manager.Instance.GameLevel;
	}

	public override void Activate (IsTriggerable triggeredObject)
	{
		triggeredObject.Activate (this);
	}
	
	public override void Deactivate (IsTriggerable triggeredObject)
	{
		triggeredObject.Deactivate (this);
	}

	public int SpawnPointId
	{
		get{return spawnPointId;}
	}
}
