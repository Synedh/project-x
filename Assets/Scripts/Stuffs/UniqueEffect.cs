    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEffect 
{
    EffectType _type;
    readonly float _valueMin;
    readonly float _valueMax;
    readonly Characteristic _carac;
    readonly Resources _prefab;

    public UniqueEffect(float valueMin, float valueMax = -1, Characteristic charac = Characteristic.CurrentHP, Resources prefab = null)
    {
        _valueMin = valueMin;
        _valueMax = valueMax;
        _carac = charac;
        _prefab = prefab;
    }

    public string ResolveUniqueEffect(EntityBehaviour sender, EntityBehaviour reciever = null, Vector2 target = new Vector2())
    {
        switch (_type)
        {
            case EffectType.Heal:
                return ResolveHeal(reciever, (int)_valueMin, (int)_valueMax).ToString();
            case EffectType.Move:
                return ResolveMove(reciever, target, (int)_valueMin).ToString();
            case EffectType.Create:
                return ResolveCreate(target, _prefab).ToString();
            case EffectType.Charac:
                return ResolveCarac(reciever, _valueMin, _carac);
            default:
                return ResolveDamage(
                    sender,
                    reciever,
                    _type,
                    (int)_valueMin,
                    (int)_valueMax,
                    _carac
                );
        }
    }

    int ResolveHeal(EntityBehaviour reciever, int valueMin, int valueMax)
    {
        return GameManager.instance.randomSeed.Next(valueMin, valueMax + 1);
    }

    int ResolveMove(EntityBehaviour reciever, Vector2 cell, int qty)
    {
        reciever.SetPushTargets(cell, qty);
        return 0;
    }

    int ResolveCreate(Vector2 position, Resources prefab)
    {
        Debug.Log("Create");
        return 0;
    }

    string ResolveCarac(EntityBehaviour reciever, float value, Characteristic charac)
    {
        reciever.character.stats[_carac] += value;
        return (int)(value * 100) + "%";
    }

    string ResolveDamage(EntityBehaviour sender, EntityBehaviour reciever, EffectType type, int valueMin, int valueMax, Characteristic charac = Characteristic.CurrentHP)
    {
        int baseDamage = GameManager.instance.randomSeed.Next(valueMin, valueMax + 1);
        float rangeDamage = 1f;
        float rangeResistance = 1f;
        float typeDamage = 1f;
        float typeResistance = 1f;
        float orientation = 1f;

        if (charac == Characteristic.CurrentHP)
        {
            if (Mathf.Abs(sender.x - reciever.x) + Mathf.Abs(sender.y - reciever.y) <= 1)
            {
                rangeDamage = sender.character.stats[Characteristic.ContactDamage];
                rangeResistance = reciever.character.stats[Characteristic.ContactResistance];
            }
            else
            {
                rangeDamage = sender.character.stats[Characteristic.DistantDamage];
                rangeResistance = reciever.character.stats[Characteristic.DistantResistance];
            }
            if (type == EffectType.Magic)
            {
                typeDamage = sender.character.stats[Characteristic.MagicDamage];
                typeResistance = reciever.character.stats[Characteristic.MagicResistance];
            }
            else
            {
                typeDamage = sender.character.stats[Characteristic.PhysicalDamage];
                typeResistance = reciever.character.stats[Characteristic.PhysicalResistance];
            }
        }

        if (sender.orientation == reciever.orientation)
            orientation = 1.5f;
        else if (sender.orientation == (reciever.orientation + 180) % 360)
            orientation = 1.0f;
        else
            orientation = 1.25f;

        int damage = - Mathf.RoundToInt(
            (baseDamage 
            + (baseDamage * (rangeDamage / rangeResistance - 1)) 
            + (baseDamage * (typeDamage / typeResistance - 1)))
            * orientation
        );
        if (reciever.character.stats[Characteristic.CurrentHP] <= -damage)
        {
            damage = (int)reciever.character.stats[Characteristic.CurrentHP];
            reciever.character.stats[Characteristic.CurrentHP] = 0f;
            reciever.Die();
            return - damage + " (die)";
        }
        reciever.character.stats[Characteristic.CurrentHP] += damage;
        return damage.ToString();
    }

    public EffectType type
    {
        get
        {
            return this._type;
        }
        set
        {
            _type = value;
        }
    }

    public float valueMin
    {
        get
        {
            return this._valueMin;
        }
    }

    public float valueMax
    {
        get
        {
            return this._valueMax;
        }
    }

    public Characteristic charac
    {
        get
        {
            return this._carac;
        }
    }

    public Resources prefab
    {
        get
        {
            return this._prefab;
        }
    }
}

