using UnityEngine;
using System.Collections;



public interface IJumpable{
	void JumpedOn();
}

public interface IRespawnable{
	void Respawn(Transform where);
}

public interface ITriggerable{
	void Triggered(EffectList effect);
	void UnTriggered(EffectList effect);
}

public interface IResettable{
	void Reset();
}

public interface IMovable{
	//Set un delais d'attente au owner
	void Wait(float seconds);
	//Arrete le owner
	void Stop(bool isStopped);
	//Set une nouvelle vitesse au owner
	void NewSpeed(float speed);
	//Set l'orientation du owner : quaternion.identity ou forward ou fullforward
	void ChangeRotation(int orientation);
	//Teleporte le owner au waypoint
	void Teleport(WaypointScript where);
	//Fait une rotation a 90° avec un delais ou non
	void RotateHalf(MovableRotation rotation);
	//Fait une rotation a 180° avec un delais ou non
	void RotateFull(MovableRotation rotation);
}