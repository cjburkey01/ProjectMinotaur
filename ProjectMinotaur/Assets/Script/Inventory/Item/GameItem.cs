using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class GameItem {

	public string UniqueId { private set; get; }
	public string DisplayName { private set; get; }
	public string Description { private set; get; }
	public int MaxStackSize { private set; get; }
    public bool Permanent { private set; get; }

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

	protected GameItem(string uid, string name, string description, int maxStackSize) : this(uid, name, description, maxStackSize, null, null, null, false) {
	}

	public GameItem(JSONNode json) : this(json["unique_id"].Value, json["name"].Value, json["description"].Value, json["max_stack_size"].AsInt, Resources.Load<GameObject>(json["model_path"].Value), Resources.Load<Sprite>(json["icon_lq_path"].Value), Resources.Load<Sprite>(json["icon_hq_path"].Value), json["permanent"].AsBool) {
	}

	public GameItem(string uid, string name, string description, int maxStackSize, GameObject model, Sprite icon32, Sprite icon512, bool permanent) {
		UniqueId = uid;
		DisplayName = name;
		Description = description;
		MaxStackSize = maxStackSize;
		Model = model;
		Icon32 = icon32;
		Icon512 = icon512;
        Permanent = permanent;
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

public class GameItemHoldable : GameItem {

	public bool IsPrimary { private set; get; }
	public Vector3 DisplayPositionOffset { private set; get; }
	public Vector3 DisplayRotationOffset { private set; get; }

	public GameItemHoldable(JSONNode json) : base(json) {
		IsPrimary = json["is_primary"].AsBool;
		DisplayPositionOffset = WeaponLoader.LoadVector3(json, "display_position_offset");
		DisplayRotationOffset = WeaponLoader.LoadVector3(json, "display_rotation_offset");
	}

}

public class WorldItem : MonoBehaviour {

	public ItemStack Stack { private set; get; }

	public static WorldItem Spawn(ItemStack stack, Vector3 pos) {
		if (stack == null || stack.IsEmpty) {
			return null;
		}
		GameObject obj = new GameObject(stack.Item.DisplayName + "x" + stack.Count);
		WorldItem item = obj.AddComponent<WorldItem>();
		item.Stack = stack.Copy;
		item.transform.position = pos;
		item.transform.rotation = Random.rotation;
		stack.Item.CreateModel(item);
		return item;
	}
	
	void Update() {
		// TODO: ROTATE ON GROUND
	}

}

public class ItemStack {

	public GameItem Item { private set; get; }
	public int Count { set; get; }
	public DataHandler Data { private set; get; }
	public bool IsEmpty {
		get {
			return Item == null || Count < 1;
		}
	}
	public ItemStack Copy {
		get {
			return new ItemStack(Item, Count, Data.Copy());
		}
	}

	public ItemStack(GameItem item, int count) : this(item, count, new DataHandler()) {
	}

	protected ItemStack(GameItem item, int count, DataHandler data) {
		Item = item;
		Count = count;
		Data = data;
	}

	public override bool Equals(object obj) {
		var stack = obj as ItemStack;
		return stack != null && EqualityComparer<GameItem>.Default.Equals(Item, stack.Item) && EqualityComparer<DataHandler>.Default.Equals(Data, stack.Data);
	}

	public override int GetHashCode() {
		return -979861770 + EqualityComparer<GameItem>.Default.GetHashCode(Item) + EqualityComparer<DataHandler>.Default.GetHashCode(Data);
	}

	public override string ToString() {
		return Count + "x" + Item + Data.ToString();
	}

}