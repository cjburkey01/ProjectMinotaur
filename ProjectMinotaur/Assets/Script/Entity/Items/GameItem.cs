using System.Collections.Generic;

public class GameItem {

	public string UniqueId { private set; get; }
	public string DisplayName { private set; get; }
	public string Description { private set; get; }
	public int MaxStackSize { private set; get; }

	public GameItem(string uid, string name, string description) {
		UniqueId = uid;
		DisplayName = name;
		Description = description;
		MaxStackSize = 1;
	}

	public override bool Equals(object obj) {
		var item = obj as GameItem;
		return item != null && UniqueId == item.UniqueId && DisplayName == item.DisplayName && Description == item.Description && MaxStackSize == item.MaxStackSize;
	}

	public override int GetHashCode() {
		var hashCode = 1813968700;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UniqueId);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
		hashCode = hashCode * -1521134295 + MaxStackSize.GetHashCode();
		return hashCode;
	}

	public override string ToString() {
		return UniqueId;
	}

}

public class ItemStack {

	public GameItem Item { private set; get; }
	public int Count { set; get; }

	public ItemStack(GameItem item, int count) {
		Item = item;
		Count = count;
	}

	public bool IsEmpty() {
		return Item == null || Count < 1;
	}

	public ItemStack Copy() {
		return new ItemStack(Item, Count);
	}

	public override bool Equals(object obj) {
		var stack = obj as ItemStack;
		return stack != null && EqualityComparer<GameItem>.Default.Equals(Item, stack.Item);
	}

	public override int GetHashCode() {
		return -979861770 + EqualityComparer<GameItem>.Default.GetHashCode(Item);
	}

	public override string ToString() {
		return Item + "x" + Count;
	}

}