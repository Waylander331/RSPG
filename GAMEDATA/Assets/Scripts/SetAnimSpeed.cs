using UnityEngine;
using System.Collections;

public class SetAnimSpeed : MonoBehaviour, ITriggerable 
{
	public float speedModifier = 1f;

	private float lastValue;
	private Animator anim;
	private float defaultSpeed;

	private bool incrementing;
	private bool increasing;
	private float speedIncrement;
	private bool isOrnithoryque;
	//private Transform ornithorynque;
	//private Transform ornithorynqueRepere;

	void Start()
	{
		/*
		switch(animSpeed)
		{
		case AnimSpeed.tresLent:
			animSpeedValue = 0.25f;
			break;
		case AnimSpeed.lent:
			animSpeedValue = 0.5f;
			break;
		case AnimSpeed.normal:
			animSpeedValue = 1f;
			break;
		case AnimSpeed.vite:
			animSpeedValue = 1.5f
			break;
		case AnimSpeed.tresVite:
			animSpeedValue = 2f;
			break;
		case AnimSpeed.intense:
			animSpeedValue = 3f;
			break;

		}*/

		anim = transform.GetChild (0).GetComponent<Animator> ();
		if (anim == null)
		{
			Debug.LogWarning ("Animator is NULL. Disabling script");
			enabled = false;
		}
		else
		{
			anim.speed = lastValue = defaultSpeed = speedModifier;
		}

		/*
		ornithorynqueRepere = transform.GetChild (0).GetChild(1).GetChild (0).GetChild(0).GetChild(1);
		ornithorynque = transform.GetChild (1);*/
	}

	void Update()
	{
		if (speedModifier != lastValue && !incrementing)
			anim.speed = lastValue = speedModifier;

		if(incrementing)
		{
			if(increasing)
			{
				if(anim.speed + speedIncrement * Time.deltaTime < speedModifier)
				{
					anim.speed += speedIncrement * Time.deltaTime;
				}
				else
				{
					anim.speed = speedModifier;
				}
			}
			else
			{
				if(anim.speed - speedIncrement * Time.deltaTime > speedModifier)
				{
					anim.speed -= speedIncrement * Time.deltaTime;
				}
				else
				{
					anim.speed = speedModifier;
				}
			}

			if(anim.speed == speedModifier)
			{
				lastValue = speedModifier;
				incrementing = false;
			}
		}

		/*
		if (isOrnithoryque)
			ornithorynque.position = ornithorynqueRepere.position;*/
	}

	public void Triggered(EffectList effect)
	{
		if(effect.GetType() == typeof(Effect_ChangeAnimSpeed))
		{
			Effect_ChangeAnimSpeed actualEffect = (Effect_ChangeAnimSpeed) effect;
			speedModifier = actualEffect.targetSpeed;

			if(!actualEffect.instantSpeedChange)
			{
				incrementing = true;
				increasing = true;
				speedIncrement = actualEffect.speedIncrement;
				if(actualEffect.isOrnithorynque)
				{
					isOrnithoryque = true;
					//ornithorynque.gameObject.SetActive(true);
				}
			}
		}
	}

	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType() == typeof(Effect_ChangeAnimSpeed))
		{
			Effect_ChangeAnimSpeed actualEffect = (Effect_ChangeAnimSpeed) effect;
			speedModifier = defaultSpeed;

			if(!actualEffect.instantSpeedChange)
			{
				incrementing = true;
				increasing = false;
				speedIncrement = actualEffect.speedIncrement;
			}
		}
	}
}
