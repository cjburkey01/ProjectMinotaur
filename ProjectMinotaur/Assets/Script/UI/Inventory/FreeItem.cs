using UnityEngine;
using UnityEngine.UI;

public class FreeItem : MonoBehaviour {
	
	public static FreeItem Instance { private set; get; }

	private ItemStack _stack;
	private RectTransform rect;

	public Image icon;
	public Image iconBackground;
	public ItemStack Stack {
		set {
			_stack = value;
			if (_stack == null || _stack.IsEmpty) {
				_stack = null;
				icon.gameObject.SetActive(false);
				iconBackground.gameObject.SetActive(false);
			} else {
				icon.gameObject.SetActive(true);
				icon.sprite = _stack.Item.Icon512;
				iconBackground.gameObject.SetActive(true);
			}
		}
		get {
			return _stack;
		}
	}

	public FreeItem() {
		Instance = this;
	}

	void Awake() {
		rect = GetComponent<RectTransform>();
		Stack = null;
	}

	void Update() {
		if (Stack != null && !Stack.IsEmpty) {
			rect.position = Input.mousePosition;
		}
	}

}