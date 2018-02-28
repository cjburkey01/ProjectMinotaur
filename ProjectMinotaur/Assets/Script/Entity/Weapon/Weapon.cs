using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public bool Permanent { private set; get; }
	public WeaponDefinition WeaponType { private set; get; }
	public Player Holder { private set; get; }
	public int clipCount;
	public int currentClipAmmo;
	
	private ParticleSystem muzzleFlash;
	private float lastFire;

	void Update() {
		if (lastFire < WeaponType.resetTime) {
			lastFire += Time.deltaTime;
		}
	}

	public void DrawFlash() {
		if (muzzleFlash != null) {
			muzzleFlash.Play(true);
			StartCoroutine(StopFlash());
		}
	}

	private IEnumerator StopFlash() {
		yield return new WaitForSeconds(WeaponType.resetTime / 2.0f);
		muzzleFlash.Stop();
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
		if (player == null) {
			transform.parent = null;
			Holder = null;
			gameObject.SetLayer(0);
			return;
		}
		Holder = player;
		gameObject.SetLayer(8);
		transform.parent = player.HandRenderer.gameObject.transform;
		transform.localPosition = WeaponType.displayPositionOffset;
		transform.localRotation = Quaternion.Euler(WeaponType.displayRotationOffset);
	}

	public Vector3 GetBarrelPosWorld() {
		return transform.TransformPoint(WeaponType.barrelPosition);
	}

	public static Weapon Create(bool permanent, Player parentPlayer, WeaponDefinition def) {
		GameObject tmp = new GameObject(def.DisplayName);
		tmp.transform.name = "Weapon: " + def.DisplayName;
		Weapon w = tmp.AddComponent<Weapon>();
		w.Init(3, permanent, def);
		if (def.drawTrail) {
			GameObject obj = Resources.Load<GameObject>("Weapon/MuzzleFlash");
			if (obj != null) {
				GameObject tmptmp = Instantiate(obj, Vector3.zero, Quaternion.identity);
				foreach (Transform trans in tmptmp.transform) {
					if (trans.parent.Equals(tmptmp.transform)) {
						w.muzzleFlash = trans.GetComponent<ParticleSystem>();
						break;
					}
				}
				if (w.muzzleFlash == null) {
					Debug.LogWarning("MuzzleFlash has no particle system.");
					Destroy(tmptmp);
				} else {
					w.muzzleFlash.Stop(true);
					tmptmp.transform.parent = w.gameObject.transform;
					tmptmp.transform.localPosition = def.barrelPosition;
				}
			} else {
				Debug.LogWarning("Failed to load MuzzleFlash prefab.");
			}
		}
		if (def.Model != null) {
			GameObject model = Instantiate(def.Model, Vector3.zero, Quaternion.identity);
			model.transform.parent = tmp.transform;
			model.transform.name = "Model";
			model.transform.localPosition = Vector3.zero;
		}
		w.SetPlayer(parentPlayer);
		return w;
	}

}