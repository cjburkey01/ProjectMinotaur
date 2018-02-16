using UnityEngine;

public class BulletTrail : MonoBehaviour {
    
	public float life = 0.5f;
    
	private float alpha = -1.0f;
	private float time = 0.0f;

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
        Color c = line.endColor;
		c.a = 1.0f / 8.0f - alpha;
		line.startColor = new Color(c.r, c.g, c.b, 0.0f);
		line.endColor = c;
	}

	public static void Create(Vector3 start, Vector3 end) {
		GameObject trailObj = new GameObject("GunTrail");
		trailObj.transform.position = Vector3.zero;
		BulletTrail trail = trailObj.AddComponent<BulletTrail>();
		trail.line = trailObj.AddComponent<LineRenderer>();
		trail.line.startWidth = 0.015f;
		trail.line.endWidth = 0.015f;
        Vector3 dir = (end - start).normalized;
        dir *= 15.0f;
        dir += start;
        if (Vector3.Distance(start, dir) >= Vector3.Distance(start, end)) {
            end = dir;
        }
		trail.line.SetPositions(new Vector3[] { start, dir, end });
        trail.line.material = Resources.Load<Material>("BulletTrailMaterial");
        trail.line.startColor = new Color(1.0f, 1.0f, 1.0f, 1.0f / 8.0f);
        trail.line.endColor = new Color(1.0f, 1.0f, 1.0f, 1.0f / 8.0f);
        trail.alpha = 1.0f;
	}

}