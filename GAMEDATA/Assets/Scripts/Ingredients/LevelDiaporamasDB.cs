using UnityEngine;
using System.Collections;

public class LevelDiaporamasDB : MonoBehaviour
{
	[System.Serializable]
	public class DiaporamaSet
	{
		public const int NB_OF_IMAGES = 3;
		public Texture[] images = new Texture[NB_OF_IMAGES];
	}

	public Texture blackScreen;
	public DiaporamaSet menu;
	public DiaporamaSet tuto;
	public DiaporamaSet hub;
	public DiaporamaSet manege;
	public DiaporamaSet feteForaine;
	public DiaporamaSet carrousel;
	public DiaporamaSet chucky;
	public DiaporamaSet freakShow;
	public DiaporamaSet boss;
}
