using UnityEngine;
using System.Collections;

public class modCam : MonoBehaviour 
{
	private float mouseSensitivity;
	private float moveSensitivity;
	private Vector3 myInput;
	private float prePauseFixedDelta;

	// Use this for initialization
	void Start () {
		//Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {

		prePauseFixedDelta = Manager.Instance.PrePauseFixedD;

		if (Input.GetMouseButtonDown (0)) {
			//Cursor.lockState = CursorLockMode.Locked;
		}

		// Not Paused
		if (Time.timeScale != 0f) 
		{
			//myInput.y += Time.deltaTime * Input.GetAxis ("Mouse X") * mouseSensitivity * Time.deltaTime / Time.timeScale;
			//myInput.x += Time.deltaTime * Input.GetAxis ("Mouse Y") * mouseSensitivity * -Time.deltaTime / Time.timeScale;
			myInput.y += Input.GetAxis ("HorizontalR") * mouseSensitivity * Time.deltaTime / Time.timeScale;
			myInput.x -= Input.GetAxis ("VerticalR") * mouseSensitivity * Time.deltaTime / Time.timeScale;
			this.transform.eulerAngles = myInput;


			this.transform.Translate (Input.GetAxis ("Vertical") * this.transform.forward * moveSensitivity * Time.deltaTime / Time.timeScale, Space.World);
			this.transform.Translate (Input.GetAxis ("Horizontal") * this.transform.right * moveSensitivity * Time.deltaTime / Time.timeScale, Space.World);
			this.transform.Translate (((Input.GetKey ("q")) ? -1f : 0f) * this.transform.up * moveSensitivity * Time.deltaTime / Time.timeScale, Space.World);
			this.transform.Translate (((Input.GetKey ("e")) ? 1f : 0f) * this.transform.up * moveSensitivity * Time.deltaTime / Time.timeScale, Space.World);
		}
		// Paused
		else
		{
			//myInput.y += prePauseFixedDelta * Input.GetAxisRaw ("Mouse X") * mouseSensitivity;
			//myInput.x += prePauseFixedDelta * Input.GetAxisRaw ("Mouse Y") * mouseSensitivity;
			myInput.y += Input.GetAxisRaw ("HorizontalR") * mouseSensitivity * prePauseFixedDelta;
			myInput.x -= Input.GetAxisRaw ("VerticalR") * mouseSensitivity * prePauseFixedDelta;
			this.transform.eulerAngles = myInput;
			
			
			this.transform.Translate (Input.GetAxisRaw ("Vertical") * this.transform.forward * moveSensitivity * prePauseFixedDelta, Space.World);
			this.transform.Translate (Input.GetAxisRaw ("Horizontal") * this.transform.right * moveSensitivity * prePauseFixedDelta, Space.World);
			this.transform.Translate (((Input.GetKey ("q")) ? -1f : 0f) * this.transform.up * moveSensitivity * prePauseFixedDelta, Space.World);
			this.transform.Translate (((Input.GetKey ("e")) ? 1f : 0f) * this.transform.up * moveSensitivity * prePauseFixedDelta, Space.World);
		}
	}

	public float MouseSensitivity{
		get{return mouseSensitivity;}
		set{mouseSensitivity = value;}
	}
	public float MoveSensitivity{
		get{return moveSensitivity;}
		set{moveSensitivity = value;}
	}
}
