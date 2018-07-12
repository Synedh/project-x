using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    string _nickname;
	Dictionary<Characteristic, float> _stats;
	Dictionary<ItemType, Item> _items;
    List<Spell> _spells;
	List<Effect> _effects;

	public Character (string nickname, Item necklace = null, Item bracelet = null, Item ring = null, Item weapon = null, List<Spell> spells = null) {

		_nickname = nickname;
		_stats = new Dictionary<Characteristic, float>() {
			{ Characteristic.MaxHP, 60f },
			{ Characteristic.CurrentHP, 60f },
			{ Characteristic.MaxAP, 6f },
			{ Characteristic.CurrentAP, 6f },
			{ Characteristic.MaxMP, 3f },
			{ Characteristic.CurrentMP, 3f },
			{ Characteristic.ContactDamage, 1f },
			{ Characteristic.DistantDamage, 1f },
			{ Characteristic.PhysicalDamage, 1f },
			{ Characteristic.MagicDamage, 1f },
			{ Characteristic.ContactResistance, 1f },
			{ Characteristic.DistantResistance, 1f },
			{ Characteristic.PhysicalResistance, 1f },
			{ Characteristic.MagicResistance, 1f },
			{ Characteristic.Speed, 0f }
        };
		_items = new Dictionary<ItemType, Item>() {
			{ ItemType.Necklace, necklace },
			{ ItemType.Bracelet, bracelet },
			{ ItemType.Ring, ring },
			{ ItemType.Weapon, weapon }
        };
        _spells = spells;
		_effects = new List<Effect>();

		UpdateStats();
		_stats[Characteristic.CurrentHP] = _stats[Characteristic.MaxHP];
		_stats[Characteristic.CurrentAP] = _stats[Characteristic.MaxAP];
		_stats[Characteristic.CurrentMP] = _stats[Characteristic.MaxMP];
	}

	public void UpdateStats()
	{
		foreach (KeyValuePair<ItemType, Item> item in _items)
		{
			if (item.Value != null)
			{
				foreach (KeyValuePair<Characteristic, float> stat in item.Value.stats)
				{
					_stats[stat.Key] += stat.Value;
				}
			}
		}

		foreach (Effect effect in _effects)
		{
			KeyValuePair<Characteristic, float> turnEffect = effect.GetEffect();
			_stats[turnEffect.Key] += turnEffect.Value;
		}
	}

    public List<KeyValuePair<float, EffectType>> DamageCalculus(Spell spell) {
        List<KeyValuePair<float, EffectType>> damages = new List<float>();
        foreach (Effect effect in spell.effects)
        {
            if (effect.type == EffectType.Physical)
            {
                // TODO physical spell
            }
            else if (effect.type == EffectType.Magic)
            {
                // TODO magical spell
            }
            else
            {
                // TODO heal spell
            }
                
        }
        return damages;
    }

    public float DamageLost(Spell spell) {
        return 0f;
    }

	public string nickname {
		get {
			return this._nickname;
		}
	}

	public Dictionary<Characteristic, float> stats {
		get {
			return this._stats;
		}
	}

	public Dictionary<ItemType, Item> items {
		get {
			return this._items;
		}
	}

	public List<Spell> spells {
		get {
			return this._spells;
		}
	}

	public List<Effect> effects {
		get {
			return this._effects;
		}
	}
}
