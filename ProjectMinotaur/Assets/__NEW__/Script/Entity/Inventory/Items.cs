using System.Collections.Generic;
using UnityEngine;

public abstract class GameItem : IGameItem {

	protected readonly string domain;
	protected readonly string path;
	protected readonly string name;
	protected readonly string desc;
	protected readonly string texturePath;
	protected readonly InventorySize size;

	protected GameItem(string domain, string path, string name, string desc, int w, int h) : this(domain, path, name, desc, null, w, h) {
	}

	protected GameItem(string domain, string path, string name, string desc, string texturePath, int w, int h) {
		this.domain = domain;
		this.path = path;
		this.name = name;
		this.desc = desc;
		this.texturePath = texturePath;
		size = new InventorySize(w, h);
	}

	public string GetDomain() {
		return domain;
	}

	public string GetResourceName() {
		return path;
	}

	public string GetName() {
		return name;
	}

	public string GetDescription() {
		return desc;
	}

	public Texture2D GetIcon() {
		return Resources.Load<Texture2D>(texturePath);
	}

	public InventorySize GetSize() {
		return size;
	}

	public override bool Equals(object obj) {
		var item = obj as GameItem;
		return item != null && domain == item.domain && path == item.path && name == item.name && desc == item.desc && texturePath == item.texturePath && size.Equals(item.size);
	}

	public override int GetHashCode() {
		var hashCode = 537909824;
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(domain);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(path);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(desc);
		hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(texturePath);
		hashCode = hashCode * -1521134295 + EqualityComparer<InventorySize>.Default.GetHashCode(size);
		return hashCode;
	}

}

public interface IGameItem {

	string GetDomain();
	string GetResourceName();
	string GetName();
	string GetDescription();
	Texture2D GetIcon();
	InventorySize GetSize();
	bool Equals(object obj);
	int GetHashCode();

}

public static class ItemHandler {

	private static List<IGameItem> items = new List<IGameItem>();

	/// <summary>
	///		Attempts to register the supplied item as an actual game item.
	/// </summary>
	/// <param name="i">The item to register</param>
	/// <returns>Whether or not the registration was successful.</returns>
	public static bool RegisterItem(IGameItem i) {
		if (items.Contains(i)) {
			return false;
		}
		items.Add(i);
		return true;
	}

	public static IGameItem[] GetItems() {
		return items.ToArray();
	}

}