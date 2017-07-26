using UnityEngine;
using System.Collections;

public class BrokenStomperTrigger : MonoBehaviour 
{
	private bool isDetecting;
	private bool isInside;

	private bool wasGrounded;
	private bool changeCheck;
	
	void Update()
	{
		if(isInside)
		{
			bool avGrounded = Manager.Instance.Avatar.IsGrounded;
			if(wasGrounded != avGrounded)
			{
				wasGrounded = avGrounded;
				changeCheck = true;
			}
			
			if(changeCheck && isInside)
			{
				changeCheck = false;
				isDetecting = avGrounded;
				SetConnectionWithPlayer(isDetecting);
			}
		}
		else if(isDetecting)
			SetConnectionWithPlayer(false);

	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			isInside = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			isInside = false;
		}
	}

	public void SetConnectionWithPlayer(bool set)
	{
		if(set)
		{
			isDetecting = true;
			Manager.Instance.Avatar.transform.parent = transform.parent.parent;
		}
		else
		{
			isDetecting = false;
			Manager.Instance.Avatar.transform.parent = null;
		}

	}

	public bool IsDetecting
	{
		get{return isDetecting;}
	}
}
