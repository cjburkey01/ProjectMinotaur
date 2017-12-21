using UnityEngine;

public abstract class IMenu : MonoBehaviour {

	public bool Enabled { private set; get; }
	protected MenuSystem MenuSystem { private set; get; }

	public abstract void OnShow();
	public abstract void OnHide();

	public void Init(MenuSystem menuSystem) {
		MenuSystem = menuSystem;
	}

	public void Show() {
		Enabled = true;
	}

	public void Hide() {
		Enabled = false;
	}

	public void ToggleShown() {
		Enabled = !Enabled;
	}

	public bool IsShown() {
		return Enabled;
	}

}