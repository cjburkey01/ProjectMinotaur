using UnityEngine;

public class MenuOptions : Menu {

	public RectTransform root;

	public override string GetName() {
		return "MenuOptions";
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

	public void OnGeneralClick() {
		
	}

	public void OnGraphicsClick() {
		if (MenuHandler.instance.MenuExists("MenuOptionsGraphics")) {
			MenuHandler.instance.OpenMenu("MenuOptionsGraphics");
		}
	}

	public void OnAudioClick() {

	}

	public void OnBackClick() {
		if (MenuHandler.instance.MenuExists("MenuPause")) {
			MenuHandler.instance.OpenMenu("MenuPause");
		}
	}

}