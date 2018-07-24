using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
	Un effet est en fait une suite d'effets sur plusieurs tours.
	Le premier est exécuté au lancement du spell, puis à chaque début de tour on récupère les effets.
	Pour un spell classique, on récupère le premier effet et ça s'arrête là.
	Pour un poison, le premier effet est vide et s'applique à partir du début de tour de la cible.
	Pour une baisse de PA/PM, il est appliqué à chaque tour (vu que les PA/PM se reset à chaque tour).
	Pour une baisse de stats, la baisse s'applique instantannément, puis est remonté au dernier tour.
*/

public enum EffectType {
    Physical,
    Magic,
    Heal,
    Charac,
    Move,
    Create
}

public class Effect
{
    readonly string _name;
    readonly Sprite _image;
    readonly EffectType _type;
    readonly string _description;
    readonly List<UniqueEffect> _effects;
    readonly List<Vector2> _cells;

	int _currentTurn;

    public Effect(string name, Sprite image, EffectType type, string description, List<UniqueEffect> effects, List<Vector2> cells)
	{
		_name = name;
		_image = image;
        _type = type;
		_description = description;
		_effects = effects;
        _cells = cells;

		_currentTurn = 0;

        foreach (UniqueEffect effect in effects)
        {
            if (effect != null)
                effect.type = type;
        }
	}

    public void Resolve(EntityBehaviour sender, EntityBehaviour reciever = null, Vector2 target = new Vector2())
	{
        UniqueEffect effect = _effects[_currentTurn++];
        if (effect != null && (reciever == null || reciever != null && reciever.isAlive))
        {
            string value = effect.ResolveUniqueEffect(sender, reciever, target);
            if (_type == EffectType.Physical || _type == EffectType.Magic || _type == EffectType.Heal || _type == EffectType.Charac)
            {
                string effectType = effect.charac.ToString();
                if (effect.charac.ToString().StartsWith("current"))
                    effectType = effect.charac.ToString().Substring(7);

                ChatBehaviour.WriteMessage(
                    reciever.character.nickname + " : " + value + " " + effectType + " by " + _name + " from " + sender.character.nickname + ".",
                    MessageType.Combat
                );
            }
        }
	}

    public void Apply(EntityBehaviour sender, Vector2 target) {
        foreach (Vector2 cell in sender.GetAoeOfEffect(this, target))
        {
            GameObject entity = GameManager.instance.grid.GetEntityOnCell((int)cell.x, (int)cell.y);

            if ((_type == EffectType.Physical || _type == EffectType.Magic || _type == EffectType.Heal)
                && entity != null) // CurrentHP
            {
                List<KeyValuePair<Effect, EntityBehaviour>> entityEffects = entity.GetComponent<EntityBehaviour>().character.effects;
                entityEffects.Add(new KeyValuePair<Effect, EntityBehaviour>(this.MemberwiseClone() as Effect, sender));
                entityEffects[entityEffects.Count - 1].Key.Resolve(sender, reciever: entity.GetComponent<EntityBehaviour>());
                if (entityEffects[entityEffects.Count - 1].Key.currentTurn >= entityEffects[entityEffects.Count - 1].Key.effects.Count)
                    entityEffects.RemoveAt(entityEffects.Count - 1);
            }
            else if (_type == EffectType.Create) // Create
            {
                Resolve(sender, target: cell);
                _currentTurn--;
            }
            else if (_type == EffectType.Move && entity != null) // Move
            {
                List<KeyValuePair<Effect, EntityBehaviour>> entityEffects = entity.GetComponent<EntityBehaviour>().character.effects;
                entityEffects.Add(new KeyValuePair<Effect, EntityBehaviour>(this.MemberwiseClone() as Effect, sender));
                if (target.Equals(cell))
                    entityEffects[entityEffects.Count - 1].Key.Resolve(sender, reciever: entity.GetComponent<EntityBehaviour>(), target: new Vector2(sender.x, sender.y));
                else
                    entityEffects[entityEffects.Count - 1].Key.Resolve(sender, reciever: entity.GetComponent<EntityBehaviour>(), target: target);
                if (entityEffects[entityEffects.Count - 1].Key.currentTurn >= entityEffects[entityEffects.Count - 1].Key.effects.Count)
                    entityEffects.RemoveAt(entityEffects.Count - 1);
            }
            else if (_type == EffectType.Charac && entity != null) // Charac
            {
                List<KeyValuePair<Effect, EntityBehaviour>> entityEffects = entity.GetComponent<EntityBehaviour>().character.effects;
                entityEffects.Add(new KeyValuePair<Effect, EntityBehaviour>(this.MemberwiseClone() as Effect, sender));
                entityEffects[entityEffects.Count - 1].Key.Resolve(sender, reciever: entity.GetComponent<EntityBehaviour>());
            }
        }
    }

    public string name
    {
        get
        {
            return this._name;
        }
    }

    public Sprite image
    {
        get
        {
            return this._image;
        }
    }

    public EffectType type
    {
        get
        {
            return this._type;
        }
    }

    public string description
    {
        get
        {
            return this._description;
        }
    }

    public List<UniqueEffect> effects
    {
        get
        {
            return this._effects;
        }
    }

    public List<Vector2> cells {
        get {
            return _cells;
        }
    }

    public int currentTurn
    {
        get
        {
            return this._currentTurn;
        }
        set
        {
            _currentTurn = value;
        }
    }
}

