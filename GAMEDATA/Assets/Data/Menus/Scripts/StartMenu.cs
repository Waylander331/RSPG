using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour {
	private bool hasStopped = false;
	public RawImage myMovie;
	private MovieTexture mt;
	public GameObject mainMenu;
	public GameObject pressStart;
	public float timeLoop = 2f;
	public float timeListen = 0.5f;

	private AudioSource myAudio;

	// Use this for initialization
	void Start () {
		pressStart.SetActive(false);
		myAudio = transform.GetComponent<AudioSource>();
		mt = (MovieTexture)myMovie.mainTexture;
		mt.loop = true;
		mt.Stop();
		mt.Play();
		Invoke ("CanListen", timeListen);
		Invoke ("CanStart", timeLoop);
	}
	
	// Update is called once per frame
	void Update () {
		if(hasStopped && (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Pause"))){
			pressStart.SetActive(false);
			mt.Stop();
			mainMenu.SetActive(true);
		}
	}

	void CanListen(){
		myAudio.Play();
	}

	void CanStart(){
		hasStopped = true;
		pressStart.SetActive(true);
	}
}
