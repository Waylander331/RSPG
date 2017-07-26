using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour 
	where T : Component
{


	private static T _instance;
	private static bool applicationIsQuitting = false;


		public static T Instance {
			get {
				if (applicationIsQuitting) 
				{
					Debug.LogWarning("[Singleton] Instance "+ typeof(T) +
					                 " already destroyed on application quit." +
					                 "Won't create again - returning null.");
					return null;
				}
				if (_instance == null) {
					_instance = FindObjectOfType (typeof(T)) as T;
					if (_instance == null) {
						GameObject obj = new GameObject ();
						obj.hideFlags = HideFlags.HideAndDontSave;
						_instance = (T)obj.AddComponent (typeof(T)); //on peut lui ajouter qqch.
					}
				}
				return _instance;
			}
	}

	public virtual void Awake ()
	{
		DontDestroyOnLoad (this.gameObject);
		if (_instance == null) {
			_instance = this as T;
		} else {
			Destroy (gameObject);
		}
	}

	public void OnDestroy () 
	{
		applicationIsQuitting = true;
	}

	public T _Instance{
		get {return _instance;}
	}  
}
