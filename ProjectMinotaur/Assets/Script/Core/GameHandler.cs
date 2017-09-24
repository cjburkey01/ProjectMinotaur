using UnityEngine;

public class GameHandler {

	public static bool paused { private set; get; }

	// Pause the game, free the cursor
	public static void Pause() {
		paused = true;
		Cursor.lockState = CursorLockMode.None;
	}

	// Play the game, lock the cursor
	public static void Unpause() {
		Cursor.lockState = CursorLockMode.Locked;
		paused = false;
	}

}