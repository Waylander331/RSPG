using UnityEngine;
using System.Collections;

public class SoundTrigger : MonoBehaviour 
{
	private float ground;
	private float roof;

	private AudioReverbFilter reverb;
	private AudioSource source;

	public float volumeNull = -3500f;
	private const float volumeFull = 0f;
	private const float fadeStrength = 1200f;
	private bool audioFadeOut;
	private bool audioFadeIn;

	void Start () 
	{
		source = GetComponent<AudioSource> ();
		reverb = GetComponent<AudioReverbFilter>();
		float height = GetComponent<BoxCollider> ().bounds.size.y;
		ground = transform.position.y - height / 2f;
		roof = transform.position.y + height / 2f;
	}

	void Update()
	{
		if(audioFadeOut)
		{
			float increment = fadeStrength * Time.deltaTime;
			if(reverb.dryLevel - increment > 1.1f * volumeNull)
			{
				reverb.dryLevel -= increment;
			}
			else
			{
				reverb.dryLevel = 1.1f * volumeNull;
				source.Stop();
			}

		}

		if (audioFadeIn) 
		{
			float targetDryLevel = GetTargetDryLevel();
			float increment = fadeStrength * Time.deltaTime;
			if(reverb.dryLevel + increment <= targetDryLevel)
			{
				reverb.dryLevel += increment;
			}
			else
			{
				reverb.dryLevel = targetDryLevel;
				audioFadeIn = false;
			}

		}

	}

	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			source.Play();
			if(reverb.dryLevel == GetTargetDryLevel())
				reverb.dryLevel = 1.1f * volumeNull;
			audioFadeOut = false;
			audioFadeIn = true;
		}
	}

	void OnTriggerExit(Collider col)
	{
		if(col.tag == "Player")
		{
			audioFadeOut = true;
			audioFadeIn = false;
		}
	}

	float GetTargetDryLevel()
	{
		float volumeRatio = (roof - Manager.Instance.Avatar.transform.position.y) / (roof - ground);
		return volumeNull + volumeRatio * Mathf.Abs (volumeFull - volumeNull);
	}

	void LateUpdate()
	{
		if(!audioFadeOut && !audioFadeIn)
		{
			reverb.dryLevel = GetTargetDryLevel();
		}

	}
}
