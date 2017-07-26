using UnityEngine;
using System.Collections;

public class DisableParentOnGround : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Ground")
			transform.SetParent (null);
	}
}
