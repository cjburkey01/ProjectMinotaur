using UnityEngine;
using UnityEngine.UI;

public class CrosshairHandler : MonoBehaviour {

	public Sprite defaultCrosshair;
	public Sprite hoverCrosshair;
	public float rotationSpeed = 5.0f;
	public bool hovered;

	private Image crosshair;

	void Start() {
		crosshair = GetComponent<Image>();
		if (crosshair == null) {
			Debug.LogError("Crosshair handler not on image object.");
			Destroy(gameObject);
		}
	}

	void Update() {
		if (!hovered) {
			crosshair.sprite = defaultCrosshair;
			Vector3 rot = transform.rotation.eulerAngles;
			rot.z += rotationSpeed * Time.deltaTime;
			rot.z %= 360.0f;
			transform.rotation = Quaternion.Euler(rot);
		} else {
			crosshair.sprite = hoverCrosshair;
		}
	}

}