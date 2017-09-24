using UnityEngine;
using UnityEngine.UI;

public class MenuPause : Menu {

	public RectTransform root;

	public override string GetName() {
		return "MenuPause";
	}

	public override void OnOpen() {
		if (root != null) {
			root.gameObject.SetActive(true);
		}
	}

	public override void OnClose() {
		if (root != null) {
			root.gameObject.SetActive(false);
		}
	}

	public void OnResumeClick() {
		PauseListener.Unpause();
	}

	public void OnOptionsClick() {
		if (MenuHandler.instance.MenuExists("MenuOptions")) {
			MenuHandler.instance.OpenMenu("MenuOptions");
		}
	}

	public void OnMainMenuClick() {

	}

	public void OnQuitClick() {
		Application.Quit();
	}

}