using UnityEngine;

public class Enemy : MonoBehaviour {

	public int damageAmt = 10;
	public float timeBetweenDamage = 1.0f;

	private float lastDamageTime = 0.0f;

	public void OnCollisionEnter(Collision collision) {
		collision.collider.SendMessage("Damage", (byte) damageAmt);
	}

}