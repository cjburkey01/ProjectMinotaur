using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour {
	
	private ItemStack _item;

	public Image itemIcon;
	public Image itemIconBackground;
	public Text itemName;
	public Text itemCount;
	public ItemList parent;
	public ItemStack Item {
		set {
			_item = value;
			if (_item == null || _item.IsEmpty) {
				itemIcon.sprite = null;
				itemName.text = "EMPTY STACK ERR";
				itemCount.text = "-1 x";
			} else {
				itemIcon.sprite = _item.Item.Icon512;
				itemName.text = _item.Item.DisplayName;
				itemCount.text = _item.Count.ToString() + ' ' + 'x';
			}
		}
		get {
			return _item;
		}
	}

	public void OnClick() {
		if (parent != null) {
			parent.OnClick(this);
		}
	}

	public void Disable() {
		itemIcon.gameObject.SetActive(false);
		itemIconBackground.gameObject.SetActive(false);
		itemName.gameObject.SetActive(false);
		itemCount.gameObject.SetActive(false);
	}

	public void Enable() {
		itemIcon.gameObject.SetActive(true);
		itemIconBackground.gameObject.SetActive(true);
		itemName.gameObject.SetActive(true);
		itemCount.gameObject.SetActive(true);
	}

	public override bool Equals(object obj) {
		var display = obj as ItemDisplay;
		return display != null &&
			   base.Equals(obj) &&
			   EqualityComparer<ItemStack>.Default.Equals(_item, display._item) &&
			   EqualityComparer<Image>.Default.Equals(itemIcon, display.itemIcon) &&
			   EqualityComparer<Image>.Default.Equals(itemIconBackground, display.itemIconBackground) &&
			   EqualityComparer<Text>.Default.Equals(itemName, display.itemName) &&
			   EqualityComparer<Text>.Default.Equals(itemCount, display.itemCount) &&
			   EqualityComparer<ItemList>.Default.Equals(parent, display.parent) &&
			   EqualityComparer<ItemStack>.Default.Equals(Item, display.Item);
	}

	public override int GetHashCode() {
		var hashCode = -1305916796;
		hashCode = hashCode * -1521134295 + base.GetHashCode();
		hashCode = hashCode * -1521134295 + EqualityComparer<ItemStack>.Default.GetHashCode(_item);
		hashCode = hashCode * -1521134295 + EqualityComparer<Image>.Default.GetHashCode(itemIcon);
		hashCode = hashCode * -1521134295 + EqualityComparer<Image>.Default.GetHashCode(itemIconBackground);
		hashCode = hashCode * -1521134295 + EqualityComparer<Text>.Default.GetHashCode(itemName);
		hashCode = hashCode * -1521134295 + EqualityComparer<Text>.Default.GetHashCode(itemCount);
		hashCode = hashCode * -1521134295 + EqualityComparer<ItemList>.Default.GetHashCode(parent);
		hashCode = hashCode * -1521134295 + EqualityComparer<ItemStack>.Default.GetHashCode(Item);
		return hashCode;
	}

}