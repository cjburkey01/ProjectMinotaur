using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour {

	public float dayLength = 600.0f;
	public float startingTime = 200.0f;
	public float nightPadding = 45.0f;

	private Light li;
	private float time = 0.0f;
	private float intensity;
	private Vector3 startingRotation;

	void Start() {
		li = GetComponent<Light>();
		if (li == null) {
			Debug.LogError("Sunlight not found.");
			gameObject.SetActive(false);
		}

		intensity = li.intensity;
		startingRotation = transform.rotation.eulerAngles;
		time = startingTime;
	}

	void Update() {
		if (!GameHandler.paused) {
			time += Time.deltaTime;
			if (time >= dayLength) {
				time %= dayLength;
			}
			Rotate();
			Intensify();
		}
	}

	private void Rotate() {
		transform.rotation = Quaternion.Euler(startingRotation);
		transform.Rotate(0.0f, -(time / dayLength) * 360, 0.0f);
	}

	private void Intensify() {
		float nightMin = (dayLength / 2.0f) + (dayLength / nightPadding);
		float nightMax = dayLength - (dayLength / nightPadding);
		if (time >= nightMin && !(time >= nightMax)) {
			li.intensity = 0.0f;
		} else {
			li.intensity = intensity;
		}
	}

}