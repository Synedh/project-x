﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject currentEntity;
    public CustomGrid grid;
    public List<GameObject> entities;
    public Spell selectedSpell;

    TimelineBehaviour timelineBehaviour;
    TurnManager turnManager;

    // Use this for initialization, called before Start()
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        InitGame();
	}

    void InitGame()
    {
        currentEntity = null;
        entities = new List<GameObject>();
    }

    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
        timelineBehaviour = GameObject.Find("Timeline").GetComponent<TimelineBehaviour>();

        entities = new List<GameObject> {
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(1, 1)) as GameObject,
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(2, 1)) as GameObject,
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(2, 2)) as GameObject
        };

		Item firstNecklace = new Item("Sauron's eye", 20, null, ItemType.Necklace, "C tré for", 
            new List<KeyValuePair<Characteristic, float>>() { 
                new KeyValuePair<Characteristic, float>(Characteristic.MaxAP, 1f),
                new KeyValuePair<Characteristic, float>(Characteristic.MaxHP, 20f)
            },
			null
		);

        Item firstRing = new Item("The One", 30, null, ItemType.Necklace, "Ca casse des culs", 
            new List<KeyValuePair<Characteristic, float>>() { 
                new KeyValuePair<Characteristic, float>(Characteristic.MaxMP, 1f),
                new KeyValuePair<Characteristic, float>(Characteristic.MagicDamage, 0.5f),
                new KeyValuePair<Characteristic, float>(Characteristic.DistantDamage, 0.5f)
            },
            null
        );

        Spell firstSpell = new Spell("Sword of  Damocles", 50, 3, 1, 1, null, "Et paf !",
                               new List<Effect>()
            {
                new Effect("hit", null, EffectType.Physical, "", "20 physical damages",
                    new List<KeyValuePair<Characteristic, float>>
                    {
                        new KeyValuePair<Characteristic, float>(Characteristic.CurrentHP, 20f)
                    }, new List<Vector2>() { new Vector2(0, 0) })
            });

        Spell secondSpell = new Spell("Fireball", 50, 3, 0, 9, null, "Brule !",
            new List<Effect>()
            {
                new Effect("hit", null, EffectType.Magic, "", "10 magical damages",
                    new List<KeyValuePair<Characteristic, float>>
                    {
                        new KeyValuePair<Characteristic, float>(Characteristic.CurrentHP, 10f)
                    },  new List<Vector2>() { new Vector2(0, 0) }),
                new Effect("Burn", null, EffectType.Magic, "", "6 AOE magical damages",
                    new List<KeyValuePair<Characteristic, float>>
                    {
                        new KeyValuePair<Characteristic, float>(Characteristic.CurrentHP, 6f)
                    }, new List<Vector2>() { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1)})
            });
        entities[0].GetComponent<EntityBehaviour>().character = new Character("Toto", firstNecklace, null, firstRing, null, new List<Spell>() {firstSpell, secondSpell});
		entities[1].GetComponent<EntityBehaviour>().character = new Character("Bill");
		entities[2].GetComponent<EntityBehaviour>().character = new Character("Boule");

        timelineBehaviour.Refresh();

        turnManager = new TurnManager(entities);
        NextTurn();
    }

    void Update()
    {

    }

    public void NextTurn()
    {
        instance.turnManager.Next();
    }

	public void RotateTo(int rot)
	{
		instance.currentEntity.GetComponent<EntityBehaviour>().Rotate(rot);
	}

    public static Object GetPrefabFromId(int id)
    {
        if (id == 1)
            return Resources.Load("Prefabs/Game/Ground");
        if (id == 2)
            return Resources.Load("Prefabs/Game/Wall");
        return null;
    }
}
