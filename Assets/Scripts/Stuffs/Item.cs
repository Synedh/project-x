using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    Necklace,
    Bracelet,
    Ring,
    Weapon
}

public class Item {
    readonly string _name;
    readonly int _price;
    readonly Sprite _image;
    readonly ItemType _itemType;
    readonly string _description;
    readonly List<KeyValuePair<Characteristic, float>> _stats;
    readonly Spell _spell; // For weapons only

	public Item(string name, int price, Sprite image, ItemType itemType, string description, List<KeyValuePair<Characteristic, float>> stats, Spell spell = null)
	{
		_name = name;
		_price = price;
		_image = image;
		_itemType = itemType;
		_description = description;
		_stats = stats;
		_spell = spell; // /!\ For weapons only /!\
	}

	public string name {
		get {
			return _name;
		}
	}

	public int price {
		get {
			return _price;
		}
	}

	public Sprite image {
		get {
			return _image;
		}
	}

	public ItemType itemType {
		get {
			return _itemType;
		}
	}

	public string description {
		get {
			return _description;
		}
	}

	public List<KeyValuePair<Characteristic, float>> stats {
		get {
			return _stats;
		}
	}

	public Spell spell {
		get {
			return _spell;
		}
	}
}
