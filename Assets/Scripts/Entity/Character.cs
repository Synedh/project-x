using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character {

    string nickname;
	Dictionary<Characteristic, float> stats;
	Dictionary<ItemType, Item> items;
    List<Spell> spells;
	List<Effect> effects;

	public Character (string nickname, Item necklace = null, Item bracelet = null, Item ring = null, Item weapon = null, List<Spell> spells = null) {

		this.nickname = nickname;
		stats = new Dictionary<Characteristic, float>() {
			{ Characteristic.MaxLife, 60f },
			{ Characteristic.Life, 60f },
			{ Characteristic.AP, 6f },
			{ Characteristic.MP, 3f },
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
		items = new Dictionary<ItemType, Item>() {
			{ ItemType.Necklace, necklace },
			{ ItemType.Bracelet, bracelet },
			{ ItemType.Ring, ring },
			{ ItemType.Weapon, weapon }
        };
        this.spells = spells;
		effects = new List<Effect>();

		UpdateStats ();
    }

	public string Nickname {
		get {
			return this.nickname;
		}
	}

	public Dictionary<Characteristic, float> Stats {
		get {
			return this.stats;
		}
	}

	public Dictionary<ItemType, Item> Items {
		get {
			return this.items;
		}
	}

	public List<Spell> Spells {
		get {
			return this.spells;
		}
	}

	public List<Effect> Effects {
		get {
			return this.effects;
		}
	}

	public void UpdateStats()
	{
		foreach (KeyValuePair<ItemType, Item> item in items)
		{
			if (item.Value != null)
			{
				foreach (KeyValuePair<Characteristic, float> stat in item.Value.Stats)
				{
					this.stats[stat.Key] += stat.Value;
				}
			}
		}

		foreach (Effect effect in effects)
		{
			KeyValuePair<Characteristic, float> turnEffect = effect.GetEffect();
			stats[turnEffect.Key] += turnEffect.Value;
		}
	}
}
