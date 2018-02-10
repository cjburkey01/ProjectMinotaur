using UnityEngine;

public class Weapon : MonoBehaviour {

	public bool Permanent { private set; get; }
	public WeaponDefinition WeaponType { private set; get; }
	public Player Holder { private set; get; }
	public int clipCount;
	public int currentClipAmmo;

	private float lastFire;

	void Update() {
		if (lastFire < WeaponType.resetTime) {
			lastFire += Time.deltaTime;
		}
	}

	public void AttemptFire() {
		if (lastFire < WeaponType.resetTime) {
			return;
		}
		if (currentClipAmmo >= WeaponType.shotsPerPrimary) {
			lastFire = 0.0f;
			Holder.MovementMotor.DoRecoil(WeaponType.recoilTime, WeaponType.recoilX, WeaponType.recoilY, WeaponType.recoilSpeed);
			for (int i = 0; i < WeaponType.shotsPerPrimary; i++) {
				DoShot();
			}
		} else {
			DoReload();
		}
	}

	private void DoShot() {
		WeaponType.OnPrimary(this);
	}

	private void DoReload() {
		WeaponType.OnReload(this);
	}

	private void Init(int startingClips, bool permanent, WeaponDefinition type) {
		Permanent = permanent;
		WeaponType = type;
		clipCount = startingClips - 1;
		currentClipAmmo = type.ammoPerClip;
	}

	public void SetPlayer(Player player) {
		Holder = player;
		transform.parent = player.LookCamera.gameObject.transform;
		transform.localPosition = WeaponType.displayPositionOffset;
		transform.rotation = Quaternion.Euler(WeaponType.displayRotationOffset);
	}

	public Vector3 GetBarrelPosWorld() {
		return transform.TransformPoint(WeaponType.barrelPosition);
	}

	public static Weapon Create(bool permanent, Player parentPlayer, WeaponDefinition def) {
		GameObject tmp = new GameObject(def.DisplayName);
		tmp.transform.name = "Weapon: " + def.DisplayName;
		Weapon w = tmp.AddComponent<Weapon>();
		w.Init(3, permanent, def);
		if (def.model != null) {
			GameObject model = Instantiate(def.model, Vector3.zero, Quaternion.identity);
			model.transform.parent = tmp.transform;
			model.transform.name = "Model";
			model.transform.localPosition = Vector3.zero;
		}
		if (parentPlayer != null) {
			w.SetPlayer(parentPlayer);
		}
		return w;
	}

}