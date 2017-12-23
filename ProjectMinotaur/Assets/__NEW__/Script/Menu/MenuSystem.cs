using UnityEngine;

public class MenuSystem : MonoBehaviour {

	public IMenu[] menus;

	void Start() {
		foreach (IMenu menu in menus) {
			menu.Init(this);
		}
		HideMenus();
	}

	public void HideMenus() {
		foreach (IMenu menu in menus) {
			HideMenu(menu);
		}
	}

	public void HideMenu(IMenu menu) {
		Debug.Log("Hiding " + menu.GetType().Name);
		menu.Hide();
		menu.OnHide();
		menu.gameObject.SetActive(false);
	}

	public void ShowMenu(IMenu menu) {
		HideMenus();
		menu.gameObject.SetActive(true);
		menu.Show();
		menu.OnShow();
	}

	public static MenuSystem GetInstance() {
		return FindObjectOfType<MenuSystem>();
	}

}