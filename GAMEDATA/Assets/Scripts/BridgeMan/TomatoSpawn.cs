using UnityEngine;
using System.Collections.Generic;

public class TomatoSpawn : MonoBehaviour 
{
	private const int amount = 3;
	public Tomato tomato;
	private const float speed = 25f;

	private const float targetOffset = 1f;
	protected Avatar player;
	protected Stack<Tomato> pool;
	protected Transform spawnAt;

	void Start () 
	{
		player = Manager.Instance.Avatar;
		spawnAt = transform.GetChild (3);

		PoolTomatoes();
	} 

	public void PoolTomatoes(){
		pool = new Stack<Tomato>(amount);
		for (int i = 0; i < amount; i++) 
		{
			Tomato temp = Instantiate(tomato, spawnAt.transform.position, Quaternion.identity) as Tomato;
			temp.SpawnScript = this;
			temp.Speed = speed;
			temp.MRenderer.enabled = false;
			pool.Push(temp);
		}
	}

	// Destroy Tomatos
	public void Destroyer(Tomato tomato)
	{
		pool.Push(tomato);
		tomato.MRenderer.enabled = false;
		tomato.TomatoSFX.TriggerTomatoSplash ();
		Invoke ("DisableTomato", 0.5f);
		tomato.enabled = false;
	}

	void DisableTomato()
	{
		tomato.enabled = false;
	}

	// Instantiate Tomato
	public bool ShootTomato()
	{
		if(pool.Count != 0)
		{
			Tomato poped = pool.Pop();
			poped.transform.position = spawnAt.transform.position;
			Vector3 direction = (player.transform.position + new Vector3(0f, targetOffset, 0f) - spawnAt.transform.position).normalized;
			poped.transform.position = spawnAt.transform.position;
			poped.Direction = direction;
			poped.enabled = true;
			poped.MRenderer.enabled = true;
			poped.CanDestroy = true;
			tomato.enabled = true;
			return true;
		}
		else
		{
			return false;
		}
	}


	public bool ShootTomato(Transform target)
	{
		if(pool.Count != 0)
		{
			for (int i = 0; i < pool.Count; i++){
				Tomato poped = pool.Pop();
				Vector3 direction = (target.position + new Vector3(0f, targetOffset, 0f) - spawnAt.transform.position).normalized;
				poped.transform.position = spawnAt.transform.position;
				poped.Direction = direction;
				poped.enabled = true;
				poped.MRenderer.enabled = true;
				poped.CanDestroy = true;
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	public Vector3 GetTomatoSpawnPosition()
	{
		return spawnAt.position;
	}

	public float TARGET_OFFSET
	{
		get{return targetOffset;}
	}
}
