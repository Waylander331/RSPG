using UnityEngine;
using System.Collections;


[System.Serializable]
public class scr_AnimCouleur : MonoBehaviour {
	
	//On veut savoir quels channels affecter
	public bool affecteRouge = false;
	public bool affecteVert = false;
	public bool affecteBleu = false;
	public bool affecteAlpha = false;
	
	//ping pong = aller retour dans la variation, sinon cyclique 
	public bool modePingPong = false;
	public bool fluctuationsAleatoires = false; //sinon, on utilise fluctu min
	//Contient la variation de couleurs / frame
	public Vector4 fluctuationsMinRGBA = new Vector4(0.1f,0.1f,0.1f,0f);
	public Vector4 fluctuationsMaxRGBA = new Vector4(0.1f,0.1f,0.1f,0f);
	//couleurs de depart (penser min = basses valeurs RGB)
	public Color couleurMin = Color.black;
	public Color couleurMax = Color.white;
	public bool affecteTint = false;
	
	private Color couleur;
	private Vector4 fluctus; //garde les fluctuations a effectuer
	private Vector4 valeursMin; //extrait des couleurs (manips plus simples)
	private Vector4 valeursMax ; //extrait des couleurs (manips plus simples)
	private Vector4 sens = new Vector4(1,1,1,1); // Pour Ping pong.... 1 => + , -1 => - :)
	private Renderer renderer;
	// Use this for initialization
	void Start () {
		//Extraction des valeurs
		valeursMin = new Vector4(couleurMin.r,couleurMin.g, couleurMin.b, couleurMin.a);
		valeursMax = new Vector4(couleurMax.r,couleurMax.g, couleurMax.b, couleurMax.a);
		couleur = valeursMin;
		//Quelles channels doivent varier?
		fluctuationsMinRGBA = new Vector4(affecteRouge?fluctuationsMinRGBA.x:0f, affecteVert?fluctuationsMinRGBA.y:0f,
			affecteBleu?fluctuationsMinRGBA.z:0f, affecteAlpha?fluctuationsMinRGBA.w:0f);
		
		fluctuationsMaxRGBA = new Vector4(affecteRouge?fluctuationsMaxRGBA.x:0f, affecteVert?fluctuationsMaxRGBA.y:0f,
			affecteBleu?fluctuationsMaxRGBA.z:0f, affecteAlpha?fluctuationsMaxRGBA.w:0f);
		
		//On pose deja la valeur des fluctuations (si c'est random, on l'ajustera chaque frame)
		fluctus = fluctuationsMinRGBA;
		renderer = this.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(fluctuationsAleatoires){
			fluctus = new Vector4(Random.Range(fluctuationsMinRGBA.x,fluctuationsMaxRGBA.x),
				Random.Range(fluctuationsMinRGBA.y,fluctuationsMaxRGBA.y),
				Random.Range(fluctuationsMinRGBA.z,fluctuationsMaxRGBA.z),
				Random.Range(fluctuationsMinRGBA.w,fluctuationsMaxRGBA.w));
			
		}
		//la nouvelle couleur est basee sur la valeur courante +/- les fluctuations voulues
		couleur = new Vector4 (couleur.r+sens.x*fluctus.x,couleur.g+sens.y*fluctus.y,couleur.b+sens.z*fluctus.z,couleur.a+sens.w*fluctus.w);
		
		if(affecteRouge){
			if(couleur.r>=valeursMax.x){
				if(modePingPong){
					sens.x = -1;
					
				}else{
					couleur.r = valeursMin.x;
				}
			}else if(couleur.r<=valeursMin.x){
				if(modePingPong){
					sens.x = 1;
					
				}else{
					couleur.r = valeursMax.x;
				}
			}
		}
		if(affecteVert){
			if(couleur.g>=valeursMax.y){
				if(modePingPong){
					sens.y = -1;
					
				}else{
					couleur.g = valeursMin.y;
				}				
			}else if(couleur.g<=valeursMin.y){
				if(modePingPong){
					sens.y = 1;
					
				}else{
					couleur.g = valeursMax.y;
				}				
			}		
		}
		if(affecteBleu){
			if(couleur.b>=valeursMax.z){
				if(modePingPong){
					sens.z = -1;
					
				}else{
					couleur.b = valeursMin.z;
				}						
			}else if(couleur.b<=valeursMin.z){
				if(modePingPong){
					sens.z = 1;
					
				}else{
					couleur.b = valeursMax.z;
				}	
			}		
		}
		if(affecteAlpha){
			if(couleur.a>=valeursMax.w){
				if(modePingPong){
					sens.w = -1;
					
				}else{
					couleur.a = valeursMin.w;
				}	
			}else if(couleur.a<=valeursMin.w){
				if(modePingPong){
					sens.w = 1;
					
				}else{
					couleur.a = valeursMax.w;
				}	
			}		
		}
		
		if(affecteTint){

			renderer.material.SetColor("_TintColor",couleur);
		}else renderer.material.color = couleur;
		
	}
}


