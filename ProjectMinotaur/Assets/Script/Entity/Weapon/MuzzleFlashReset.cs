using UnityEngine;

public class MuzzleFlashReset : MonoBehaviour {

	void OnEnable() {
		ParticleSystem ps = GetComponent<ParticleSystem>();
		if (ps != null) {
			ps.Stop(true);
		}
	}

}