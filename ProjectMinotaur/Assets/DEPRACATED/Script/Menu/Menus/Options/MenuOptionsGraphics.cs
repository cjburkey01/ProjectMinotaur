using UnityEngine;
using UnityEngine.UI;

public class MenuOptionsGraphics : Menu {

	public RectTransform root;
	public Color OnColor = new Color(0.0f, 0.91f, 0.0f);
	public Color OffColor = new Color(0.91f, 0.0f, 0.0f);
	public GraphicsOptionsHandler options;

	public Button antialiasing;
	public Button ambientOcclusion;
	public Button motionBlur;
	public Button bloom;
	public Button colorGrading;
	public Button vignette;
	public Button dithering;

	public void RefreshButtons() {
		ColorButton(antialiasing, options.antialiasing);
		ColorButton(ambientOcclusion, options.ambientOcclusion);
		ColorButton(motionBlur, options.motionBlur);
		ColorButton(bloom, options.bloom);
		ColorButton(colorGrading, options.colorGrading);
		ColorButton(vignette, options.vignette);
		ColorButton(dithering, options.dithering);
	}

	public override string GetName() {
		return "MenuOptionsGraphics";
	}

	public override void OnOpen() {
		if (root != null) {
			RefreshButtons();
			root.gameObject.SetActive(true);
		}
	}

	public override void OnClose() {
		if (root != null) {
			root.gameObject.SetActive(false);
		}
	}

	public void OnAntialiasingClick() {
		options.antialiasing = !options.antialiasing;
		options.SaveSettings();
		ColorButton(antialiasing, options.antialiasing);
		options.RefreshSettings();
	}

	public void OnAmbientOcclusionClick() {
		options.ambientOcclusion = !options.ambientOcclusion;
		options.SaveSettings();
		ColorButton(ambientOcclusion, options.ambientOcclusion);
		options.RefreshSettings();
	}

	public void OnBloomClick() {
		options.bloom = !options.bloom;
		options.SaveSettings();
		ColorButton(bloom, options.bloom);
		options.RefreshSettings();
	}

	public void OnMotionBlurClick() {
		options.motionBlur = !options.motionBlur;
		options.SaveSettings();
		ColorButton(motionBlur, options.motionBlur);
		options.RefreshSettings();
	}

	public void OnColorGradingClick() {
		options.colorGrading = !options.colorGrading;
		options.SaveSettings();
		ColorButton(colorGrading, options.colorGrading);
		options.RefreshSettings();
	}

	public void OnVignetteClick() {
		options.vignette = !options.vignette;
		options.SaveSettings();
		ColorButton(vignette, options.vignette);
		options.RefreshSettings();
	}

	public void OnDitheringClick() {
		options.dithering = !options.dithering;
		options.SaveSettings();
		ColorButton(dithering, options.dithering);
		options.RefreshSettings();
	}

	public void OnBackClick() {
		if (MenuHandler.instance.MenuExists("MenuOptions")) {
			MenuHandler.instance.OpenMenu("MenuOptions");
		}
	}

	private Color Darken(Color inColor) {
		Color clone = new Color(inColor.r, inColor.g, inColor.b, inColor.a);
		clone.r = Mathf.Max(0.0f, clone.r - 0.1f);
		clone.g = Mathf.Max(0.0f, clone.g - 0.1f);
		clone.b = Mathf.Max(0.0f, clone.b - 0.1f);
		return clone;
	}

	private void ColorButton(Button button, bool enabled) {
		ColorBlock tmp = button.colors;
		tmp.normalColor = (enabled) ? OnColor : OffColor;
		tmp.highlightedColor = Darken(tmp.normalColor);
		tmp.pressedColor = Darken(tmp.highlightedColor);
		button.colors = tmp;
	}

}