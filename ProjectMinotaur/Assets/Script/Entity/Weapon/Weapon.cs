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
	}

	public static Weapon Create(bool permanent, WeaponDefinition def) {
		GameObject tmp = new GameObject(def.DisplayName);
		Weapon w = tmp.AddComponent<Weapon>();
		w.Init(permanent, def);
		return w;
	}

}