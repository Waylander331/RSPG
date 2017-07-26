using UnityEngine;
using System.Collections;

public class CanonLookAt : MonoBehaviour {

	private BossCanon canon;

	void Start () {
		canon = this.GetComponentInChildren<BossCanon>();
	}

	void Update () {
		if(canon.IsUnTriggered)transform.LookAt (Manager.Instance.Avatar.transform.position);
		//else transform.LookAt (canon.AlternativeTarget.position);
	}

	public void Disable(){
		this.enabled = false;
	}
}
