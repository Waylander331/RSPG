using UnityEngine;
using System.Collections;

public class AddScreenVisibleListeners : MonoBehaviour 
{
	void Awake()
	{
		MeshRenderer[] renderedObj = GetComponentsInChildren<MeshRenderer> ();
		SkinnedMeshRenderer[] skinnedRendererdObj = GetComponentsInChildren<SkinnedMeshRenderer> ();

		foreach (MeshRenderer r in renderedObj)
			if (r.GetComponent<ScreenVisibleListener> () == null)
				r.gameObject.AddComponent<ScreenVisibleListener> ();

		foreach (SkinnedMeshRenderer r in skinnedRendererdObj)
			if (r.GetComponent<ScreenVisibleListener> () == null)
				r.gameObject.AddComponent<ScreenVisibleListener> ();
	}
}
