using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public System.Random randomSeed;

    public Team team1;
    public Team team2;
    public List<GameObject> entities;
    public GameObject currentEntity;
    public CustomGrid grid;
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
        team1 = new Team();
        team2 = new Team();
        entities = new List<GameObject>();
    }

    void Start()
    {
        randomSeed = new System.Random();
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
        timelineBehaviour = GameObject.Find("Timeline").GetComponent<TimelineBehaviour>();

        entities = new List<GameObject> {
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 5)) as GameObject,
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 7)) as GameObject,
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 10)) as GameObject,
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 12)) as GameObject
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

        Spell firstSpell = new Spell("Sword of Damocles", 50, 3, 1, 1, RangeType.Classical, null, "Et paf !",
                               new List<Effect>()
            {
                new Effect("Sword of Damocles", null, EffectType.Physical, "", "12-14 physical damage",
                    new List<UniqueEffect> { new UniqueEffect(12, 14, charac: Characteristic.CurrentHP) }, 
                    new List<Vector2>() { new Vector2(0, 0) })
            });

        Spell secondSpell = new Spell("Fireball", 50, 3, 2, 5, RangeType.Classical, null, "Brule !",
            new List<Effect>()
            {
                new Effect("Fireball", null, EffectType.Magic, "", "6-8 magical damage",
                    new List<UniqueEffect> { new UniqueEffect(6, 8, charac: Characteristic.CurrentHP) },
                    new List<Vector2>() { new Vector2(0, 0) }),
                new Effect("Fireball AOE", null, EffectType.Magic, "", "4 AOE magical damage (2 turns)",
                    new List<UniqueEffect> {
                        null,
                    new UniqueEffect(4, 4, charac: Characteristic.CurrentHP),
                    new UniqueEffect(4, 4, charac: Characteristic.CurrentHP)
                    },
                    new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, 0), new Vector2(0, 1)})
            });

        Spell thirdSpell = new Spell("Punch", 50, 3, 1, 2, RangeType.Line, null, "Falcon punch !",
            new List<Effect>()
            {
                new Effect("Punch", null, EffectType.Physical, "", "1-3 physical damage",
                    new List<UniqueEffect> { new UniqueEffect(1, 3, charac: Characteristic.CurrentHP) },
                    new List<Vector2>() { new Vector2(0, 0) }),
                new Effect("Punch", null, EffectType.Move, "", "Push 2 cells",
                    new List<UniqueEffect> { new UniqueEffect(2) },
                    new List<Vector2>() { new Vector2(0, 0)})
            });

        Spell fourthSpell = new Spell("Attirance", 50, 3, 2, 8, RangeType.Line, null, "Ahhhttirance !",
            new List<Effect>()
            {
                new Effect("Attirance", null, EffectType.Move, "", "6 cells attirance",
                    new List<UniqueEffect> { new UniqueEffect(-6) },
                    new List<Vector2>() { new Vector2(0, 0) })
            });

        Spell fifthSpell = new Spell("Coup de bambou", 50, 3, 0, 6, RangeType.Classical, null, "Un coup de mou ?",
            new List<Effect>()
            {
                new Effect("Vulné", null, EffectType.Charac, "", "20% de résistance contact en moins",
                    new List<UniqueEffect> {
                        new UniqueEffect(-0.2f, charac: Characteristic.ContactResistance),
                        null },
                    new List<Vector2>() { new Vector2(0, 0) }),
                new Effect("Vulné", null, EffectType.Charac, "", "20% de résistance distance en moins",
                    new List<UniqueEffect> {
                        new UniqueEffect(-0.2f, charac: Characteristic.DistantResistance),
                        null },
                    new List<Vector2>() { new Vector2(0, 0) })
            });

        entities[0].GetComponent<EntityBehaviour>().character = new Character("Toto", firstNecklace, null, firstRing, null, new List<Spell>() {firstSpell, secondSpell, thirdSpell, fourthSpell, fifthSpell});
		entities[1].GetComponent<EntityBehaviour>().character = new Character("Bill");
        entities[2].GetComponent<EntityBehaviour>().character = new Character("Boule");
        entities[3].GetComponent<EntityBehaviour>().character = new Character("Bidule");

        team1.Add(entities[0].GetComponent<EntityBehaviour>());
        team1.Add(entities[1].GetComponent<EntityBehaviour>());
        team2.Add(entities[2].GetComponent<EntityBehaviour>());
        team2.Add(entities[3].GetComponent<EntityBehaviour>());

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
