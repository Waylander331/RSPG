using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

public class Diaporama : MonoBehaviour 
{
	public Shader defaultShader;
	ScreenOverlay blank;
	ScreenOverlay[] images;

	private const float imageDuration = 5f;
	private const float blankDuration = 0.1f;

	private const float invokeDuration = 0.1f;

	private int currentImage = -1;

	private Camera cam;

	void Start()
	{
		cam = GetComponent<Camera> ();
		cam.enabled = false;
		GenerateScreenOverlays ();
		FetchImagesFromDB();
		foreach (ScreenOverlay overlay in images)
		{
			overlay.enabled = false;
		}
		blank.enabled = true;
		if(images.Length > 0)
			StartDiaporama ();
	}

	void StartDiaporama()
	{
		ShowNext ();
	}

	void ShowNext()
	{
		currentImage++;
		cam.enabled = true;
		images [currentImage % images.Length].enabled = true;
		if(images.Length > 1)
		{
			Invoke ("TurnOff", invokeDuration);
			Invoke ("TurnBlank", imageDuration);
		}
	}

	void TurnBlank()
	{
		cam.enabled = true;
		images [currentImage % images.Length].enabled = false;
		Invoke ("TurnOff", invokeDuration);
		Invoke ("ShowNext", blankDuration);
	}

	void TurnOff()
	{
		cam.enabled = false;
	}

	void GenerateScreenOverlays()
	{
		images = new ScreenOverlay[FetchImageCount()];
		ScreenOverlay[] overlays = GetComponents<ScreenOverlay> ();

		int diff = overlays.Length - (images.Length + 1);

		if(diff > 0) // too much overlays
		{
			// Destroy Excess
			for(int i = 0; i < diff; i++)
			{
				Destroy(overlays[overlays.Length - 1 - i]);
				overlays[overlays.Length - 1 - i] = null;
			}
		}

		int index = 0;
		for(int i = 0; i < overlays.Length; i++)
		{
			if(overlays[i] != null)
			{
				overlays[i].blendMode = ScreenOverlay.OverlayBlendMode.AlphaBlend;
				overlays[i].overlayShader = defaultShader;
				overlays[i].intensity = 1f;
				overlays[i].texture = null;
				if(index == 0)
					blank = overlays[i];
				else
					images[index-1] = overlays[i];
				index++;
			}
		}

		if(diff < 0) // not enough overlay
		{
			diff = Mathf.Abs(diff);
			for(int i = 0; i < diff; i++)
			{
				ScreenOverlay newOverlay = gameObject.AddComponent<ScreenOverlay>();
				newOverlay.blendMode = ScreenOverlay.OverlayBlendMode.AlphaBlend;
				newOverlay.overlayShader = defaultShader;
				newOverlay.intensity = 1f;
				newOverlay.texture = null;
				if(index == 0)
					blank = newOverlay;
				else
					images[index-1] = newOverlay;
				index++;
			}
		}
	}

	void FetchImagesFromDB()
	{
		LevelDiaporamasDB diapoDB = Manager.Instance.GetComponent<LevelDiaporamasDB> ();

		blank.texture = (Texture2D)diapoDB.blackScreen;

		LevelDiaporamasDB.DiaporamaSet levelDiapos;

		if (Manager.Instance.LevelName == "0_HUB_YL")
		{
			switch(transform.parent.GetComponent<ElevatorAnimationController>().levelToLoad)
			{
			case Effect_Change_Level.enum1.Menu:
				levelDiapos = diapoDB.menu;
				break;
			case Effect_Change_Level.enum1.Tuto:
				levelDiapos = diapoDB.tuto;
				break;
			case Effect_Change_Level.enum1.Hub_YL:
				levelDiapos = diapoDB.hub;
				break;
			case Effect_Change_Level.enum1.Manege_MAG:
				levelDiapos = diapoDB.manege;
				break;
			case Effect_Change_Level.enum1.FeteForaine_JLC:
				levelDiapos = diapoDB.feteForaine;
				break;
			case Effect_Change_Level.enum1.Carrousel_PD:
				levelDiapos = diapoDB.carrousel;
				break;
			case Effect_Change_Level.enum1.Chucky_PC:
				levelDiapos = diapoDB.chucky;
				break;
			case Effect_Change_Level.enum1.FreakShow_Jlem:
				levelDiapos = diapoDB.freakShow;
				break;
			case Effect_Change_Level.enum1.BossFight_GDT:
				levelDiapos = diapoDB.boss;
				break;
			default:
				levelDiapos = diapoDB.menu;
				break;
			}
		}
		else if(Manager.Instance.LevelName == "0_TUTO_P3_YL")
		{
			if(transform.parent.GetComponent<ElevatorAnimationController>().levelToLoad == Effect_Change_Level.enum1.Menu)
			{
				levelDiapos = diapoDB.menu;
			}
			else
			{
				levelDiapos = diapoDB.hub;
			}
		}
		else
		{
			switch(Manager.Instance.LevelName)
			{
			case "1_MANEGE_MAG":
				levelDiapos = diapoDB.manege;
				break;
			case "2_FETEFORAINE_JLC":
				levelDiapos = diapoDB.feteForaine;
				break;
			case "3_CARROUSEL_PD":
				levelDiapos = diapoDB.carrousel;
				break;
			case "4_CHUCKY_PC":
				levelDiapos = diapoDB.chucky;
				break;
			case "5_FREAKSHOW_JL":
				levelDiapos = diapoDB.freakShow;
				break;
			case "0_HUB_GDT":
				levelDiapos = diapoDB.boss;
				break;
			default:
				levelDiapos = diapoDB.menu;
				break;
			}
		}

		for(int i = 0; i < images.Length; i++)
		{
			if(levelDiapos.images[i] != null)
				images[i].texture = (Texture2D)levelDiapos.images[i];
		}
	}

	private int FetchImageCount()
	{
		LevelDiaporamasDB diapoDB = Manager.Instance.GetComponent<LevelDiaporamasDB> ();

		int count = 0;
		if (Manager.Instance.LevelName == "0_HUB_YL")
		{
			switch(transform.parent.GetComponent<ElevatorAnimationController>().levelToLoad)
			{
			case Effect_Change_Level.enum1.Menu:
				foreach(Texture t in diapoDB.menu.images)
					if(t != null)
						count++;
				break;
			case Effect_Change_Level.enum1.Tuto:
				foreach(Texture t in diapoDB.tuto.images)
					if(t != null)
						count++;;
				break;
			case Effect_Change_Level.enum1.Hub_YL:
				foreach(Texture t in diapoDB.hub.images)
					if(t != null)
						count++;
				break;
			case Effect_Change_Level.enum1.Manege_MAG:
				foreach(Texture t in diapoDB.manege.images)
					if(t != null)
						count++;
				break;
			case Effect_Change_Level.enum1.FeteForaine_JLC:
				foreach(Texture t in diapoDB.feteForaine.images)
					if(t != null)
						count++;
				break;
			case Effect_Change_Level.enum1.Carrousel_PD:
				foreach(Texture t in diapoDB.carrousel.images)
					if(t != null)
						count++;
				break;
			case Effect_Change_Level.enum1.Chucky_PC:
				foreach(Texture t in diapoDB.chucky.images)
					if(t != null)
						count++;
				break;
			case Effect_Change_Level.enum1.FreakShow_Jlem:
				foreach(Texture t in diapoDB.freakShow.images)
					if(t != null)
						count++;
				break;
			case Effect_Change_Level.enum1.BossFight_GDT:
				foreach(Texture t in diapoDB.boss.images)
					if(t != null)
						count++;
				break;
			}
		}
		else if(Manager.Instance.LevelName == "0_TUTO_P3_YL")
		{
			if(transform.parent.GetComponent<ElevatorAnimationController>().levelToLoad == Effect_Change_Level.enum1.Menu)
			{
				foreach(Texture t in diapoDB.menu.images)
					if(t != null)
						count++;
			}
			else
			{
				foreach(Texture t in diapoDB.hub.images)
					if(t != null)
						count++;
			}
		}
		else
		{
			switch(Manager.Instance.LevelName)
			{
			case "1_MANEGE_MAG":
				foreach(Texture t in diapoDB.manege.images)
					if(t != null)
						count++;
				break;
			case "2_FETEFORAINE_JLC":
				foreach(Texture t in diapoDB.feteForaine.images)
					if(t != null)
						count++;
				break;
			case "3_CARROUSEL_PD":
				foreach(Texture t in diapoDB.carrousel.images)
					if(t != null)
						count++;
				break;
			case "4_CHUCKY_PC":
				foreach(Texture t in diapoDB.chucky.images)
					if(t != null)
						count++;
				break;
			case "5_FREAKSHOW_JL":
				foreach(Texture t in diapoDB.freakShow.images)
					if(t != null)
						count++;
				break;
			case "0_HUB_GDT":
				foreach(Texture t in diapoDB.boss.images)
					if(t != null)
						count++;
				break;
			}
		}
		return count;
	}
}
