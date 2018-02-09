using UnityEngine;

public class BulletTrail : MonoBehaviour {

	public float life = 0.25f;

	private float alpha = -1.0f;
	private float time = 0.0f;

	private Vector3 start;
	private Vector3 end;

	private LineRenderer line;

	void Update() {
		if (line == null || alpha < 0.0f) {
			return;
		}
		if (time >= life) {
			Destroy(gameObject);
			return;
		}
		time += Time.deltaTime;
		alpha = time / life;
		Color c = line.startColor;
		c.a = 1.0f - alpha;
		line.startColor = c;
		line.endColor = c;
	}

	public static void Create(Vector3 direction, float distance, Weapon weapon) {
		if (weapon == null || weapon.WeaponType == null/* || !weapon.WeaponType.drawTrail*/) {
			return;
		}
		GameObject trailObj = new GameObject("GunTrail");
		trailObj.transform.position = Vector3.zero;
		BulletTrail trail = trailObj.AddComponent<BulletTrail>();
		trail.start = weapon.GetBarrelPosWorld();
		trail.end = trail.start + (direction.normalized * distance);
		trail.line = trailObj.AddComponent<LineRenderer>();
		trail.line.startWidth = 0.025f;
		trail.line.endWidth = 0.025f;
		trail.line.SetPositions(new Vector3[] { trail.start, trail.end });
		trail.line.material = Resources.Load<Material>("BulletTrailMaterial");
		trail.alpha = 1.0f;
	}

}