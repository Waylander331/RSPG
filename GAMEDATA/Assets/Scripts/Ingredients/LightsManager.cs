using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LightsManager : MonoBehaviour, ITriggerable
{
	
	#region Basic light variable
	public List<TabLights> myLights = new List<TabLights>();
	[SerializeField] private bool enabled;
	[SerializeField] private bool intensity;
	[SerializeField] private float lightIntensity;
	[SerializeField] private enum color
	{
		Default, Normal, Red, Blue, Green, Yellow
	};
	[SerializeField] private color mainColor;
	#endregion

	#region Lerp Light variable
	[SerializeField] private color switchColor;
	[SerializeField] private Color a;
	[SerializeField] private Color b;

	[SerializeField] private bool jiggleAndWiggleColor = false;
	[SerializeField] private float lerpTimer;
	[SerializeField] private float timer;
	[SerializeField] private float rTimer;
	[SerializeField] private bool repeatLerp;
	[SerializeField] private bool lerpIt = false;

	[SerializeField] private bool canSwitch;
	#endregion

	#region Light Flicker variable
	[SerializeField] private bool flicker;
	[SerializeField] private bool inTrigger = false;
	[SerializeField] private float flickerTimer;
	[SerializeField] delegate IEnumerator<WaitForSeconds> DelEnum();
	[SerializeField] DelEnum delEnum;
	[SerializeField] private bool disableUntriggered;
 	#endregion

	#region LerpAllColor variable
	[SerializeField] private bool lerpAll = false;
	[SerializeField] private int colorSwitch;
	[SerializeField] private bool lerpAllColor;
	#endregion



	void Start()
	{
		//repeatLerp = true;
		foreach(TabLights light in myLights)
		{
			if(enabled)
			{
				light.lights.enabled = true;
			}
			else if(!enabled)
			{
				light.lights.enabled = false;
			}

			if(intensity)
			{
				light.lights.intensity = lightIntensity;
			}

			switch(mainColor)
			{
			case color.Blue:
				light.lights.color = Color.blue;
				a = Color.blue;
				break;
				
			case color.Green:
				light.lights.color = Color.green;
				a = Color.green;
				break;
				
			case color.Red:
				light.lights.color = Color.red;
				a = Color.red;
				break;
				
			case color.Yellow:
				light.lights.color = Color.yellow;
				a = Color.yellow;
				break;

			case color.Normal:
				light.lights.color = Color.white;
				a = Color.white;
				break;
			case color.Default:
				break;
			}

			switch(switchColor)
			{
			case color.Blue:
				//light.color = Color.blue;
				b = Color.blue;
				break;
				
			case color.Green:
				//light.color = Color.green;
				b = Color.green;
				break;
				
			case color.Red:
				//light.color = Color.red;
				b = Color.red;
				break;
				
			case color.Yellow:
				//light.color = Color.yellow;
				b = Color.yellow;
				break;
			}
		}
	}

	void Update()
	{
		if(canSwitch)
		{
			LerpColor();
		}

		if(lerpAll)
		{
			LerpAllColor();
		}

		if ( delEnum != null)
		{
			StartCoroutine(delEnum());
			delEnum = null;
		}
	}


	public void Triggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{
			inTrigger = true;
			foreach(TabLights light in myLights)
			{
				if(!lerpAllColor)
				{
					if(!flicker)
					{
						light.lights.enabled = true;
					}

					if(jiggleAndWiggleColor)
					{
						canSwitch = true;
					}
				}
				else if (lerpAllColor)
				{
					lerpAll = true;
				}
			}
			if(flicker)
			{
				delEnum = Flicker;
			}
		}
	}

	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))

		if(disableUntriggered)
		{
			{
				inTrigger = false;
				lerpAll = false;
				foreach(TabLights light in myLights)
				{
					light.lights.enabled = false;
					canSwitch = false;

					if(repeatLerp)
					{
						light.lights.color = a;
						lerpIt = false;
					}
				}
				delEnum = null;
			}
		}
	}

	IEnumerator<WaitForSeconds> Flicker()
	{
		foreach(TabLights light in myLights)
		{
			yield return new WaitForSeconds(flickerTimer);
			light.lights.enabled = true;
			yield return new WaitForSeconds(flickerTimer);
			light.lights.enabled = false;

			if(inTrigger)
			{
				delEnum = Flicker;
			}
		}
	}

	public void LerpColor()
	{
		foreach(TabLights light in myLights)
		{
			if(!lerpIt)
			{
				light.lights.color = Color.Lerp(light.lights.color, b, Time.deltaTime * lerpTimer);
				timer += Time.deltaTime;
				rTimer = 0;

			}

			if(repeatLerp && timer >= lerpTimer)
			{
				lerpIt = true;
				light.lights.color = Color.Lerp(light.lights.color, a, Time.deltaTime * lerpTimer);
				rTimer += Time.deltaTime;

				if(rTimer >= lerpTimer)
				{
					timer = 0;
					lerpIt = false;
				}
			}
		}
	}

	public void LerpAllColor()
	{
		foreach(TabLights light in myLights)
		{
			light.lights.color = Color.Lerp(light.lights.color, b, Time.deltaTime * lerpTimer);
			timer += Time.deltaTime;

			if(timer >= lerpTimer)
			{
				if(colorSwitch == 4)
				{
					colorSwitch = 0;
				}

				timer = 0;
				colorSwitch += 1;
			}

			switch(colorSwitch)
			{
			case 1:
				b = Color.blue;
				break;
				
			case 2:
				b = Color.green;
				break;
				
			case 3:
				b = Color.red;
				break;
				
			case 4:
				b = Color.yellow;
				break;
			}
		}
	}
}


[System.Serializable]
public class TabLights
{
	public Light lights;
}








