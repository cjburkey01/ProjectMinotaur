using UnityEngine;

public class ItemListWeapons : ItemList {
	
	public Hotbar hotbar;
	public ItemDisplay primary;
	public ItemDisplay secondary;

	public override void OpenInventory() {
		primary.parent = this;
		secondary.parent = this;

		primary.Item = hotbar.Primary.Stack;
		secondary.Item = hotbar.Secondary.Stack;

		UpdateInventory();
	}

	public override void CloseInventory() {
	}

	public override void UpdateInventory() {
		hotbar.SetWeapon(true, Weapon.Create(hotbar.Player, primary.Item));
		hotbar.SetWeapon(false, Weapon.Create(hotbar.Player, secondary.Item));

		UpdateSlot(primary);
		UpdateSlot(secondary);
	}

	private void UpdateSlot(ItemDisplay slot) {
		if (slot.Item == null || slot.Item.IsEmpty) {
			slot.Disable();
		} else {
			slot.Enable();
		}
	}

	public override void OnClick(ItemDisplay display) {
		if (display == null) {
			return;
		}
		ItemStack onCursor = FreeItem.Instance.Stack;
		if (primary.Equals(display)) {
			if (onCursor != null && !onCursor.IsEmpty) {
				TrySwap(true, onCursor);
			} else {
				if (primary.Item != null && !primary.Item.IsEmpty && !primary.Item.Item.Permanent) {
					FreeItem.Instance.Stack = primary.Item.Copy;
					primary.Item = null;
				}
			}
		} else if (secondary.Equals(display)) {
			if (onCursor != null && !onCursor.IsEmpty) {
				TrySwap(false, onCursor);
			} else {
				if (secondary.Item != null && !secondary.Item.IsEmpty && !secondary.Item.Item.Permanent) {
					FreeItem.Instance.Stack = secondary.Item.Copy;
					secondary.Item = null;
				}
			}
		}
		UpdateInventory();
	}

	public override void OnClick() {	// Item must be put into a specific slot, it can't just be added to this pseudo-inventory
	}

	private bool TrySwap(bool primary, ItemStack stack) {
		if (stack == null || stack.IsEmpty || !(stack.Item is GameItemHoldable)) {
			return false;
		}
		if ((stack.Item as GameItemHoldable).IsPrimary != primary) {
			return false;
		}
		ItemStack inSlot = ((primary) ? this.primary : secondary).Item;
		if (inSlot == null || inSlot.IsEmpty || inSlot.Item.Permanent) {
			((primary) ? this.primary : secondary).Item = stack.Copy;
			FreeItem.Instance.Stack = null;
		} else {
			FreeItem.Instance.Stack = inSlot.Copy;
			((primary) ? this.primary : secondary).Item = stack.Copy;
		}
		return true;
	}

}