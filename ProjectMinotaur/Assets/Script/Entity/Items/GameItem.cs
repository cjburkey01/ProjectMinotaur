using System.Collections.Generic;
using UnityEngine;

public class GameItem {

	public string UniqueId { private set; get; }
	public string DisplayName { private set; get; }
	public string Description { private set; get; }
	public int MaxStackSize { private set; get; }

	protected GameObject model;
	protected Sprite icon32;
	protected Sprite icon512;

	public GameObject Model {
		protected set {
			model = value;
		}
		get {
			if (model == null) {
				Debug.LogWarning("Item has no model: " + UniqueId);
			}
			return model;
		}
	}

	public Sprite Icon32 {
		protected set {
			icon32 = value;
		}
		get {
			if (icon32 == null) {
				Debug.LogWarning("Item has no SD icon: " + UniqueId);
			}
			return icon32;
		}
	}

	public Sprite Icon512 {
		protected set {
			icon512 = value;
		}
		get {
			if (icon512 == null) {
				Debug.LogWarning("Item has no HD icon: " + UniqueId);
			}
			return icon512;
		}
	}

	protected GameItem(string uid, string name, string description, int maxStackSize) : this(uid, name, description, maxStackSize, null, null, null) {
	}

	public GameItem(string uid, string name, string description, int maxStackSize, GameObject model, Sprite icon32, Sprite icon512) {
		UniqueId = uid;
		DisplayName = name;
		Description = description;
		MaxStackSize = maxStackSize;
		Model = model;
		Icon32 = icon32;
		Icon512 = icon512;
	}

	public virtual void CreateModel(WorldItem item) { }

	public override bool Equals(object obj) {
		var item = obj as GameItem;
		return item != null && UniqueId == item.UniqueId && DisplayName == item.DisplayName && Description == item.Description && MaxStackSize == item.MaxStackSize && EqualityComparer<GameObject>.Default.Equals(Model, item.Model) && EqualityComparer<Sprite>.Default.Equals(Icon32, item.Icon32) && EqualityComparer<Sprite>.Default.Equals(Icon512, item.Icon512);
	}

	public override int GetHashCode() {
		var hashCode = 1813968700;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UniqueId);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
		hashCode = hashCode * -1521134295 + EqualityComparer<GameObject>.Default.GetHashCode(Model);
		hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(Icon32);
		hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(Icon512);
		hashCode = hashCode * -1521134295 + MaxStackSize.GetHashCode();
		return hashCode;
	}

	public override string ToString() {
		return UniqueId;
	}

}

public class WorldItem : MonoBehaviour {

	public ItemStack Stack { private set; get; }

	public static WorldItem Spawn(ItemStack stack, Vector3 pos) {
		if (stack == null || stack.IsEmpty()) {
			return null;
		}
		GameObject obj = new GameObject(stack.Item.DisplayName + "x" + stack.Count);
		WorldItem item = obj.AddComponent<WorldItem>();
		item.Stack = stack.Copy();
		item.transform.position = pos;
		item.transform.rotation = Random.rotation;
		stack.Item.CreateModel(item);
		return item;
	}
	
	void Update() {
		// TODO: ROTATE
	}

}

public class ItemStack {

	public GameItem Item { private set; get; }
	public int Count { set; get; }
	public ItemData Data { private set; get; }

	public ItemStack(GameItem item, int count) {
		Item = item;
		Count = count;
		Data = new ItemData();
	}

	public bool IsEmpty() {
		return Item == null || Count < 1;
	}

	public ItemStack Copy() {
		return new ItemStack(Item, Count);
	}

	public override bool Equals(object obj) {
		var stack = obj as ItemStack;
		return stack != null && EqualityComparer<GameItem>.Default.Equals(Item, stack.Item);
	}

	public override int GetHashCode() {
		return -979861770 + EqualityComparer<GameItem>.Default.GetHashCode(Item);
	}

	public override string ToString() {
		return Item + "x" + Count;
	}

}

public class ItemData {

	private Dictionary<string, object> data;

	public ItemData() {
		data = new Dictionary<string, object>();
	}

	public void Set(string key, object value) {
		if (data.ContainsKey(key)) {
			data[key] = value;
		} else {
			data.Add(key, value);
		}
	}

	public object Get(string key) {
		if (!data.ContainsKey(key)) {
			return null;
		}
		object ret;
		if (!data.TryGetValue(key, out ret) || ret == null) {
			return null;
		}
		return ret;
	}

	public T Get<T>(string key, T def) {
		object at = Get(key);
		if (!(at is T)) {
			return def;
		}
		if (at == null || at.Equals(def)) {
			return def;
		}
		return (T) at;
	}

}