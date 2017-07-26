using UnityEngine;
using System.Collections;

public class MiniRezbox : MonoBehaviour {

	public void Destruction(){
		GetComponentInParent<Rezbox>().Destruction();
	}
	
}
