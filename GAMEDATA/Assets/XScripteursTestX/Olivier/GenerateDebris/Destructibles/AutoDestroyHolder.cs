using UnityEngine;
using System.Collections;

public class AutoDestroyHolder : MonoBehaviour 
{
	void Update()
	{
		if (transform.childCount == 0)
			Destroy (gameObject);
	}
}
