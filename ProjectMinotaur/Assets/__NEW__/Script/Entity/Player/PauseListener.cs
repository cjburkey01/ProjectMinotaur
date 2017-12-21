using UnityEngine;

public class PauseListener : MonoBehaviour {

	private PlayerMove ply;

	void Update() {
		if (ply == null) {
			ply = FindObjectOfType<PlayerMove>();
		}
		if (Input.GetButtonDown("Cancel") && !ply.locked) {
			TogglePause();
		}
	}

	public static void TogglePause() {
		if (GameHandler.paused) {
			Unpause();
		} else {
			Pause();
		}
	}

	public static void Pause() {
		GameHandler.Pause();
	}

	public static void Unpause() {
		GameHandler.Unpause();
	}

}