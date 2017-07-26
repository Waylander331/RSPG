using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class ScreenFlicker : MonoBehaviour, ITriggerable
{
	public Door[] doorsToOpen;

	private bool flickerActive;

	public Vector2 radius;
	public float angle;
	public float minGlitchingPowerOnX;
	public float maxGlitchingPowerOnX;

	public float minGlitchingPowerOnY;
	public float maxGlitchingPowerOnY;

	private bool xFlickering;
	private bool yFlickering;

	private Vector2 currentCenter;

	private float xDistanceToReach;
	private float yDistanceToReach;
	private float maxDistanceToReach;

	private bool xCenterIncreasing;
	private bool yCenterIncreasing;

	private float xCurrentValue = 0f;
	private float yCurrentValue = 0f;

	public float minDelay;
	public float maxDelay;

	public float minSpeed;
	public float maxSpeed;

	public float shakeDuration;
	private camEffect camFX;

	// Destroyed effect
	private NoiseAndScratches noise;
	private VignetteAndChromaticAberration chromaticAberration;
	private Vortex vortex;
	private ScreenOverlay overlay;
	private LinkedTV linkedTvs;
	private bool firstGlitch = true;
	private int currentTvIndex = -1;
	private ScreenRendererManager screenRendererManager;

	private RenderTexture rtex;

	void Awake()
	{
		linkedTvs = GetComponent<LinkedTV> ();
		noise = GetComponent<NoiseAndScratches> ();
		chromaticAberration = GetComponent<VignetteAndChromaticAberration> ();
		vortex = GetComponent<Vortex> ();
		vortex.radius = radius;
		vortex.angle = angle;
		overlay = GetComponent<ScreenOverlay> ();
		screenRendererManager = GetComponent<ScreenRendererManager> ();
	}
	void Start()
	{
		currentCenter = new Vector2 (-radius.x - 1f, -radius.y - 1f);
		vortex.center = currentCenter;
		maxDistanceToReach = radius.x * 2f + 0.25f;

		camFX = GameObject.Find ("Camera_Controller").GetComponent<camEffect> ();

		rtex = Resources.Load("PresCamTexture") as RenderTexture;
		rtex.Create();
		GetComponent<Camera>().targetTexture = rtex;
	}

	void OnLevelWasLoaded(){
		Invoke ("WaitForManager", 0.001f);
	}
	void WaitForManager(){
		if(Manager.Instance.LevelName != "Splash" && Manager.Instance.LevelName != "EcranTitre" && Manager.Instance.LevelName != "Credits" 
		   && Manager.Instance.LevelName != "GalleryModels" && Manager.Instance.LevelName != "CINEMATIQUE_FIN" && Manager.Instance.LevelName != "Save_And_Quit"){
			
			linkedTvs = GetComponent<LinkedTV> ();
			noise = GetComponent<NoiseAndScratches> ();
			chromaticAberration = GetComponent<VignetteAndChromaticAberration> ();
			vortex = GetComponent<Vortex> ();
			vortex.radius = radius;
			vortex.angle = angle;
			overlay = GetComponent<ScreenOverlay> ();
			screenRendererManager = GetComponent<ScreenRendererManager> ();
			
			currentCenter = new Vector2 (-radius.x - 1f, -radius.y - 1f);
			vortex.center = currentCenter;
			maxDistanceToReach = radius.x * 2f + 0.25f;
			
			camFX = GameObject.Find ("Camera_Controller").GetComponent<camEffect> ();
			
			rtex = Resources.Load("PresCamTexture") as RenderTexture;
			rtex.Create();
			GetComponent<Camera>().targetTexture = rtex;
		}
	}

	void Update()
	{
		if(xFlickering)
		{
			float speed = Random.Range(minSpeed, maxSpeed);

			if(xCenterIncreasing)
			{
				if(xCurrentValue + speed > xDistanceToReach)
				{
					speed = xDistanceToReach - xCurrentValue;
					xFlickering = false;
				}
				currentCenter.x += speed;
				xCurrentValue += speed;
			}
			else
			{
				if(xCurrentValue - speed < xDistanceToReach)
				{
					speed = xCurrentValue;
					xFlickering = false;
				}
				currentCenter.x -= speed;
				xCurrentValue -= speed;
			}
			vortex.center.x = currentCenter.x;
		}

		if(yFlickering)
		{
			float speed = Random.Range(minSpeed, maxSpeed);
			
			if(yCenterIncreasing)
			{
				if(yCurrentValue + speed > yDistanceToReach)
				{
					speed = yDistanceToReach - yCurrentValue;
					yFlickering = false;
				}
				currentCenter.y += speed;
				yCurrentValue += speed;
			}
			else
			{
				if(yCurrentValue - speed < yDistanceToReach)
				{
					speed = yCurrentValue;
					yFlickering = false;
				}
				currentCenter.y -= speed;
				yCurrentValue -= speed;
			}
			vortex.center.y = currentCenter.y;
		}

		if(flickerActive)
		{
			if (!xFlickering && !IsInvoking ("StartFlickeringOnX"))
				InvokeFlickeringOnX ();
			if (!yFlickering && !IsInvoking ("StartFlickeringOnY"))
				InvokeFlickeringOnY ();
		}
	}

	void InvokeFlickeringOnX()
	{
		Invoke ("StartFlickeringOnX", Random.Range (minDelay, maxDelay));
	}

	void InvokeFlickeringOnY()
	{
		Invoke ("StartFlickeringOnY", Random.Range (minDelay, maxDelay));
	}

	void StartFlickeringOnX()
	{
		if(firstGlitch)
		{
			Invoke ("EnableChromaticAberration", 0.75f);
			firstGlitch = false;
		}
		xCenterIncreasing = !xCenterIncreasing;

		float randomGlitchPower = Random.Range (minGlitchingPowerOnX, maxGlitchingPowerOnX);
		xDistanceToReach = xCenterIncreasing ? 
			maxDistanceToReach - randomGlitchPower : randomGlitchPower;
		xFlickering = true;
	}

	void StartFlickeringOnY()
	{
		if(firstGlitch)
		{
			Invoke ("EnableChromaticAberration", 0.5f);
			firstGlitch = false;
		}
		yCenterIncreasing = !yCenterIncreasing;
		float randomGlitchPower = Random.Range (minGlitchingPowerOnY, maxGlitchingPowerOnY);
		if (randomGlitchPower < 0f)
			randomGlitchPower = 0f;
		yDistanceToReach = yCenterIncreasing ? 
			maxDistanceToReach - randomGlitchPower : randomGlitchPower;
		yFlickering = true;
	}

	public void BreakTV(GameObject screen)
	{
		if (screen.transform.parent.name == "TV_A 2") 
		{
			OpenDoors();
		}

		int index = linkedTvs.GetTvIndex (screen.transform.parent.gameObject.GetComponent<LinkedCam>());
		if(index != -1)
		{
			currentTvIndex = index;
			if (!linkedTvs.IsDestroyed(currentTvIndex))
			{
				linkedTvs.SetDestroyed(currentTvIndex);
				camFX.TempCamShake (shakeDuration);
				noise.enabled = true;
				Invoke ("EnableSparks", shakeDuration / 2f);
				Invoke ("Break", shakeDuration);
			}
		}
	}

	void EnableSparks()	
	{
		linkedTvs.GetSparks (currentTvIndex).SetActive(true);
	}

	void Break()
	{
		if(screenRendererManager.CurrentlyRendered != -1 && linkedTvs.ScreenInfos[screenRendererManager.CurrentlyRendered].IsBroken) 
			vortex.enabled = true;
		flickerActive = true;
		SoundManager.Instance.TVBreakRandom ();
	}

	void EnableChromaticAberration()
	{
		if(screenRendererManager.CurrentlyRendered != -1 && linkedTvs.ScreenInfos[screenRendererManager.CurrentlyRendered].IsBroken)
			chromaticAberration.enabled = true;
	}

	void OpenDoors()
	{
		for(int i = 0; i < doorsToOpen.Length; i++)
		{
			doorsToOpen[i].Triggered(new Effect_Open());
		}
	}

	public void Triggered(EffectList effect)
	{
		if(effect.GetType () == typeof(Effect_TurnOnTv))
		{
			overlay.enabled = false;
		}
	}

	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType () == typeof(Effect_TurnOnTv))
		{
			overlay.enabled = true;
		}
	}

	public void SetDestroyedEffect(bool destroyed)
	{
		noise.enabled 
			= chromaticAberration.enabled
			= vortex.enabled 
			= destroyed;
	}

	public RenderTexture RTex
	{
		get {return rtex;}
	}
}
