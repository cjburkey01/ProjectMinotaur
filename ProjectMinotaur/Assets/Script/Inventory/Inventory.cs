using System.Collections.Generic;
using UnityEngine;

public sealed class Inventory {

	public readonly string name;
	public readonly int slots;
    private readonly List<ItemStack> items;

	public Inventory(string name, int slots) {
		this.name = name;
		this.slots = slots;
        items = new List<ItemStack>();
	}

	/// <summary>
	///		Attempts to add the item stack to the inventory; this will also attempt merge with other items stacks with the same item and data information.
	/// </summary>
	/// <param name="stack">The stack to add.</param>
	/// <returns>Returns true on success, or false if the supplied itemstack was empty or the inventory is full.</returns>
	public bool Add(ItemStack inS) {
		ItemStack inStack = inS.Copy;
		foreach (ItemStack item in items) {
			if (item != null && !item.IsEmpty&& item.Item.Equals(inStack.Item) && item.Data.Equals(inStack.Data) && item.Count < item.Item.MaxStackSize) {
				int toAdd = item.Item.MaxStackSize - item.Count;
				item.Count += Mathf.Min(toAdd, inStack.Count);
				inStack.Count -= Mathf.Min(toAdd, inStack.Count);
				if (toAdd >= inStack.Count) {
					return true;
				}
			}
		}
        if (items.Count >= slots) {
            return false;
		}
        items.Add(inStack);
        return true;
	}

	public bool Remove(ItemStack stack) {
		if (items.Contains(stack)) {
			items.Remove(stack);
			return true;
		}
		return false;
	}

    public ItemStack Take(GameItem item) {
        ItemStack found = GetItem(item);
		if (found != null && !found.IsEmpty) {
			items.Remove(found);
			return found;
		}
		return null;
    }

	public ItemStack GetItem(GameItem item) {
		foreach (ItemStack stack in items) {
			if (stack != null && !stack.IsEmpty&& stack.Item.Equals(item)) {
				return stack;
			}
		}
		return null;
	}

	public ItemStack[] GetItems() {
		return items.ToArray();
	}

	public override bool Equals(object obj) {
		var inventory = obj as Inventory;
		return inventory != null && EqualityComparer<string>.Default.Equals(name, inventory.name) && slots == inventory.slots && EqualityComparer<List<ItemStack>>.Default.Equals(items, inventory.items);
	}

	public override int GetHashCode() {
		var hashCode = -1360609007;
		hashCode = hashCode * -1521134295 + name.GetHashCode();
		hashCode = hashCode * -1521134295 + slots.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<List<ItemStack>>.Default.GetHashCode(items);
		return hashCode;
	}

	public override string ToString() {
		return "Inventory: " + name;
	}

}