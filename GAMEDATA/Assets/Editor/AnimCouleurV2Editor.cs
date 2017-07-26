using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AnimCouleurV2))]
public class AnimCouleurV2Editor : Editor
{
	private AnimCouleurV2 ac;

	void Awake(){
		ac = (AnimCouleurV2) target;
	}

	public  override void OnInspectorGUI()
	{



		GUILayout.Label("Options");
		ac.modePingPong =  GUILayout.Toggle(ac.modePingPong, "Mode Ping Pong");

		ac.rate = EditorGUILayout.Slider ("Delai avant de changer",ac.rate,0f,3f);

		GUILayout.Label("Channels affectes");
		ac.affecteRouge = GUILayout.Toggle(ac.affecteRouge, "Affecte le rouge");
		ac.affecteVert = GUILayout.Toggle(ac.affecteVert, "Affecte le vert");
		ac.affecteBleu = GUILayout.Toggle(ac.affecteBleu, "Affecte le bleu");
		ac.affecteAlpha = GUILayout.Toggle(ac.affecteAlpha, "Affecte l'Alpha");
		GUILayout.Label("Modes");
		ac.fluctuationsAleatoires = GUILayout.Toggle(ac.fluctuationsAleatoires, "Mode numerique");




		//ping pong = aller retour dans la variation, sinon cyclique 

	//	Debug.Log("Fluct: "+ ac.fluctuationsAleatoires);
	if(ac.fluctuationsAleatoires)
		{	GUILayout.Label("Mode aleatoire entre valeurs");
			ac.fluctuationRougeMin = EditorGUILayout.Slider("Rouge Min: ",ac.fluctuationRougeMin ,0f,1f);
			ac.fluctuationRougeMax = EditorGUILayout.Slider("Rouge Max: ",ac.fluctuationRougeMax ,0f,1f);
			ac.fluctuationVertMin = EditorGUILayout.Slider("Vert Min: ",ac.fluctuationVertMin ,0f,1f);
			ac.fluctuationVertMax = EditorGUILayout.Slider("Vert Max: ",ac.fluctuationVertMax ,0f,1f);
			ac.fluctuationBleuMin = EditorGUILayout.Slider("Bleu Min: ",ac.fluctuationBleuMin ,0f,1f);
			ac.fluctuationBleuMax = EditorGUILayout.Slider("Bleu Max: ",ac.fluctuationBleuMax ,0f,1f);
			ac.fluctuationAlphaMin = EditorGUILayout.Slider("Bleu Min: ",ac.fluctuationBleuMin ,0f,1f);
			ac.fluctuationAlphaMax = EditorGUILayout.Slider("Bleu Max: ",ac.fluctuationBleuMax ,0f,1f);
		}
		else {
			GUILayout.Label("Variations par couleur");
			ac.coefficientR = EditorGUILayout.Slider ("Facteur R",ac.coefficientR,0f,1f);
			ac.coefficientG = EditorGUILayout.Slider ("Facteur G",ac.coefficientG,0f,1f);
			ac.coefficientB = EditorGUILayout.Slider ("Facteur B",ac.coefficientB,0f,1f);
			ac.coefficientA = EditorGUILayout.Slider ("Facteur A",ac.coefficientA,0f,1f);
			ac.couleurPale = EditorGUILayout.ColorField("Plus pale",ac.couleurPale);
			ac.couleurFonce = EditorGUILayout.ColorField("Plus fonce",ac.couleurFonce);
		}

	}
}
