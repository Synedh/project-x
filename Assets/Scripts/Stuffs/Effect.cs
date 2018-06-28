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
	string name;
	Sprite image;
	string chatMessage;
	string description;
	List<KeyValuePair<Characteristic, float>> effects;

	int currentTurn;

	public Effect(string name, Sprite image, string chatMessage, string description, List<KeyValuePair<Characteristic, float>> effects)
	{
		this.name = name;
		this.image = image;
		this.chatMessage = chatMessage;
		this.description = description;
		this.effects = effects;

		currentTurn = 0;
	}

	void PrintEffect(Characteristic type, float value)
	{
		// Print effect to chat
		Debug.Log("Effet " + type + " de " + value + ".");
	}

	public KeyValuePair<Characteristic, float> GetEffect()
	{
		KeyValuePair<Characteristic, float> effect = effects[currentTurn];
		PrintEffect(effect.Key, effect.Value);
		currentTurn++;

		return effect;
	}

	public string Name {
		get {
			return this.name;
		}
	}

	public Sprite Image {
		get {
			return this.image;
		}
	}

	public string ChatMessage {
		get {
			return this.chatMessage;
		}
	}

	public string Description {
		get {
			return this.description;
		}
	}

	public List<KeyValuePair<Characteristic, float>> Effects {
		get {
			return this.effects;
		}
	}
}

