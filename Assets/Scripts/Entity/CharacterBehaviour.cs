using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {

    public string nickname;
    public Dictionary<string, float> caracteristics;
    public Dictionary<string, Item> items;
    public List<Spell> spells;
	public List<Effect> effects;


    // Use this for initialization
    void Start () {
        nickname = "";
        caracteristics = new Dictionary<string, float>() {
            {"life", 0f},
            {"AP", 0f},
            {"MP", 0f},
            {"contact_damage", 0f},
            {"distant_damage", 0f},
            {"physical_damage", 0f},
            {"magic_damage", 0f},
            {"contact_resistance", 0f},
            {"distant_resistance", 0f},
            {"physical_resistance", 0f},
            {"magic_resistance", 0f},
            {"speed", 0f}
        };
        items = new Dictionary<string, Item>() {
            {"collar", null},
            {"ring", null},
            {"bracelet", null},
			{"weapon", null}
        };
        spells = new List<Spell>();
		effects = new List<Effect>();
    }
	
	// Update is called once per frame
	void Update () {

    }

	public void UpdateStats()
	{
		foreach (KeyValuePair<string, Item> item in items)
		{
			foreach (KeyValuePair<string, float> stat in item.Value.Stats) {
				this.caracteristics[stat.Key] += stat.Value;
			}
		}

		foreach (Effect effect in effects)
		{
			KeyValuePair<string, float> turnEffect = effect.GetEffect();
			caracteristics[turnEffect.Key] += turnEffect.Value;
		}
	}

	public void LoadCharacter(string nickname, Dictionary<string, Item> items, List<Spell> spells)
    {
        this.nickname = nickname;
		this.items = items;
		this.spells = spells;

		UpdateStats();
    }
}
