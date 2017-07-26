using UnityEngine;
using System.Collections;

public class FootprintSFX : MonoBehaviour 
{
	private bool fading;
	private float currentAlpha;
	private const float alphaDecreaseSpeed = 1f;
	private const float alphaDecreaseDelay = 3f; 
	private Material mat;

	void Start()
	{
		mat = GetComponent<MeshRenderer> ().material;
		Invoke ("DecreaseAlpha", alphaDecreaseDelay);
		currentAlpha = mat.color.a;
	}

	void Update()
	{
		if(fading)
		{
			if(currentAlpha < 0f)
			{
				Destroy(gameObject);
			}
			else
			{
				Color newAlpha = mat.color;
				currentAlpha = newAlpha.a -= alphaDecreaseSpeed * Time.deltaTime;
				mat.color = newAlpha;
			}
		}
	}

	void DecreaseAlpha()
	{
		fading = true;
	}
}
