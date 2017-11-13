using UnityEngine;
using UnityEngine.AI;

public class Enemy : IHealthHaver {

	public int damageAmt = 10;
	public float timeBetweenDamage = 1.0f;
	public GameObject player;
	public float enemySpeed;

	private float timeSinceLastDamage = 0.0f;
	private Transform playerFind;
	private NavMeshAgent nav; 

	void Start(){

		playerFind = GameObject.FindGameObjectWithTag ("Player").transform;
		nav = GetComponent<NavMeshAgent> ();
	}


	public void OnCollisionStay(Collision collision) {
		if (timeSinceLastDamage >= timeBetweenDamage) {
			collision.collider.SendMessage("Damage", new object[] { (byte) damageAmt, EntityDamageOrigin.ENEMY_DAMAGE, this });
			timeSinceLastDamage = 0.0f;
		}
	}

	void Update() {
		if (timeSinceLastDamage < timeBetweenDamage * 2) {
			timeSinceLastDamage += Time.deltaTime;
		}

		//finds player
		nav.SetDestination (playerFind.position);
	}

}
