using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : IMenu {

	public GameObject contentPanel;
	public GameObject buttonPrefab;

	private bool created;

	public override void OnShow() {
		if (created) {
			return;
		}
		GraphicsOptionsHandler opts = FindObjectOfType<GraphicsOptionsHandler>();
		foreach (KeyValuePair<string, bool> option in opts.GetOptions()) {
			string nm = opts.GetNames()[option.Key];
			GameObject obj = Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
			Text btnTxt = obj.GetComponentInChildren<Text>();
			btnTxt.text = nm;
			obj.GetComponent<RectTransform>().SetParent(contentPanel.transform, false);
			Button btn = obj.GetComponent<Button>();
			btn.onClick.AddListener(delegate { UpdateOptions(option.Key, opts, btn); });
			Color(btn, option.Value);
		}
		created = true;
	}

	private void UpdateOptions(string key, GraphicsOptionsHandler opts, Button btn) {
		opts.GetOptions()[key] = !opts.GetOptions()[key];
		opts.SaveSettings();
		opts.RefreshSettings();
		Color(btn, opts.GetOptions()[key]);
	}

	private void Color(Button btn, bool en) {
		ColorBlock colors = btn.colors;
		colors.normalColor = (en) ? new Color(10.0f / 255.0f, 175.0f / 255.0f, 10.0f / 255.0f) : new Color(175.0f / 255.0f, 10.0f / 255.0f, 10.0f / 255.0f);
		colors.highlightedColor = colors.normalColor;
		colors.pressedColor = colors.normalColor;
		btn.colors = colors;
	}

	public override void OnHide() {
	}

}