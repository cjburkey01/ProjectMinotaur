using UnityEngine;

public abstract class Menu : MonoBehaviour {

	public abstract string GetName();

	public abstract void OnOpen();

	public abstract void OnClose();

}