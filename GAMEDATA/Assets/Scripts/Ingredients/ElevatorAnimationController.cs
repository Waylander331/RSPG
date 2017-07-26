using UnityEngine;
using System.Collections;

public class ElevatorAnimationController : MonoBehaviour 
{
	private bool isBossElevator;
	public Effect_Change_Level.enum1 levelToLoad;
	private ElevatorGearAnimation gearAnimationScript;

	private ElevatorDoorAnimation doorAnimationScript;
	private ElevatorTrigger doorTrigger;
	private ElevatorTrigger insideTrigger;
	
	private bool levelWasLoaded = true;

	private camEffect camEffectScript;
	private Effect_Fix_Camera fixCamEffect;
	private bool isLoading;
	private bool inScriptedEvent;


	private Character_Cont character_ContScript;
	private Effect_Change_Level changeLevelEffect;

	// Delays
	private float delayAfterLoad = 1.5f;
	private float delayForScriptedEvent = 1.5f;
	private float closeDoorDelay = 2f;
	private float delayBeforeLoading = 2f;

	private CutSceneMaker exitCutscene;
	private CutSceneMaker enterCutscene;

	private bool queueScriptedEvent;

	void Awake()
	{
		changeLevelEffect = transform.GetChild (8).GetComponent<Effect_Change_Level> ();
		changeLevelEffect.levelId = levelToLoad;
		if (IsInvoking ("StartScriptedEvent"))
			CancelInvoke ("StartScriptedEvent");
	}

	void Start()
	{
		gearAnimationScript = GetComponent<ElevatorGearAnimation> ();
		doorAnimationScript = GetComponent<ElevatorDoorAnimation> ();
		doorTrigger = transform.GetChild (3).GetComponent<ElevatorTrigger> ();
		insideTrigger = transform.GetChild (4).GetComponent<ElevatorTrigger> ();
		camEffectScript = transform.GetChild (5).GetComponent<camEffect> ();
		fixCamEffect = transform.GetChild (6).GetComponent<Effect_Fix_Camera> ();
		character_ContScript = transform.GetChild (7).GetComponent<Character_Cont> ();
		exitCutscene = transform.GetChild (10).GetComponent<CutSceneMaker> ();
		enterCutscene = transform.GetChild (11).GetComponent<CutSceneMaker> ();

		Transform checkpoint = transform.GetChild(13);
		bool isCheckpoint = checkpoint.tag == "Checkpoint";

		bool playerInside = false;
		if(!Manager.Instance.Reloading)
		{
			switch(Manager.Instance.LevelName)
			{
			case "0_HUB_YL":
				if(Manager.Instance.SpawnID == (int)changeLevelEffect.levelId - 2)
					playerInside = true;
				break;
			case "1_MANEGE_MAG":
				playerInside = true;
				break;
			case "2_FETEFORAINE_JLC":
				playerInside = true;
				break;
			case "3_CARROUSEL_PD":
				playerInside = true;
				break;
			case "4_CHUCKY_PC":
				playerInside = true;
				break;
			case "5_FREAKSHOW_JLem":
				playerInside = true;
				break;
			case "0_TUTO_P3_YL":
				if(levelToLoad == Effect_Change_Level.enum1.Menu)
					playerInside = true;
				break;
			case "HUB_GDT":
				playerInside = true;
				break;
			default:
				playerInside = true;
				break;
			}
		}

		if(playerInside)
		{
			SpawnFromElevator();
		}
		else
		{
			if(isCheckpoint)
				checkpoint.gameObject.SetActive(false); // Checkpoint
		}
		if (isCheckpoint)
			checkpoint.parent = null;
		isBossElevator = levelToLoad == Effect_Change_Level.enum1.BossFight_GDT;
	}

	void Update()
	{
		if(!inScriptedEvent && !queueScriptedEvent)
		{
			if(!levelWasLoaded)
			{
				if(doorTrigger.IsInside)
				{
					if(!doorAnimationScript.IsOpen() && (!isBossElevator || Manager.Instance.AllStarsTaken()))
						doorAnimationScript.Open ();
				}
				else
				{
					if(!insideTrigger.IsInside)
					{
						if(!doorAnimationScript.IsClosed())
							doorAnimationScript.Close ();
					}
					else
					{
						if(doorAnimationScript.IsOpen() && !IsInvoking("StartScriptedEvent"))
						{
							Invoke ("StartScriptedEvent", delayForScriptedEvent);
						}
					}
				}
			}

			if(IsInvoking("StartScriptedEvent") && !insideTrigger.IsInside)
			{
				CancelInvoke("StartScriptedEvent");
			}
		}

		if(queueScriptedEvent && Manager.Instance.Avatar.IsGrounded)
		{
			StartScriptedEvent();
		}

		if (queueScriptedEvent && !insideTrigger.IsInside)
			queueScriptedEvent = false;
	}


	void LateUpdate()
	{
		if(!queueScriptedEvent)
		{
			if(inScriptedEvent && doorAnimationScript.IsClosed () && !IsInvoking("ChangeLevel") && !doorAnimationScript.IsInvoking("Close") && !levelWasLoaded)
			{
				isLoading = true;
				Invoke ("ChangeLevel", delayBeforeLoading);
			}
			
			if(!inScriptedEvent && levelWasLoaded && !insideTrigger.IsInside)
				levelWasLoaded = false;
		}
	}

	void StartScriptedEvent()
	{
		if(Manager.Instance.Avatar.IsGrounded && insideTrigger.IsInside)
		{
			inScriptedEvent = true;
			UpdateInElevatorForAvatar ();
			enterCutscene.activeCutScene ();
			Manager.Instance.Avatar.InElevator = true;
			doorAnimationScript.Invoke ("Close", closeDoorDelay);
			SoundManager.Instance.StopMusic();
			queueScriptedEvent = false;
		}
		else
		{
			queueScriptedEvent = true;
		}
	}

	void ChangeLevel()
	{
		levelWasLoaded = true;
		Manager.Instance.SpawnID = changeLevelEffect.SpawnPointId;
		character_ContScript.Triggered (changeLevelEffect);
	}

	void StopScriptedEvent()
	{
		doorAnimationScript.Open();
	}

	public void UpdateInElevatorForAvatar()
	{
		Manager.Instance.Avatar.InElevator = inScriptedEvent;
	}

	void SpawnFromElevator()
	{
		Manager.Instance.Avatar.transform.position = transform.position;
		inScriptedEvent = true;
		UpdateInElevatorForAvatar();
		exitCutscene.activeCutScene ();
		Invoke ("StopScriptedEvent", delayAfterLoad);
		SoundManager.Instance.FromElevator = true;
	}

	public bool InScriptedEvent
	{
		set{inScriptedEvent = value;}
	}
}
