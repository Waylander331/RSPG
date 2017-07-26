using UnityEngine;
using System.Collections;

public class DestroyFrame : MonoBehaviour 
{
	// Preloaded or generated at runtime
	public bool atRuntime = false; 
	
	// whether the number of debris generated are rounded down
	// or rounded up.
	public bool roundDown = false; 	
	public bool consideringThickness = false;
	public string debrisTag;
	public string debrisLayer;
	
	
	public Vector3 
		frameOffset,
		debrisOffset;
	
	public float 
		explosionPower, 
		explosionRadius,
		explosionUpwardModifier,
		debrisMass = 1f,
		disableHolderColliderDelay = 0.25f;

	public Vector3 directionModifier;
	public float modifierStrength;

	public GameObject
		framePrefab,
		debrisInstances, // Generated debris
		debrisHolder; // Empty gameObject working as parent
	
	public float
		// debris' properties
		scaleWidth = 0.025f,
		scaleHeight = 0.025f,
		scaleDepth = 0.025f,
		distanceBetweenDebris = 0.1f,
		
		// frame's properties
		bufferOnWidth = 0.025f,
		bufferOnHeight = 0.025f,
		bufferOnDepth = 0.025f;


	
	private GameObject[] debrisList; // keep in memory the a tab with all the debris
	private GameObject frame;
	private MeshRenderer 
		frameRenderer;// to enable or disable the frame's renderer
	private Renderer myRenderer; // the destructile object's own renderer used to get its center points (Renderer.bounds.center)
	
	void Start()
	{
		directionModifier.Normalize ();
		//myRenderer = GetComponent<Renderer> ();
		// Instantiate the frame and turn off its renderer, then spawns all the debris
		if (!atRuntime) 
		{
			frame = (GameObject) Instantiate(framePrefab, transform.position + frameOffset, transform.rotation); // Note : Pivot du frame décalé
			frameRenderer = frame.GetComponent<MeshRenderer>();
			frameRenderer.enabled = false;
			SpawnDebris();
		}
		
	}
	
	void Update () 
	{
		if (Input.GetKey (KeyCode.Space)) 
		{
			if (atRuntime) 
				SpawnDebris ();
			else
				ActivateDebris ();
			
			AddExplosionForce();
			debrisHolder.GetComponent<DisableCollider>().InvokeDisable(disableHolderColliderDelay);
			debrisHolder.GetComponent<DirectionModifier>().StartMoving(directionModifier, modifierStrength);
			Destroy (gameObject);
		} 
	}
	
	void ActivateDebris()
	{
		// Correct the frame's and holder's position, and turn on the frame's renderer
		frame.transform.position = transform.position + frameOffset; // Note : Pivot du frame décalé
		frame.transform.rotation = transform.rotation;
		frameRenderer.enabled = true;
		debrisHolder.transform.position = transform.position + debrisOffset;
		debrisHolder.transform.rotation = transform.rotation;
		
		// add a rigidbody to all the debris
		for (int i = 0; i < debrisList.Length; i++)
		{
			debrisList[i].GetComponent<MeshRenderer>().enabled = true;
			Rigidbody rigidbody = debrisList[i].AddComponent<Rigidbody>();
			rigidbody.mass = 1f;
		}
	}
	
	void AddExplosionForce()
	{
		Vector3 explosionPos = transform.position + debrisOffset;
		
		Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
		foreach(Collider hit in colliders) 
		{
			if (hit && hit.tag == debrisTag && hit.GetComponent<Rigidbody>())
			{
				hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPos, explosionRadius, explosionUpwardModifier);
				Debug.Log ("AddExplosionForce");
			}
		}
		
	}
	
	// Function that spawns debris
	// was implemented to act as runtime or on preload.
	void SpawnDebris()
	{
		Vector3 debrisDimension, toDestroyDimension;
		
		int
			nbOfCubesOnWidth,
			nbOfCubesOnHeight,
			nbOfCubesOnDepth,
			nbOfCubes;
		
		// to center the debris placement when they dont exactly match
		// the destructible object
		float
			widthOverflow,
			heightOverflow,
			depthOverflow;
		
		
		// Applies the scale and distanceBetweenDebris
		debrisDimension = debrisInstances.GetComponent<MeshFilter>().sharedMesh.bounds.size;
		debrisDimension.x = debrisDimension.x * scaleWidth + distanceBetweenDebris;
		debrisDimension.y *= debrisDimension.y * scaleHeight + distanceBetweenDebris;
		debrisDimension.z *= debrisDimension.z * scaleDepth + distanceBetweenDebris;
		toDestroyDimension.x = GetComponent<MeshFilter> ().sharedMesh.bounds.size.x * transform.localScale.x;
		toDestroyDimension.y = GetComponent<MeshFilter> ().sharedMesh.bounds.size.y * transform.localScale.y;
		toDestroyDimension.z = GetComponent<MeshFilter> ().sharedMesh.bounds.size.z * transform.localScale.z;
		
		// resize toDestroyDimension considering its thickness
		if (consideringThickness) 
		{
			toDestroyDimension.x -= (2f * bufferOnWidth);
			toDestroyDimension.y -= (2f * bufferOnHeight);
			toDestroyDimension.z -= (2f * bufferOnDepth);
		}
		Debug.Log ("cubeDimension : " + debrisDimension);
		Debug.Log ("toDestroyDimension : " + toDestroyDimension);
		
		// count how much debris will be made in width, height and depth
		if(roundDown)
		{
			nbOfCubesOnWidth = Mathf.FloorToInt(toDestroyDimension.x / debrisDimension.x);
			nbOfCubesOnHeight = Mathf.FloorToInt (toDestroyDimension.y / debrisDimension.y);
			nbOfCubesOnDepth = Mathf.FloorToInt (toDestroyDimension.z / debrisDimension.z);
		}
		else
		{
			nbOfCubesOnWidth = Mathf.CeilToInt(toDestroyDimension.x / debrisDimension.x);
			nbOfCubesOnHeight = Mathf.CeilToInt (toDestroyDimension.y / debrisDimension.y);
			nbOfCubesOnDepth = Mathf.CeilToInt (toDestroyDimension.z / debrisDimension.z);
		}
		
		nbOfCubes = nbOfCubesOnWidth * nbOfCubesOnHeight * nbOfCubesOnDepth;
		debrisList = new GameObject[nbOfCubes];
		Debug.Log ("Tab Length : " + debrisList.Length);
		
		Debug.Log ("nbOfCubeOnWidth : " + nbOfCubesOnWidth);
		Debug.Log ("nbOfCubeOnHeight : " + nbOfCubesOnHeight);
		Debug.Log ("nbOfCubeOnDepth : " + nbOfCubesOnDepth);
		
		// the difference between the lengths of the debris and that of the destroyedObject
		// used to center the debris
		widthOverflow = nbOfCubesOnWidth * debrisDimension.x - toDestroyDimension.x;
		heightOverflow = nbOfCubesOnHeight * debrisDimension.y - toDestroyDimension.y;
		depthOverflow = nbOfCubesOnDepth * debrisDimension.z - toDestroyDimension.z;
		
		Debug.Log ("widthOverflow : " + widthOverflow);
		Debug.Log ("heightOverflow : " + heightOverflow);
		Debug.Log ("depthOverflow : " + depthOverflow);
		debrisHolder = (GameObject) Instantiate (debrisHolder, transform.position + debrisOffset, Quaternion.identity);
		
		Vector3 spawnPosition = Vector3.zero;
		
		// place the cubes relative to the destructibleObject center (pivot point)
		
		int i = 0;
		for (int x = 0; x < nbOfCubesOnWidth; x++) 
		{
			spawnPosition.x = (transform.position.x - toDestroyDimension.x/2f - widthOverflow/2f + debrisDimension.x/2f) + x * debrisDimension.x;
			for(int y = 0; y < nbOfCubesOnHeight; y++)
			{
				spawnPosition.y = ((transform.position.y - toDestroyDimension.y/2f - heightOverflow/2f + debrisDimension.y/2f) + y * debrisDimension.y);
				for(int z = 0; z < nbOfCubesOnDepth; z++)
				{
					spawnPosition.z = (transform.position.z - toDestroyDimension.z / 2f - depthOverflow /2f + debrisDimension.z/2f) + z * debrisDimension.z;
					GameObject instance = (GameObject) Instantiate (debrisInstances, Vector3.zero, Quaternion.identity);
					instance.transform.SetParent(debrisHolder.transform);
					instance.transform.position = spawnPosition + debrisOffset;
					instance.transform.localScale = new Vector3(scaleWidth, scaleHeight, scaleDepth);
					instance.tag = debrisTag;
					instance.layer = LayerMask.NameToLayer(debrisLayer);
					//cube.transform.r = holder.transform.rotation;
					
					if(!atRuntime)
					{
						debrisList[i++] = instance;
						instance.GetComponent<MeshRenderer>().enabled = false;
						Destroy (instance.GetComponent<Rigidbody>());
					}
				}
			}
		}
		
		if (atRuntime)
		{
			Instantiate (framePrefab, transform.position + frameOffset, transform.rotation); // Note : Pivot du frame décalé
			debrisHolder.transform.rotation = transform.rotation;
		}
	}
}
