using UnityEngine;

public class InventoryPlayer : MonoBehaviour, IInventory {

	private readonly InventorySize size = new InventorySize(5, 5);
	private InventorySlot[] inv;

	public InventoryPlayer() {
		inv = new InventorySlot[size.x * size.y];
	}

	public InventorySize GetSize() {
		return size;
	}

	public InventorySlot[] GetContents() {
		return (InventorySlot[]) inv.Clone();
	}

	public bool WillFitInInventory(IGameItem item) {
		if (item.GetSize().x > size.x || item.GetSize().y > size.y) {
			return false;
		}

		// TODO: DO THIS.

		return false;
	}

	public void AddToInventory(IGameItem item) {
		// TODO: DO THIS.
	}

	public void SetItem(int x, int y, IGameItem item) {
		if (x > size.x || y > size.y) {
			return;
		}
		inv[y * size.x + x].Item = item;
	}

	public IGameItem GetItem(int x, int y) {
		if (x > size.x || y > size.y) {
			return null;
		}
		return inv[y * size.x + x].Item;
	}

	public bool IsSlotOccupied(int x, int y) {
		// TODO: DO THIS.
		return false;
	}

}