public class Hotbar {

	private Player player;
	public readonly int Size = 3;
	public Inventory Inventory { private set; get; }
	public Weapon Primary;
	public Weapon Secondary;

	public int Slot { private set; get; }
	public bool IsPrimarySelected { private set; get; }

	public Hotbar(Player player) {
		this.player = player;
		Inventory = new Inventory("PlayerInventoryToolbar", Size);
	}

	public void SwitchWeapon() {
		SelectWeapon(!IsPrimarySelected);
	}

	public void SelectWeapon(bool primary) {
		IsPrimarySelected = primary;
	}

	public Weapon GetWeapon() {
		return (IsPrimarySelected) ? Primary : Secondary;
	}

	public void SetWeapon(bool which, Weapon weapon) {
		if (weapon == null) {
			weapon = Weapon.Create(true, DefaultWeapons.Fist);
		}
		if (which) {
			Primary = weapon;
		} else {
			Secondary = weapon;
		}
	}

	public void UpdateSlot(int slot) {
		Slot = slot % Size;
	}

	public ItemStack GetStackInSlot(int slot) {
		return Inventory.GetItemStack(Slot);
	}

	public bool AddItem(ItemStack stack) {
		return Inventory.Add(stack);
	}

}