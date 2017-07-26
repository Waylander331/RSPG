using UnityEngine;
using System.Collections;

public class BirdsParticles : MonoBehaviour 
{
	public float radius = 1f;

	// Rotation
	public bool rotating = true;
	public float rotationSpeed = 200f;

	// rotationNoise
	public bool rotationNoise = true;
	public float 
		noiseSpeed = 0.02f,
		noiseStrength = 0.5f;

	// scaleNoise
	public bool 
		scaleNoise = true;
	public float
		scaleSpeed = 0.05f,
		scaleStrength = 0.05f;

	private bool
		isRotating = false,
		isRotationNoising = false,
		isScaleNoising = false;

	private bool spawned = false;
	private float 
		xModifier, 
		yModifier;
	private Quaternion initialRotation;
	private bool turnedOff = true;

	private bool scaleTurnedOff = true;

	private float initialScale; 

	private float rotationNoiseValue = 0f;
	private float scaleNoiseValue = 0f;

	public GameObject birdInstance;
	private Vector3 initScale;
	private float scaleModifier;

	void Start()
	{
		initialRotation = transform.rotation;
		initScale = transform.localScale;
	}

	public void SpawnBirds()
	{
		for (int i = 0; i < 4; i++) 
		{
			GameObject instance = (GameObject) Instantiate (birdInstance, Vector3.zero, Quaternion.identity);
			instance.transform.SetParent(transform);
			Vector3 localPos = Vector3.zero;
			switch(i)
			{
				case 0:
					localPos.x = radius;
					break;
				case 1:
					localPos.x = -radius;
					break;
				case 2:
					localPos.y = radius;
					break;
				case 3:
					localPos.y = -radius;
					break;
			}
			instance.transform.localPosition = localPos;
		}
		isRotating = rotating;
		isRotationNoising = rotationNoise;
		isScaleNoising = scaleNoise;
	}

	public void DestroyBirds()
	{
		int nbChild = transform.childCount;

		for (int i = 0; i < nbChild; i++)
			Destroy (transform.GetChild(i).gameObject);

		isRotating = false;
		isRotationNoising = false;
		isScaleNoising = false;
		Straighten ();
		InitializeScale ();
	}

	void Update()
	{
		UpdateNoiseModifiers ();

		// Rotation
		if(isRotating)
			transform.Rotate (new Vector3(0f, 0f, rotationSpeed) * Time.deltaTime, Space.Self);

		// Rotation Noise
		if (isRotationNoising) 
		{
			transform.Rotate(new Vector3(xModifier, yModifier, 0f) * noiseStrength);
		}

		rotationNoiseValue += noiseSpeed;

		// Scale Noise
		if (isScaleNoising) 
		{
			transform.localScale = new Vector3(initScale.x + scaleStrength * scaleModifier, initScale.y + scaleStrength * scaleModifier, 0f);
		}
		scaleNoiseValue += scaleSpeed;
		
		if (!rotationNoise && !turnedOff) 
		{
			turnedOff = true;
			Straighten();
		}

		if (rotationNoise && turnedOff) 
		{
			turnedOff = false;
		}

		if (!scaleNoise && !scaleTurnedOff) 
		{
			scaleTurnedOff = true;
			InitializeScale();
		}
		
		if (scaleNoise && scaleTurnedOff) 
		{
			scaleTurnedOff = false;
		}
	}

	void InitializeScale()
	{
		transform.localScale = initScale;
	}

	void UpdateNoiseModifiers()
	{
		xModifier = Mathf.Cos (rotationNoiseValue);
		yModifier = Mathf.Sin (rotationNoiseValue);
		scaleModifier = Mathf.Sin (scaleNoiseValue);
	}

	void Straighten()
	{
		transform.rotation = initialRotation;
	}

	public bool Spawned {
		get {
			return spawned;
		}
		set {
			spawned = value;
		}
	}
}
