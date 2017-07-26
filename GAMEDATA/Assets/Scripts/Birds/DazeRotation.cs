using UnityEngine;
using System.Collections;

public class DazeRotation : MonoBehaviour 
{
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

	private Quaternion initialRotation;
	private bool turnedOff = true;

	private float xModifier, yModifier;
	
	private bool scaleTurnedOff = true;
	
	private float initialScale; 
	
	private float rotationNoiseValue = 0f;
	private float scaleNoiseValue = 0f;

	private Vector3 initScale;
	private float scaleModifier;
	
	void Start()
	{
		initialRotation = transform.rotation;
		initScale = transform.localScale;
	}
	
	void Update()
	{
		UpdateNoiseModifiers ();
		
		// Rotation
		if(rotating)
			transform.Rotate (new Vector3(0f, 0f, rotationSpeed) * Time.deltaTime, Space.Self);
		
		// Rotation Noise
		if (rotationNoise) 
		{
			transform.Rotate(new Vector3(xModifier, yModifier, 0f) * noiseStrength);
		}
		
		rotationNoiseValue += noiseSpeed;
		
		// Scale Noise
		if (scaleNoise) 
		{
			transform.localScale = new Vector3(initScale.x + scaleStrength * scaleModifier, initScale.y + scaleStrength * scaleModifier, initScale.z);
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
}
