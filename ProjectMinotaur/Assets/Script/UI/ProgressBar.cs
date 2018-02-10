using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public RectTransform progressCenter;
	public float progress = 0.5f;
	public bool damp = true;
	public float damping = 0.1f;

	public Text Child { private set; get; }

	private Vector2 vel;
	private float fullWidth;

	void Start() {
		fullWidth = progressCenter.sizeDelta.x;

		Child = GetComponentInChildren<Text>();

		// Force resize the progress bar when it is created rather than let it drift from 100% to the default.
		Vector2 tmp = progressCenter.sizeDelta;
		tmp.x = 0;
		progressCenter.sizeDelta = tmp;
	}

	void Update() {
		Vector2 newSize = progressCenter.sizeDelta;
		newSize.x = progress * fullWidth;
		if (damp) {
			progressCenter.sizeDelta = Vector2.SmoothDamp(progressCenter.sizeDelta, newSize, ref vel, damping, 1000.0f, Time.deltaTime);
		} else {
			progressCenter.sizeDelta = newSize;
		}
	}

}