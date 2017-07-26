using UnityEngine;
using System.Collections;

public class ExplosionForcePoint : MonoBehaviour 
{
	public GameObject[] columnsDebris;
	public float duration = 1f;
	public float radius = 10f;
	public float force = 100f;
	public float upwardModifier = 1f;
	public float mass = 1f;
	public ForceMode forceMode;

	void Update()
	{
		Collider[] inRadius = Physics.OverlapSphere (transform.position, radius);
		for(int i = 0; i < inRadius.Length; i++)
		{
			Collider col = inRadius[i];
			if(col.attachedRigidbody != null)
			{
				bool foundIt = false;
				for(int j = 0; !foundIt && j < columnsDebris.Length; j++)
				{
					if(columnsDebris[j] == col.gameObject)
					{
						col.attachedRigidbody.AddExplosionForce(force, transform.position, radius, upwardModifier, forceMode);
					}
				}
			}
		}
	}

	public void InvokeSelfDestruct()
	{
		Invoke ("DestroySelf", duration);
	}

	void DestroySelf()
	{
		Destroy (gameObject);
	}
}
