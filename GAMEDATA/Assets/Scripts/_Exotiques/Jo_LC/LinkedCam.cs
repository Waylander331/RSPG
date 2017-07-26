using UnityEngine;
using System.Collections;

public class LinkedCam : MonoBehaviour 
{
	public ScreenFlicker linkedCamera;
	private bool isApplied; 

	void Awake (){
		if(Application.loadedLevelName != "CINEMATIQUE_FIN")linkedCamera.GetComponent<LinkedTV>().AddTV(gameObject);
		isApplied = false;
	}

	void Update(){
		if (!isApplied && linkedCamera.RTex.IsCreated()){ 
			RenderTexToMat();
			//Debug.Log ("herp de derp");
		}
	}

	void OnLevelWasLoaded(){
		//if(Application.loadedLevelName != "CINEMATIQUE_FIN" && Application.loadedLevelName != "2_FETEFORAINE_JLC")linkedCamera.GetComponent<LinkedTV>().AddTV(gameObject);
		isApplied = false;
	}

	public void RenderTexToMat(){
		Material[] temp = GetComponent<Renderer>().materials;
		
		int i = 0;

		while (i < temp.Length){
			if (temp[i].name.Contains("Pres")){
				temp[i].SetTexture("_MainTex", linkedCamera.RTex);
				temp[i].SetTexture("_EmissionMap", linkedCamera.RTex);
			}
			i++;
		}
		isApplied = true;
	}

	public ScreenFlicker LinkedCamFlicker
	{
		get{return linkedCamera;}
	}
}
