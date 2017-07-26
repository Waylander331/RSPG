using UnityEngine;
using System.Collections;

// La classe générique des effets. Sera le parent de chaque effets.
public abstract class EffectList : MonoBehaviour 
{
	public abstract void Activate(IsTriggerable triggeredObject); // C'est dans cette fonction que l'on exécute l'effet.
	public abstract void Deactivate (IsTriggerable triggeredObject);
}
