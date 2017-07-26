using UnityEngine;
using System.IO;
using System.Collections;

public class ScreenShotMaker : MonoBehaviour {

	public string myFolder;

	void Start () 
	{
		DontDestroyOnLoad(gameObject); //stays on level change
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		if(Input.GetKeyDown(KeyCode.F12)) //F12 is pressed			
		{
			Directory.CreateDirectory("c:\\Screenshots\\" + myFolder + "\\");  //creates a folder if there is none.			
			print ("ScreenShot Taken 'Screenshot_" + System.DateTime.Now.ToString() + "'");
			string timeTag = System.DateTime.Now.ToString().Replace("/","").Replace(" ","").Replace(":",""); 

			StartCoroutine("TakeShot",timeTag);
		}
	}

	IEnumerator TakeShot(string photoName){
		yield return new WaitForEndOfFrame(); //waits for scene to be fully rendered

		int width = Screen.width;
		int height = Screen.height;
		Texture2D tex = new Texture2D (width,height, TextureFormat.RGB24, false);
		Rect screenSize = new Rect(0,0,width,height);

		tex.ReadPixels (screenSize,0,0); //captures all pixels in the screen.
		tex.Apply();
		
		var bytes = tex.EncodeToPNG();
		File.WriteAllBytes(@"c:\\Screenshots\\" + myFolder + "\\screenshot_"+ photoName +".png", bytes); //places the screensht in the specified location.
		Destroy(tex);
	}

	public string MyFolder{
		get{return myFolder;}
		set{myFolder = value;}
	}
}
