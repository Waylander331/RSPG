using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor (typeof (SoundManager))]
[CanEditMultipleObjects]
public class SoundManagerEditor : Editor {

	private SoundManager pool;

	private Vector3 poolPos = new Vector3(1000,1000,1000);

	SerializedProperty avatarProp;
	SerializedProperty armsProp;	
	SerializedProperty bridgemanProp;
	SerializedProperty switchPrefabProp;
	SerializedProperty barsProp;	
	SerializedProperty doorProp;
	SerializedProperty scissorLiftProp;
	SerializedProperty collectibleProp;
	SerializedProperty chariotProp;
	SerializedProperty sFXProp;
	SerializedProperty bossFightProp;
	SerializedProperty cinematiqueProp;
	SerializedProperty menuProp;
	SerializedProperty creditsProp;

	SerializedProperty musicProp;

	
	void OnEnable () {
		pool = (SoundManager)target;

		avatarProp = serializedObject.FindProperty ("avatar");
		armsProp = serializedObject.FindProperty ("arms");		
		bridgemanProp = serializedObject.FindProperty ("bridgeman");
		switchPrefabProp = serializedObject.FindProperty ("switchPrefab");
		barsProp = serializedObject.FindProperty ("bars");		
		doorProp = serializedObject.FindProperty ("door");
		scissorLiftProp = serializedObject.FindProperty ("scissorLift");
		collectibleProp = serializedObject.FindProperty ("collectible");
		chariotProp = serializedObject.FindProperty ("chariot");
		sFXProp = serializedObject.FindProperty ("sFX");
		bossFightProp = serializedObject.FindProperty ("bossFight");
		cinematiqueProp = serializedObject.FindProperty ("cinematique");
		menuProp = serializedObject.FindProperty ("menu");
		creditsProp = serializedObject.FindProperty ("credits");

		musicProp = serializedObject.FindProperty ("music");
	}

	public override void OnInspectorGUI(){
		serializedObject.Update ();
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(avatarProp, new GUIContent ("Avatar"));
		EditorGUILayout.PropertyField(armsProp, new GUIContent ("Arms"));
		EditorGUILayout.PropertyField(bridgemanProp, new GUIContent ("Bridgeman"));
		EditorGUILayout.PropertyField(switchPrefabProp, new GUIContent ("Switch"));
		EditorGUILayout.PropertyField(barsProp, new GUIContent ("Bars"));
		EditorGUILayout.PropertyField(doorProp, new GUIContent ("Door"));
		EditorGUILayout.PropertyField(scissorLiftProp, new GUIContent ("ScissorLift & PF"));
		EditorGUILayout.PropertyField(collectibleProp, new GUIContent ("Collectibles"));
		EditorGUILayout.PropertyField(chariotProp, new GUIContent ("Chariot"));
		EditorGUILayout.PropertyField(sFXProp, new GUIContent ("SFX"));
		EditorGUILayout.PropertyField(bossFightProp, new GUIContent ("BossFight"));
		EditorGUILayout.PropertyField(cinematiqueProp, new GUIContent ("Cinematique"));
		EditorGUILayout.PropertyField(menuProp, new GUIContent ("Menu"));
		EditorGUILayout.PropertyField(creditsProp, new GUIContent ("Credits"));
		EditorGUILayout.Space();
		EditorGUILayout.PropertyField(musicProp, new GUIContent ("Music"));
		EditorGUILayout.Space();

		if(GUILayout.Button("Apply")){
			pool.ReadFile();
			if(pool.Sounds.Count == 0){
				for(int i = 0; i < pool.SoundsEffects.Length; i++){
					pool.Sounds.Add (pool.SoundsEffects[i].name, pool.SoundsEffects[i]);
				}
			}
			else {
				pool.Sounds.Clear();
				DestroyImmediate(GameObject.Find ("SoundsPrefabs"));
				for(int i = 0; i < pool.SoundsEffects.Length; i++){
					pool.Sounds.Add (pool.SoundsEffects[i].name, pool.SoundsEffects[i]);
				}
			}

			GameObject group = new GameObject("SoundsPrefabs");
			group.AddComponent<SoundsPrefab>();

			//************AVATAR**************//
			if(avatarProp.boolValue == true){
				GameObject groupAvatar = new GameObject("AvatarSounds");

				for(int i = 0; i < 23; i++){
					GameObject avatar = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					avatar.transform.parent = groupAvatar.transform;
					LoadAudioClip(avatar, pool.SoundsEffects[i].name);
				}

				groupAvatar.transform.parent = group.transform;
			}

			//************ARMS**************//
			if(armsProp.boolValue == true){
				GameObject groupArms = new GameObject("ArmsSounds");

				for(int i = 23; i < 29; i++){
					GameObject arms = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					arms.transform.parent = groupArms.transform;
					LoadAudioClip(arms, pool.SoundsEffects[i].name);
				}
				
				groupArms.transform.parent = group.transform;
			}

			//************BRIDGEMAN**************//
			if(bridgemanProp.boolValue == true){
				GameObject groupBridgeman = new GameObject("BridgeManSounds");

				for(int i = 29; i < 42; i++){
					GameObject bridgeman = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					bridgeman.transform.parent = groupBridgeman.transform;
					LoadAudioClip(bridgeman, pool.SoundsEffects[i].name);
				}

				groupBridgeman.transform.parent = group.transform;

				GameObject groupTomato = new GameObject("TomatoSounds");

				for(int i = 42; i < 44; i++){
					GameObject tomato = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					tomato.transform.parent = groupTomato.transform;
					LoadAudioClip(tomato, pool.SoundsEffects[i].name);
				}
							
				groupTomato.transform.parent = group.transform;
			}

			//************SWITCH**************//
			if(switchPrefabProp.boolValue == true){
				GameObject groupSwitch = new GameObject("SwitchSounds");

				for(int i = 44; i < 46; i++){
					GameObject switchPrefab = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					switchPrefab.transform.parent = groupSwitch.transform;
					LoadAudioClip(switchPrefab, pool.SoundsEffects[i].name);
				}
				
				groupSwitch.transform.parent = group.transform;
			}

			//************BAR**************//
			if(barsProp.boolValue == true){
				GameObject groupBar = new GameObject("BarSounds");

				for(int i = 46; i < 49; i++){
					GameObject bars = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					bars.transform.parent = groupBar.transform;
					LoadAudioClip(bars, pool.SoundsEffects[i].name);
				}
				
				groupBar.transform.parent = group.transform;
			}

			//************DOOR**************//
			if(doorProp.boolValue == true){
				GameObject groupDoor = new GameObject("DoorSounds");

				for(int i = 49; i < 51; i++){
					GameObject door = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					door.transform.parent = groupDoor.transform;
					LoadAudioClip(door, pool.SoundsEffects[i].name);
				}
				
				groupDoor.transform.parent = group.transform;
			}

			//************SCISSORLIFT&PF**************//
			if(scissorLiftProp.boolValue == true){
				GameObject groupScissorLift = new GameObject("ScissorLiftSounds");

				for(int i = 51; i < 52; i++){
					GameObject scissorLift = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					scissorLift.transform.parent = groupScissorLift.transform;
					LoadAudioClip(scissorLift, pool.SoundsEffects[i].name);
				}

				groupScissorLift.transform.parent = group.transform;
			}

			//************COLLECTIBLE**************//
			if(collectibleProp.boolValue == true){
				GameObject groupCollectible = new GameObject("CollectiblesSounds");

				for(int i = 52; i < 55; i++){
					GameObject collectible = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					collectible.transform.parent = groupCollectible.transform;
					LoadAudioClip(collectible, pool.SoundsEffects[i].name);
				}

				groupCollectible.transform.parent = group.transform;
			}

			//************LETHAL**************//
			if(chariotProp.boolValue == true){
				GameObject groupChariot = new GameObject("ChariotSounds");

				for(int i = 55; i < 56; i++){
					GameObject chariot = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					chariot.transform.parent = groupChariot.transform;
					LoadAudioClip(chariot, pool.SoundsEffects[i].name);
					chariot.GetComponent<AudioSource>().loop = true;
					chariot.AddComponent<AudioReverbFilter>();
					chariot.GetComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.Off;
					chariot.GetComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.User;
					chariot.GetComponent<AudioReverbFilter>().dryLevel = -300;
				}

				groupChariot.transform.parent = group.transform;
			}

			//************SFX**************//
			if(sFXProp.boolValue == true){
				GameObject groupSFX = new GameObject("SFXSounds");

				for(int i = 56; i < 65; i++){
					GameObject sFX = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					sFX.transform.parent = groupSFX.transform;
					LoadAudioClip(sFX, pool.SoundsEffects[i].name);
				}

				groupSFX.transform.parent = group.transform;
			}

			//************BOSSFIGHT**************//
			if(bossFightProp.boolValue == true){
				GameObject groupBossFight = new GameObject("BossFightSounds");
				
				for(int i = 65; i < 71; i++){
					GameObject bF = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					bF.transform.parent = groupBossFight.transform;
					LoadAudioClip(bF, pool.SoundsEffects[i].name);
				}
				
				groupBossFight.transform.parent = group.transform;
			}

			//************CINEMATIQUE**************//
			if(cinematiqueProp.boolValue == true){
				GameObject groupCinematique = new GameObject("CinematiqueSounds");
				
				for(int i = 71; i < 77; i++){
					GameObject cinematique = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					cinematique.transform.parent = groupCinematique.transform;
					LoadAudioClip(cinematique, pool.SoundsEffects[i].name);
				}
				
				groupCinematique.transform.parent = group.transform;
			}

			//************MENU**************//
			if(menuProp.boolValue == true){
				GameObject groupMenu = new GameObject("MenuSounds");
				
				for(int i = 77; i < 79; i++){
					GameObject menu = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					menu.transform.parent = groupMenu.transform;
					LoadAudioClip(menu, pool.SoundsEffects[i].name);
				}
				
				groupMenu.transform.parent = group.transform;
			}

			//************CREDITS**************//
			if(creditsProp.boolValue == true){
				GameObject groupCredits = new GameObject("CreditsSounds");
				
				for(int i = 79; i < 93; i++){
					GameObject credits = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					credits.transform.parent = groupCredits.transform;
					LoadAudioClip(credits, pool.SoundsEffects[i].name);
				}
				
				groupCredits.transform.parent = group.transform;
			}
		
			//************MUSIC**************//
			if(musicProp.boolValue == true){
				GameObject groupMusic = new GameObject("Music");

				for(int i = pool.Sounds.Count-1; i > pool.Sounds.Count-10; i--){
					GameObject music = Instantiate(pool.prefab, poolPos, Quaternion.identity) as GameObject;
					music.transform.parent = groupMusic.transform;
					LoadMusic(music, pool.SoundsEffects[i].name);
					music.AddComponent<AudioReverbFilter>();
					music.GetComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.Off;
					music.GetComponent<AudioReverbFilter>().reverbPreset = AudioReverbPreset.User;
					music.GetComponent<AudioReverbFilter>().dryLevel = 0;
				}

				groupMusic.transform.parent = group.transform;
			}

		}

		if(GUILayout.Button("Remove")){
			if(pool.Sounds.Count != 0){
				pool.Sounds.Clear();
			}
			DestroyImmediate(GameObject.Find ("SoundsPrefabs"));
		}

		serializedObject.ApplyModifiedProperties ();
	}

	public void LoadAudioClip(GameObject prefab, string name){
		prefab.name = name;
		prefab.GetComponent<AudioSource>().clip = AssetDatabase.LoadAssetAtPath(pool.Sounds[name].path, typeof(AudioClip)) as AudioClip;
		prefab.GetComponent<AudioSource>().volume = pool.Sounds[name].volume;
		prefab.GetComponent<AudioSource>().priority = pool.Sounds[name].priority;
		prefab.GetComponent<AudioSource>().loop = false;
	}

	public void LoadMusic(GameObject prefab, string name){
		prefab.name = name;
		prefab.GetComponent<AudioSource>().clip = AssetDatabase.LoadAssetAtPath(pool.Sounds[name].path, typeof(AudioClip)) as AudioClip;
		prefab.GetComponent<AudioSource>().volume = pool.Sounds[name].volume;
		prefab.GetComponent<AudioSource>().priority = pool.Sounds[name].priority;
		prefab.GetComponent<AudioSource>().loop = true;
		prefab.tag = "Music";
	}
}



