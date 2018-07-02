using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

	string name;
	int price;
	Sprite image;
	ItemType itemType;
	string description;
	List<KeyValuePair<Characteristic, float>> stats;
	Spell spell; // For weapons only

	public Item(string name, int price, Sprite image, ItemType itemType, string description, List<KeyValuePair<Characteristic, float>> stats, Spell spell = null)
	{
		this.name = name;
		this.price = price;
		this.image = image;
		this.itemType = itemType;
		this.description = description;
		this.stats = stats;
		this.spell = spell; // /!\ For weapons only /!\
	}

	public string Name {
		get {
			return this.name;
		}
	}

	public int Price {
		get {
			return this.price;
		}
	}

	public Sprite Image {
		get {
			return this.image;
		}
	}

	public ItemType ItemType {
		get {
			return this.itemType;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	public List<KeyValuePair<Characteristic, float>> Stats {
		get {
			return this.stats;
		}
	}

	public Spell Spell {
		get {
			return this.spell;
		}
	}
}
