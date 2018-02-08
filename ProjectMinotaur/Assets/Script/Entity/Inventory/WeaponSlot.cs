using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour {

	public static readonly Color DEFAULT_COLOR = new Color(88 / 256.0f, 88 / 256.0f, 88 / 256.0f, 1.0f);
	public static readonly Color SELECTED_COLOR = new Color(124 / 256.0f, 15 / 256.0f, 15 / 256.0f, 1.0f);

	private Image self;
	private Image iconSlot;

	private bool hasInit = false;

	private void Init() {
		if (hasInit) {
			return;
		}
		hasInit = true;
		self = GetComponent<Image>();
		iconSlot = GetComponentsInChildren<Image>()[1];
		if (self == null || iconSlot == null) {
			Debug.LogError("Weapon slot didn't have an image object attached.");
			Destroy(gameObject);
		}
		SetSelected(false);
	}

	public void SetIcon(Sprite spr) {
		Init();
		iconSlot.sprite = spr;
	}

	public void SetSelected(bool selected) {
		Init();
		if (self != null) {
			if (selected) {
				self.color = SELECTED_COLOR;
			} else {
				self.color = DEFAULT_COLOR;
			}
		}
	}

}