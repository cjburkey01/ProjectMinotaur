using UnityEngine;

public class GameHandler {

	public static bool paused { private set; get; }

	// Pause the game, free the cursor
	public static void Pause() {
		Cursor.lockState = CursorLockMode.None;
		paused = true;
	}

	// Play the game, lock the cursor
	public static void Unpause() {
		paused = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

}