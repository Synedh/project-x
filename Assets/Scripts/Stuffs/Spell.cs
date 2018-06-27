using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

	string name;
	Sprite image;
	string description;
	List<Effect> effects;

	public Spell(string name, Sprite image, string description, List<Effect> effects)
	{
		this.name = name;
		this.image = image;
		this.description = description;
		this.effects = effects;
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

	public List<Effect> Effects {
		get {
			return this.effects;
		}
	}
}
