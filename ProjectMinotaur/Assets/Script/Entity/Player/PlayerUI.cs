using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public ProgressBar healthBar;
	public ProgressBar ammoBar;
	public WeaponSlot primarySlot;
	public WeaponSlot secondarySlot;
	public CrosshairHandler crossshair;

	private Player player;

	void Start() {
		primarySlot.SetSelected(true);
		secondarySlot.SetSelected(false);
	}

	void Update() {
		if (GetPlayer() == null || player.Toolbar == null || player.Toolbar.GetWeapon() == null) {
			healthBar.gameObject.SetActive(false);
			ammoBar.gameObject.SetActive(false);
			primarySlot.gameObject.SetActive(false);
			secondarySlot.gameObject.SetActive(false);
			crossshair.gameObject.SetActive(false);
		} else {
			healthBar.gameObject.SetActive(true);
			ammoBar.gameObject.SetActive(player.Toolbar.GetWeapon().WeaponType.ammoPerClip > 0);
			primarySlot.gameObject.SetActive(true);
			secondarySlot.gameObject.SetActive(true);
			crossshair.gameObject.SetActive(true);

			healthBar.progress = GetPlayer().GetHealth() / 100.0f;
			ammoBar.progress = (float) player.Toolbar.GetWeapon().currentClipAmmo / player.Toolbar.GetWeapon().WeaponType.ammoPerClip;
			if (ammoBar.Child != null) {
				ammoBar.Child.text = player.Toolbar.GetWeapon().clipCount + " | " + player.Toolbar.GetWeapon().currentClipAmmo;
			}
		}
	}

	private Player GetPlayer() {
		if (player == null) {
			player = FindObjectOfType<Player>();
		}
		return player;
	}

}