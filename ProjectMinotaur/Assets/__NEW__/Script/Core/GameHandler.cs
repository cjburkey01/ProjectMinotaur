using UnityEngine;

public static class GameHandler {

	public static bool Paused { private set; get; }

	public static void Pause() {
		Paused = true;
	}

	public static void Unpause() {
		Paused = false;
	}

}