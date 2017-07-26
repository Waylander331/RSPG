using UnityEngine;
using System.Collections;

public class PlayingSoundState : MonoBehaviour 
{
	private bool playingPistonSound;
	private bool playingStomperSound;

	public bool PlayingPistonSound
	{
		get{return playingPistonSound;}
		set{playingPistonSound = value;}
	}

	public bool PlayingStomperSound
	{
		get{return playingStomperSound;}
		set{playingStomperSound = value;}
	}
}
