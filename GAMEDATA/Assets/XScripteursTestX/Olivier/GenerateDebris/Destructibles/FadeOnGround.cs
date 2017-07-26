using UnityEngine;
using System.Collections;

public class FadeOnGround : MonoBehaviour 
{
	private bool 
		onGround = false,
		fading = false;

	public float 
		fadingDelay,
		fadingIncrement;

	private float currentAlpha;

	private MeshRenderer mesh;

	// Explosion
	Vector3 explosionPosition; 

	public bool OnGround 
	{
		get {return onGround;}
	}

	public bool Fading {
		get {
			return fading;
		}
	}

	void Start()
	{
		mesh = GetComponent<MeshRenderer>();
		currentAlpha = mesh.material.color.a;
		Debug.Log ("Alpha : " + currentAlpha);
	}

	void Update()
	{
		if (fading) 
		{
			currentAlpha -= fadingIncrement;
			Debug.Log ("Current Alpha : " + currentAlpha);
			Color newColor = mesh.material.color;
			newColor.a = currentAlpha;
			mesh.material.color = newColor;
		}
		if (currentAlpha <= 0f)
			Destroy (gameObject);
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log ("OnCollisionEnter");
		if (collision.gameObject.tag == "Ground") 
		{
			Invoke ("Fade", fadingDelay);
			Debug.Log("Fading");
		}
	}

	void Fade()
	{
		fading = true;
	}
}
