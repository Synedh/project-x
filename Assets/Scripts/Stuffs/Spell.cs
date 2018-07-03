﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell {

	string _name;
	int _price;
	int _cost;
	int _rangeMin;
	int _rangeMax;
	Sprite _image;
	string _description;
	List<Effect> _effects;
	List<Vector2> _cells;

	public Spell(string name, int price, int cost, int rangeMin, int rangeMax, Sprite image, string description, List<Effect> effects, List<Vector2> cells)
	{
		_name = name;
		_price = price;
		_cost = cost;
		_rangeMin = rangeMin;
		_rangeMax = rangeMax;
		_image = image;
		_description = description;
		_effects = effects;
		_cells = cells;
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

	public int cost {
		get {
			return _cost;
		}
	}

	public int rangeMin {
		get {
			return _rangeMin;
		}
	}

	public int rangeMax {
		get {
			return _rangeMax;
		}
	}

	public Sprite image {
		get {
			return _image;
		}
	}

	public string description {
		get {
			return _description;
		}
	}

	public List<Effect> effects {
		get {
			return _effects;
		}
	}

	public List<Vector2> cells {
		get {
			return _cells;
		}
	}
}
