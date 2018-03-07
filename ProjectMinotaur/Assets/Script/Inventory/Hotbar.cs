using UnityEngine;

public class Hotbar {

	private Player player;
	public Weapon Primary;
	public Weapon Secondary;

	public int Slot { private set; get; }
	public bool IsPrimarySelected { private set; get; }

	public Hotbar(Player player) {
		this.player = player;
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
			player.MainInventory.Set(15, Primary.Stack);
		} else {
			if (Secondary != null) {
				Object.Destroy(Secondary.gameObject);
			}
			Secondary = weapon;
			player.MainInventory.Set(16, Secondary.Stack);
		}
		if (Primary != null && Primary.WeaponType.Icon32 != null) {
			player.PlayerOverlay.primarySlot.SetIcon(Primary.WeaponType.Icon32);
		}
		if (Secondary != null && Secondary.WeaponType.Icon32 != null) {
			player.PlayerOverlay.secondarySlot.SetIcon(Secondary.WeaponType.Icon32);
		}
	}

	public void UpdateSlot(int slot) {
		Slot = slot % 2;
	}

	public ItemStack GetStackInSlot(int slot) {
		return player.MainInventory.GetItemStack(15 + Slot);
	}

}