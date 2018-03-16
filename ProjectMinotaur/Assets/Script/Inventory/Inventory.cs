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
	///		Attempts to add the item stack to the inventory.
	/// </summary>
	/// <param name="stack">The stack to add.</param>
	/// <returns>Returns true on success, or false if the supplied itemstack was empty or the inventory is full.</returns>
	public bool Add(ItemStack stack) {
        if (items.Count >= slots) {
            return false;
		}
        items.Add(stack);
        return true;
	}

    public ItemStack Take(ItemStack stack) {
        
    }

	public override bool Equals(object obj) {
		var inventory = obj as Inventory;
		return inventory != null && EqualityComparer<string>.Default.Equals(name, inventory.name) && slots == inventory.slots && EqualityComparer<List<ItemStack>>.Default.Equals(contents, inventory.contents);
	}

	public override int GetHashCode() {
		var hashCode = -1360609007;
		hashCode = hashCode * -1521134295 + name.GetHashCode();
		hashCode = hashCode * -1521134295 + slots.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<ItemStack[]>.Default.GetHashCode(contents);
		return hashCode;
	}

	public override string ToString() {
		return "Inventory: " + name;
	}

}