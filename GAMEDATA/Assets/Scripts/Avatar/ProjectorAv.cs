using UnityEngine;
using System.Collections;

public class ProjectorAv : MonoBehaviour {
	
	
	private Transform myFocus;
	public int maxDistance;
	private int startMax;
	private Vector3 myFocusY;
	private LightsManager myLightEffect;
	private Light myLight;
	private Color startColor;
	private float startIntensity;
	
	void Awake(){
		startMax = maxDistance;
		myLight = GetComponent<Light>();
		startColor = myLight.color;
		startIntensity = myLight.intensity;
		myLightEffect = GetComponent<LightsManager>();
		myFocusY = Vector3.zero;
		if(myFocus != null){
			myFocusY = myFocus.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(myFocus != null){
			myFocusY = new Vector3(myFocus.position.x,myFocus.position.y + maxDistance, myFocus.position.z);
			this.transform.LookAt(myFocus.position,Vector3.up);
		}
		else{
			myFocus = Manager.Instance.Avatar.transform;
		}
		if(Manager.Instance.IsHyped){
			myLight.intensity = 8;
			myLightEffect.LerpAllColor();
			maxDistance = startMax - 2;
		}
		else{
			myLight.color = startColor;
			myLight.intensity = startIntensity;
			maxDistance = startMax;
		}
	}
	void LateUpdate(){
		if(myFocus != null){
			/*if(Vector3.Distance (this.transform.position,myFocus.position) > maxDistance +0.5f){
				this.transform.position = Vector3.Lerp (this.transform.position,myFocusY,0.01f);
			}
			else{
				if(Vector3.Distance (this.transform.position,myFocus.position) < maxDistance - 0.5f){
					this.transform.position = Vector3.Lerp (this.transform.position ,this.transform.position - myFocusY,0.01f);
				}
			}*/
			this.transform.position = Vector3.Lerp (this.transform.position,myFocusY,Time.deltaTime);
		}
		/*if(this.transform.position.y - myFocus.position.y <= 4f){
			this.transform.position = Vector3.Lerp (this.transform.position,myFocus.transform.position - Vector3.up, 0.01f);
		}*/
	}
}
