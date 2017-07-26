using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class light_controller : MonoBehaviour, ITriggerable {

	// POUR ENLEVER UN BUG AVEC L'INTERFACE ITRIGGERABLE
	private EffectList effect;
	public bool activeOnStart;
	public List<Light> myLights;
	void Start(){
		if (activeOnStart) {
			foreach (Light obj in myLights) {
				obj.enabled = true;
			}
		} else {
			foreach (Light obj in myLights) {
				obj.enabled = false;
			}
		}
	}
	public void Triggered(EffectList effect){
		if(effect.GetType() == typeof(Effect_Default)){
			foreach(Light obj in myLights){
				obj.enabled = true;
			}
			
		}
	}
	
	public void UnTriggered(EffectList effect){
		if(effect.GetType() == typeof(Effect_Default)){
			foreach (Light obj in myLights) {
				obj.enabled = false;
			}
			
		}
		
	}
}
