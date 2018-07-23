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

	string _name;
	int _price;
	Sprite _image;
	ItemType _itemType;
	string _description;
	List<KeyValuePair<Characteristic, float>> _stats;
	Spell _spell; // For weapons only

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
