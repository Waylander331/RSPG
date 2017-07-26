using UnityEngine;
using System.Collections;

[System.Serializable]
public class Effect_MC : EffectList {

	#region Vars et Enums
	public enum Level{
		Tuto, Hub, Manege, FeteForaine, Carrousel, Chucky, FreakShow
	};
	public enum Replique{
		Mort1, Mort2, Mort3, Danger1, Danger2, Danger3, Danger4, Etoile1, Etoile2, Etoile3, Etoile4, Etoile5, Bar1, Bar2, Roller1, Roller2, Brutus1, Brutus2, Bridgeman1, Bridgeman2, Switch, Backstage1, Backstage2, Backstage3, Backstage4, Reussite1, Reussite2, Reussite3, Reussite4, Reussite5
	};
	public enum Tuto{
		Entree, TutoP1, TutoP2, TutoP3, TutoFini
	};
	public enum Hub{
		 FFEtoiles, CanBoss, BossStart, Boss1, Boss2, Boss3, Boss4, BossWin
	};
	public enum Manege{
		Quai, Etoile, SurManege1, SurManege2, Prouesse, OrigineEtoile
	};
	public enum FeteForaine{
		Intro, NoAccess, YesAccess, Ecran
	};
	public enum Carrousel{
		Intro, BackstagePF, EtoileTop, Timer, Cage, CageExit, BackstageArms, Pont
	};
	public enum Chucky{
		Intro, Bouche, Fesse, SousSol, PFStar, Generateur
	};
	public enum FreakShow{
		Intro, Cabinet, PiliersGauche, CheminDroite, Pics, ScieEtoile, EtoileBStageD, EtoileBStageG
	};
	
	public Level niveau; 
	public Tuto tutoLines;
	public Hub hubLines;
	public Manege manegeLines;
	public FeteForaine feteForaineLines;
	public Carrousel carrouselLines;
	public Chucky chuckyLines;
	public FreakShow freakShowLines;
	public Replique genericLine;

	public bool levelSpecific;

	public AudioClip line;

	public bool neutre = false;
	public bool blase = false;
	public bool colerique = false;
	public bool enjoue = false;
	public bool impressionne = false;
	public bool inquiet = false;
	public bool nargueur = false;
	public bool panique = false;
	public bool enjoueABlase = false;
	public bool bossFight = false;

	public bool isBackstage;
	public bool enterAndExit;

	public enum Condition{
		EtoileSpecifique, UneEtoile, ToutesEtoiles, EtoilesDeTelNiveau, UnCollectible, SwitchActive, BossPossible, ColonneDetruite
	};
	public bool hasCondition;
	public bool isTrue;
	public GameObject conditionalObject;
	public int conditionalInt;
	public Condition condition;

	private bool wasSaid;
	private bool conditionMet = false; 

	#endregion

/*	void Start(){
		DontDestroyOnLoad(this);
	}

	void Update(){
		if(!Manager.Instance.CanLoad && Manager.Instance.CurrentLevel != Manager.Instance.LevelToLoad)
			Destroy(this);
	}*/

	#region Condition Check
	bool CheckCondition(){

		if(hasCondition){
			switch (condition){
			case Condition.EtoileSpecifique:
				conditionMet = EtoileSpecifique (conditionalObject);
				break;
			case Condition.UneEtoile:
				conditionMet = UneEtoile();
				break;
			case Condition.ToutesEtoiles:
				conditionMet = ToutesEtoiles();
				break;
			case Condition.EtoilesDeTelNiveau:
				conditionMet = EtoilesDeTelNiveau (conditionalInt);
				break;
			case Condition.UnCollectible:
				conditionMet = UnCollectible (conditionalObject);
				break;
			case Condition.SwitchActive:
				conditionMet = SwitchActive (conditionalObject);
				break;
			/*case Condition.PontCarrousel:
				break;*/
			case Condition.BossPossible:
				conditionMet = BossPossible (conditionalObject);
				break;
			case Condition.ColonneDetruite:
				conditionMet = ColonneDetruite (conditionalObject, conditionalInt);
				break;
			}
		}
		else conditionMet = true;

		return conditionMet;
	}
	#endregion

	#region Condition Check Functions

	bool EtoileSpecifique(GameObject etoile){
		return etoile.GetComponentInChildren<Stars>().IsTaken;
	}
	bool UneEtoile(){
		return (Manager.Instance.StarBin[Manager.Instance.GameLevel] < 7);
	}
	bool ToutesEtoiles(){
		return (Manager.Instance.StarBin[Manager.Instance.GameLevel] == 0);
	}
	bool EtoilesDeTelNiveau(int niveau){
		return (Manager.Instance.StarBin[niveau] < 7);
	}
	bool UnCollectible(GameObject collectible){
		return collectible.GetComponentInChildren<Collectible>().IsTaken;
	}
	bool SwitchActive(GameObject trigger){
		return trigger.GetComponent<Switch>().Active;
	}
/*	bool PontCarrousel(){
	}*/
	bool BossPossible(GameObject bm){
		return bm.GetComponent<BossManager>().CanBoss;
	}
	bool ColonneDetruite(GameObject bm, int id){
		return bm.GetComponent<BossManager>().TriggedCanon[id];
	}

	#endregion

	#region Trigger Functions
	public override void Activate(IsTriggerable triggeredObject){
		if(!wasSaid && (!hasCondition  || (hasCondition && CheckCondition() == isTrue))) {
			triggeredObject.Activate (this);
			wasSaid = true;
		}
	}
	public override void Deactivate (IsTriggerable triggeredObject){
	}
	#endregion

	#region Accesseurs
	public AudioClip Line{
		get {return Line;}
	}
	public AudioClip Anim{
		get {return Anim;}
	}
	public bool Neutre{
		get {return neutre;}
	}
	public bool Blase{
		get {return blase;}
	}
	public bool Colerique{
		get {return colerique;}
	}
	public bool Enjoue{
		get {return enjoue;}
	}
	public bool Impressionne{
		get {return impressionne;}
	}
	public bool Inquiet{
		get {return inquiet;}
	}
	public bool Nargueur{
		get {return nargueur;}
	}
	public bool Panique{
		get {return panique;}
	}
	public bool EnjoueABlase{
		get {return enjoueABlase;}
	}
	#endregion
}
