using UnityEngine;
using System.Collections;

public class XCheck : MonoBehaviour 
{
	public bool bossLights = false;
	public int level;

	public GameObject initialObject;
	public GameObject swapObject;

	public GameObject[] myCheck;
	
	private Mesh initialMesh;
	private Mesh swapMesh;
	private MeshRenderer initialRend;
	private MeshRenderer swapRend;

	private MeshFilter meshFilt;
	

	private int starTaken;


	
	// Use this for initialization
	void Start () 
	{
		initialMesh = initialObject.GetComponent<MeshFilter>().sharedMesh;
		swapMesh = swapObject.GetComponent<MeshFilter>().sharedMesh;

		initialRend = initialObject.GetComponent<MeshRenderer>();
		swapRend = swapObject.GetComponent<MeshRenderer>();

		SwitchMesh();
	}

	void OnLevelWasLoaded()
	{
		initialMesh = initialObject.GetComponent<MeshFilter>().sharedMesh;
		swapMesh = swapObject.GetComponent<MeshFilter>().sharedMesh;
		
		initialRend = initialObject.GetComponent<MeshRenderer>();
		swapRend = swapObject.GetComponent<MeshRenderer>();
		SwitchMesh();
	}


	void SwitchMesh()
	{

		if(!bossLights)
		{
			starTaken = Manager.Instance.StarBin[level];	

			switch(starTaken)
			{

			case 6:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("monkey1"))
					{
						//obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;
				
			case 5:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("monkey1"))
					{
						//obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;
				
			case 4:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("monkey1") || obj.name == ("monkey2"))
					{
						//obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;
				
			case 3:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("monkey1"))
					{
						//obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;
				
			case 2:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("monkey1") || obj.name == ("monkey2"))
					{
						//obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;
				
			case 1:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("monkey1") || obj.name == ("monkey2"))
					{
						//obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;
				
			case 0:
				foreach(GameObject obj in myCheck)
				{
					//obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
					obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
				}
				break;
			}
		}
		else if (bossLights)
		{
			starTaken = Manager.Instance.TotalStarsTaken;

			switch(starTaken)
			{
			case 1:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 2:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 3:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 4:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 5:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 6:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 7:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 8:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 9:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7") || obj.name == ("Cross 8"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 10:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7") || obj.name == ("Cross 8") || obj.name == ("Cross 9"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 11:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7") || obj.name == ("Cross 8") || obj.name == ("Cross 9")
					   || obj.name == ("Cross 10"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 12:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7") || obj.name == ("Cross 8") || obj.name == ("Cross 9")
					   || obj.name == ("Cross 10") || obj.name == ("Cross 11"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 13:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7") || obj.name == ("Cross 8") || obj.name == ("Cross 9")
					   || obj.name == ("Cross 10") || obj.name == ("Cross 11") || obj.name == ("Cross 12"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 14:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7") || obj.name == ("Cross 8") || obj.name == ("Cross 9")
					   || obj.name == ("Cross 10") || obj.name == ("Cross 11") || obj.name == ("Cross 12") || obj.name == ("Cross 13"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			case 15:
				foreach(GameObject obj in myCheck)
				{
					if(obj.name == ("Cross") || obj.name == ("Cross 1")|| obj.name == ("Cross 2") || obj.name == ("Cross 3") || obj.name == ("Cross 4")
					   || obj.name == ("Cross 5") || obj.name == ("Cross 6") || obj.name ==("Cross 7") || obj.name == ("Cross 8") || obj.name == ("Cross 9")
					   || obj.name == ("Cross 10") || obj.name == ("Cross 11") || obj.name == ("Cross 12") || obj.name == ("Cross 13") || obj.name == ("Cross 14"))
					{
						obj.GetComponent<MeshFilter>().sharedMesh = swapMesh;
						obj.GetComponent<MeshRenderer>().sharedMaterial = swapRend.sharedMaterial;
					}
				}
				break;

			}
		}
	}
}







