using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour {

	public RectTransform progressCenter;
	public bool damp = true;
	public float damping = 0.1f;
	public float Progress {
		set {
			if (fullWidth < 0) {
				Init();
			}
			_prog = value;
		}
		get {
			return _prog;
		}
	}

	public Text Child { private set; get; }

	private Vector2 vel;
	private float fullWidth = -1.0f;
	private float _prog = 0.5f;

	void Start() {
		if (fullWidth < 0.0f) {
			Init();
		}
	}

	void Init() {
		fullWidth = progressCenter.sizeDelta.x;
		Child = GetComponentInChildren<Text>();
		Reset();
	}

	public void Reset() {
		Progress = 0.0f;
		Instant();
	}

	public void Instant() {
		Vector2 tmp = progressCenter.sizeDelta;
		tmp.x = Progress * fullWidth;
		progressCenter.sizeDelta = tmp;
	}

	void Update() {
		Vector2 newSize = progressCenter.sizeDelta;
		newSize.x = Progress * fullWidth;
		if (damp) {
			progressCenter.sizeDelta = Vector2.SmoothDamp(progressCenter.sizeDelta, newSize, ref vel, damping, 1000.0f, Time.deltaTime);
		} else {
			progressCenter.sizeDelta = newSize;
		}
	}

}