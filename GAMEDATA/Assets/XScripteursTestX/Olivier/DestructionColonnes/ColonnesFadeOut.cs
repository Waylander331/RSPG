using UnityEngine;
using System.Collections;

public class ColonnesFadeOut : MonoBehaviour 
{
	public float alphaDecreaseIncrement = 0.02f;
	public float fadingDelay;
	private float currentAlpha = 1f;
	private Rigidbody rb;
	public float delayBeforeCollisionDetect;
	private bool detectingCollision;
	private bool collisionDetected;

	private bool fading;
	private MeshRenderer mesh;

	public Rigidbody RB
	{
		set{rb = value;}
	}

	void Start()
	{
		mesh = GetComponent<MeshRenderer>();
		currentAlpha = mesh.material.color.a;
	}

	void Update()
	{
		if (fading) 
		{
			currentAlpha -= alphaDecreaseIncrement;
			Color newColor = mesh.material.color;
			newColor.a = currentAlpha;
			mesh.material.color = newColor;
		}
		if (currentAlpha <= 0f)
			Destroy (gameObject);


	}

	void OnCollisionStay(Collision col)
	{
		if(detectingCollision && !collisionDetected)
		{
			if(col.gameObject.layer == LayerMask.NameToLayer("Collision"))
			{
				collisionDetected = true;
				Invoke ("Fade", fadingDelay);
			}
		}

	}

	public void InvokeDetectCollision()
	{
		Invoke ("DetectCollisions", delayBeforeCollisionDetect);
	}

	void Fade()
	{
		fading = true;
		Destroy(GetComponent<Rigidbody> ());
	}

	void DetectCollisions()
	{
		detectingCollision = true;
	}
}
