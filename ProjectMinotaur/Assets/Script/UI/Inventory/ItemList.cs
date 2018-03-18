using UnityEngine;
using UnityEngine.UI;

public class ItemList : MonoBehaviour {
	
	public Scrollbar scrollBar;
	public Inventory inventory;
	public ItemDisplay itemPrefab;

	public virtual void OpenInventory() {
		UpdateInventory();
		FreeItem.Instance.gameObject.SetActive(true);
		scrollBar.value = 1.0f;
	}

	public virtual void CloseInventory() {
		FreeItem.Instance.gameObject.SetActive(false);
	}

	public virtual void UpdateInventory() {
		foreach (Transform child in transform) {
			Destroy(child.gameObject);
		}
		foreach (ItemStack item in inventory.GetItems()) {
			if (item == null || item.IsEmpty) {
				continue;
			}
			if (itemPrefab == null) {
				Debug.LogError("ItemList prefab is null.");
				return;
			}
			ItemDisplay itemDisplay = Instantiate(itemPrefab.gameObject).GetComponent<ItemDisplay>();
			if (itemDisplay == null) {
				Debug.LogError("ItemDisplay not found on item render prefab.");
				return;
			}
			itemDisplay.parent = this;
			itemDisplay.Item = item;
			itemDisplay.transform.SetParent(transform, false);
		}
	}

	// Grab an item in the clicked slot
	public virtual void OnClick(ItemDisplay display) {
		if (!inventory.Remove(display.Item)) {
			Debug.LogError("Failed to remove item stack from inventory: " + display.Item);
		} else {
			if (FreeItem.Instance.Stack != null && !FreeItem.Instance.Stack.IsEmpty) {
				inventory.Add(FreeItem.Instance.Stack);
			}
			FreeItem.Instance.Stack = display.Item;
		}
		UpdateInventory();
	}

	// Add an item to the inventory.
	public virtual void OnClick() {
		if (FreeItem.Instance.Stack != null && !FreeItem.Instance.Stack.IsEmpty) {
			if (inventory.Add(FreeItem.Instance.Stack)) {
				FreeItem.Instance.Stack = null;
			}
		}
		UpdateInventory();
	}

}