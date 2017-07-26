using UnityEngine;
using System.Collections;

public class Split : MonoBehaviour 
{
	private Transform bottomPiece;
	private Transform topPiece;
	private Transform explosionPoint;
	private float mass;

	void Start()
	{
		if(transform.childCount >= 3)
		{
			bottomPiece = transform.GetChild (0);
			topPiece = transform.GetChild (1);
			explosionPoint = transform.GetChild(2);
			bottomPiece.gameObject.SetActive (false);
			topPiece.gameObject.SetActive (false);
		}
		else
		{
			Debug.LogError("GameObject <" + gameObject.name + "> has the script Split but doesn't have enough children.\nDestroying script on Start.");
			Destroy (this);
		}

		Vector3 explosionDirection = explosionPoint.position - transform.position;
		explosionDirection.y = 0f;
		float diameter = explosionPoint.GetComponent<SphereCollider> ().radius * 2f;
		//Debug.Log ("Diameter : " + diameter);
		explosionDirection = explosionDirection.normalized * diameter + explosionDirection.normalized * diameter / 2f;
		//Debug.Log ("Direction : " + explosionDirection.magnitude);
		explosionPoint.position = transform.position - explosionDirection;
		//Debug.Log ((transform.position - explosionPoint.position).magnitude);

		mass = explosionPoint.GetComponent<ExplosionForcePoint> ().mass;
	}

	/*
	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.KeypadMinus))
			StartSplit ();
	}*/

	public void StartSplit()
	{
		bottomPiece.gameObject.SetActive (true);
		topPiece.gameObject.SetActive (true);
		explosionPoint.gameObject.SetActive (true);
		bottomPiece.parent = transform.parent;
		topPiece.parent = transform.parent;
		explosionPoint.parent = transform.parent;
		explosionPoint.GetComponent<ExplosionForcePoint> ().InvokeSelfDestruct ();
		explosionPoint.GetComponent<SphereCollider> ().enabled = true;
		Rigidbody bottom = bottomPiece.gameObject.AddComponent<Rigidbody> ();
		Rigidbody top = topPiece.gameObject.AddComponent<Rigidbody> ();
		ColonnesFadeOut bottomFade = bottomPiece.GetComponent<ColonnesFadeOut> ();
		ColonnesFadeOut topFade = topPiece.GetComponent<ColonnesFadeOut> ();
		bottomFade.RB = bottom;
		topFade.RB = top;
		bottomFade.InvokeDetectCollision ();
		topFade.InvokeDetectCollision ();

		bottom.mass = top.mass = mass;
		Destroy (gameObject);
	}

	void OnDrawGizmos()
	{
		if(explosionPoint == null) explosionPoint = transform.GetChild (2);
		Vector3 arrowPos = transform.position;
		Vector3 arrowDir = explosionPoint.position - transform.position;
		arrowDir.y = 0f;
		arrowDir = arrowDir.normalized * 2f;
		
		DrawShape.DrawArrowForGizmo (arrowPos, arrowDir, Color.blue, 0.7f, 35);
	}
}
