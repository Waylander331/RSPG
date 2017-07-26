using UnityEngine;
using System.Collections.Generic;

public class Roller : MonoBehaviour, ITriggerable {

	private BoxCollider bc;

	[SerializeField] private int speed;
	private int carpetSpeed;

	[SerializeField] private Vector3 sens = Vector3.zero;

	[SerializeField] private enum Surface{
		Floor, Wall
	};
	[SerializeField] private Surface rollerSurface;

	[SerializeField] private bool isFloor;

	[SerializeField] private bool isReversed;

	public List<TabRollers> listRollers = new List<TabRollers>();

	private Renderer[] texRoller;
	private float offset;
	private int tiling;

	private bool positive;

	[SerializeField] private int newSpeed;

	private Vector3 center;

	private Transform pivotCenter;
	private Vector3 rollerStart;
	private Vector3 rollerEnd;

	private Renderer rollerStartTrans;
	private Renderer rollerEndTrans;

	public float offsetBoute;

	// Use this for initialization
	void Start (){
		rollerStartTrans = listRollers[0].prefab.GetComponent<Renderer>();
		rollerEndTrans = listRollers[listRollers.Count-1].prefab.GetComponent<Renderer>();
		pivotCenter = transform.GetChild(0).GetComponent<Transform>();
		bc = pivotCenter.GetComponent<BoxCollider>();
		rollerStart = listRollers[0].prefab.transform.position;
		rollerEnd = listRollers[listRollers.Count-1].prefab.transform.position;
		texRoller = GetComponentsInChildren<Renderer>();

		for(int i = 3; i < listRollers.Count; i++){
			Vector3 temp = bc.size;
			temp.z += 2;
			bc.size = temp;
		}
		pivotCenter.position = (rollerEnd + rollerStart)/2;

		tiling = 1;

		carpetSpeed = speed;

		switch(rollerSurface){
		case Surface.Floor:
			isFloor = true;
			break;
		case Surface.Wall:
			isFloor = false;
			break;
		}
	}
	
	void Update (){
		offset = Time.time * carpetSpeed/4.5f;
		foreach(Renderer uv in texRoller){
			if(!isReversed){
				uv.material.SetTextureScale("_MainTex", new Vector2(1, tiling));
				uv.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
			}
			else {
				uv.material.SetTextureScale("_MainTex", new Vector2(1, -tiling));
				/*if(uv == rollerStartTrans){
					uv.material.SetTextureOffset("_MainTex", new Vector2(0, offset + offsetBoute));
				} else if(uv == rollerEndTrans){
					uv.material.SetTextureOffset("_MainTex", new Vector2(0, offset + offsetBoute));
				} else uv.material.SetTextureOffset("_MainTex", new Vector2(0, offset));*/
			}
			uv.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
		}
		/*Debug.Log (transform.GetChild (3).GetComponent<Renderer>().materials[0].GetTextureOffset("_MainTex"));
		Debug.Log (transform.GetChild (6).GetComponent<Renderer>().materials[0].GetTextureOffset("_MainTex"));*/
	}


	public void Triggered(EffectList effect){
		if(effect.GetType() == typeof (Effect_Default)){
			isReversed = !isReversed;
		}
		if(effect.GetType() == typeof (Effect_Move)){
			carpetSpeed = newSpeed;
		}
	}
	public void UnTriggered(EffectList effect){
		if(effect.GetType() == typeof (Effect_Default)){
			isReversed = !isReversed;
		}
		if(effect.GetType() == typeof (Effect_Move)){
			carpetSpeed = newSpeed;
		}
	}


	//Accesseurs
	public BoxCollider Bc {
		get {return bc;}
		set {bc = value;}
	}

	public Vector3 Sens {
		get {return sens;}
		set {sens = value;}
	}

	public bool IsFloor {
		get {return isFloor;}
		set {isFloor = value;}
	}

	public bool IsReversed {
		get {return isReversed;}
		set {isReversed = value;}
	}

	public bool Positive {
		get {return positive;}
		set {positive = value;}
	}

	public int NewSpeed {
		get {return newSpeed;}
		set {newSpeed = value;}
	}

	public int CarpetSpeed {
		get {return carpetSpeed;}
		set {carpetSpeed = value;}
	}

}

[System.Serializable]
public class TabRollers{
	public GameObject prefab;
}





