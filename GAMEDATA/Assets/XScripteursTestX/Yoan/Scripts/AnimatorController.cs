using UnityEngine;
using System.Collections;

public class AnimatorController : MonoBehaviour 
{
	public Animation myAnime;

	private bool canRewind = false;

	void Start()
	{
		//myAnime["PositionChange"].speed = 0;
		myAnime.Stop ();
		if(myAnime == null)
		{
			myAnime = GetComponent<Animation>();
		}
	}

	/*void OnStateEnter(Animator animator,AnimatorStateInfo info, int state)
	{
		animator.speed = 1;
	}*/

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Upper")
		{
			myAnime["PositionChange"].speed = 0;
			canRewind = true;

		}

		if(canRewind && other.tag == "Lower")
		{
			myAnime["PositionChange"].speed = 0;
			//myAnime["PositionChange"].time = 0.0f;
			Debug.Log ("Setting it to 0");
			canRewind = false;
		}
	}
	

	void Update()
	{
		Debug.Log("Speed is" + myAnime["PositionChange"].speed);

		if(canRewind && Input.GetKeyDown("space"))
		{
			myAnime["PositionChange"].speed = -1;
		}
		else if (Input.GetKeyDown("space"))
		{
			//myAnime["PositionChange"].time = 0.0f;
			Debug.Log ("trying to go up");
			myAnime.Play();
			myAnime["PositionChange"].speed = 1;
		}

	}


}
