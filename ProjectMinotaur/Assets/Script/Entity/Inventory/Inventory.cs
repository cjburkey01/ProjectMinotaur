using System.Collections.Generic;

public sealed class Inventory {

	public string Name { private set; get; }
	public Player OpenTo { private set; get; }
	public int Slots { private set; get; }
	private ItemStack[] contents;

	public Inventory(string name, int slots) {
		Name = name;
		Slots = slots;
		contents = new ItemStack[slots];
	}

	/// <summary>
	///		Attempts to add the item stack to the inventory.
	/// </summary>
	/// <param name="stack">The stack to add.</param>
	/// <returns>Returns true on success, or false if the supplied itemstack was empty or the inventory is full.</returns>
	public bool Add(ItemStack stack) {
		if (stack == null || stack.IsEmpty()) {
			return false;
		}
		int count = stack.Count;
		for (int i = 0; i < Slots && count > 0; i ++) {
			if (!HasItem(i)) {	// Empty slot
				if (count > stack.Item.MaxStackSize) {	// Fill up this slot and move on, the input has too many items.
					count -= stack.Item.MaxStackSize;
					contents[i] = new ItemStack(stack.Item, stack.Item.MaxStackSize);
				} else {	// Just fill it up, the input stack is less than the maximum stack size.
					contents[i] = new ItemStack(stack.Item, count);
					return true;
				}
				continue;
			}
			if (!GetItemStack(i).Item.Equals(stack.Item)) {	// Wrong type of item.
				continue;
			}
			int open = stack.Item.MaxStackSize - GetItemStack(i).Count;
			int sub = count - open;
			if (sub > 0) {	// If there will be left over items after this slot is filled.
				GetItemStack(i).Count = stack.Item.MaxStackSize;
				count = sub;
				continue;
			}
			// No left over items, we're done after this.
			GetItemStack(i).Count += count;
			return true;
		}
		return false;	// Inventory filled up.
	}

	// Will return the item stack in the specified slot, or null if one was not found or the slot contains an empty itemstack.
	public ItemStack GetItemStack(int slot) {
		if (slot < 0 || slot >= Slots) {
			return null;
		}
		if (!contents[slot].IsEmpty()) {
			return contents[slot];
		}
		return null;
	}

	// Returns whether or not the slot contains an item.
	public bool HasItem(int slot) {
		return contents[slot] != null;
	}

	public void ShowInventory(Player player) {
		OpenTo = player;
	}

	public void CloseInventory() {
		OpenTo = null;
	}

	public override bool Equals(object obj) {
		var inventory = obj as Inventory;
		return inventory != null && EqualityComparer<Player>.Default.Equals(OpenTo, inventory.OpenTo) && Slots == inventory.Slots && EqualityComparer<ItemStack[]>.Default.Equals(contents, inventory.contents);
	}

	public override int GetHashCode() {
		var hashCode = -1360609007;
		hashCode = hashCode * -1521134295 + EqualityComparer<Player>.Default.GetHashCode(OpenTo);
		hashCode = hashCode * -1521134295 + Slots.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<ItemStack[]>.Default.GetHashCode(contents);
		return hashCode;
	}

	public override string ToString() {
		return "Inventory: " + Name;
	}

}