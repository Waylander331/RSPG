using UnityEngine;
using System.Collections.Generic;

public class LiftingPlatform : MonoBehaviour, ITriggerable
{
	public Animation myAnime;

	private List<Transform> limits =  new List<Transform>();
	private MeshRenderer limitsRend;
	private bool canRewind = false;
	private bool playerIn = false;

	
	public enum Height{
		Six, SixHalf, Seven, SevenHalf, Eight, EightHalf, Nine, NineHalf, Ten 
	};
	public Height platformHeight;

	void Start()
	{
		myAnime["vibrateUp"].speed = 0;
		Transform temp = transform.FindChild("Lowertrigger");
		Transform temp1 = transform.FindChild("UpperTrigger");

		limits.Add (temp);
		limits.Add(temp1);

		// Unparent the triggers, disable their mesh renderer and set their height to the Enum position chosen from the inspector.
		foreach(Transform obj in limits)
		{
			obj.transform.parent = null;
			limitsRend = obj.GetComponent<MeshRenderer>();
			limitsRend.enabled = false;
			if(obj.tag == ("UpperTrigger"))
			{
				switch(platformHeight)
				{
				case (Height.Six):
					Vector3 six = new Vector3(obj.transform.position.x,this.transform.position.y + 6.108f,obj.transform.position.z);
					obj.transform.position = six;
					break;
				case (Height.SixHalf):
					Vector3 sixHalf = new Vector3(obj.transform.position.x,this.transform.position.y + 6.608f,obj.transform.position.z);
					obj.transform.position = sixHalf;
					break;
				case (Height.Seven):
					Vector3 seven = new Vector3(obj.transform.position.x,this.transform.position.y + 7.108f,obj.transform.position.z);
					obj.transform.position = seven;
					break;
				case(Height.SevenHalf):
					Vector3 sevenHalf = new Vector3(obj.transform.position.x,this.transform.position.y + 7.608f,obj.transform.position.z);
					obj.transform.position = sevenHalf;
					break;
				case(Height.Eight):
					Vector3 eight = new Vector3(obj.transform.position.x,this.transform.position.y + 8.108f,obj.transform.position.z);
					obj.transform.position = eight;
					break;
				case(Height.EightHalf):
					Vector3 eightHalf = new Vector3(obj.transform.position.x,this.transform.position.y + 8.608f,obj.transform.position.z);
					obj.transform.position = eightHalf;
					break;
				case(Height.Nine):
					Vector3 nine = new Vector3(obj.transform.position.x,this.transform.position.y + 9.108f,obj.transform.position.z);
					obj.transform.position = nine;
					break;
				case(Height.NineHalf):
					Vector3 nineHalf = new Vector3(obj.transform.position.x,this.transform.position.y + 9.608f,obj.transform.position.z);
					obj.transform.position = nineHalf;
					break;
				case(Height.Ten):
					Vector3 ten = new Vector3(obj.transform.position.x,this.transform.position.y + 10.108f,obj.transform.position.z);
					obj.transform.position = ten;
					break;
				}
			}
		}
	}

	public void Triggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{
			if(!canRewind)
			{
				//myAnime.Play();
				playerIn = true;
				myAnime["vibrateUp"].speed = 0.8f;
				SoundManager.Instance.PlayAudio("SLMove");
			}
		}
		if(effect.GetType() == typeof (Effect_Move))
		{
			if(!canRewind)
			{
				//myAnime.Play();
				playerIn = true;
				myAnime["vibrateUp"].speed = 0.8f;
				SoundManager.Instance.PlayAudio("SLMove");
			}
		}
	}

	public void UnTriggered(EffectList effect)
	{
		if(effect.GetType() == typeof (Effect_Default))
		{
			playerIn = false;
			if (canRewind)
			{
				//myAnime.Play();
				myAnime["vibrateUp"].speed = -0.4f;
				SoundManager.Instance.PlayAudio("SLMove");
			}
		}
		if(effect.GetType() == typeof (Effect_Move))
		{
			playerIn = false;
			if (canRewind)
			{
				//myAnime.Play();
				myAnime["vibrateUp"].speed = -0.4f;
				SoundManager.Instance.PlayAudio("SLMove");
			}
		}
	}



	public bool CanRewind {
		get {
			return canRewind;
		}
		set {
			canRewind = value;
		}
	}

	public bool PlayerIn {
		get {
			return playerIn;
		}
		set {
			playerIn = value;
		}
	}
}
