using UnityEngine;
using System.Collections;

public class ElevatorDoorAnimation : MonoBehaviour 
{
	private Transform leftDoor;
	private Transform rightDoor;

	private float openingSpeed = 2f;
	private float closingSpeed = 1.5f;

	private float distance;
	private float doorWidthModifier = 0.65f;
	
	private float distanceTraveled;

	private bool opening;
	private bool closing;

	private bool playedOpeningAudio;
	private bool playedClosingAudio;

	void Start()
	{
		leftDoor = transform.GetChild (0);
		rightDoor = transform.GetChild (1);

		openingSpeed = Mathf.Abs (openingSpeed);
		closingSpeed = Mathf.Abs (closingSpeed);
		distance = Mathf.Abs (distance);
		distance = leftDoor.GetComponent<MeshFilter> ().sharedMesh.bounds.size.x * leftDoor.localScale.x * doorWidthModifier;
	}

	void LateUpdate()
	{
		if(opening && distanceTraveled != distance)
		{
			float moveDistance = openingSpeed * Time.deltaTime;

			if(distanceTraveled + moveDistance < distance)
			{
				distanceTraveled += moveDistance;
			}
			else
			{
				distanceTraveled += moveDistance = distance - distanceTraveled;
				opening = false;
			}

			leftDoor.Translate(-leftDoor.right * moveDistance, Space.World);
			rightDoor.Translate(rightDoor.right * moveDistance, Space.World);
		}

		else if(closing && distanceTraveled != 0f)
		{
			float moveDistance = closingSpeed * Time.deltaTime;

			if(distanceTraveled - moveDistance > 0f)
			{
				distanceTraveled -= moveDistance;
			}
			else
			{
				distanceTraveled -= moveDistance = distanceTraveled;
				closing = false;
			}

			leftDoor.Translate(leftDoor.right * moveDistance, Space.World);
			rightDoor.Translate(-rightDoor.right * moveDistance, Space.World);
		}

		// SOUND PLAYER
		if (!playedClosingAudio && IsClosed ())
		{
			SoundManager.Instance.PlayAudio ("DoorElevatorClose");
			playedClosingAudio = true;
		}

		if (playedClosingAudio && !IsClosed ())
			playedClosingAudio = false;

		if(!playedOpeningAudio && opening && !IsOpen())
		{
			SoundManager.Instance.PlayAudio("DoorElevatorOpen");
			playedOpeningAudio = true;
		}

		if (playedOpeningAudio && IsClosed () || closing)
			playedOpeningAudio = false;
	}

	public void Open()
	{
		opening = true;
		closing = false;
	}

	public void Close()
	{
		opening = false;
		closing = true;
		if(Manager.Instance.Avatar.InElevator){
			SoundManager.Instance.PlayAudio("ElevatorSong");
		}
	}

	public void Stop()
	{
		opening = false;
		closing = false;
	}

	public void JumpOpen()
	{
		leftDoor.Translate(-leftDoor.right * (distance - distanceTraveled));
		rightDoor.Translate(rightDoor.right * (distance - distanceTraveled));
		distanceTraveled = distance;
	}

	public void JumpClose()
	{	
		leftDoor.Translate(leftDoor.right * distanceTraveled);
		rightDoor.Translate(-rightDoor.right * distanceTraveled);
		distanceTraveled = 0f;
	}

	public bool IsOpen()
	{
		return distanceTraveled == distance;
	}

	public bool IsClosed()
	{
		return distanceTraveled == 0f;
	}

	public bool Closing {
		get {return closing;}
	}
}
