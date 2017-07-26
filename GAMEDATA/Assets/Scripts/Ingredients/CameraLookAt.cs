using UnityEngine;
using System.Collections;

public class CameraLookAt : MonoBehaviour 
{
	private Transform topCamera;
	private float initialHeight;

	void Start()
	{
		topCamera = transform.GetChild (0);
	}

	void Update()
	{
		topCamera.LookAt (Manager.Instance.Avatar.transform.position + Vector3.up * 1f);
		//Vector3 eulers = topCamera.eulerAngles;
		//topCamera.rotation = Quaternion.Euler (0f, eulers.y, 0f);
	}

	void OnBecameVisible()
	{
		Debug.Log ("BecameVisible");
		enabled = true;
	}

	void OnBecameInvisible()
	{
		Debug.Log ("BecameInvisible");
		enabled = false;
	}
}
