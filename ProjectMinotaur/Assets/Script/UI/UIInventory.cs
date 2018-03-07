using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour {
	
	public Inventory Inventory { private set; get; }
	private Dictionary<int, UIItemSlot> slots;

	public void SetInventory(Inventory inventory) {
		UIItemSlot[] found = GetComponentsInChildren<UIItemSlot>();
		if (found.Length != inventory.slots) {
			Debug.LogError("Incorrect number of slots for inventory: " + inventory.name + ". UI Has: " + found.Length + ", but " + inventory.slots + " in inventory.");
			return;
		}
		Inventory = inventory;
		slots = new Dictionary<int, UIItemSlot>();
		foreach (UIItemSlot f in found) {
			slots.Add(f.slotId, f);
			f.SetParent(this);
		}
	}

	public void RefreshInventory() {
		for (int i = 0; i < slots.Count; i ++) {
			slots[i].SetItem(Inventory.GetItemStack(i));
		}
	}

	public void OnClick(UIItemSlot slot) {
		Debug.Log("Inventory click on slot: " + slot.slotId);
	}

	public override bool Equals(object obj) {
		var inventory = obj as UIInventory;
		return inventory != null && base.Equals(obj) && EqualityComparer<Inventory>.Default.Equals(Inventory, inventory.Inventory) && EqualityComparer<Dictionary<int, UIItemSlot>>.Default.Equals(slots, inventory.slots);
	}

	public override int GetHashCode() {
		var hashCode = 2066035421;
		hashCode = hashCode * -1521134295 + base.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<Inventory>.Default.GetHashCode(Inventory);
		hashCode = hashCode * -1521134295 + EqualityComparer<Dictionary<int, UIItemSlot>>.Default.GetHashCode(slots);
		return hashCode;
	}

}