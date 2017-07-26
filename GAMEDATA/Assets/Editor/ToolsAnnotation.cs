/*
Author : Léa
Resume : Tool to show or hide annotations
*/

using UnityEngine;
using System.Collections;
using UnityEditor;

public class ToolsAnnotation : Editor {
	private static bool isVisible = false;

	//Montre ou cache le TextMesh des Annotations (en utilisant l'alpha)
	[MenuItem("Tools/Show\\Hide Annotation %&a",false,21)]
	private static void NewMenuShowHide(){
		if(!isVisible){
			foreach (TextMesh temp in AnnotationWindow.myTextMesh){
				if(AnnotationWindow.myTextMesh.Count != 0){
					if(temp.color.a != 1){
						Color temp1 = temp.color;
						temp1.a = 1;
						temp.color = temp1;
					}
				}
			}
			isVisible = true;
		}
		else{
			foreach(TextMesh temp in AnnotationWindow.myTextMesh){
				if(AnnotationWindow.myTextMesh.Count != 0){
					if(temp.color.a != 0){
						Color temp1 = temp.color;
						temp1.a = 0;
						temp.color = temp1;
					}
				}
			}
			isVisible = false;
		}
	}
}
