using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item {

	string name;
	Sprite image;
	string description;
	List<KeyValuePair<string, float>> stats;
	List<Effect> effects;

	public Item(string name, Sprite image, string description, List<KeyValuePair<string, float>> stats, List<Effect> effects)
	{
		this.name = name;
		this.image = image;
		this.description = description;
		this.stats = stats;
		this.effects = effects; // /!\ For weapons only /!\
	}

	public string Name {
		get {
			return this.name;
		}
	}

	public Sprite Image {
		get {
			return this.image;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	public List<KeyValuePair<string, float>> Stats {
		get {
			return this.stats;
		}
	}

	public List<Effect> Effects {
		get {
			return this.effects;
		}
	}
}
