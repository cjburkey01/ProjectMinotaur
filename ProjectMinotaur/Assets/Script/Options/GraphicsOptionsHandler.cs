using UnityEngine;
using UnityEngine.PostProcessing;

public class GraphicsOptionsHandler : MonoBehaviour {

	public bool antialiasing;
	public bool ambientOcclusion;
	public bool motionBlur;
	public bool bloom;
	public bool colorGrading;
	public bool vignette;
	public bool dithering;
	public float fieldOfView = 90.0f;

	private Camera attachedCamera;
	private PostProcessingProfile ppp;

	// Initialize all the variables
	void Start() {
		attachedCamera = GetComponent<Camera>();
		if (attachedCamera == null) {
			Debug.LogError("GraphicsOptionsHandler must be a component on a camera.");
			gameObject.SetActive(false);
		}

		PostProcessingBehaviour ppb = GetComponent<PostProcessingBehaviour>();
		if (ppb == null) {
			Debug.LogError("PostProcessingBehaviour not found on camera.");
		}

		ppp = ppb.profile;
		if (ppp == null) {
			Debug.LogError("PostProcessingProfile not found in PostProcessingBehaviour.");
		}

		RefreshSettings();
	}

	// Reload the graphics settings. Useful for in-game editing
	public void RefreshSettings() {
		ReloadSettings();
		if (ppp != null) {
			ppp.antialiasing.enabled = antialiasing;
			ppp.ambientOcclusion.enabled = ambientOcclusion;
			ppp.motionBlur.enabled = motionBlur;
			ppp.bloom.enabled = bloom;
			ppp.colorGrading.enabled = colorGrading;
			ppp.vignette.enabled = vignette;
			ppp.dithering.enabled = dithering;
			attachedCamera.fieldOfView = fieldOfView;
			print("Graphics settings reloaded.");
		} else {
			Debug.LogWarning("Graphics settings couldn't be reloaded.");
		}
	}

	public void SaveSettings() {
		SetBool("antialiasing", antialiasing);
		SetBool("ambientOcclusion", ambientOcclusion);
		SetBool("motionBlur", motionBlur);
		SetBool("bloom", bloom);
		SetBool("colorGrading", colorGrading);
		SetBool("vignette", vignette);
		SetBool("dithering", dithering);
		ReloadSettings();
	}

	private void ReloadSettings() {
		antialiasing = GetBool("antialiasing", true);
		ambientOcclusion = GetBool("ambientOcclusion", true);
		motionBlur = GetBool("motionBlur", true);
		bloom = GetBool("bloom", true);
		colorGrading = GetBool("colorGrading", true);
		vignette = GetBool("vignette", true);
		dithering = GetBool("dithering", true);
	}

	private void SetBool(string key, bool value) {
		PlayerPrefs.SetString(key, ((value) ? ("true") : ("false")));
		PlayerPrefs.Save();
	}

	private bool GetBool(string key, bool defaultValue) {
		if (PlayerPrefs.HasKey(key)) {
			return PlayerPrefs.GetString(key).Equals("true");
		}
		SetBool(key, defaultValue);
		return defaultValue;
	}

}