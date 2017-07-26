using UnityEngine;
using System.Collections;

[System.Serializable]
public class AnimCouleurV2 : MonoBehaviour {
	
	//On veut savoir quels channels affecter
	public bool affecteRouge = false;
	public bool affecteVert = false;
	public bool affecteBleu = false;
	public bool affecteAlpha = false;
	public float rate = 0.1f;
	//ping pong = aller retour dans la variation, sinon cyclique 
	public bool modePingPong = false;
	public bool fluctuationsAleatoires = false; //sinon, on utilise fluctu min
	//Contient la variation de couleurs / frame
	private Vector4 fluctuationsRGBA = new Vector4(0.1f,0.1f,0.1f,0f);
	//private Vector4 fluctuationsMaxRGBA = new Vector4(0.1f,0.1f,0.1f,0f);

	public float fluctuationRougeMin = 0f;
	public float fluctuationRougeMax = 1f;
	public float fluctuationVertMin = 0f;
	public float fluctuationVertMax = 1f;
	public float fluctuationBleuMin = 0f;
	public float fluctuationBleuMax = 1f;
	public float fluctuationAlphaMin = 0f;
	public float fluctuationAlphaMax = 1f;

	//Pour standard
	public float coefficientR = 0.1f;
	public float coefficientG = 0.1f;
	public float coefficientB = 0.1f;
	public float coefficientA = 0.1f;
	//couleurs de depart (penser min = basses valeurs RGB)
	public Color couleurPale = Color.white;
	public Color couleurFonce = Color.black;

	//public bool affecteTint = false;
	
	private Color couleur;
	private Vector4 fluctus; //garde les fluctuations a effectuer
	private Vector4 plusFonce; //extrait des couleurs (manips plus simples)
	private Vector4 plusPale ; //extrait des couleurs (manips plus simples)
	private Vector4 sens = new Vector4(1,1,1,1); // Pour Ping pong.... 1 => + , -1 => - :)
	private Renderer myRend;
	// Use this for initialization
	void Start () {
		//Extraction des valeurs
		plusFonce = new Vector4(couleurFonce.r,couleurFonce.g, couleurFonce.b, couleurFonce.a);
		plusPale = new Vector4(couleurPale.r,couleurPale.g, couleurPale.b, couleurPale.a);
		couleur = plusFonce;

		
		//On pose deja la valeur des fluctuations (si c'est random, on l'ajustera chaque frame)

		myRend = this.GetComponent<Renderer>();
		if(rate!=0f){
			Invoke("AdapteEtAffecte",rate);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(rate == 0){
			AdapteEtAffecte();
		}

	}

	void AdapteEtAffecte(){

		AdapteCouleur();
		AAffecter();
		if(rate!=0){
			Invoke("AdapteEtAffecte",rate);
		}
	}


	void AdapteCouleur(){

		if(fluctuationsAleatoires){
			fluctus = new Vector4(affecteRouge?Random.Range(fluctuationRougeMin,fluctuationRougeMax):0f, 
			                      affecteVert?Random.Range(fluctuationVertMin,fluctuationVertMax):0f,
			                      affecteBleu?Random.Range(fluctuationBleuMin,fluctuationBleuMax):0f,
			                      affecteAlpha? Random.Range(fluctuationAlphaMin,fluctuationAlphaMax):0f);

		}
		else{
			//Quelles channels doivent varier?
			fluctuationsRGBA = new Vector4(affecteRouge?coefficientR:0f, affecteVert?coefficientG:0f,
			                               affecteBleu?coefficientB:0f, affecteAlpha?coefficientA:0f);
			
			fluctus = fluctuationsRGBA;
		}

		couleur = new Vector4 (couleur.r+sens.x*fluctus.x,couleur.g+sens.y*fluctus.y,couleur.b+sens.z*fluctus.z,couleur.a+sens.w*fluctus.w);
		myRend.material.color = couleur;
	}

	void AAffecter(){
		if(affecteRouge){
			if(couleur.r>=plusPale.x){
				if(modePingPong){
					sens.x = -1;
					
				}else{
					couleur.r = plusFonce.x;
				}
			}else if(couleur.r<=plusFonce.x){
				if(modePingPong){
					sens.x = 1;
					
				}else{
					couleur.r = plusPale.x;
				}
			}
		}
		if(affecteVert){
			if(couleur.g>=plusPale.y){
				if(modePingPong){
					sens.y = -1;
					
				}else{
					couleur.g = plusFonce.y;
				}				
			}else if(couleur.g<=plusFonce.y){
				if(modePingPong){
					sens.y = 1;
					
				}else{
					couleur.g = plusPale.y;
				}				
			}		
		}
		if(affecteBleu){
			if(couleur.b>=plusPale.z){
				if(modePingPong){
					sens.z = -1;
					
				}else{
					couleur.b = plusFonce.z;
				}						
			}else if(couleur.b<=plusFonce.z){
				if(modePingPong){
					sens.z = 1;
					
				}else{
					couleur.b = plusPale.z;
				}	
			}		
		}
		if(affecteAlpha){
			if(couleur.a>=plusPale.w){
				if(modePingPong){
					sens.w = -1;
					
				}else{
					couleur.a = plusFonce.w;
				}	
			}else if(couleur.a<=plusFonce.w){
				if(modePingPong){
					sens.w = 1;
					
				}else{
					couleur.a = plusPale.w;
				}	
			}		
		}

	}

}


