using UnityEngine;
using System.Collections;

public class LinkedTV : MonoBehaviour 
{
	private LinkedCam[] linkedTVs;
	private ScreenInfo[] tvInfos;
	private int tvCount = 0;


	public void Start()
	{
		if (tvInfos != null)
		{
			for (int i = 0; i < tvInfos.Length; i++)
			{
				tvInfos[i] = new ScreenInfo();
				if(linkedTVs[i] != null)
				{
					//Debug.Log (linkedTVs[i].name);
					GetSparks(linkedTVs[i]).SetActive(false);
				}
			}
		}
	}

	void OnLevelWasLoaded(){
		if (tvInfos != null)
		{
			for (int i = 0; i < tvInfos.Length; i++)
			{
				tvInfos[i] = new ScreenInfo();
				if(linkedTVs[i] != null)
				{
					//Debug.Log (linkedTVs[i].name);
					GetSparks(linkedTVs[i]).SetActive(false);
				}
			}
		}
	}

	public void AddTV(GameObject tv)
	{
		LinkedCam linkedCam = tv.GetComponent<LinkedCam>();
		if (linkedCam == null)
			Debug.LogWarning ("LinkedTV/AddTV(GameObject) : No script LinkedCam on TV");
		else
		{
			tvCount++;
			ResizeArray(ref linkedTVs, tvCount);
			linkedTVs [tvCount - 1] = linkedCam;
			ResizeArray(ref tvInfos, tvCount);
			tvInfos[tvCount - 1] = new ScreenInfo();
		}
	}

	public GameObject GetSparks(LinkedCam tv)
	{
		if(tv != null)
		{
			int index = GetTvIndex(tv);
			if(index != -1)
			{
				return linkedTVs[index].transform.GetChild(1).gameObject;
			}
		}
		return null;
	}

	public GameObject GetSparks(int index)
	{
		return linkedTVs[index].transform.GetChild(1).gameObject;
	}

	public GameObject GetTV(GameObject tv)
	{
		LinkedCam linkedCam = tv.GetComponent<LinkedCam>();
		if(linkedCam != null)
		{
			int index = GetTvIndex(linkedCam);
			if(index != -1)
			{
				return linkedTVs[index].gameObject;
			}
		}
		return null;
	}

	public int GetTvIndex(LinkedCam tv)
	{
		if(tv != null)
		{
			for(int i = 0; i < linkedTVs.Length; i++)
			{
				if(tv == linkedTVs[i])
					return i;
			}
		}
		return -1;
	}

	public bool IsDestroyed(int index)
	{
		return tvInfos[index].IsBroken;
	}

	public void SetDestroyed(int index)
	{
		tvInfos[index].IsBroken = true;
	}

	void ResizeArray(ref LinkedCam[] array, int length)
	{
		LinkedCam[] temp = new LinkedCam[0];
		if (array != null)
			temp = array;
		array = new LinkedCam[length];
		for(int i = 0; i < array.Length && (array == null || array != null && i < temp.Length); i++)
		{
			array[i] = temp[i];
		}
	}

	void ResizeArray (ref ScreenInfo[] array, int length)
	{
		ScreenInfo[] temp = new ScreenInfo[0];
		if (array != null)
			temp = array;
		array = new ScreenInfo[length];
		for(int i = 0; i < array.Length && (array == null || array != null && i < temp.Length); i++)
		{
			array[i] = temp[i];
		}
	}

	public LinkedCam[] LinkedTVs
	{
		get{return linkedTVs;}
	}

	public ScreenInfo[] ScreenInfos
	{
		get{return tvInfos;}
	}
}
