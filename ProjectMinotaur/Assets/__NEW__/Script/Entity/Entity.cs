using UnityEngine;

public abstract class Entity : MonoBehaviour {

	private byte health = 100;

	public virtual void Start() {
		PMEventSystem.GetEventSystem().AddListener<EntityAttackEvent>(OnAttackEvent);
	}

	private void OnAttackEvent<T>(T e) where T : EntityAttackEvent {
		OnAttack(e.GetAttacker(), e.GetDamage());
	}

	public void SetHealth(int setH) {
		if (setH > 100) {
			setH = 100;
		}
		if (setH < 0) {
			setH = 0;
		}
		health = (byte) setH;
	}

	public void AddHealth(int addH) {
		SetHealth(health + addH);
	}

	public void TakeHealth(int takeH) {
		SetHealth(health - takeH);
	}

	public int GetHealth() {
		return health;
	}

	public abstract void OnAttack(Entity attacker, int damage);

}

public abstract class EntityEvent : IPMEvent {

	private bool cancelled;
	private readonly Entity entity;

	protected EntityEvent(Entity entity) {
		this.entity = entity;
	}

	public string GetName() {
		return GetType().Name;
	}

	public bool IsCancellable() {
		return true;
	}

	public bool IsCancelled() {
		return cancelled;
	}

	public void Cancel() {
		cancelled = true;
	}

	public Entity GetEntity() {
		return entity;
	}

}