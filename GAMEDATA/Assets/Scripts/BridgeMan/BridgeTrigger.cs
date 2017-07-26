using UnityEngine;
using System.Collections;

public class BridgeTrigger : MonoBehaviour 
{
	private bool isKickDetector = false;
	bool isDetecting;
	private AnimationBridgeman animScript;
	private MeshCollider collider;
	private LayerMask collision = 1 << 9;
	private LayerMask wallJumpable = 1 << 11;

	void Start()
	{
		collider = GetComponent<MeshCollider> ();
	}

	void OnTriggerEnter(Collider col)
	{
		if (col.tag == "Player")
		{
			isDetecting = true;
			if(isKickDetector)
			{
				Manager.Instance.Avatar.SetBridgeMan(animScript);
			}
		}
	}

	void OnTriggerExit(Collider col)
	{
		if (col.tag == "Player")
		{
			isDetecting = false;
			if(isKickDetector)
			{
				Manager.Instance.Avatar.ResetBridgeman();
			}
		}
	}

	public bool IsSeeing()
	{
		Vector3 tomatoSpawnPos = animScript.TomatoScript.GetTomatoSpawnPosition();
		Vector3 targetPos = Manager.Instance.Avatar.transform.position + new Vector3 (0f, animScript.TomatoScript.TARGET_OFFSET, 0f);
		
		Ray ray = new Ray (tomatoSpawnPos, targetPos - tomatoSpawnPos);
		float detectionRange = Vector3.Distance(tomatoSpawnPos, targetPos);
		
		return !Physics.Raycast (ray, detectionRange, collision) && !Physics.Raycast(ray, detectionRange, wallJumpable);
	}

	public bool IsDetecting
	{
		get{return isDetecting;}
	}

	public AnimationBridgeman AnimScript
	{
		set{animScript = value;}
	}

	public bool IsKickDetector
	{
		set{isKickDetector = value;}
	}
}
