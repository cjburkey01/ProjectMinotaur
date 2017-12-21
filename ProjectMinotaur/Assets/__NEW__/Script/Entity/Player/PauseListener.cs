using UnityEngine;

public class PauseListener : MonoBehaviour {

	public IMenu optionsMenu;

	private PlayerMove ply;

	void Update() {
		if (ply == null) {
			ply = FindObjectOfType<PlayerMove>();
			if (ply == null) {
				return;
			}
		}
		if (Input.GetButtonDown("Cancel") && !ply.locked) {
			TogglePause();
		}
	}

	public void TogglePause() {
		if (GameHandler.Paused) {
			Unpause();
		} else {
			Pause();
		}
	}

	public void Pause() {
		GameHandler.Pause();
		MenuSystem.GetInstance().ShowMenu(optionsMenu);
	}

	public void Unpause() {
		MenuSystem.GetInstance().HideMenu(optionsMenu);
		GameHandler.Unpause();
	}

	public static PauseListener GetInstance() {
		return FindObjectOfType<PauseListener>();
	}

}