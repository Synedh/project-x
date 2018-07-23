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
    Move,
    Create
}

public class Effect
{
	string _name;
	Sprite _image;
    EffectType _type;
	string _chatMessage;
	string _description;
    List<UniqueEffect> _effects;
    List<Vector2> _cells;

	int _currentTurn;

    public Effect(string name, Sprite image, EffectType type, string chatMessage, string description, List<UniqueEffect> effects, List<Vector2> cells)
	{
		_name = name;
		_image = image;
        _type = type;
		_chatMessage = chatMessage;
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

    public void Resolve(EntityBehaviour sender, EntityBehaviour reciever = null)
	{
        UniqueEffect effect = _effects[_currentTurn++];
        if (effect != null)
        {
            int value = effect.ResolveUniqueEffect(sender, reciever);
            ChatBehaviour.WriteMessage(
                reciever.character.nickname + " : " + value + " " + effect.carac + " by " + _name + " from " + sender.character.nickname + ".",
                MessageType.Combat
            );
        }
	}

    public void Apply(EntityBehaviour sender, Vector2 target) {
        foreach (Vector2 cell in sender.GetAoeOfEffect(this, target))
        {
            GameObject entity = GameManager.instance.grid.GetEntityOnCell((int)cell.x, (int)cell.y);
            if (entity != null)
            {
                List<KeyValuePair<Effect, EntityBehaviour>> entityEffects = entity.GetComponent<EntityBehaviour>().character.effects;
                entityEffects.Add(new KeyValuePair<Effect, EntityBehaviour> (this.MemberwiseClone() as Effect, sender));
                entityEffects[entityEffects.Count - 1].Key.Resolve(sender, entity.GetComponent<EntityBehaviour>());
                if (entityEffects[entityEffects.Count - 1].Key.currentTurn >= entityEffects[entityEffects.Count - 1].Key.effects.Count)
                    entityEffects.RemoveAt(entityEffects.Count - 1);
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

    public string chatMessage
    {
        get
        {
            return this._chatMessage;
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

