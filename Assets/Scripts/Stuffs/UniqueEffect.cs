    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueEffect 
{
    EffectType _type;
    int _valueMin;
    int _valueMax;
    Vector2 _position;
    Characteristic _carac;
    Resources _prefab;

    public UniqueEffect(int valueMin, int valueMax = 0, Vector2 position = new Vector2(), Characteristic carac = Characteristic.CurrentHP, Resources prefab = null)
    {
        _valueMin = valueMin;
        _valueMax = valueMax;
        _position = position;
        _carac = carac;
        _prefab = prefab;
    }

    public int ResolveUniqueEffect(EntityBehaviour sender, EntityBehaviour reciever = null)
    {
        switch (_type)
        {
            case EffectType.Heal:
                return ResolveHeal(sender, reciever, _valueMin, _valueMax);
                break;
            case EffectType.Move:
                return ResolveMove(sender, reciever, _position, _valueMin);
                break;
            case EffectType.Create:
                return ResolveCreate(sender, _position, _prefab);
                break;
            default:
                return ResolveCarac(sender, reciever, _type, _valueMin, _valueMax, _carac);
                break;
        }
    }

    static int ResolveHeal(EntityBehaviour sender, EntityBehaviour reciever, int valueMin, int valueMax)
    {
        Debug.Log("Heal");
        return 0;
    }

    static int ResolveMove(EntityBehaviour sender, EntityBehaviour reciever, Vector2 position, float qty)
    {
        Debug.Log("Move");
        return 0;
    }

    static int ResolveCreate(EntityBehaviour sender, Vector2 position, Resources prefab)
    {
        Debug.Log("Create");
        return 0;
    }

    static int ResolveCarac(EntityBehaviour sender, EntityBehaviour reciever, EffectType type, int valueMin, int valueMax, Characteristic carac)
    {
        int baseDamage = GameManager.instance.randomSeed.Next(valueMin, valueMax + 1);
        float rangeDamage = 1f;
        float rangeResistance = 1f;
        float typeDamage = 1f;
        float typeResistance = 1f;
        float orientation = 1f;

        if (carac == Characteristic.CurrentHP)
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

        return Mathf.RoundToInt(
            (baseDamage 
            + (baseDamage * (rangeDamage / rangeResistance - 1)) 
            + (baseDamage * (typeDamage / typeResistance - 1)))
            * orientation
        );
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

    public Vector2 position
    {
        get
        {
            return this._position;
        }
    }

    public Characteristic carac
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

