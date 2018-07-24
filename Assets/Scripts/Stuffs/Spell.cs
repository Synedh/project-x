using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public enum RangeType
{
    Classical,
    Line,
    Diagonal
}

public class Spell {

    readonly string _name;
    readonly int _price;
    readonly int _cost;
    readonly int _rangeMin;
    readonly int _rangeMax;
    readonly RangeType _rangeType;
    readonly Sprite _image;
    readonly string _description;
    readonly List<Effect> _effects;

	public Spell(string name, int price, int cost, int rangeMin, int rangeMax, RangeType rangeType, Sprite image, string description, List<Effect> effects)
	{
		_name = name;
		_price = price;
		_cost = cost;
		_rangeMin = rangeMin;
		_rangeMax = rangeMax;
        _rangeType = rangeType;
		_image = image;
		_description = description;
		_effects = effects;
	}

    public void Apply(EntityBehaviour sender, Vector2 cell)
    {
        ChatBehaviour.WriteMessage(sender.character.nickname + " use " + _name, MessageType.Combat);
        sender.Rotate(cell);
        foreach (Effect effect in _effects)
            effect.Apply(sender, cell);
        sender.character.stats[Characteristic.CurrentAP] -= this.cost;
    }

    public static Spell SpellLoader(int spellId) {
        using (StreamReader r = new StreamReader(GameManager.spellPath + spellId.ToString() + ".json"))
            return JsonConvert.DeserializeObject<Spell>(r.ReadToEnd());
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

    public int rangeMax
    {
        get
        {
            return _rangeMax;
        }
    }

    public RangeType rangeType
    {
        get
        {
            return _rangeType;
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
}
