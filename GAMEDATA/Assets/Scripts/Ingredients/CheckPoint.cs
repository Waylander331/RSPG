using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{
	private bool active = true;
	private string triggerWarningMessage = "No trigger inside checkpoint : ";
	private Transform respawnPoint;
	private CheckpointLightManager lightManager;
	private bool isElevatorCheckpoint;

	public bool isInBackstage;

	void Awake()
	{
		respawnPoint = transform.GetChild (0).GetChild (0);
		if(transform.childCount > 1)
			lightManager = transform.GetChild (1).GetComponent<CheckpointLightManager>();
	}

	void Start()
	{
		if (transform.parent != null && transform.parent.tag != "Elevator")
			transform.parent = null;
		else if (transform.parent != null && transform.parent.tag == "Elevator")
		{
			isElevatorCheckpoint = true;
			transform.GetChild(0).GetChild(1).GetComponent<CheckPointTrigger>().IsElevatorCheckpoint = isElevatorCheckpoint;
		}

		triggerWarningMessage += gameObject.name;
		if(transform.childCount >= 1)
		{
			CheckPointTrigger trigger = transform.GetChild (0).GetChild (1).GetComponent<CheckPointTrigger>();
			if(trigger != null)
				trigger.Checkpoint = this;
			else
				Debug.LogWarning (triggerWarningMessage);
		}
		else
			Debug.LogWarning (triggerWarningMessage);
	}

	public bool Active
	{
		get{return active;}
		set{active = value;}
	}

	public Transform RespawnPoint
	{
		get{return respawnPoint;}
	}

	/*
	public int MyLevel
	{
		get{return myLevel;}
		set{myLevel = value;}
	}*/

	public CheckpointLightManager CheckpointLightManager
	{
		get{return lightManager;}
	}

	public bool IsElevatorCheckpoint
	{
		get{return isElevatorCheckpoint;}
	}
}
