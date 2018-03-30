using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour {
	
	private Toaster parent;
	private string str;
	private Text text;
	private float life = 0.0f;
	private float totalLife = -1.0f;
	private float fadeTime = -1.0f;

	void Awake() {
		text = GetComponent<Text>();
		if (text == null) {
			Debug.LogWarning("Toast prefab doesn't have a text ui component");
			Destroy(gameObject);
			return;
		}
	}
	
	void Update() {
		if (totalLife < 0.0f || fadeTime < 0.0f) {
			return;
		}
		text.text = str;
		life += Time.deltaTime;
		if (life >= totalLife - fadeTime) {
			if (life > totalLife) {
				parent.RemoveToast(this);
				Destroy(gameObject);
				return;
			}
			Color c = text.color;
			c.a = (totalLife - life) / fadeTime;
			text.color = c;
		}
	}

	public void SetToast(Toaster parent, string text) {
		this.parent = parent;
		str = text;
	}

	public void StartCountdown(float life, float fade) {
		totalLife = life;
		fadeTime = fade;
	}

}