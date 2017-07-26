using UnityEngine;
using System.Collections;

public class DirectionModifier : MonoBehaviour 
{
	private Vector3 direction;
	private float speed = 0f;
	private bool moving = false;

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground") 
		{
			moving = false;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (moving) 
		{
			transform.Translate (direction * speed * Time.deltaTime, Space.Self);
		}
	}

	public void StartMoving(Vector3 direction, float force)
	{
		if (direction != Vector3.zero && force != 0f) 
		{
			this.direction = direction;
			speed = force;
			moving = true;
		}
	}
}
