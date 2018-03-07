using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour {
	
	public int slotId;

	private Image icon;
	private ItemStack stack;
	private UIInventory inventory;

	void OnEnable() {
		icon = GetComponentsInChildren<Image>()[1];
	}

	public void SetParent(UIInventory inv) {
		inventory = inv;
	}

	public void SetItem(ItemStack stack) {
		this.stack = (stack == null) ? null : stack.Copy();
		RefreshIcon();
	}

	public ItemStack GetStack() {
		return stack;
	}
	
	private void RefreshIcon() {
		if (stack == null || stack.IsEmpty()) {
			//Debug.Log("Before " + icon.sprite);
			icon.sprite = null;
			//Debug.Log("After " + icon.sprite);
			return;
		}
		icon.sprite = stack.Item.Icon512;
	}

	public void OnClick() {
		inventory.OnClick(this);
	}
	
	public override bool Equals(object obj) {
		var slot = obj as UIItemSlot;
		return slot != null && base.Equals(obj) && slotId == slot.slotId && EqualityComparer<Image>.Default.Equals(icon, slot.icon) && EqualityComparer<ItemStack>.Default.Equals(stack, slot.stack) && EqualityComparer<UIInventory>.Default.Equals(inventory, slot.inventory);
	}

	public override int GetHashCode() {
		var hashCode = -1513938294;
		hashCode = hashCode * -1521134295 + base.GetHashCode();
		hashCode = hashCode * -1521134295 + slotId.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<Image>.Default.GetHashCode(icon);
		hashCode = hashCode * -1521134295 + EqualityComparer<ItemStack>.Default.GetHashCode(stack);
		hashCode = hashCode * -1521134295 + EqualityComparer<UIInventory>.Default.GetHashCode(inventory);
		return hashCode;
	}

}