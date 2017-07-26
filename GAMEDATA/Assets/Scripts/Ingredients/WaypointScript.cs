using UnityEngine;
using System.Collections;

public class WaypointScript : MonoBehaviour {

	[SerializeField] private int id; 
	[SerializeField] private GameObject[] owners;
	//[SerializeField] private GameObject owner;

	[SerializeField] private enum State{
		Multiple, OnceOnly
	};
	[SerializeField] private State doingState;
	[SerializeField] private int state;

	[SerializeField] private bool firstPassage;

	[SerializeField] private float timeOfWait = 0;
	[SerializeField] private bool isStopped = false;

	[SerializeField] private enum Orientation{
		None, KeepOrientation, FullForward, Forward
	};
	[SerializeField] private Orientation platformOrientation;
	[SerializeField] private int orientation;

	[SerializeField] private bool isHalf = false;
	[SerializeField] private float timeOfWaitHalf = 0;
	[SerializeField] private bool isFull = false;
	[SerializeField] private float timeOfWaitFull = 0;

	[SerializeField] private enum Speed{
		None, VerySlow, Slow, Medium, Fast, VeryFast
	};
	[SerializeField] private Speed platformSpeed;
	[SerializeField] private float newSpeed;
	
	[SerializeField] private bool isGoing;
	[SerializeField] private bool isReturned;
	[SerializeField] private WaypointScript wpGo;
	[SerializeField] private WaypointScript wpReturn;

	private Movable target;

	void Start (){
		foreach(GameObject owner in owners){
			target = owner.GetComponent<Movable>();
		}

		switch(doingState){
		case State.OnceOnly:
			state = 0;
			break;
		case State.Multiple:
			state = 1;
			break;
		}

		switch(platformOrientation){
		case Orientation.None:
			break;
		case Orientation.KeepOrientation:
			orientation = 0;
			break;
		case Orientation.FullForward:
			orientation = 1;
			break;
		case Orientation.Forward:
			orientation = 2;
			break;
		}

		switch(platformSpeed){
		case Speed.None:
			break;
		case Speed.VerySlow:
			newSpeed = 1;
			break;
		case Speed.Slow:
			newSpeed = 3;
			break;
		case Speed.Medium:
			newSpeed = 5;
			break;
		case Speed.Fast:
			newSpeed = 7;
			break;
		case Speed.VeryFast:
			newSpeed = 10;
			break;
		default:
			newSpeed = 5;
			break;
		}

		if(this.transform != null){
			if(firstPassage){
				foreach(GameObject owner in owners){
					if(owner != null){
						if(owner.transform.position == this.transform.position){
							if(platformSpeed != Speed.None){
								target.SetNewSpeed(newSpeed);
							}
							if(timeOfWait > 0){
								target.SetDelay(timeOfWait);
							}
							if(!target.IsReversed && isGoing){
								target.TeleportTarget(wpGo);
							}
							if(target.IsReversed && isReturned){
								target.TeleportTarget(wpReturn);
							}
							if(isStopped){
								target.StopTarget(isStopped);
							}
							if(platformOrientation != Orientation.None){
								target.SetRotation(orientation);
							}
							if(isHalf){
								target.SetHalfRotation(new MovableRotation(isHalf, timeOfWaitHalf));
							}
							if(isFull){
								target.SetFullRotation(new MovableRotation(isFull, timeOfWaitFull));
							}
							
							firstPassage = false;
							
							timeOfWait = 0;
							isStopped = false;
							isHalf = false;
							isFull = false;
							platformOrientation = Orientation.None;
							platformSpeed = Speed.None;
							isGoing = false;
							isReturned = false;
							wpGo = null;
							wpReturn = null;
						}
					}
				}
			}
			
			if(state == 1){
				foreach(GameObject owner in owners){
					if(owner != null){
						if(owner.transform.position == this.transform.position){
							if(platformSpeed != Speed.None){
								target.SetNewSpeed(newSpeed);
							}
							if(timeOfWait > 0){
								target.SetDelay(timeOfWait);
							}
							if(!target.IsReversed && isGoing){
								target.TeleportTarget(wpGo);
							}
							if(target.IsReversed && isReturned){
								target.TeleportTarget(wpReturn);
							}
							if(isStopped){
								target.StopTarget(isStopped);
							}
							if(platformOrientation != Orientation.None){
								target.SetRotation(orientation);
							}
							if(isHalf){
								target.SetHalfRotation(new MovableRotation(isHalf, timeOfWaitHalf));
							}
							if(isFull){
								target.SetFullRotation(new MovableRotation(isFull, timeOfWaitFull));
							}
						}
					}
				}
			}
		}
	}

	void Update(){
		//Appel les fonctions sous conditions
		if(state == 0){
			firstPassage = true;
		} else firstPassage = false;

		if(this.transform != null){
			if(firstPassage){
				foreach(GameObject owner in owners){
					if(owner != null){
						if(owner.transform.position == this.transform.position){
							if(platformSpeed != Speed.None){
								target.SetNewSpeed(newSpeed);
							}
							if(timeOfWait > 0){
								target.SetDelay(timeOfWait);
							}
							if(!target.IsReversed && isGoing){
									target.TeleportTarget(wpGo);
							}
							if(target.IsReversed && isReturned){
									target.TeleportTarget(wpReturn);
							}
							if(isStopped){
								target.StopTarget(isStopped);
							}
							if(platformOrientation != Orientation.None){
								target.SetRotation(orientation);
							}
							if(isHalf){
								target.SetHalfRotation(new MovableRotation(isHalf, timeOfWaitHalf));
							}
							if(isFull){
								target.SetFullRotation(new MovableRotation(isFull, timeOfWaitFull));
							}

							firstPassage = false;

							timeOfWait = 0;
							isStopped = false;
							isHalf = false;
							isFull = false;
							platformOrientation = Orientation.None;
							platformSpeed = Speed.None;
							isGoing = false;
							isReturned = false;
							wpGo = null;
							wpReturn = null;
						}
					}
				}
			}

			if(state == 1){
				foreach(GameObject owner in owners){
					if(owner != null){
						if(owner.transform.position == this.transform.position){
							if(platformSpeed != Speed.None){
								target.SetNewSpeed(newSpeed);
							}
							if(timeOfWait > 0){
								target.SetDelay(timeOfWait);
							}
							if(!target.IsReversed && isGoing){
								target.TeleportTarget(wpGo);
							}
							if(target.IsReversed && isReturned){
								target.TeleportTarget(wpReturn);
							}
							if(isStopped){
								target.StopTarget(isStopped);
							}
							if(platformOrientation != Orientation.None){
								target.SetRotation(orientation);
							}
							if(isHalf){
								target.SetHalfRotation(new MovableRotation(isHalf, timeOfWaitHalf));
							}
							if(isFull){
								target.SetFullRotation(new MovableRotation(isFull, timeOfWaitFull));
							}
						}
					}
				}
			}
		}

		switch(doingState){
		case State.OnceOnly:
			state = 0;
			break;
		case State.Multiple:
			state = 1;
			break;
		}

		switch(platformOrientation){
		case Orientation.None:
			break;
		case Orientation.KeepOrientation:
			orientation = 0;
			break;
		case Orientation.FullForward:
			orientation = 1;
			break;
		case Orientation.Forward:
			orientation = 2;
			break;
		}

		switch(platformSpeed){
		case Speed.None:
			break;
		case Speed.VerySlow:
			newSpeed = 1;
			break;
		case Speed.Slow:
			newSpeed = 3;
			break;
		case Speed.Medium:
			newSpeed = 5;
			break;
		case Speed.Fast:
			newSpeed = 7;
			break;
		case Speed.VeryFast:
			newSpeed = 10;
			break;
		default:
			newSpeed = 5;
			break;
		}
	}


	void OnDrawGizmos(){
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere(transform.position, 0.40f);
	}

	public int Id {
		get {return id;}
		set {id = value;}
	}

	public GameObject[] Owners{
		get {return owners;}
		set{owners = value;}
	}
}
