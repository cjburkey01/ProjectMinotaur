using UnityEngine;

public class Enemy : Entity {

	public int damageAmt = 10;
	public float timeBetweenDamage = 1.0f;
	public GameObject player;
	public float enemySpeed;

	private float timeSinceLastDamage = 0.0f;

	void OnCollisionStay(Collision coll) {
		if (timeSinceLastDamage >= timeBetweenDamage) {
			Entity ent = coll.gameObject.GetComponent<Entity>();
			if (ent == null) {
				return;
			}
			PMEventSystem.GetEventSystem().TriggerEvent(new EntityAttackEvent(ent, this, damageAmt));
			timeSinceLastDamage = 0.0f;
		}
	}

	void Update() {
		if (timeSinceLastDamage <= timeBetweenDamage) {
			timeSinceLastDamage += Time.deltaTime;
		}
	}

}

public class EntityAttackEvent : EntityEvent {

	private readonly Entity attacker;
	private readonly int damage;

	public EntityAttackEvent(Entity entity, Entity attacker, int damage) : base(entity) {
		this.attacker = attacker;
		this.damage = damage;
	}

	public Entity GetAttacker() {
		return attacker;
	}

	public int GetDamage() {
		return damage;
	}

}