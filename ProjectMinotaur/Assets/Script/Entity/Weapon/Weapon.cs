using UnityEngine;

public class Weapon : MonoBehaviour {

	public bool Permanent { private set; get; }
	public WeaponDefinition WeaponType { private set; get; }
	public Player Holder { private set; get; }
	public int clipCount;
	public int currentClipAmmo;

	private void Init(bool permanent, WeaponDefinition type) {
		Permanent = permanent;
		WeaponType = type;
		clipCount = 3;
		currentClipAmmo = type.ammoPerClip;
	}

	public void SetPlayer(Player player) {
		Holder = player;
		transform.parent = player.gameObject.transform;
		transform.localPosition = WeaponType.displayPositionOffset;
		transform.rotation = Quaternion.Euler(WeaponType.displayRotationOffset);
	}

	public static Weapon Create(bool permanent, Player parentPlayer, WeaponDefinition def) {
		GameObject tmp = new GameObject(def.DisplayName);
		tmp.transform.name = "Weapon: " + def.DisplayName;
		Weapon w = tmp.AddComponent<Weapon>();
		w.Init(permanent, def);
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