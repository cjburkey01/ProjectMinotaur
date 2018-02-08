﻿using UnityEngine;

/// <summary>
///		Fires when the weapon is used.
/// </summary>
/// <param name="instance">The instance of the weapon that is using the primary attack.</param>
/// <returns>True if default damaging behavior will be used, or false if a custom handler will be used.</returns>
public delegate bool OnWeaponAttack(Weapon instance);

/// <summary>
///		Fires when the weapon is reloaded.
/// </summary>
/// <param name="instance">The instance of the weapon that is reloading.</param>
/// <returns>True if the default reload behavior will be used, or false if a custom reloading should be used.</returns>
public delegate bool OnWeaponReload(Weapon instance);

public sealed class WeaponDefinition : GameItem {

	public readonly float resetTime;
	public readonly int damage;
	public readonly float maxDistance;
	public readonly float spray;    // In degrees
	public readonly int ammoPerClip;
	public readonly int shotsPerPrimary;
	public readonly Vector3 displayPositionOffset;
	public readonly Vector3 displayRotationOffset;
	public readonly Vector3 barrelPosition;

	private OnWeaponAttack primaryAction = ((weapon) => { return true; });
	private OnWeaponAttack secondaryAction = ((weapon) => { return true; });
	private OnWeaponReload reloadAction = ((weapon) => { return true; });

	/// <summary>
	///		Creates a new type of weapon with the supplied parameters.
	/// </summary>
	/// <param name="name">The display name of this weapon.</param>
	/// <param name="desc">The displayed description of the weapon.</param>
	/// <param name="resetTime">How many seconds elapse before the weapon is ready to fire again.</param>
	/// <param name="damage">How much damage the weapon does (will not be used if custom events are enabled).</param>
	/// <param name="maxDistance">How far away a target may be, at most, for the damage to be afflicted.</param>
	/// <param name="spray">Degrees at which each bullet may leave the barrel.</param>
	/// <param name="ammoPerClip">How many bullets are held per clip.</param>
	/// <param name="shotsPerPrimary">How many bullets are fired per shot.</param>
	/// <param name="displayPositionOffset">Offsset locally from the hand of the player.</param>
	/// <param name="displayRotationOffset">The rotation of the weapon compared to the hand.</param>
	/// <param name="barrelPosition">The position of the end of the barrel (relative to the gun)</param>
	public WeaponDefinition(string unique, string name, string desc, float resetTime, int damage, float maxDistance, float spray, int ammoPerClip, int shotsPerPrimary, Vector3 displayPositionOffset, Vector3 displayRotationOffset, Vector3 barrelPosition) : base(unique, name, desc) {
		this.resetTime = resetTime;
		this.damage = damage;
		this.maxDistance = maxDistance;
		this.spray = spray;
		this.ammoPerClip = ammoPerClip;
		this.shotsPerPrimary = shotsPerPrimary;
		this.displayPositionOffset = displayPositionOffset;
		this.displayRotationOffset = displayRotationOffset;
		this.barrelPosition = barrelPosition;
	}

	/// <summary>
	///		Initializes all of the events with the provided replacements.
	/// </summary>
	/// <param name="primary">Called upon the primary attack.</param>
	/// <param name="secondary">Called upon the secondary attack.</param>
	/// <param name="reload">Called upon the reloading of the gun.</param>
	public void SetCustomEvents(OnWeaponAttack primary, OnWeaponAttack secondary, OnWeaponReload reload) {
		SetOnPrimary(primary);
		SetOnSecondary(secondary);
		SetOnReload(reload);
	}

	public void SetOnPrimary(OnWeaponAttack primary) {
		primaryAction = primary;
	}

	public void SetOnSecondary(OnWeaponAttack secondary) {
		secondaryAction = secondary;
	}

	public void SetOnReload(OnWeaponReload reload) {
		reloadAction = reload;
	}

	public void OnPrimary(Weapon instance) {
		bool def = primaryAction.Invoke(instance);
		if (def) {

		}
	}

	public void OnSecondary(Weapon instance) {
		bool def = secondaryAction.Invoke(instance);
		if (def) { }
	}

	/// <summary>
	///		Called when the weapon is reloaded.
	/// </summary>
	/// <param name="instance">The instance of the weapon that reloaded.</param>
	public void OnReload(Weapon instance) {
		bool def = reloadAction.Invoke(instance);
		if (def) {
			if (ammoPerClip > 0) {
				if (instance.clipCount > 0) {
					instance.clipCount--;
					instance.currentClipAmmo = ammoPerClip;
				}
			}
		}
	}

}