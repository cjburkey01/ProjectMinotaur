using UnityEngine;

public class PauseListener : MonoBehaviour {

	void Update() {
		if (Input.GetButtonDown("Cancel")) {
			TogglePauseMenu();
		}
	}

	public static void TogglePauseMenu() {
		if (GameHandler.paused) {
			Unpause();
		} else {
			Pause();
		}
	}

	public static void Pause() {
		GameHandler.Pause();
		OpenMenu();
	}

	public static void Unpause() {
		GameHandler.Unpause();
		CloseMenu();
	}

	private static void OpenMenu() {
		if (MenuHandler.instance != null && MenuHandler.instance.MenuExists("MenuPause")) {
			MenuHandler.instance.OpenMenu("MenuPause");
		}
	}

	private static void CloseMenu() {
		if (MenuHandler.instance != null) {
			MenuHandler.instance.CloseMenu();
		}
	}

}