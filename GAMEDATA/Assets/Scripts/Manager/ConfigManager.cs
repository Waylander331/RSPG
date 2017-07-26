using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ConfigManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string path = Application.dataPath + "\\Config.txt";
		if (!File.Exists (path)) {
			string textConfig = "invert_y_look = false;" + Environment.NewLine;
			textConfig += "invert_x_look = false;" + Environment.NewLine;
			textConfig += "isDoux = superDoux;" + Environment.NewLine;
			textConfig += "HeyKids?wannaBuySomeMagic? = false;" + Environment.NewLine;
			File.WriteAllText (path, textConfig);
		}
		if (File.Exists (path)) {
			bool bTemp;
			string readConfig = File.ReadAllText(path);

			bool.TryParse(ExtractString(readConfig,"invert_y_look = ",";"),out bTemp);
			Camera.main.GetComponent<camBehavior>().InvertedY = bTemp;

			bool.TryParse(ExtractString(readConfig,"invert_x_look = ",";"),out bTemp);
			Camera.main.GetComponent<camBehavior>().InvertedX = bTemp;
		}
	}
	
	// Update is called once per frame
	private static string ExtractString(string s, string start,string end)
	{
		int startIndex = s.IndexOf(start) + start.Length;
		int endIndex = s.IndexOf(end, startIndex);
		return s.Substring(startIndex, endIndex - startIndex);
	}
}
