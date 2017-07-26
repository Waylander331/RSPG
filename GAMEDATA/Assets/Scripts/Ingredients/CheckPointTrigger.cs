using UnityEngine;
using System.Collections;

public class CheckPointTrigger : MonoBehaviour 
{
	private CheckPoint checkpoint;
	private bool avatarIsInside;
	private bool isElevatorCheckpoint;

	void OnTriggerEnter(Collider col)
	{
		if (checkpoint.Active && col.tag == "Player")
		{
			avatarIsInside = true;
			Manager.Instance.SetCheckpoint(checkpoint);
			if(isElevatorCheckpoint)
			{
				transform.parent.parent.gameObject.SetActive(false);
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (checkpoint.Active && col.tag == "Player")
		{
			avatarIsInside = false;
		}
	}

	public CheckPoint Checkpoint
	{
		set{checkpoint = value;}
	}

	public bool AvatarIsInside
	{
		set{avatarIsInside = value;}
	}

	public bool IsElevatorCheckpoint
	{
		set{isElevatorCheckpoint = value;}
	}
}
