using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class GraphicsOptionsHandler : MonoBehaviour {

	public PostProcessingProfile ppp;

	private Camera attachedCamera;
	private readonly Dictionary<string, bool> options = new Dictionary<string, bool>();
	private readonly Dictionary<string, string> names = new Dictionary<string, string>();

	// Initialize all the variables
	void Start() {
		options.Add("antialiasing", false);
		options.Add("ambientOcclusion", false);
		options.Add("motionBlur", false);
		options.Add("bloom", false);
		options.Add("colorGrading", false);
		options.Add("vignette", false);
		options.Add("dithering", false);

		names.Add("antialiasing", "Antialiasing");
		names.Add("ambientOcclusion", "Ambient Occlusion");
		names.Add("motionBlur", "Motion Blur");
		names.Add("bloom", "Bloom");
		names.Add("colorGrading", "Color Grading");
		names.Add("vignette", "Vignette");
		names.Add("dithering", "Dithering");

		attachedCamera = GetComponent<Camera>();
		if (attachedCamera == null) {
			Debug.LogError("GraphicsOptionsHandler must be a component on a camera.");
			gameObject.SetActive(false);
		}

		PostProcessingBehaviour ppb = GetComponent<PostProcessingBehaviour>();
		if (ppb == null) {
			ppb = gameObject.AddComponent<PostProcessingBehaviour>();
			ppb.profile = ppp;
		}

		RefreshSettings();
	}

	public Dictionary<string, bool> GetOptions() {
		return options;
	}

	public Dictionary<string, string> GetNames() {
		return names;
	}

	// Reload the graphics settings. Useful for in-game editing
	public void RefreshSettings() {
		LoadSettings();
		if (ppp != null) {
			ppp.antialiasing.enabled = options["antialiasing"];
			ppp.ambientOcclusion.enabled = options["ambientOcclusion"];
			ppp.motionBlur.enabled = options["motionBlur"];
			ppp.bloom.enabled = options["bloom"];
			ppp.colorGrading.enabled = options["colorGrading"];
			ppp.vignette.enabled = options["vignette"];
			ppp.dithering.enabled = options["dithering"];

			attachedCamera.fieldOfView = 90.0f;
			print("Graphics settings reloaded.");
		} else {
			Debug.LogWarning("Graphics settings couldn't be reloaded.");
		}
	}

	public void SaveSettings() {
		SetBool("antialiasing", options["antialiasing"]);
		SetBool("ambientOcclusion", options["ambientOcclusion"]);
		SetBool("motionBlur", options["motionBlur"]);
		SetBool("bloom", options["bloom"]);
		SetBool("colorGrading", options["colorGrading"]);
		SetBool("vignette", options["vignette"]);
		SetBool("dithering", options["dithering"]);
	}

	private void LoadSettings() {
		options["antialiasing"] = GetBool("antialiasing", true);
		options["ambientOcclusion"] = GetBool("ambientOcclusion", true);
		options["motionBlur"] = GetBool("motionBlur", true);
		options["bloom"] = GetBool("bloom", true);
		options["colorGrading"] = GetBool("colorGrading", true);
		options["vignette"] = GetBool("vignette", true);
		options["dithering"] = GetBool("dithering", true);
	}

	private void SetBool(string key, bool value) {
		OptionsHandler.Instance.Set(key, value);
	}

	private bool GetBool(string key, bool defaultValue) {
		if (!OptionsHandler.Instance.Has(key)) {
			OptionsHandler.Instance.Set(key, defaultValue);
			return defaultValue;
		}
		return OptionsHandler.Instance.GetBool(key);
	}

}