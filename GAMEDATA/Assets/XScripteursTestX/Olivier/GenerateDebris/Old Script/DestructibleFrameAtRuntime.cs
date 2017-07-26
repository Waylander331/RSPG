using UnityEngine;
using System.Collections;

public class DestructibleFrame : MonoBehaviour 
{

	public GameObject 
		debrisInstances,
		debrisHolder;
	
	public float
		scaleWidth = 1f,
		scaleHeight = 1f,
		scaleDepth = 1f,
		distanceBetweenDebris = 0f,
		frameThicknessOnWidth,
		frameThicknessOnHeight;
	
	private Vector3 debrisDimension, toDestroyDimension;
	
	private int
		nbOfCubeOnWidth,
		nbOfCubeOnHeight,
		nbOfCubeOnDepth,
		nbOfCubes;
	
	float
		widthOverflow,
		heightOverflow,
		depthOverflow;
	
	public bool roundDown = true;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
	
	private GameObject holder;
	
	private GameObject[] children;
	
	/*
	 * TODO
	 * Optimization
	 * AddExplosionForce
	 * "Dégradé"
	 */ 
	
	void Start () 
	{
		debrisDimension = debrisInstances.GetComponent<MeshRenderer>().bounds.size;
		debrisDimension.x = debrisDimension.x * scaleWidth + distanceBetweenDebris;
		debrisDimension.y *= debrisDimension.y * scaleHeight + distanceBetweenDebris;
		debrisDimension.z *= debrisDimension.z * scaleDepth + distanceBetweenDebris;

		toDestroyDimension = GetComponent<MeshRenderer>().bounds.size;
		toDestroyDimension.x -= 2f * frameThicknessOnWidth;
		toDestroyDimension.y -= 2f * frameThicknessOnHeight;
		Debug.Log ("cubeDimension : " + debrisDimension);
		Debug.Log ("toDestroyDimension : " + toDestroyDimension);
		
		// Debris' info
		
		if(roundDown)
		{
			nbOfCubeOnWidth = Mathf.FloorToInt(toDestroyDimension.x / debrisDimension.x);
			nbOfCubeOnHeight = Mathf.FloorToInt (toDestroyDimension.y / debrisDimension.y);
			nbOfCubeOnDepth = Mathf.FloorToInt (toDestroyDimension.z / debrisDimension.z);
		}
		else
		{
			nbOfCubeOnWidth = Mathf.CeilToInt(toDestroyDimension.x / debrisDimension.x);
			nbOfCubeOnHeight = Mathf.CeilToInt (toDestroyDimension.y / debrisDimension.y);
			nbOfCubeOnDepth = Mathf.CeilToInt (toDestroyDimension.z / debrisDimension.z);
		}
		nbOfCubes = nbOfCubeOnWidth * nbOfCubeOnHeight * nbOfCubeOnDepth;
		
		Debug.Log ("nbOfCubeOnWidth : " + nbOfCubeOnWidth);
		Debug.Log ("nbOfCubeOnHeight : " + nbOfCubeOnHeight);
		Debug.Log ("nbOfCubeOnDepth : " + nbOfCubeOnDepth);
		Debug.Log ("nbOfCubes : " + nbOfCubes);
		
		widthOverflow = nbOfCubeOnWidth * debrisDimension.x - toDestroyDimension.x;
		heightOverflow = nbOfCubeOnHeight * debrisDimension.y - toDestroyDimension.y;
		depthOverflow = nbOfCubeOnDepth * debrisDimension.z - toDestroyDimension.z;
		
		Debug.Log ("widthOverflow : " + widthOverflow);
		Debug.Log ("heightOverflow : " + heightOverflow);
		Debug.Log ("depthOverflow : " + depthOverflow);
		holder = (GameObject) Instantiate (debrisHolder, transform.position, transform.rotation);
		
		Vector3 spawnPosition = Vector3.zero;
		
		for (int x = 0; x < nbOfCubeOnWidth; x++) 
		{
			spawnPosition.x = (transform.position.x - toDestroyDimension.x / 2f - widthOverflow / 2f + debrisDimension.x / 2f) + x * debrisDimension.x;
			for(int y = 0; y < nbOfCubeOnHeight; y++)
			{
				spawnPosition.y = (transform.position.y - toDestroyDimension.y / 2f - heightOverflow / 2f + debrisDimension.x / 2f) + y * debrisDimension.y;
				for(int z = 0; z < nbOfCubeOnDepth; z++)
				{
					spawnPosition.z = (transform.position.z - toDestroyDimension.z / 2f - depthOverflow / 2f + debrisDimension.x / 2f) + z * debrisDimension.z;
					GameObject cube = (GameObject) Instantiate (debrisInstances, Vector3.zero, Quaternion.identity);
					cube.transform.SetParent(holder.transform);
					cube.transform.position = spawnPosition;
					cube.transform.localScale = new Vector3(scaleWidth, scaleHeight, scaleDepth);
					Quaternion rotation = new Quaternion();
					rotation.Set(0f, 0f, 0f, 1f);
					cube.transform.localRotation = rotation;
				}
			}
		}
		int childCount = holder.transform.childCount;
		children = new GameObject[childCount];
		Debug.Log ("Children.Length" + children.Length);
		for(int i = 0; i < childCount; i++)
		{
			children[i] = holder.transform.GetChild(i).gameObject;
		}
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			SpawnDebris();
		}
	}
	
	public void SpawnDebris()
	{
		holder.transform.position = transform.position;
		holder.transform.rotation = transform.rotation;
		for (int i = 0; i < children.Length; i++)
		{
			children[i].GetComponent<MeshRenderer>().enabled = true;
			Rigidbody rigidbody = children[i].AddComponent<Rigidbody>();
			rigidbody.mass = 22f;
			//rigidbody.drag = Random.Range (0f, 10f);
		}
		Destroy (gameObject);
	}
}
