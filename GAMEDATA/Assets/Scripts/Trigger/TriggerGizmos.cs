using UnityEngine;
using System.Collections;

public class TriggerGizmos : MonoBehaviour 
{
	public bool isVisible = true;

	private const float 
		defaultAlpha = 0.25f,
		onSelectedAlpha = 0.5f;
	
	private Color
		defaultColor = Color.magenta,
		onSelectedColor = Color.magenta;

	void OnDrawGizmos()
	{
		if(isVisible)
		{
			Color color = defaultColor;
			color.a = defaultAlpha;
			
			Gizmos.color = color;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawCube(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f));
		}
	}
	
	void OnDrawGizmosSelected()
	{
		if(isVisible)
		{
			Gizmos.color = new Color();
			
			Color color = onSelectedColor;
			color.a = onSelectedAlpha;
			
			Gizmos.color = color;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawCube(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f));
		}
	}
}
