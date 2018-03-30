using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toaster : MonoBehaviour {

	public static Toaster Instance { private set; get; }
	
	public float toastLife = 10.0f;
	public float toastFade = 2.0f;
	public float fadeSmoothing = 0.1f;
	public GameObject toastPrefab;

	private List<Toast> toast = new List<Toast>();
	private Image image;

	private float defAlpha;
	private float goalAlpha;
	private float currentAlpha;
	private float alphaV;

	public Toaster() {
		Instance = this;
	}

	void Awake() {
		image = GetComponent<Image>();
		if (image != null) {
			defAlpha = image.color.a;
		}
		UpdateTransparency();
	}

	void Update() {
		currentAlpha = Mathf.SmoothDamp(currentAlpha, goalAlpha, ref alphaV, fadeSmoothing);
		Color c = image.color;
		c.a = currentAlpha;
		image.color = c;
	}

	public void AddToast(string text) {
		if (toastPrefab == null) {
			Debug.LogWarning("ToastPrefab not set on Toaster object");
			return;
		}
		GameObject toastObj = Instantiate(toastPrefab, transform, false);
		Toast toast = toastObj.GetComponent<Toast>();
		if (toast == null) {
			Debug.LogWarning("ToastPrefab does not have a toast component");
			return;
		}
		toast.SetToast(this, text);
		toast.StartCountdown(toastLife, toastFade);
		this.toast.Add(toast);
		UpdateTransparency();
	}

	public void RemoveToast(Toast toast) {
		this.toast.Remove(toast);
		UpdateTransparency();
	}

	private void UpdateTransparency() {
		if (image == null) {
			return;
		}
		if (toast.Count < 1) {
			goalAlpha = 0.0f;
			return;
		}
		goalAlpha = defAlpha;
	}

}