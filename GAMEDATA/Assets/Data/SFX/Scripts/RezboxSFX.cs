using UnityEngine;
using System.Collections;

public class RezboxSFX : MonoBehaviour
{
	private Transform rezboxEffect;

	private const float destroyDelay = 1f;

	void Awake()
	{
		transform.parent.GetComponent<Rezbox> ().SFX = this;
	}

	void Start()
	{
		rezboxEffect = transform.GetChild (0);
		rezboxEffect.gameObject.SetActive (false);
	}

	public void ActivateEffect()
	{
		transform.parent = null;
		rezboxEffect.gameObject.SetActive (true);
		Invoke ("DeactivateEffect", destroyDelay);
	}

	void DeactivateEffect()
	{
		Destroy (gameObject);
	}


}
