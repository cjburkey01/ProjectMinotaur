using UnityEngine;

public class Hotbar {

	private Player player;
	public readonly int Size = 2;
	public Inventory Inventory { private set; get; }
	public Weapon Primary;
	public Weapon Secondary;

	public int Slot { private set; get; }
	public bool IsPrimarySelected { private set; get; }

	public Hotbar(Player player) {
		this.player = player;
		Inventory = new Inventory("PlayerInventoryToolbar", Size);
		SetWeapon(true, null);
		SetWeapon(false, null);
		SelectWeapon(true);
	}

	public void Exit() {
		if (Primary != null) {
			Object.Destroy(Primary.gameObject);
		}
		if (Secondary != null) {
            Object.Destroy(Secondary.gameObject);
		}
	}

	public void SwitchWeapon() {
		SelectWeapon(!IsPrimarySelected);
		player.PlayerOverlay.primarySlot.SetSelected(IsPrimarySelected);
		player.PlayerOverlay.secondarySlot.SetSelected(!IsPrimarySelected);
	}

	public void SelectWeapon(bool primary) {
		IsPrimarySelected = primary;
		Primary.gameObject.SetActive(IsPrimarySelected);
		Secondary.gameObject.SetActive(!IsPrimarySelected);
	}

	public Weapon GetWeapon() {
		return (IsPrimarySelected) ? Primary : Secondary;
	}

	public void SetWeapon(bool which, Weapon weapon) {
		if (weapon == null) {
			weapon = Weapon.Create(true, player, DefaultWeapons.Fist);
		}
		if (which) {
			if (Primary != null) {
				Object.Destroy(Primary.gameObject);
			}
			Primary = weapon;
		} else {
			if (Secondary != null) {
				Object.Destroy(Secondary.gameObject);
			}
			Secondary = weapon;
		}
		if (Primary != null && Primary.WeaponType.Icon != null) {
			player.PlayerOverlay.primarySlot.SetIcon(Primary.WeaponType.Icon);
		}
		if (Secondary != null && Secondary.WeaponType.Icon != null) {
			player.PlayerOverlay.secondarySlot.SetIcon(Secondary.WeaponType.Icon);
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