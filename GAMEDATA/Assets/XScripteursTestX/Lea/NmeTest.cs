using UnityEngine;
using System.Collections;

public class NmeTest : MonoBehaviour, IMovable {

	public float speed;
	private Movable target;

	void Start () {
		target = this.GetComponent<Movable>();
		if(target.WpsList.Count != 0){
			this.transform.position = target.WpsList[target.Courant].transform.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(target.WpsList.Count != 0){
			if(Vector3.Distance (this.transform.position, target.WpsList[target.Next].transform.position) < 0.1f){
				this.transform.position = target.WpsList[target.Next].transform.position;
				target.WpSetNext();

			} else {
				this.transform.position = Vector3.Lerp(this.transform.position, target.WpsList[target.Next].transform.position, speed * Time.deltaTime);
			}
		}
	}

	public void Wait(float seconds){
		speed = 0;
		Invoke ("Delay", seconds);
	}

	public void Stop(bool isStopped){
		if(isStopped){
			speed = 0f;
		}
	}

	public void NewSpeed(float newSpeed){
		speed = newSpeed;
	}

	public void ChangeRotation(int orientation){
		/*if(!isIdentity){
			this.transform.forward = target.WpsList[target.Next].transform.position - this.transform.position;
		}*/
	}

	public void Teleport(WaypointScript where){
		//this.transform.position = target.WpsList[where].transform.position;
		this.transform.position = where.transform.position;
	}

	public void RotateHalf(MovableRotation rotation){
		/*if(half){
			if(this.transform.rotation.z < 90){
				Quaternion temp = this.transform.rotation;
				temp.z += 5 * Time.deltaTime;
				this.transform.rotation = temp;
			}
		}
		else {
			if(this.transform.rotation.z > 0){
				Quaternion temp = this.transform.rotation;
				temp.z -= 5  * Time.deltaTime;
				this.transform.rotation = temp;
			}
		}*/
	}

	public void RotateFull(MovableRotation rotation){
		/*if(full){
			if(this.transform.rotation.z < 180){
				Quaternion temp = this.transform.rotation;
				temp.z += 10 * Time.deltaTime;
				this.transform.rotation = temp;
			}
		}
		else {
			if(this.transform.rotation.z > 0){
				Quaternion temp = this.transform.rotation;
				temp.z -= 10 * Time.deltaTime;
				this.transform.rotation = temp;
			}
		}*/
	}


	void Delay(){
		speed = 2;
	}
}





