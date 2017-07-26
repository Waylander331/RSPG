using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LightDimmer))]

public class LightDimmerEditor: Editor {

	private LightDimmer ld;
	void Awake(){
		ld= (LightDimmer) target;
	}

	public  override void OnInspectorGUI(){
		ld.effect = (LightDimmer.TypeOfEffect)EditorGUILayout.EnumPopup("Mode",ld.effect);


		GUILayout.Label("Options");

		switch(ld.effect){
		
		case LightDimmer.TypeOfEffect.fluorescent:
			EditorGUILayout.LabelField("Luminosite Min:", ld.minLuminosity.ToString());
			EditorGUILayout.LabelField("Luminosite Max:", ld.maxLuminosity.ToString());
			EditorGUILayout.MinMaxSlider(ref ld.minLuminosity, ref ld.maxLuminosity, 0f, 3f);
			//GUILayout.
			break;
		case LightDimmer.TypeOfEffect.fire:
			EditorGUILayout.LabelField("Luminosite Min:", ld.minLuminosity.ToString());
			EditorGUILayout.LabelField("Luminosite Max:", ld.maxLuminosity.ToString());
			EditorGUILayout.MinMaxSlider(ref ld.minLuminosity, ref ld.maxLuminosity, 0f, 3f);
		/*	ld.minLuminosity =  EditorGUILayout.FloatField("Luminosite Min", ld.minLuminosity );
			ld.maxLuminosity =  EditorGUILayout.FloatField("Luminosite Max", ld.maxLuminosity);*/
			EditorGUILayout.LabelField("% couleur min:", ld.minColor.ToString());
			EditorGUILayout.LabelField("% couleur Max:", ld.maxColor.ToString());
			EditorGUILayout.MinMaxSlider(ref ld.minColor, ref ld.maxColor, 0f, 1f);
			EditorGUILayout.LabelField("Delai changement min:", ld.minDelai.ToString());
			EditorGUILayout.LabelField("Delai changement  Max:", ld.maxDelai.ToString());
			EditorGUILayout.MinMaxSlider(ref ld.minDelai, ref ld.maxDelai, 0.001f, 0.5f);
		/*	ld.minDelai =  EditorGUILayout.FloatField("Delai min", ld.minDelai );
			ld.maxDelai =  EditorGUILayout.FloatField("Delai Max", ld.maxDelai);*/
			break;
		case LightDimmer.TypeOfEffect.slowPulse:
			ld.pingPong = GUILayout.Toggle(ld.pingPong, "Ping Pong");
			EditorGUILayout.LabelField("Luminosite Min:", ld.minLuminosity.ToString());
			EditorGUILayout.LabelField("Luminosite Max:", ld.maxLuminosity.ToString());
			EditorGUILayout.MinMaxSlider(ref ld.minLuminosity, ref ld.maxLuminosity, 0f, 3f);

			break;
		case LightDimmer.TypeOfEffect.fastPulse:
			ld.pingPong = GUILayout.Toggle(ld.pingPong, "Ping Pong");
			EditorGUILayout.LabelField("Luminosite Min:", ld.minLuminosity.ToString());
			EditorGUILayout.LabelField("Luminosite Max:", ld.maxLuminosity.ToString());
			EditorGUILayout.MinMaxSlider(ref ld.minLuminosity, ref ld.maxLuminosity, 0f, 3f);

			break;
		case LightDimmer.TypeOfEffect.customPulse:
			ld.pingPong = GUILayout.Toggle(ld.pingPong, "Ping Pong");
			ld.incLuminosity =  EditorGUILayout.FloatField("Increments", ld.incLuminosity<0?0:ld.incLuminosity);
			EditorGUILayout.LabelField("Luminosite Min:", ld.minLuminosity.ToString());
			EditorGUILayout.LabelField("Luminosite Max:", ld.maxLuminosity.ToString());
			EditorGUILayout.MinMaxSlider(ref ld.minLuminosity, ref ld.maxLuminosity, 0f, 3f);
			ld.delai =  EditorGUILayout.FloatField("Delai", ld.delai<0.001f?0.001f:ld.delai );
			break;

		}
	}

}
