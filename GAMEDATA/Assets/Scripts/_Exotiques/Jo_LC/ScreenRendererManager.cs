using UnityEngine;
using System.Collections;

public class ScreenRendererManager : MonoBehaviour 
{
	LinkedTV linkedTvScript;
	ScreenFlicker camEffects;
	int currentlyRendered = 0;
	ScreenVisibleListener[] screenVisibleListeners;
	LayerMask wallJumpable = 1 << 11;

	void Start()
	{
		linkedTvScript = GetComponent<LinkedTV> ();
		camEffects = GetComponent<ScreenFlicker> ();

		if (linkedTvScript.LinkedTVs == null || linkedTvScript.LinkedTVs.Length == 0)
			enabled = false;
		else
		{
			screenVisibleListeners = new ScreenVisibleListener[linkedTvScript.LinkedTVs.Length];
			for(int i = 0; i < screenVisibleListeners.Length; i++)
			{
				screenVisibleListeners[i] = linkedTvScript.LinkedTVs[i].GetComponent<ScreenVisibleListener>();
			}
			camEffects.SetDestroyedEffect (false);
		}
	}

	void LateUpdate()
	{
		UpdateVisibleStatus ();
		CheckTvToRender ();
	}

	void RenderNew(int tvIndex)
	{
		camEffects.SetDestroyedEffect (linkedTvScript.IsDestroyed(tvIndex));
	}

	void CheckTvToRender()
	{
		int current = currentlyRendered;
		for(int i = 0; i < linkedTvScript.LinkedTVs.Length; i++)
		{
			if
			(
				i != currentlyRendered && linkedTvScript.ScreenInfos[i].IsVisible &&
			    (
					!linkedTvScript.ScreenInfos[currentlyRendered].IsVisible || 
						Vector3.Distance (Manager.Instance.Avatar.transform.position, linkedTvScript.LinkedTVs[i].transform.position)
						< Vector3.Distance(Manager.Instance.Avatar.transform.position, linkedTvScript.LinkedTVs[currentlyRendered].transform.position)
				)
			)
			{
				currentlyRendered = i;
			}
		}
		RenderNew (currentlyRendered);
	}

	void UpdateVisibleStatus()
	{
		for(int i = 0; i < linkedTvScript.LinkedTVs.Length && i < screenVisibleListeners.Length; i++)
		{
			if(screenVisibleListeners[i].IsVisible)
			{
				Ray ray = new Ray(Camera.main.transform.position, linkedTvScript.LinkedTVs[i].transform.position - Camera.main.transform.position);
				RaycastHit hit;
				linkedTvScript.ScreenInfos[i].IsVisible = 
					Physics.Raycast(ray, out hit, Vector3.Distance(Camera.main.transform.position, linkedTvScript.LinkedTVs[i].transform.position), wallJumpable) && 
						hit.transform.tag == "TVScreen";
			}
			else
			{
				linkedTvScript.ScreenInfos[i].IsVisible = false;
			}
		}
	}

	public int CurrentlyRendered
	{
		get{return currentlyRendered;}
	}
}
