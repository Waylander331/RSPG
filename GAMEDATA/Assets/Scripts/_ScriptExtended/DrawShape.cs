using UnityEngine;
using System.Collections;

public static class DrawShape
{
//	public static void DrawArrowForGizmo(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
//	{
//		Gizmos.DrawRay(pos, direction);
//		
//		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
//		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
//		Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
//		Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
//	}
	
	public static void DrawArrowForGizmo(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
	{
		Gizmos.color = color;
		Gizmos.DrawRay(pos, direction);
		
		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
		Gizmos.DrawRay(pos + direction, right * arrowHeadLength);
		Gizmos.DrawRay(pos + direction, left * arrowHeadLength);
	}
	
//	public static void DrawArrowForDebug(Vector3 pos, Vector3 direction, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
//	{
//		Debug.DrawRay(pos, direction);
//		
//		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
//		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
//		Debug.DrawRay(pos + direction, right * arrowHeadLength);
//		Debug.DrawRay(pos + direction, left * arrowHeadLength);
//	}
	public static void DrawArrowForDebug(Vector3 pos, Vector3 direction, Color color, float arrowHeadLength = 0.25f, float arrowHeadAngle = 20.0f)
	{
		Debug.DrawRay(pos, direction, color);
		
		Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180+arrowHeadAngle,0) * new Vector3(0,0,1);
		Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,180-arrowHeadAngle,0) * new Vector3(0,0,1);
		Debug.DrawRay(pos + direction, right * arrowHeadLength, color);
		Debug.DrawRay(pos + direction, left * arrowHeadLength, color);
	}

	public static void DrawXForDebug(Vector3 pos, Color color, float XSize = 1f)
	{	
		Vector3 pos1 = new Vector3(pos.x-XSize/2,pos.y-XSize/2,pos.z-XSize/2);
		Vector3 pos2 = new Vector3(pos.x+XSize/2,pos.y-XSize/2,pos.z-XSize/2);
		Vector3 pos3 = new Vector3(pos.x-XSize/2,pos.y-XSize/2,pos.z+XSize/2);
		Vector3 pos4 = new Vector3(pos.x+XSize/2,pos.y-XSize/2,pos.z+XSize/2);
		
		Debug.DrawRay(pos1, (pos-pos1).normalized*(XSize*1.7f), color);
		Debug.DrawRay(pos2, (pos-pos2).normalized*(XSize*1.7f), color);
		Debug.DrawRay(pos3, (pos-pos3).normalized*(XSize*1.7f), color);
		Debug.DrawRay(pos4, (pos-pos4).normalized*(XSize*1.7f), color);
	}

	public static void DrawXForGizmo(Vector3 pos, Color color, float XSize = 1f)
	{	
		Gizmos.color = color;
		Vector3 pos1 = new Vector3(pos.x-XSize/2,pos.y-XSize/2,pos.z-XSize/2);
		Vector3 pos2 = new Vector3(pos.x+XSize/2,pos.y-XSize/2,pos.z-XSize/2);
		Vector3 pos3 = new Vector3(pos.x-XSize/2,pos.y-XSize/2,pos.z+XSize/2);
		Vector3 pos4 = new Vector3(pos.x+XSize/2,pos.y-XSize/2,pos.z+XSize/2);
		
		Gizmos.DrawRay(pos1, (pos-pos1).normalized*(XSize*1.7f));
		Gizmos.DrawRay(pos2, (pos-pos2).normalized*(XSize*1.7f));
		Gizmos.DrawRay(pos3, (pos-pos3).normalized*(XSize*1.7f));
		Gizmos.DrawRay(pos4, (pos-pos4).normalized*(XSize*1.7f));
	}
}