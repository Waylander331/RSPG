using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour, ITriggerable {

	private Transform left;
	private Transform right;
	private Transform up;

	private Vector3 posInit;
	private Vector3 posInitLeft;
	private Vector3 posInitRight;
	private Vector3 posInitUp;

	//***Open - Inspector***//
	[SerializeField] private float timerOpen;
	[SerializeField] private Speed openSpeed;
	[SerializeField] private enum DirectionO{
		Left, Right, Up
	};
	[SerializeField] private DirectionO doorDirectionO;

	//***Close - Inspector***//
	[SerializeField] private float timerClose;
	[SerializeField] private Speed closeSpeed;

	//***Inspector***//
	[SerializeField] private enum Speed{
		Slow, Medium, Fast
	};
	[SerializeField] private enum StartPosition{
		Close, OpenLeft, OpenRight, OpenUp
	};
	[SerializeField] private StartPosition startPosition;
	[SerializeField] private float timerReset;


	//private Vector3 direction;

	private int oSpeed;
	private int cSpeed;

	private bool isTimerOpen;
	private bool isTimerClose;

	private bool isOpen;

	private bool isOpening;
	private bool isClosing;

	// Use this for initialization
	void Start () {
		left = transform.GetChild (0);
		right = transform.GetChild (1);
		up = transform.GetChild (2);

		posInit = transform.position;
		posInitLeft = left.position;
		posInitRight = right.position;
		posInitUp = up.position;

		switch(startPosition){
		case StartPosition.Close:
			transform.position = posInit;
			isOpen = false;
			break;
		case StartPosition.OpenLeft:
			transform.position = posInitLeft;
			doorDirectionO = DirectionO.Left;
			isOpen = true;
			break;
		case StartPosition.OpenRight:
			transform.position = posInitRight;
			doorDirectionO = DirectionO.Right;
			isOpen = true;
			break;
		case StartPosition.OpenUp:
			transform.position = posInitUp;
			doorDirectionO = DirectionO.Up;
			isOpen = true;
			break;
		}

		switch(openSpeed){
		case Speed.Slow:
			oSpeed = 2;
			break;
		case Speed.Medium:
			oSpeed = 4;
			break;
		case Speed.Fast:
			oSpeed = 6;
			break;
		}

		switch(closeSpeed){
		case Speed.Slow:
			cSpeed = 2;
			break;
		case Speed.Medium:
			cSpeed = 4;
			break;
		case Speed.Fast:
			cSpeed = 6;
			break;
		}

		/*switch(doorDirectionO){
		case DirectionO.Left:
			//direction = posInitLeft;
			break;
		case DirectionO.Right:
			//direction = posInitRight;
			break;
		case DirectionO.Up:
			//direction = posInitUp;
			break;
		}*/
	}
	
	// Update is called once per frame
	void Update () {

		if(isOpening){
			Open();
		}
		if(isClosing){
			Close ();
		}

		switch(openSpeed){
		case Speed.Slow:
			oSpeed = 2;
			break;
		case Speed.Medium:
			oSpeed = 4;
			break;
		case Speed.Fast:
			oSpeed = 6;
			break;
		}

		switch(closeSpeed){
		case Speed.Slow:
			cSpeed = 2;
			break;
		case Speed.Medium:
			cSpeed = 4;
			break;
		case Speed.Fast:
			cSpeed = 6;
			break;
		}

	}


	public void Triggered(EffectList effect){
		if(effect.GetType() == typeof (Effect_Default)){
			if(!isOpen){
				isOpening = true;
			}
			if(isOpen){
				isClosing = true;
			}
		}

		if(effect.GetType() == typeof (Effect_Close)){
			isTimerClose = true;
			Invoke ("DelayClose", timerClose);
		}

		if(effect.GetType() == typeof (Effect_Open)){
			isTimerOpen = true;
			Invoke ("DelayOpen", timerOpen);
		}
	}

	public void UnTriggered(EffectList effect){
		if(effect.GetType() == typeof (Effect_Default)){
			if(!isOpen){
				isOpening = !isOpening;
			}
			if(isOpen){
				isClosing = !isClosing;
			}
		}

		if(effect.GetType() == typeof (Effect_Close)){
			isTimerClose = true;
			Invoke ("DelayClose", timerClose);
		}

		if(effect.GetType() == typeof (Effect_Open)){
			isTimerOpen = true;
			Invoke ("DelayOpen", timerOpen);
		}
	}

	void DelayOpen(){
		if(isTimerOpen && !isOpen){
			isOpening = true;
			isTimerOpen = false;
		}
	}

	void Open(){
		if(doorDirectionO == DirectionO.Left){
			if(Vector3.Distance(posInitLeft, transform.position - transform.right * oSpeed * Time.deltaTime) < oSpeed * Time.deltaTime){
				transform.position = posInitLeft;
				isOpen = true;
				isOpening = false;
				if(timerReset > 0){
					isTimerClose = true;
					Invoke ("DelayClose", timerReset);
				}
			}
			else this.transform.Translate(- transform.right * oSpeed * Time.deltaTime, Space.World);
		}
		if(doorDirectionO == DirectionO.Right){
			if(Vector3.Distance(posInitRight, transform.position + transform.right * oSpeed * Time.deltaTime) < oSpeed * Time.deltaTime){
				transform.position = posInitRight;
				isOpen = true;
				isOpening = false;
				if(timerReset > 0){
					isTimerClose = true;
					Invoke ("DelayClose", timerReset);
				}

			}
			else this.transform.Translate(transform.right * oSpeed * Time.deltaTime, Space.World);
		}
		if(doorDirectionO == DirectionO.Up){
			if(Vector3.Distance(posInitUp, transform.position + transform.up * oSpeed * Time.deltaTime) < oSpeed * Time.deltaTime){
				transform.position = posInitUp;
				isOpen = true;
				isOpening = false;
				if(timerReset > 0){
					isTimerClose = true;
					Invoke ("DelayClose", timerReset);
				}

			}
			else this.transform.Translate(transform.up * oSpeed * Time.deltaTime, Space.World);
		}
	}

	void DelayClose(){
		if(isTimerClose && isOpen){
			isClosing = true;
			isTimerClose = false;
		}
	}

	void Close(){
		if(startPosition == StartPosition.OpenRight || doorDirectionO == DirectionO.Right){
			if(Vector3.Distance(posInit, transform.position - transform.right * cSpeed * Time.deltaTime) < cSpeed * Time.deltaTime){
				transform.position = posInit;
				isOpen = false;
				isClosing = false;
			}
			else this.transform.Translate(-transform.right * cSpeed * Time.deltaTime, Space.World);
		}
		if(startPosition == StartPosition.OpenLeft || doorDirectionO == DirectionO.Left){
			if(Vector3.Distance(posInit, transform.position + transform.right * cSpeed * Time.deltaTime) < cSpeed * Time.deltaTime){
				transform.position = posInit;
				isOpen = false;
				isClosing = false;
			}
			else {
				this.transform.Translate(transform.right * cSpeed * Time.deltaTime, Space.World);
			}
		}
		if(startPosition == StartPosition.OpenUp || doorDirectionO == DirectionO.Up){
			if(Vector3.Distance(posInit, transform.position - transform.up * cSpeed * Time.deltaTime) < cSpeed * Time.deltaTime){
				transform.position = posInit;
				isOpen = false;
				isClosing = false;

			}
			else this.transform.Translate(- transform.up * cSpeed * Time.deltaTime, Space.World);
		}
	}

	void OnDrawGizmos(){
		Gizmos.color = Color.grey;
		Gizmos.DrawSphere(posInitLeft, 0.1f);
		Gizmos.DrawSphere(posInitRight, 0.1f);
		Gizmos.DrawSphere(posInitUp, 0.1f);
		
	}
}






