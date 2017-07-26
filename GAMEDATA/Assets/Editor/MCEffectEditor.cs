using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Effect_MC))]
public class MCEffectEditor : Editor {

	public override void OnInspectorGUI (){

		serializedObject.Update ();
		Effect_MC eff = (Effect_MC) target;

		eff.levelSpecific = EditorGUILayout.Toggle("Level Specific", eff.levelSpecific);
		if(eff.levelSpecific) {
			eff.niveau = (Effect_MC.Level) EditorGUILayout.EnumPopup("Niveau : ", eff.niveau);

			#region Hub Lines 
			if(eff.niveau == Effect_MC.Level.Hub){

				eff.hubLines = (Effect_MC.Hub) EditorGUILayout.EnumPopup("Replique HUB : ", eff.hubLines);

				switch (eff.hubLines){
				case Effect_MC.Hub.FFEtoiles: 
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Hub/FFEtoiles.ogg", typeof (AudioClip)) as AudioClip;
					CaseInquiet(eff);
					if(GUI.changed) Debug.Log("Cher public, nous avons eu un hic temporaire avec nos caméras. Entre-temps, notre petite gazelle a recueilli quelques étoiles... Mille fois pardon.");
					break;
				case Effect_MC.Hub.CanBoss: 
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Hub/CanBoss.ogg", typeof (AudioClip)) as AudioClip;
					CaseInquiet(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Heu... Il y a un problème avec cet élévateur. Ignorez-le.");
					break;
				case Effect_MC.Hub.BossStart: 
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Hub/BossStart.ogg", typeof (AudioClip)) as AudioClip;
					CaseColerique (eff);
					eff.bossFight = true;
					if(GUI.changed) Debug.Log("Non! Je t'avais dit de pas aller là! Petite sotte, tu vas le payer!");
					break;
				case Effect_MC.Hub.Boss1: 
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Hub/Boss1.ogg", typeof (AudioClip)) as AudioClip;
					CaseColerique (eff);
					eff.bossFight = true;
					if(GUI.changed) Debug.Log("Tu n'y arriveras pas!");
					break;
				case Effect_MC.Hub.Boss2: 
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Hub/Boss2.ogg", typeof (AudioClip)) as AudioClip;
					CaseColerique (eff);
					eff.bossFight = true;
					if(GUI.changed) Debug.Log("T'es expulsée! Expulsée j'te dis! Quitte mon plateau!");
					break;
				case Effect_MC.Hub.Boss3:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Hub/Boss3.ogg", typeof (AudioClip)) as AudioClip;
					CasePanique (eff);
					eff.bossFight = true;
					if(GUI.changed) Debug.Log("Si tu arretes maintenant, il y aura du gateau!");
					break;
				case Effect_MC.Hub.Boss4:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Hub/Boss4.ogg", typeof (AudioClip)) as AudioClip;
					CasePanique (eff);
					eff.bossFight = true;
					if(GUI.changed) Debug.Log("Le prix est à toi! J'ai caché la clé dans la porte de sortie! Juré.");
					break;
				}
			}
			#endregion
			#region Tutorial Lines
			else if(eff.niveau == Effect_MC.Level.Tuto){

				eff.tutoLines = (Effect_MC.Tuto) EditorGUILayout.EnumPopup("Replique TUTO : ", eff.tutoLines);

				switch (eff.tutoLines){
				case Effect_MC.Tuto.Entree:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Tuto/Entree.ogg", typeof (AudioClip)) as AudioClip;
					if(GUI.changed) Debug.Log("Candidate quatre-sept-neuf, préparez-vous à votre audition.");
					CaseNeutre(eff);
					eff.bossFight = false;
					break;
				case Effect_MC.Tuto.TutoP1:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/0_Tuto/TutoP1.ogg", typeof (AudioClip)) as AudioClip;
					if(GUI.changed) Debug.Log("Pour votre première épreuve, tentez de franchir ce gouffre pour récupérer le trophée là-haut.");
					CaseNeutre(eff);
					eff.bossFight = false;
					break;
				case Effect_MC.Tuto.TutoP2:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/TutoP2.ogg", typeof (AudioClip)) as AudioClip;
					if(GUI.changed) Debug.Log("Il va te falloir bouger un peu plus cette fois!");
					CaseNeutre(eff);
					eff.bossFight = false;
					break;
				case Effect_MC.Tuto.TutoP3:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/TutoP3.ogg", typeof (AudioClip)) as AudioClip;
					if(GUI.changed) Debug.Log("Je vous présente Brutus, notre homme-fort, et Chester, notre échassier. Détrompez vous; ils ne sont pas là pour vous aider!");
					CaseNargueur (eff);
					eff.bossFight = false;
					break;
				case Effect_MC.Tuto.TutoFini:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/TutoFini.ogg", typeof (AudioClip)) as AudioClip;
					if(GUI.changed) Debug.Log("Félicitations! Vous avez réussi votre audition! Entrez en scène quand vous êtes prète.");
					CaseEnjoue(eff);
					eff.bossFight = false;
					break;
				}
			}
			#endregion
			#region Manege Lines
			else if(eff.niveau == Effect_MC.Level.Manege){

				eff.manegeLines = (Effect_MC.Manege) EditorGUILayout.EnumPopup("Replique MANEGE : ", eff.manegeLines);

				switch (eff.manegeLines){
				case Effect_MC.Manege.Etoile:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/1_Manege/Etoile.ogg", typeof (AudioClip)) as AudioClip;
					CaseBlase(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Prépare toi à embarquer sur un manège digne de tes pires cauchemars!");
					break;
				case Effect_MC.Manege.Prouesse:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/1_Manege/Prouesse.ogg", typeof (AudioClip)) as AudioClip;
					CaseImpressionne(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Wow, elle n'est pas peureuse la petite gazelle!");
					break;
				case Effect_MC.Manege.Quai:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/1_Manege/Quai.ogg", typeof (AudioClip)) as AudioClip;
					CaseNeutre (eff); 
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Prépare toi à embarquer sur un manège digne de tes pires cauchemars!");
					break;
				case Effect_MC.Manege.SurManege1:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/1_Manege/SurManege1.ogg", typeof (AudioClip)) as AudioClip;
					CaseNargueur(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Ça va pas trop vite pour vous?");
					break;
				case Effect_MC.Manege.SurManege2:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/1_Mannege/SurManege2.ogg", typeof (AudioClip)) as AudioClip;
					CaseImpressionne(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Je croyais que tu serais plus étourdie!");
					break;
				case Effect_MC.Manege.OrigineEtoile:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/1_Mannege/OrigineEtoile.ogg", typeof (AudioClip)) as AudioClip;
					CaseEnjoue(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Tu croyais qu'on t'accorderait les points juste en t'y rendant? Ce n'est pas ça l'objectif du jeu, petite.");
					break;
				}
			}
			#endregion
			#region Fete Foraine Lines
			else if(eff.niveau == Effect_MC.Level.FeteForaine){

				eff.feteForaineLines = (Effect_MC.FeteForaine) EditorGUILayout.EnumPopup("Replique FETE FORAINE : ", eff.feteForaineLines);

				switch(eff.feteForaineLines){
				case Effect_MC.FeteForaine.Ecran:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/2_FeteForaine/Ecran.ogg", typeof (AudioClip)) as AudioClip;
					CaseColerique(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Il vaut cher cet écran!");
					break;
				case Effect_MC.FeteForaine.Intro:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/2_FeteForaine/Intro.ogg", typeof (AudioClip)) as AudioClip;
					CaseInquiet(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Ce plateau est en construction. Il n'y a rien à voir ici; les étoiles sont toutes entreposées au grenier.");
					break;
				case Effect_MC.FeteForaine.NoAccess:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/2_FeteForaine/NoAccess.ogg", typeof (AudioClip)) as AudioClip;
					CaseBlase(eff); 
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Tu ne peux pas monter là, on a ôté tous les accès.");
					break;
				case Effect_MC.FeteForaine.YesAccess:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/2_FeteForaine/YesAccess.ogg", typeof (AudioClip)) as AudioClip;
					CaseEnjoue(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Hé! Tu n'as pas le droit d'être là! Ça ne fait pas partie du jeu! Il n'y a pas de cameras!");
					break;
				}
			}
			#endregion
			#region Carrousel Lines
			else if(eff.niveau == Effect_MC.Level.Carrousel){

				eff.carrouselLines = (Effect_MC.Carrousel) EditorGUILayout.EnumPopup("Replique CARROUSEL : ", eff.carrouselLines);

				switch(eff.carrouselLines){
				case Effect_MC.Carrousel.BackstagePF:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/BackstagePF.ogg", typeof (AudioClip)) as AudioClip;
					CaseNargueur (eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("T'as pas le droit d'être là, gamine! Prépare-toi à tomber... enfin.");
					break;
				case Effect_MC.Carrousel.Cage:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/Cage.ogg", typeof (AudioClip)) as AudioClip;
					CaseNargueur (eff); 
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("La gazelle est prise au piège!");
					break;
				case Effect_MC.Carrousel.CageExit:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/CageExit.ogg", typeof (AudioClip)) as AudioClip;
					CaseNeutre (eff); 
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Ah, dommage.");
					break;
				case Effect_MC.Carrousel.EtoileTop:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/EtoileTop.ogg", typeof (AudioClip)) as AudioClip;
					CaseBlase (eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Félicitations! Admirons comme notre compétitrice préférée est fière d'avoir atteint cet objectif si facile.");
					break;
				case Effect_MC.Carrousel.Intro:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/Intro.ogg", typeof (AudioClip)) as AudioClip;
					CaseNeutre (eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Bienvenue dans notre magnifique carrousel! Nos poneys sont une fière commandite de Pferdefleisch incorporé!");
					break;
				case Effect_MC.Carrousel.Timer:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/Timer.ogg", typeof (AudioClip)) as AudioClip;
					CaseEnjoue (eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Oh bon sang! Elle n'y arrivera jamais à temps!");
					break;
				case Effect_MC.Carrousel.BackstageArms:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/BackstageArms.ogg", typeof (AudioClip)) as AudioClip;
					CasePanique (eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Brutus! Brutus! Sors-la de là!");
					break;
				case Effect_MC.Carrousel.Pont:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/3_Carrousel/Pont.ogg", typeof (AudioClip)) as AudioClip;
					CaseNargueur (eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Ah la pauvre! Elle n'a pas compris qu'elle doit faire tourner le pont pour traverser.");
					break;
				}
			}
			#endregion
			#region Chucky Lines
			else if(eff.niveau == Effect_MC.Level.Chucky){

				eff.chuckyLines = (Effect_MC.Chucky) EditorGUILayout.EnumPopup("Replique CHUCKY : ", eff.chuckyLines);

				switch(eff.chuckyLines){
				case Effect_MC.Chucky.Bouche:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/4_Chucky/Intro.ogg", typeof (AudioClip)) as AudioClip;
					CaseEnjoue(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Non! Ne Sautez pas! Ne sautez pas!");
					break;
				case Effect_MC.Chucky.Fesse:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/4_Chucky/Bouche.ogg", typeof (AudioClip)) as AudioClip;
					CasePanique(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("DBon, bravo. Vous êtes entrée. Bonne chance pour sortir, maintenant!");
					break;
				case Effect_MC.Chucky.Intro:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/4_Chucky/Fesse.ogg", typeof (AudioClip)) as AudioClip;
					CaseNargueur(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Admirez notre pièce maîtresse! Aussi mortelle qu'elle est belle!");
					break;
				case Effect_MC.Chucky.SousSol:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/4_Chucky/SousSol.ogg", typeof (AudioClip)) as AudioClip;
					CaseColerique (eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Hé! Pourquoi tu passes par là? Non! Tombe! Allez, tombe!");
					break;
				case Effect_MC.Chucky.PFStar:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/4_Chucky/PFStar.ogg", typeof (AudioClip)) as AudioClip;
					CaseInquiet(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("On a oublié de spécifier dans les règlements que l'arrière-scène est pour nos employés seulement?");
					break;
				case Effect_MC.Chucky.Generateur:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/4_Chucky/Generateur.ogg", typeof (AudioClip)) as AudioClip;
					CaseInquiet(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log("Est-ce qu'on avait oublié de réparer le générateur?");
					break;
				}
			}
			#endregion
			#region Freak Show Lines
			else if(eff.niveau == Effect_MC.Level.FreakShow){

				eff.freakShowLines = (Effect_MC.FreakShow) EditorGUILayout.EnumPopup("Replique FREAKSHOW : ", eff.freakShowLines);

				switch(eff.freakShowLines){
				case Effect_MC.FreakShow.Cabinet:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/Cabinet.ogg", typeof (AudioClip)) as AudioClip;
					CaseEnjoue(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("Bwahaha! Regardez ça, ils sont horribles! Pas trop peur, petite gazelle?");
					break;
				case Effect_MC.FreakShow.Pics:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/Pics.ogg", typeof (AudioClip)) as AudioClip;
					CaseBlase(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("Attention à où tu marches; tu pourrais te transpercer le pied. Ou trébucher et transpercer... Beaucoup plus!");
					break;
				case Effect_MC.FreakShow.CheminDroite:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/CheminDroite.ogg", typeof (AudioClip)) as AudioClip;
					CaseNargueur(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("Ah, tu prends le chemin facile? Quelle déception.");
					break;
				case Effect_MC.FreakShow.Intro:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/Intro.ogg", typeof (AudioClip)) as AudioClip;
					CaseEnjoue(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("C'est ici qu'on garde notre collections de bizarreries. Vous souhaitez vous y joindre?");
					break;
				case Effect_MC.FreakShow.PiliersGauche:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/PiliersGauche.ogg", typeof (AudioClip)) as AudioClip;
					CaseNeutre(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("Ils ne pensent jamais à regarder par en haut...");
					break;
				case Effect_MC.FreakShow.EtoileBStageD:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/EtoileBStageD.ogg", typeof (AudioClip)) as AudioClip;
					CaseInquiet(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("On a oublié de bloquer ce trou?");
					break;
				case Effect_MC.FreakShow.EtoileBStageG:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/EtoileBStageG.ogg", typeof (AudioClip)) as AudioClip;
					CaseInquiet(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("Il n'y a rien à voir par là! Retournez d'où vous êtes arrivée s'il vous plaît!");
					break;
				case Effect_MC.FreakShow.ScieEtoile:
					eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/5_Freakshow/ScieEtoile.ogg", typeof (AudioClip)) as AudioClip;
					CasePanique(eff);
					eff.bossFight = false;
					if(GUI.changed) Debug.Log ("Allez, laisse-toi frapper! Ça chatouille seulement, promis!");
					break;
				}
			}
			#endregion
		}
		#region Generic Line
		else{

			eff.genericLine = (Effect_MC.Replique) EditorGUILayout.EnumPopup("Replique : ", eff.genericLine);

			switch (eff.genericLine){

				#region Mort et Danger
			case Effect_MC.Replique.Mort1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Mort1.ogg", typeof(AudioClip)) as AudioClip;
				CaseBlase(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Dommage. La victoire sera pour une autre fois.");
				break;
			case Effect_MC.Replique.Mort2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Mort2.ogg", typeof(AudioClip)) as AudioClip;
				CaseNargueur(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Voyez, elle n'est pas parfaite après tout.");
				break;
			case Effect_MC.Replique.Mort3 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Mort3.ogg", typeof(AudioClip)) as AudioClip;
				CaseImpressionne(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Mais quelle débarque spectaculaire mesdames et messieux!");
				break;
			case Effect_MC.Replique.Danger1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Danger1.ogg", typeof(AudioClip)) as AudioClip;
				CaseEnjoue(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Restez attentifs; je prédis beaucoup d'action!");
				break;
			case Effect_MC.Replique.Danger2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Danger2.ogg", typeof(AudioClip)) as AudioClip;
				CaseNargueur(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Regardez bien le piège qu'on lui a tendu!");
				break;
			case Effect_MC.Replique.Danger3 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Danger3.ogg", typeof(AudioClip)) as AudioClip;
				CaseBlase(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Elle ne réussira jamais, la petite gazelle.");
				break;
			case Effect_MC.Replique.Danger4:
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Danger4.ogg", typeof(AudioClip)) as AudioClip;
				CaseEnjoue(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Mon pauvre petit coeur ne peut pas supporter toute cette tension!");
				break;
				#endregion
				#region Etoiles
			case Effect_MC.Replique.Etoile1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Etoile1.ogg", typeof(AudioClip)) as AudioClip;
				CaseBlase(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Elle est facile, celle-là.");
				break;
			case Effect_MC.Replique.Etoile2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Etoile2.ogg", typeof(AudioClip)) as AudioClip;
				CaseImpressionne(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Ça alors, c'était vachement inattendu.");
				break;
			case Effect_MC.Replique.Etoile3 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Etoile3.ogg", typeof(AudioClip)) as AudioClip;
				CaseNeutre(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Vos singeries vous mènent un peu plus près du but.");
				break;
			case Effect_MC.Replique.Etoile4 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Etoile4.ogg", typeof(AudioClip)) as AudioClip;
				CasePanique(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Quoi! Tu n'es pas supposée - euh! Bravo?");
				break;
			case Effect_MC.Replique.Etoile5 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Etoile5.ogg", typeof(AudioClip)) as AudioClip;
				CaseInquiet(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Tu ne devrais pas avoir trop de misère à dénicher le grand prix, toi...");
				break;
				#endregion
				#region Ingredients et NMEs
			case Effect_MC.Replique.Bar1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Bar1.ogg", typeof(AudioClip)) as AudioClip;
				CaseImpressionne(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Un vrai petit singe, celle-là!");
				break;
			case Effect_MC.Replique.Bar2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Bar2.ogg", typeof(AudioClip)) as AudioClip;
				CaseNargueur(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Pas trop étourdie?");
				break;
			case Effect_MC.Replique.Roller1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Roller1.ogg", typeof(AudioClip)) as AudioClip;
				CaseNargueur(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Essaie de rester là-dessus, petite!");
				break;
			case Effect_MC.Replique.Roller2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Roller2.ogg", typeof(AudioClip)) as AudioClip;
				CaseImpressionne(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Wow! Il roule vite ce tapis là!");
				break;
			case Effect_MC.Replique.Brutus1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Brutus1.ogg", typeof(AudioClip)) as AudioClip;
				CaseEnjoue(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Brutus! Montre à la jolie gazelle qui est en charge ici!");
				break;
			case Effect_MC.Replique.Brutus2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Brutus2.ogg", typeof(AudioClip)) as AudioClip;
				CaseColerique(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Hé! T'es pas supposé l'aider, elle.");
				break;
			case Effect_MC.Replique.Bridgeman1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Bridgeman1.ogg", typeof(AudioClip)) as AudioClip;
				CaseNeutre(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Regardez là-haut! C'est Chester.");
				break;
			case Effect_MC.Replique.Bridgeman2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Bridgeman2.ogg", typeof(AudioClip)) as AudioClip;
				CaseNargueur(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Vous savez à quel genre de personne on lance des tomates? Aux nuls!");
				break;
			case Effect_MC.Replique.Switch :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Switch.ogg", typeof(AudioClip)) as AudioClip;
				CaseNeutre(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("La quantité de mouvement que tu transmets en vitesse angulaire à la guérite est ensuite transmise à une dynamo alimentant les circuits de... Bah je vous laisserai découvrir le reste!");
				break;
				#endregion
				#region Backstage
			case Effect_MC.Replique.Backstage1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Backstage1.ogg", typeof(AudioClip)) as AudioClip;
				CaseInquiet(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Arrête! La caméra ne te voit pas quand tu passes par là...");
				break;
			case Effect_MC.Replique.Backstage2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Backstage2.ogg", typeof(AudioClip)) as AudioClip;
				CaseColerique(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Pas par là. Pas. Par. Là!");
				break;
			case Effect_MC.Replique.Backstage3 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Backstage3.ogg", typeof(AudioClip)) as AudioClip;
				CaseColerique(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Écoute moi petite sotte. Arrête de tricher comme ça, tu fais baisser nos cottes d'écoute.");
				break;
			case Effect_MC.Replique.Backstage4 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Backstage4.ogg", typeof(AudioClip)) as AudioClip;
				CasePanique(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Elle va gagner. Elle triche, et elle va partir avec le prix. Non, non, je ne la laisserai pas!");
				break;
				#endregion
				#region Reussites
			case Effect_MC.Replique.Reussite1 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Reussite1.ogg", typeof(AudioClip)) as AudioClip;
				CaseImpressionne(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Saperlipopette! Elle est passée!");
				break;
			case Effect_MC.Replique.Reussite2 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Reussite2.ogg", typeof(AudioClip)) as AudioClip;
				CaseInquiet(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Oh! Je n'aurais jamais cru que notre petite gazelle pourrait se rendre jusque là.");
				break;
			case Effect_MC.Replique.Reussite3 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Reussite3.ogg", typeof(AudioClip)) as AudioClip;
				CaseBlase(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("C'était pas si difficile, mais applaudissons pareil...");
				break;
			case Effect_MC.Replique.Reussite4 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Reussite4.ogg", typeof(AudioClip)) as AudioClip;
				CaseEnjoue(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Ouh! La victoire approche!");
				break;
			case Effect_MC.Replique.Reussite5 :
				eff.line = AssetDatabase.LoadAssetAtPath("Assets/Data/Sounds/Presentateur/_Generique/Reussite5.ogg", typeof(AudioClip)) as AudioClip;
				CaseImpressionne(eff);
				eff.bossFight = false;
				if(GUI.changed) Debug.Log("Je croyais que vous n'y parviendriez jamais!");
				break;
				#endregion
			}
		}
		#endregion

		eff.isBackstage = EditorGUILayout.Toggle("Backstage", eff.isBackstage);

		#region Conditions
		eff.hasCondition = EditorGUILayout.Toggle("Conditionnel", eff.hasCondition);
		if(eff.hasCondition){

			eff.condition = (Effect_MC.Condition) EditorGUILayout.EnumPopup("Condition : ", eff.condition);
			eff.isTrue = EditorGUILayout.Toggle("Doit etre vrai", eff.isTrue);

			if(eff.condition == Effect_MC.Condition.EtoileSpecifique || eff.condition == Effect_MC.Condition.SwitchActive || eff.condition == Effect_MC.Condition.BossPossible){
				eff.conditionalObject = EditorGUILayout.ObjectField ("Objet conditionnel", eff.conditionalObject, typeof(GameObject), true) as GameObject;
				if(eff.condition == Effect_MC.Condition.EtoileSpecifique) EditorGUILayout.HelpBox ("Mettre une etoile", MessageType.Info, false);
				else if (eff.condition == Effect_MC.Condition.SwitchActive) EditorGUILayout.HelpBox ("Mettre un Trigger de Switch", MessageType.Info, false);
				else EditorGUILayout.HelpBox ("Mettre le Boss Manager", MessageType.Info, false); 
			}
			else if (eff.condition == Effect_MC.Condition.EtoilesDeTelNiveau){
				eff.conditionalInt = EditorGUILayout.IntSlider ("Numero de niveau", eff.conditionalInt, 1, 5);
			}
			else if (eff.condition == Effect_MC.Condition.ColonneDetruite){
				eff.conditionalInt = EditorGUILayout.IntSlider ("ID de colonne", eff.conditionalInt, 0, 2);
				eff.conditionalObject = EditorGUILayout.ObjectField ("Objet conditionnel", eff.conditionalObject, typeof(GameObject), true) as GameObject;
				EditorGUILayout.HelpBox ("Mettre le Boss Manager", MessageType.Info,false);
			}
		}
		#endregion

		EditorUtility.SetDirty(target);
	}

	#region Emotions
	void CaseNeutre(Effect_MC eff){
		eff.neutre = true;
		eff.blase = false;
		eff.colerique = false;
		eff.enjoue = false;
		eff.impressionne = false;
		eff.inquiet = false;
		eff.nargueur = false;
		eff.panique = false;
	}
	void CaseBlase(Effect_MC eff){
		eff.neutre = false;
		eff.blase = true;
		eff.colerique = false;
		eff.enjoue = false;
		eff.impressionne = false;
		eff.inquiet = false;
		eff.nargueur = false;
		eff.panique = false;
	}
	void CaseColerique(Effect_MC eff){
		eff.neutre = false;
		eff.blase = false;
		eff.colerique = true;
		eff.enjoue = false;
		eff.impressionne = false;
		eff.inquiet = false;
		eff.nargueur = false;
		eff.panique = false;
	}
	void CaseEnjoue(Effect_MC eff){
		eff.neutre = false;
		eff.blase = false;
		eff.colerique = false;
		eff.enjoue = true;
		eff.impressionne = false;
		eff.inquiet = false;
		eff.nargueur = false;
		eff.panique = false;
	}
	void CaseImpressionne(Effect_MC eff){
		eff.neutre = false;
		eff.blase = false;
		eff.colerique = false;
		eff.enjoue = false;
		eff.impressionne = true;
		eff.inquiet = false;
		eff.nargueur = false;
		eff.panique = false;
	}
	void CaseInquiet(Effect_MC eff){
		eff.neutre = false;
		eff.blase = false;
		eff.colerique = false;
		eff.enjoue = false;
		eff.impressionne = false;
		eff.inquiet = true;
		eff.nargueur = false;
		eff.panique = false;
	}
	void CaseNargueur(Effect_MC eff){
		eff.neutre = false;
		eff.blase = false;
		eff.colerique = false;
		eff.enjoue = false;
		eff.impressionne = false;
		eff.inquiet = false;
		eff.nargueur = true;
		eff.panique = false;
	}
	void CasePanique(Effect_MC eff){
		eff.neutre = false;
		eff.blase = false;
		eff.colerique = false;
		eff.enjoue = false;
		eff.impressionne = false;
		eff.inquiet = false;
		eff.nargueur = false;
		eff.panique = true;
	}
	#endregion
}
