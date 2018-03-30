using UnityEngine;

public class Hotbar {

	public Player Player { private set; get; }
	public Weapon Primary;
	public Weapon Secondary;
	public ItemListWeapons InventoryWeapons { private set; get; }

	public int Slot { private set; get; }
	public bool IsPrimarySelected { private set; get; }

	public Hotbar(Player player, ItemListWeapons inventoryWeapons) {
		Player = player;
		InventoryWeapons = inventoryWeapons;
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
	}

	public void SelectWeapon(bool primary) {
		IsPrimarySelected = primary;
		Player.PlayerOverlay.primarySlot.SetSelected(IsPrimarySelected);
		Player.PlayerOverlay.secondarySlot.SetSelected(!IsPrimarySelected);
		if (Primary != null && Primary.gameObject != null) {
			Primary.gameObject.SetActive(IsPrimarySelected);
		}
		if (Secondary != null && Secondary.gameObject != null) {
			Secondary.gameObject.SetActive(!IsPrimarySelected);
		}
	}

	public Weapon GetWeapon() {
		return (IsPrimarySelected) ? Primary : Secondary;
	}

	public void SetWeapon(bool which, Weapon weapon) {
		if (weapon == null) {
			weapon = Weapon.Create(Player, DefaultWeapons.Fist);
		}
		if (which) {
			if (Primary != null) {
				Object.Destroy(Primary.gameObject);
			}
			Primary = weapon;
			InventoryWeapons.primary.Item = (Primary != null) ? Primary.Stack : null;
		} else {
			if (Secondary != null) {
				Object.Destroy(Secondary.gameObject);
			}
			Secondary = weapon;
			InventoryWeapons.secondary.Item = (Secondary != null) ? Secondary.Stack : null;
		}
		if (Primary != null && Primary.WeaponType.Icon32 != null) {
			Player.PlayerOverlay.primarySlot.SetIcon(Primary.WeaponType.Icon32);
		}
		if (Secondary != null && Secondary.WeaponType.Icon32 != null) {
			Player.PlayerOverlay.secondarySlot.SetIcon(Secondary.WeaponType.Icon32);
		}
		SelectWeapon(IsPrimarySelected);
	}

	public void UpdateSlot(int slot) {
		Slot = slot % 2;
	}

}