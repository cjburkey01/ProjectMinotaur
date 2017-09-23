using UnityEngine;
using UnityEngine.PostProcessing;

public class GraphicsOptionsHandler : MonoBehaviour {

	public bool antialiasing = true;
	public bool ambientOcclusion = true;
	public bool motionBlur = true;
	public bool bloom = true;
	public bool colorGrading = true;
	public bool vignette = true;
	public bool dithering = true;
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

}