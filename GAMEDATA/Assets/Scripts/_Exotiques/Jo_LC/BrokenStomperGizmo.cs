using UnityEngine;
using System.Collections;

public class BrokenStomperGizmo : MonoBehaviour 
{
	private Transform parent;
	private bool onEditor = true;
	private Vector3 initPos;

	void Start()
	{
		initPos = transform.position;
	}

	void OnDrawGizmos()
	{
		parent = transform.parent;

		BrokenStomper stomper = parent.GetComponent<BrokenStomper> ();
		int modifier = 1;
		if(stomper != null && stomper.Reversed)
			modifier = -1;

		Vector3 myPos = onEditor ? transform.position : initPos;
		DrawShape.DrawArrowForGizmo (myPos, Vector3.down * stomper.descentDistance * modifier, Color.blue);
	}

	public bool OnEditor
	{
		set{onEditor = value;}
	}

	public Vector3 InitPos
	{
		set{initPos = value;}
	}
}
