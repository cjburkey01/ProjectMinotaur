using System.Collections.Generic;
using UnityEngine;

public class GameItem {

	public string UniqueId { private set; get; }
	public string DisplayName { private set; get; }
	public string Description { private set; get; }
	public int MaxStackSize { private set; get; }

	protected GameObject model;
	protected Sprite icon;

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

	public Sprite Icon {
		protected set {
			icon = value;
		}
		get {
			if (icon == null) {
				Debug.LogWarning("Item has no icon: " + UniqueId);
			}
			return icon;
		}
	}

	protected GameItem(string uid, string name, string description, int maxStackSize) : this(uid, name, description, maxStackSize, null, null) {
	}

	public GameItem(string uid, string name, string description, int maxStackSize, GameObject model, Sprite icon) {
		UniqueId = uid;
		DisplayName = name;
		Description = description;
		MaxStackSize = maxStackSize;
		Model = model;
		Icon = icon;
	}

	public virtual void CreateModel(WorldItem item) { }

	public override bool Equals(object obj) {
		var item = obj as GameItem;
		return item != null && UniqueId == item.UniqueId && DisplayName == item.DisplayName && Description == item.Description && MaxStackSize == item.MaxStackSize && EqualityComparer<GameObject>.Default.Equals(Model, item.Model) && EqualityComparer<Sprite>.Default.Equals(Icon, item.Icon);
	}

	public override int GetHashCode() {
		var hashCode = 1813968700;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(UniqueId);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
		hashCode = hashCode * -1521134295 + EqualityComparer<GameObject>.Default.GetHashCode(Model);
		hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(Icon);
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

}

public class ItemStack {

	public GameItem Item { private set; get; }
	public int Count { set; get; }

	public ItemStack(GameItem item, int count) {
		Item = item;
		Count = count;
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