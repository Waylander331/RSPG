using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour 
{
	private float launchRange = 0.6f;
	private float distanceCovered = 0f;

	private bool atTargetPosition;

	private bool launching;
	private bool returning;

	/// <summary>
	/// Launch spikes at the specified speed.
	/// Returns true when it reaches targetPos
	/// </summary>
	/// <param name="speed">Speed.</param>
	public bool Launch(float speed)
	{
		float distance = speed * Time.deltaTime;

		if(distanceCovered + distance < launchRange)
		{
			transform.Translate (transform.up * distance, Space.World);
			distanceCovered += distance;
			launching = false;
			return false;
		} 
		else 
		{
			distance = launchRange - distanceCovered;
			distanceCovered = launchRange;
			transform.Translate (transform.up * distance, Space.World);
			if(!launching)
			{
				launching = true;
			}
			return true;
		}
	}
	/// <summary>
	/// Launch spikes at the specified speed.
	/// Returns true when it reaches targetPos
	/// </summary>
	/// <param name="speed">Speed.</param>
	public bool Return(float speed)
	{
		float distance = speed * Time.deltaTime;

		if(distanceCovered - distance > 0f)
		{
			transform.Translate (-transform.up * distance, Space.World);
			distanceCovered -= distance;
			returning = false;
			return false;
		}
		else
		{
			distance = distanceCovered;
			transform.Translate(-transform.up * distance, Space.World);
			distanceCovered = 0f;
			if(!returning)
			{
				returning = true;
			}
			return true;
		}
	}
}
