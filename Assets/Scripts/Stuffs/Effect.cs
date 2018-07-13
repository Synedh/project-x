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

public class Effect
{
	string _name;
	Sprite _image;
    EffectType _type;
	string _chatMessage;
	string _description;
    List<KeyValuePair<Characteristic, float>> _effects;
    List<Vector2> _cells;

	int currentTurn;

    public Effect(string name, Sprite image, EffectType type, string chatMessage, string description, List<KeyValuePair<Characteristic, float>> effects, List<Vector2> cells)
	{
		_name = name;
		_image = image;
        _type = type;
		_chatMessage = chatMessage;
		_description = description;
		_effects = effects;
        _cells = cells;

		currentTurn = 0;
	}

	void PrintEffect(Characteristic type, float value)
	{
		// Print effect to chat
		Debug.Log("Effet " + type + " de " + value + ".");
	}

	public KeyValuePair<Characteristic, float> GetEffect()
	{
		KeyValuePair<Characteristic, float> effect = _effects[currentTurn];
		PrintEffect(effect.Key, effect.Value);
		currentTurn++;

		return effect;
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

    public List<KeyValuePair<Characteristic, float>> effects
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
}

