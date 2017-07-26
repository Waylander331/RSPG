using UnityEngine;
using System.Collections;

public class GenerateDebrisMeshSize : MonoBehaviour 
{
	public GameObject 
		cubeInstances,
		empty;
	
	public float
		scaleWidth = 1f,
		scaleHeight = 1f,
		scaleDepth = 1f,
		distanceBetweenDebris = 0f;
	
	private Vector3 cubeDimension, toDestroyDimension;
	
	
	public bool floor = false;
	
	/*
	 * TODO
	 * Optimization
	 * AddExplosionForce
	 * "Dégradé"
	 */ 
	
	void Start () 
	{
		cubeDimension = cubeInstances.GetComponent<MeshFilter>().sharedMesh.bounds.size;
		cubeDimension.x = cubeDimension.x * scaleWidth + distanceBetweenDebris;
		cubeDimension.y *= cubeDimension.y * scaleHeight + distanceBetweenDebris;
		cubeDimension.z *= cubeDimension.z * scaleDepth + distanceBetweenDebris;
		toDestroyDimension.x = GetComponent<MeshFilter> ().sharedMesh.bounds.size.x * transform.localScale.x;
		toDestroyDimension.y = GetComponent<MeshFilter> ().sharedMesh.bounds.size.y * transform.localScale.y;
		toDestroyDimension.z = GetComponent<MeshFilter> ().sharedMesh.bounds.size.z * transform.localScale.z;
		Debug.Log ("cubeDimension : " + cubeDimension);
		Debug.Log ("toDestroyDimension : " + toDestroyDimension);
	}
	
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			//Instantiate(remains, transform.position, transform.rotation);
			SpawnDebris();
		}
	}
	
	public void SpawnDebris()
	{
		/*
		Vector3 positionOffset = transform.position;
		Quaternion rotationOffset = transform.rotation;

		float 
			width = toDestroyDimension.x,
			height = toDestroyDimension.y,
			depth = toDestroyDimension.z,
			cubeWidth = cubeDimension.x,
			cubeHeight = cubeDimension.y,
			cubeDepth = cubeDimension.z;

		int 
			nbOfCubesOnWidth = (int) Mathf.Floor(width / cubeWidth),
			nbOfCubesOnHeight = (int) Mathf.Floor(height / cubeHeight),
			nbOfCubesOnDepth = (int) Mathf.Floor(depth / cubeDepth);

		GameObject holder = (GameObject) Instantiate (empty, transform.position, transform.rotation);
		// Créé des instances de haut en bas
		for(int i = 0; i < nbOfCubesOnWidth; i++)
		{
			Vector3 spawningPosition = Vector3.zero;
			spawningPosition.x = (positionOffset.x - width/2f + cubeWidth/2f) + i * (positionOffset.x + cubeWidth);
			for(int j = 0; j < nbOfCubesOnHeight; j++)
			{
				spawningPosition.y = (positionOffset.y - height/2f + cubeHeight/2f) + j * (positionOffset.y + cubeHeight);
				for(int z = 0; z < nbOfCubesOnDepth; z++)
				{				
					spawningPosition.z = (positionOffset.z - depth/2f + cubeDepth/2f) + z * (positionOffset.z + cubeDepth);
					GameObject cube = (GameObject) Instantiate(cubeInstances, Vector3.zero, rotationOffset);
					cube.transform.SetParent(holder.transform);
					cube.transform.localPosition = spawningPosition;
				}
			}
		}
		*/
		
		int 
			nbOfCubeOnWidth,
			nbOfCubeOnHeight,
			nbOfCubeOnDepth;
		
		if(floor)
		{
			nbOfCubeOnWidth = Mathf.FloorToInt(toDestroyDimension.x / cubeDimension.x);
			nbOfCubeOnHeight = Mathf.FloorToInt (toDestroyDimension.y / cubeDimension.y);
			nbOfCubeOnDepth = Mathf.FloorToInt (toDestroyDimension.z / cubeDimension.z);
		}
		else
		{
			nbOfCubeOnWidth = Mathf.CeilToInt(toDestroyDimension.x / cubeDimension.x);
			nbOfCubeOnHeight = Mathf.CeilToInt (toDestroyDimension.y / cubeDimension.y);
			nbOfCubeOnDepth = Mathf.CeilToInt (toDestroyDimension.z / cubeDimension.z);
		}
		
		Debug.Log ("nbOfCubeOnWidth : " + nbOfCubeOnWidth);
		Debug.Log ("nbOfCubeOnHeight : " + nbOfCubeOnHeight);
		Debug.Log ("nbOfCubeOnDepth : " + nbOfCubeOnDepth);
		
		float 
			widthOverflow = nbOfCubeOnWidth * cubeDimension.x - toDestroyDimension.x,
			heightOverflow = nbOfCubeOnHeight * cubeDimension.y - toDestroyDimension.y,
			depthOverflow = nbOfCubeOnDepth * cubeDimension.z - toDestroyDimension.z;
		
		Debug.Log ("widthOverflow : " + widthOverflow);
		Debug.Log ("heightOverflow : " + heightOverflow);
		Debug.Log ("depthOverflow : " + depthOverflow);
		GameObject holder = (GameObject) Instantiate (empty, transform.position, Quaternion.identity);
		Vector3 spawnPosition = Vector3.zero;
		int i = 0;
		for (int x = 0; x < nbOfCubeOnWidth; x++) 
		{
			spawnPosition.x = (transform.position.x - toDestroyDimension.x / 2f - widthOverflow /2f) + x * cubeDimension.x;
			for(int y = 0; y < nbOfCubeOnHeight; y++)
			{
				spawnPosition.y = (transform.position.y - toDestroyDimension.y / 2f - heightOverflow /2f) + y * cubeDimension.y;
				for(int z = 0; z < nbOfCubeOnDepth; z++)
				{
					spawnPosition.z = (transform.position.z - toDestroyDimension.z / 2f - depthOverflow /2f) + z * cubeDimension.z;
					GameObject cube = (GameObject) Instantiate (cubeInstances, Vector3.zero, Quaternion.identity);
					cube.transform.SetParent(holder.transform);
					cube.transform.position = spawnPosition;
					cube.transform.localScale = new Vector3(scaleWidth, scaleHeight, scaleDepth);
					//cube.transform.r = holder.transform.rotation;
					Debug.Log ("SpawningCube : " + i);
				}
			}
		}
		holder.transform.rotation = transform.rotation;
		Destroy (gameObject);	
	}
}
