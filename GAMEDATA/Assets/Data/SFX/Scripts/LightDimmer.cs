using UnityEngine;
using System.Collections;

public class LightDimmer : MonoBehaviour {

	public enum TypeOfEffect{
		fluorescent, fire, slowPulse, fastPulse, customPulse
	}
	public TypeOfEffect effect;
	public float maxLuminosity = 1f;
	public float minLuminosity = 0f;
	public float maxColor = 1f;
	public float minColor = 0f;
	public float minDelai = 0.01f;
	public float maxDelai = 0.2f;
	public float delai = 0.01f;
	private Light myLight;
	private float curLuminosity;
	private float curColor;
	public float incLuminosity = 0.01f;
	private TypeOfEffect curEffect;
	private int sens = 1;
	public bool pingPong = true;
	// Use this for initialization
	void Start () {
		myLight = this.GetComponent<Light>();
		SetChange();
	}
	

	/*void UpdateChangement(){
		switch(effect){
		case TypeOfEffect.fluorescent:

			break;
		}
	}*/

	void SetChange(){
		switch(effect){
			case TypeOfEffect.fluorescent:
			RandomLuminosity();
			Invoke ("SetChange",0.001f);
			break;

			case TypeOfEffect.fire:
			RandomLuminosity();
			curColor += Mathf.Sign(Random.Range(-1,1))*Random.Range(0.01f,0.05f);
			curColor = curColor>maxColor?maxColor:(curColor< minColor?  minColor:curColor);
			Color temp = myLight.color;
			temp.g = curColor;
			myLight.color = temp;
			Invoke ("SetChange",Random.Range(minDelai,maxDelai));
			break;

		case TypeOfEffect.slowPulse:
			delai = delai = 0.05f;
			incLuminosity = 0.01f;
			Pulse();
			Invoke ("SetChange",delai);
			break;
		case TypeOfEffect.fastPulse:
			delai = 0.025f;
			incLuminosity = 0.03f;
			Pulse();
			Invoke ("SetChange",delai);
			break;

		case TypeOfEffect.customPulse:

			Pulse();
			Invoke ("SetChange",delai);
			break;
		
		}


	}
	void RandomLuminosity(){
		curLuminosity += Mathf.Sign(Random.Range(-1,1))*Random.Range(0.01f,0.05f);
		curLuminosity = curLuminosity>maxLuminosity?maxLuminosity:(curLuminosity< minLuminosity?  minLuminosity:curLuminosity);
		myLight.intensity = curLuminosity;
	}
	void Pulse(){
		curLuminosity += sens * incLuminosity; //Mathf.PingPong(minLuminosity,maxLuminosity - minLuminosity);
		if(pingPong){
			if(curLuminosity > maxLuminosity){
					curLuminosity = maxLuminosity;
					sens = -1;
			}
			if(curLuminosity < minLuminosity){
					curLuminosity = minLuminosity;
					sens = 1;
			}
		}
		else {
			if(curLuminosity > maxLuminosity){
				curLuminosity = minLuminosity;
			}
			if(curLuminosity < minLuminosity){
				curLuminosity = maxLuminosity;
			}
		}
		myLight.intensity = curLuminosity;
	}
}
