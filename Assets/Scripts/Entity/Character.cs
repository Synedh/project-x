using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct CharacterBuilder {
    public string nickname;
    public int necklaceId;
    public int braceletId;
    public int ringId;
    public int weaponId;
    public List<int> spellIds;
}

public class Character {
    readonly string _nickname;
    readonly Dictionary<Characteristic, float> _stats;
    readonly Dictionary<ItemType, Item> _items;
    readonly List<Spell> _spells;
    readonly List<KeyValuePair<Effect, EntityBehaviour>> _effects;
    public Team team;

    public Character (string nickname, Item necklace, Item bracelet, Item ring, Item weapon, List<Spell> spells) {

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
        _effects = new List<KeyValuePair<Effect, EntityBehaviour>>();

        UpdateStats();
        _stats[Characteristic.CurrentHP] = _stats[Characteristic.MaxHP];
        _stats[Characteristic.CurrentAP] = _stats[Characteristic.MaxAP];
        _stats[Characteristic.CurrentMP] = _stats[Characteristic.MaxMP];
    }

    public static Character CharacterLoader(CharacterBuilder characterBuilder) {
        List<Item> items = new List<Item>();
        List<Spell> spells = new List<Spell>();

        foreach (int itemId in new List<int> { 
            characterBuilder.necklaceId,
            characterBuilder.braceletId,
            characterBuilder.ringId,
            characterBuilder.weaponId })
        {
            if (itemId >= 0)
                items.Add(Item.ItemLoader(itemId));
            else
                items.Add(null);
        }

        foreach (int spellId in characterBuilder.spellIds)
        {
            if (spellId >= 0)
                spells.Add(Spell.SpellLoader(spellId));
        }

        return new Character(
            characterBuilder.nickname,
            items[0],
            items[1],
            items[2],
            items[3],
            spells
        );
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

    public List<KeyValuePair<Effect, EntityBehaviour>> effects {
        get {
            return this._effects;
        }
    }
}
