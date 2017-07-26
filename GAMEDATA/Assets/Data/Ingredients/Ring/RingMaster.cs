using UnityEngine;
using System.Collections;

public class RingMaster : MonoBehaviour {

	//public int nomberOfRings = 3;
	//private bool turnOffEverything = false;
	private int nbRings = 0;
	public bool isTuto;

	/*public enum MyColor {
		blue, red, yellow
	}*/
	/*public MyColor couleur ;*/

	private Ring[] myRings;

	public void Awake(){
		/*Color tempCol = couleur == MyColor.blue?Color.cyan:(couleur == MyColor.yellow? Color.yellow:Color.red);*/
		myRings = GetComponentsInChildren<Ring>();
		if(myRings.Length % 3 != 0){
			Debug.LogError("Yo, faut que tu mette un multiple de 3 pour tes rings, t'as juste a ouvrir le ringMaster et ctrl + D sur une ring :D");
		}
		Renderer[] setColor = this.GetComponentsInChildren<Renderer>();
		foreach(Renderer mat in setColor){
			mat.material.color = /*tempCol;*/Color.cyan;
			mat.material.SetColor("_EmissionColor",/*tempCol*/Color.cyan);
		}
		foreach(Ring ring in myRings){
			ring.MyColor =/* tempCol*/ Color.cyan;
		}
	}

	public void Update(){
		if(isTuto && nbRings == myRings.Length && !Manager.Instance.IsHyped && !Manager.Instance.IsSlow){
			nbRings = 0;
			foreach(Ring ring in myRings){
				ring.gameObject.SetActive(true);
			}
		}
	}

	public void AddOneRing(){
		nbRings ++;
		if(nbRings % 3 == 0){
			CancelInvoke("ResetAll");
			Manager.Instance.IsHyped = true;
		}

	}

}
