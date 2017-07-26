using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CreditsMaster : MonoBehaviour {
	public TextAsset fichierCredit;
	private string[] contenuLignes;
	private string[]  contenuPolice;
	private int ligneCourante = 0;
	private int lignesTotales = 0;
	private string textComplet;
	public float tempsEntreUpdates = 2f;
	private int index = 0;
	public Text titre;
	public Text noms;
	// Use this for initialization
	void Start () {
		ReadFile();

		InvokeRepeating("Display",1f,tempsEntreUpdates);
	}

	void Update (){
		if(/*!Manager.Instance.IsFromCin && */(Input.GetButtonDown("Fire2") || Input.GetButtonDown ("Pause"))){
			Application.LoadLevel("EcranTitre");
		}
	}

	void ReadFile(){
		//on copie le contenu du fichier dans un string temporaire
		string tempContenu= fichierCredit.text ;
		tempContenu = tempContenu.ToUpper();
		
		
		string[] lignes = tempContenu.Split("\n"[0]);
		
		//maintenant qu'on connait le nombre de lignes, on initialise
		//les tableaux a la bonne grandeur :)
		
		contenuLignes = new string[lignes.Length];
		contenuPolice = new string[lignes.Length];
		
		//on lit chaque ligne et on store dans les tableaux!
		foreach (string ligne in lignes) {
			
			DecortiqueEtStore(ligne);
			
		}
		for(int i=0;i<lignesTotales; i++){
			textComplet+=contenuLignes[i] +"\n";
		}
		//lastLine = contenuLignes.Length -1;
	}
	void DecortiqueEtStore(string ligne){
		//on regarde où se trouve le # (fin de nom)
		
		int posDiese = ligne.IndexOf("#");
		
		//On retire tout entre le debut et le diese, 
		if(posDiese >0) {		//0 = au debut, donc "Espace"
			string tempString = ligne.Substring(0,posDiese);
			//string temp2 = tempString;
			
			contenuLignes[ligneCourante] = tempString;
			
			
			string police = ligne.Substring(posDiese+1,1 );
			contenuPolice[ligneCourante] = police;
			
		}
		else {
			contenuLignes[ligneCourante] ="";
			contenuPolice[ligneCourante] = "E";
		}
		ligneCourante++;
		lignesTotales++;
	}	

	void Display(){
		if(contenuPolice[index]=="T"){
			titre.text = contenuLignes[index];
			noms.text = "";
			index++;
		}

		while(contenuPolice[index]!="E"){
			noms.text += contenuLignes[index]+"\n";

			index++;
		
	
		}
		if(contenuPolice[index]=="E"){
			index++;
		}

		if(index >= contenuPolice.Length){ 
			//Manager.Instance.IsFromCin = false;
			Application.LoadLevel(1);
		}
	}

}
