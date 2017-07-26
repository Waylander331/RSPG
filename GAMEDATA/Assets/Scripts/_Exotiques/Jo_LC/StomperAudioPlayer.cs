using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class StomperAudioPlayer : MonoBehaviour 
{
	public void PlayPiston(Transform[] stompers)
	{
		foreach (Transform source in stompers)
			if(source != null)
				SoundManager.Instance.PlayAudioInMyself (source.gameObject, "scissorlift_SLmove");
	}

	public void PlayStomper(Transform[] stompers)
	{
		foreach (Transform source in stompers)
			if(source != null)
				SoundManager.Instance.PlayAudioInMyself (source.gameObject, "SFX_stomper");
	}

	/*
	Transform[] GetNearestStompers(bool pistonSound)
	{
		List<PlayingSoundState> availableStompers = new List<PlayingSoundState>();
		for(int i = 0; i < stompers.Length; i++)
		{
			if(pistonSound && stompers[i].PlayingPistonSound || !pistonSound && stompers[i].PlayingStomperSound)
			{
				availableStompers.Add(stompers[i]);
			}
		}

		int count = availableStompers.Count;
		if(count > 3)
		{
			for(int i = 0; i < availableStompers.Count; i++)
			{
				if(ava
			}
		}
		else if(count > 2)
		{
			int[] nearestIndex = new int[] {-1, -1};

			if(Vector3.Distance(stompers[1].transform.position, Manager.Instance.Avatar.transform.position) <
			   Vector3.Distance(stompers[0].transform.position, Manager.Instance.Avatar.transform.position))
			{
				nearestIndex[0] = 1;
				nearestIndex[1] = 0;
			}
			
			for(int i = 2; i < stompers.Length; i++)
			{
				float dist = Vector3.Distance(stompers[i].transform.position, Manager.Instance.Avatar.transform.position);
				if(dist < Vector3.Distance(stompers[nearestIndex[0]].transform.position, Manager.Instance.Avatar.transform.position))
				{
					nearestIndex[0] = i;
				}
				else if(dist < Vector3.Distance(stompers[nearestIndex[1]].transform.position, Manager.Instance.Avatar.transform.position))
				{
					nearestIndex[1] = i;
				}
			}
			returnValue[0] = stompers[nearestIndex[0]].transform;
			returnValue[1] = stompers[nearestIndex[1]].transform;
			return returnValue;
		}
		else
		{
			if(stompers.Length > 1)
				return new Transform[]{stompers[0].transform, stompers[1].transform};
			else if(stompers.Length > 0)
				return new Transform[]{stompers[0].transform, null};
			else
				return new Transform[]{null, null};
		}
	}*/
	 
}
