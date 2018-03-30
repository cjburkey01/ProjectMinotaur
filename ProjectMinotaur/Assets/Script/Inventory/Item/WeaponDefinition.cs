using UnityEngine;
using SimpleJSON;

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

public sealed class WeaponDefinition : GameItemHoldable {

	public static readonly float DEG_TO_RAD = Mathf.PI / 180.0f;
	
	public readonly float resetTime;
	public readonly int damage;
	public readonly float maxDistance;
	public readonly float spray;    // In degrees
	public readonly int ammoPerClip;
	public readonly int shotsPerPrimary;
	public readonly Vector3 barrelPosition;
	public readonly bool drawTrail;
	public readonly bool auto;
	public readonly float recoilTime;
	public readonly float recoilX;
	public readonly float recoilY;
	public readonly float recoilSpeed;
	public readonly float swayAmount;
	public readonly float swayMax;
	public readonly float swaySmooth;

	private OnWeaponAttack primaryAction = ((weapon) => { return true; });
	private OnWeaponAttack secondaryAction = ((weapon) => { return true; });
	private OnWeaponReload reloadAction = ((weapon) => { return true; });

	/// <summary>
	///		Creates a new type of weapon with the supplied parameters.
	/// </summary>
	/// <param name="unique">A unique ID with which to keep track of this weapon.</param>
	/// <param name="name">The display name of this weapon.</param>
	/// <param name="desc">The displayed description of the weapon.</param>
	/// <param name="isPrimary">Whether this should go into the primary or secondary weapon slot.</param>
	/// <param name="resetTime">How many seconds elapse before the weapon is ready to fire again.</param>
	/// <param name="damage">How much damage the weapon does (will not be used if custom events are enabled).</param>
	/// <param name="maxDistance">How far away a target may be, at most, for the damage to be afflicted.</param>
	/// <param name="spray">Degrees at which each bullet may leave the barrel.</param>
	/// <param name="ammoPerClip">How many bullets are held per clip.</param>
	/// <param name="shotsPerPrimary">How many bullets are fired per shot.</param>
	/// <param name="displayPositionOffset">Offsset locally from the hand of the player.</param>
	/// <param name="displayRotationOffset">The rotation of the weapon compared to the hand.</param>
	/// <param name="barrelPosition">The position of the end of the barrel (relative to the gun)</param>
	/// <param name="modelPath">The location of the model (prefab) for this weapon.</param
	/// <param name="iconPath">The location of the icon (sprite) to be displayed for this weapon.</param>
	/// <param name="drawTrail">Whether or not to draw a bullet trail"</param>
	/// <param name="auto">Whether or not the weapon can be "held down"</param>
	/*public WeaponDefinition(string unique, string name, string desc, bool isPrimary, float resetTime, int damage, float maxDistance, float spray, int ammoPerClip, int shotsPerPrimary, Vector3 displayPositionOffset, Vector3 displayRotationOffset, Vector3 barrelPosition, string modelPath, string icon32Path, string icon512Path, bool drawTrail, bool auto, float recoilTime, float recoilX, float recoilY, float recoilSpeed) : base(unique, name, desc, 1) {
		this.isPrimary = isPrimary;
		this.resetTime = resetTime;
		this.damage = damage;
		this.maxDistance = maxDistance;
		this.spray = spray;
		this.ammoPerClip = ammoPerClip;
		this.shotsPerPrimary = shotsPerPrimary;
		this.displayPositionOffset = displayPositionOffset;
		this.displayRotationOffset = displayRotationOffset;
		this.barrelPosition = barrelPosition;
		Model = Resources.Load<GameObject>(modelPath);
		Icon32 = Resources.Load<Sprite>(icon32Path);
		Icon512 = Resources.Load<Sprite>(icon512Path);
		this.drawTrail = drawTrail;
		this.auto = auto;
		this.recoilTime = recoilTime;
		this.recoilX = recoilX;
		this.recoilY = recoilY;
		this.recoilSpeed = recoilSpeed;
	}*/

	public WeaponDefinition(JSONNode json) : base(json) {
		resetTime = json["reset_time"].AsFloat;
		damage = json["damage"].AsInt;
		maxDistance = json["max_distance"].AsFloat;
		spray = json["spray"].AsFloat;
		ammoPerClip = json["ammo_per_clip"];
		shotsPerPrimary = json["shots_per_primary"];
		barrelPosition = WeaponLoader.LoadVector3(json, "barrel_position_offset");
		drawTrail = json["draw_trail"].AsBool;
		auto = json["full_auto"].AsBool;
		recoilTime = json["recoil_time"].AsFloat;
		recoilX = json["recoil_x"].AsFloat;
		recoilY = json["recoil_y"].AsFloat;
		recoilSpeed = json["recoil_speed"].AsFloat;
		swayAmount = json["sway_amount"].AsFloat;
		swayMax = json["sway_max"].AsFloat;
		swaySmooth = json["sway_smooth"].AsFloat;
	}

	public override void CreateModel(WorldItem item) {
		if (model != null) {
			GameObject obj = Object.Instantiate(model, item.transform);
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
		}
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
			instance.SetCurrentClipAmmo(instance.GetCurrentClipAmmo() - 1);

			RaycastHit hit;
			Vector3 start = instance.Holder.LookCamera.transform.position;
			Vector3 dir = GetBulletSpread(instance.Holder.LookCamera.transform.forward) * Vector3.forward;

			float dist = maxDistance;
			if (Physics.Raycast(instance.Holder.LookCamera.transform.position, dir, out hit, maxDistance)) {
				dist = hit.distance;
				Debug.Log("Hit: " + hit.collider.gameObject.transform.name + " @ " + dist + " meters away");
			}
			Vector3 end = (hit.distance <= 0.0f) ? (start + (dir * dist)) : (hit.point);
			if (drawTrail) {
				BulletTrail.Create(instance.transform.TransformPoint(barrelPosition), end);
				instance.DrawFlash();
			}
		}
	}

	private Quaternion GetBulletSpread(Vector3 dir) {
		Quaternion fireRot = Quaternion.LookRotation(dir);
		Quaternion rand = Random.rotation;
		fireRot = Quaternion.RotateTowards(fireRot, rand, spray);
		return fireRot;
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
				if (instance.GetClipCount() > 0) {
					instance.SetClipCount(instance.GetClipCount() - 1);
					instance.SetCurrentClipAmmo(ammoPerClip);
				}
			}
		}
	}

}