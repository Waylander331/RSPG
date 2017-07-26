using UnityEngine;
using System.Collections;

public class SpikesSequence : MonoBehaviour
{
	public bool active = true;
	private bool wasActive = true;

	public SpikeSequenceParams[] sequences;
	private bool 
		launching = false,
		returning = false;

	private int sequenceIndex;

	private Spikes spikes;
	private BoxCollider lethal;
	private bool noSequence;

	private bool playedSound;

	private SpikesAudioPlayer spikesAudio;
	private bool spikesAudioIsNull;

	void Start () 
	{
		spikes = transform.GetChild (0).GetComponent<Spikes>();
		lethal = GetComponent<BoxCollider> ();
		if (transform.childCount > 1)
			spikesAudio = transform.GetChild (1).GetComponent<SpikesAudioPlayer> ();
		if (spikesAudio == null)
			spikesAudioIsNull = true;

		if(sequences.Length != 0)
		{
			Invoke ("TriggerLaunch", sequences[sequenceIndex].launchDelay);
		}
		else
		{
			noSequence = true;
		}
	}

	void Update () 
	{
		if(active != wasActive)
		{
			CancelInvoke("TriggerLaunch");
			CancelInvoke ("TriggerReturn");
			wasActive = active;
		}

		if(active)
		{
			if(!noSequence)
			{
				if(launching)
				{
					if(spikes.Launch (sequences[sequenceIndex].launchSpeed))
					{
						launching = false;
						if(!IsInvoking("TriggerReturn"))
							Invoke ("TriggerReturn", sequences[sequenceIndex].returnDelay);
					}
				}
				else if(returning)
				{
					if(spikes.Return (sequences[sequenceIndex].returnSpeed))
					{
						returning = false;
						StartNewSequence();
					}
				}
			}
		}
	}

	void StartNewSequence()
	{
		if (++sequenceIndex == sequences.Length)
			sequenceIndex = 0;
		if(!IsInvoking("TriggerLaunch"))Invoke ("TriggerLaunch", sequences[sequenceIndex].launchDelay);
		lethal.enabled = false;
	}

	void TriggerLaunch()
	{
		launching = true;
		lethal.enabled = true;
		if (!spikesAudioIsNull)
			spikesAudio.PlayAudioOut ();
	}

	void TriggerReturn()
	{
		returning = true;
		if (!spikesAudioIsNull)
			spikesAudio.PlayAudioIn ();
	}
}
