using UnityEngine;

public class BulletTrail : MonoBehaviour {

	public float life = 0.05f;

	private float alpha = -1.0f;
	private float time = 0.0f;

	private Vector3 start;
	private Vector3 end;

	private LineRenderer line;

	void Update() {
		time += Time.deltaTime;
		if (line == null || alpha < 0.0f) {
			return;
		}
		if (time >= life) {
			Destroy(gameObject);
			return;
		}
		alpha = time / life / 8.0f;
		Color c = line.startColor;
		c.a = 0.125f - alpha;
		line.startColor = c;
		line.endColor = c;
	}

	public static void Create(Vector3 start, Vector3 end) {
		GameObject trailObj = new GameObject("GunTrail");
		trailObj.transform.position = Vector3.zero;
		BulletTrail trail = trailObj.AddComponent<BulletTrail>();
		trail.start = start;
		trail.end = end;
		trail.line = trailObj.AddComponent<LineRenderer>();
		trail.line.startWidth = 0.015f;
		trail.line.endWidth = 0.015f;
		trail.line.SetPositions(new Vector3[] { trail.start, trail.end });
		trail.line.material = Resources.Load<Material>("BulletTrailMaterial");
		trail.alpha = 1.0f;
	}

}