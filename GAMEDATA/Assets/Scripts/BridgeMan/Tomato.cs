using UnityEngine;
using System.Collections;

public class Tomato : MonoBehaviour 
{
	private float speed; 
	private Vector3 direction;
	private LayerMask destroyOnHit;

	private Ray ray;
	private TomatoSpawn spawnScript;
	private bool shot = false;
	private bool canDestroy = true;

	private MeshRenderer mRenderer;

	private TomatoSFX tomatoSFX;

	RaycastHit hit;

	void Awake()
	{
		mRenderer = GetComponent<MeshRenderer> ();
		destroyOnHit += 1 << 0;
		destroyOnHit += 1 << 9;
		destroyOnHit += 1 << 10;
		destroyOnHit += 1 << 11;
	}

	void Update()
	{

		Debug.DrawRay(transform.position, direction * 0.2f);
		ray = new Ray(transform.position, direction);

		if(!Physics.Raycast(ray, out hit, speed * Time.deltaTime, destroyOnHit))
		{
			transform.Translate (speed * direction * Time.deltaTime, Space.World);
			if(canDestroy)
			{
				Invoke("DestroyAfterDelay", 5f);
			}
		}
		else
		{
			if(LayerMask.LayerToName(hit.transform.gameObject.layer) == "Player")
			{
				Manager.Instance.Avatar.Daze();
				Manager.Instance.Avatar.KnockBack(direction);
			}
			SoundManager.Instance.TomatoSplatRandom();
			spawnScript.Destroyer(this);
			canDestroy = false;
			if(IsInvoking("DestroyAfterDelay"))
				CancelInvoke("DestroyAfterDelay");
		}
	}

	void DestroyAfterDelay()
	{
		spawnScript.Destroyer(this);
		canDestroy = false;
		CancelInvoke("DestroyAfterDelay");
	}

	public bool CanDestroy
	{
		set{canDestroy = value;}
	}

	public TomatoSpawn SpawnScript
	{
		set{spawnScript = value;}
	}

	public Vector3 Direction
	{
		set{direction = value;}
	}

	public float Speed 
	{
		set{speed = value;}
	}

	public LayerMask DestroyOnHit
	{
		set{destroyOnHit = value;}
	}

	public TomatoSFX TomatoSFX
	{
		get{return tomatoSFX;}
		set{tomatoSFX = value;}
	}

	public MeshRenderer MRenderer
	{
		get{return mRenderer;}
	}
}
