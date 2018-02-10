using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoilSystem : MonoBehaviour {

	/*private float recoil;
	private float maxRecoilX = -20.0f;
	private float maxRecoilY = 20.0f;
	private float recoilSpeed = 2.0f;

	public void DoRecoil(float recoil, float maxRecoilX, float maxRecoilY, float recoilSpeed) {
		this.recoil = recoil;
		this.maxRecoilX = -maxRecoilX;
		this.maxRecoilY = Random.Range(-maxRecoilY, maxRecoilY);
		this.recoilSpeed = recoilSpeed;
	}

	void Update() {
		if (recoil > 0.0f) {
			Quaternion maxRecoil = Quaternion.Euler(maxRecoilX, maxRecoilY, 0.0f);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, maxRecoil, Time.deltaTime * recoilSpeed);
			recoil -= Time.deltaTime;
		}
	}*/

}