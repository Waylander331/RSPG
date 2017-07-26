using UnityEngine;
using System.Collections;

[System.Serializable]
public class ScreenInfo 
{
	private bool isBroken = false;
	private bool isVisible = false;

	public ScreenInfo()
	{
		isBroken = false;
		isVisible = false;
	}

	public bool IsBroken
	{
		get{return isBroken;}
		set{isBroken = value;}
	}

	public bool IsVisible
	{
		get{return isVisible;}
		set{isVisible = value;}
	}
}
