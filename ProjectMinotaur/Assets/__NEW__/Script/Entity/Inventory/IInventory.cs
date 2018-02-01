using System.Collections.Generic;

public interface IInventory {

	InventorySize GetSize();
	InventorySlot[] GetContents();
	bool WillFitInInventory(IGameItem item);
	void AddToInventory(IGameItem item);
	void SetItem(int x, int y, IGameItem item);
	IGameItem GetItem(int x, int y);
	bool IsSlotOccupied(int x, int y);

}

public class InventorySlot {

	public readonly int x;
	public readonly int y;
	private IGameItem _Item;

	public IGameItem Item {
		set {
			_Item = Item;
		}
		get {
			return _Item;
		}
	}

	public InventorySlot(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj) {
		var slot = obj as InventorySlot;
		return slot != null && x == slot.x && y == slot.y && EqualityComparer<IGameItem>.Default.Equals(_Item, slot._Item);
	}

	public override int GetHashCode() {
		var hashCode = 166230456;
		hashCode = hashCode * -1521134295 + x.GetHashCode();
		hashCode = hashCode * -1521134295 + y.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<IGameItem>.Default.GetHashCode(_Item);
		return hashCode;
	}

}

public struct InventorySize {

	public readonly int x;
	public readonly int y;

	public InventorySize(int x, int y) {
		this.x = x;
		this.y = y;
	}

	public override bool Equals(object obj) {
		if (!(obj is InventorySize)) {
			return false;
		}
		var size = (InventorySize) obj;
		return x == size.x && y == size.y;
	}

	public override int GetHashCode() {
		var hashCode = 1502939027;
		hashCode = hashCode * -1521134295 + base.GetHashCode();
		hashCode = hashCode * -1521134295 + x.GetHashCode();
		hashCode = hashCode * -1521134295 + y.GetHashCode();
		return hashCode;
	}

}