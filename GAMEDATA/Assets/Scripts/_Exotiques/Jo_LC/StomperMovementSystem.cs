using UnityEngine;
using System.Collections;

public class StomperMovementSystem : MonoBehaviour 
{
	public StomperMovementSystem synchronizeWith; // makes "self" listen to "synchronizeWith" System
	public float initialDelay;
	public float range;

	public float goingDelay;
	public float goingSpeed;

	public float returnDelay;
	public float returnSpeed;
	
	public Transform[] stompers;
	private Transform[] pistons;
	private Transform[] stomperSFX;
	private StomperAudioPlayer audioPlayer;

	private bool goReversed;
	private bool returnReversed;
	private bool returning;
	private bool going;
	private float currentPos;

	private float halfSize;
	private Vector3[] initialPos;
	private bool onPlay = false;

	private StomperMovementSystem systemClient; // the system that listen to "self"
	private bool clientTriggered;

	void Awake()
	{
		if(synchronizeWith != null) synchronizeWith.SystemClient = this;
		audioPlayer = GetComponent<StomperAudioPlayer> ();
		pistons = new Transform[stompers.Length];
		stomperSFX = new Transform[stompers.Length];
		for (int i = 0; i < stompers.Length; i++)
		{
			pistons[i] = stompers[i].GetChild(3);
			stomperSFX[i] = stompers[i].GetChild (4);
		}
	}

	void Start()
	{
		if(synchronizeWith != null)
		{
			range = synchronizeWith.range;
			goingSpeed = synchronizeWith.goingSpeed;
			goingDelay = synchronizeWith.goingDelay;
			returnSpeed = synchronizeWith.returnSpeed;
			returnDelay = synchronizeWith.returnDelay;
		}
		else
			Invoke ("Go", initialDelay);

		goReversed = range * goingSpeed < 0f;
		returnReversed = range * returnSpeed < 0f;
		range = Mathf.Abs (range);
		goingSpeed = Mathf.Abs (goingSpeed);
		initialDelay = Mathf.Abs (initialDelay);
		goingDelay = Mathf.Abs (goingDelay);
		returnDelay = Mathf.Abs (returnDelay);
		returnSpeed = Mathf.Abs (returnSpeed);

		Transform stomperMesh = stompers [0].GetChild (0).GetChild (0);
		halfSize = stomperMesh.GetComponent<MeshFilter> ().sharedMesh.bounds.size.y * stomperMesh.localScale.y / 2f;
		initialPos = new Vector3[stompers.Length];
		
		for(int i = 0; i < stompers.Length; i++)
		{
			if(stompers[i] != null)
			{
				initialPos[i] = stomperMesh.position;
			}
		}
		onPlay = true;
	}

	void Update()
	{
		if(going)
		{
			float movement = goingSpeed * Time.deltaTime;
			if(currentPos + movement < range)
			{
				currentPos += movement;
			}
			else
			{
				movement = range - currentPos;
				currentPos = range;
				going = false;
				Invoke ("Return", returnDelay);
				if(audioPlayer != null)
					audioPlayer.PlayStomper(stompers);
			}

			if(goReversed) movement = -movement;
			foreach(Transform stomper in stompers)
				stomper.Translate(new Vector3(0f, -movement, 0f), Space.World);
		}
		if (returning)
		{
			float movement = returnSpeed * Time.deltaTime;
			if(currentPos - movement > 0f)
			{
				currentPos -= movement;
			}
			else
			{
				movement = currentPos;
				currentPos = 0f;
				returning = false;
				Invoke ("Go", goingDelay);
			}

			if(returnReversed) movement = -movement; 
			foreach (Transform stomper in stompers)
				stomper.Translate(new Vector3(0f, movement, 0f), Space.World);
		}
	}

	public void StartNow()
	{
		Invoke("Go", initialDelay);
	}

	void Go()
	{
		going = true;
		if (audioPlayer != null)
			audioPlayer.PlayPiston (stompers);
	}

	void Return()
	{
		returning = true;
		if(systemClient != null && !clientTriggered)
		{
			systemClient.StartNow();
			clientTriggered = true;
		}
		if (audioPlayer != null)
			audioPlayer.PlayPiston (stompers);
	}

	void OnDrawGizmosSelected()
	{
		Transform stomperMesh = stompers [0].GetChild (0).GetChild (0);
		halfSize = stomperMesh.GetComponent<MeshFilter> ().sharedMesh.bounds.size.y * stomperMesh.localScale.y / 2f;
		for(int i = 0; i < stompers.Length; i++)
		{
			if(stompers[i] != null)
			{
				if(!onPlay)
					DrawShape.DrawArrowForGizmo(stomperMesh.position - Vector3.down * (halfSize), Vector3.down * (range + halfSize), Color.blue);
				else
					DrawShape.DrawArrowForGizmo(initialPos[i] - Vector3.down * (halfSize), Vector3.down * (range + halfSize), Color.blue);

				Gizmos.color = Color.white;
				Gizmos.DrawLine (transform.position, stompers[i].transform.position);
			}
		}
	}

	public StomperMovementSystem SystemClient
	{
		set{systemClient = value;}
	}

	public Transform[] Stompers
	{
		get{return stompers;}
	}
}
