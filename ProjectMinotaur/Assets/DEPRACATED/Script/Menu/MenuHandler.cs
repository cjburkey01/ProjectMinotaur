using UnityEngine;

public class MenuHandler : MonoBehaviour {

	public static MenuHandler instance { private set; get; }

	public Menu[] menus;
	private Menu openMenu;

	void Start() {
		instance = this;
		foreach (Menu menu in menus) {
			menu.OnClose();
		}
	}

	public void OpenMenu(string name) {
		CloseMenu();
		foreach (Menu menu in menus) {
			if (menu.GetName().Equals(name)) {
				openMenu = menu;
				menu.OnOpen();
				return;
			}
		}
	}

	public void CloseMenu() {
		if (openMenu != null) {
			openMenu.OnClose();
			openMenu = null;
		}
	}

	public bool IsMenuOpen() {
		return openMenu != null;
	}

	public bool MenuExists(string name) {
		foreach (Menu menu in menus) {
			if (menu.GetName().Equals(name)) {
				return true;
			}
		}
		return false;
	}

	public string GetOpenMenu() {
		return ((IsMenuOpen()) ? (openMenu.GetName()) : (null));
	}

}