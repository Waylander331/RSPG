﻿using UnityEngine;
using System.Collections;

public class BridgeManVisibleListener : MonoBehaviour 
{
	private bool isVisible;

	void OnBecameVisible()
	{
		isVisible = true;
	}

	void OnBecameInvisible()
	{
		isVisible = false;
	}

	public bool IsVisible
	{
		get{return isVisible;}
	}
}
