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

    public static string itemPath = "Assets/Resources/Items/";
    public static string spellPath = "Assets/Resources/Spells/";

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

        Item firstRing = Item.ItemLoader(0);
        Item firstNecklace = Item.ItemLoader(1);

        Spell firstSpell = Spell.SpellLoader(0);
        Spell secondSpell = Spell.SpellLoader(1);
        Spell thirdSpell = Spell.SpellLoader(2);
        Spell fourthSpell = Spell.SpellLoader(3);
        Spell fifthSpell = Spell.SpellLoader(4);

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
        /*
        Item item = Item.ItemLoader("Assets/Resources/Items/0.json");
        Debug.Log(item.name);*/
    }

    void Update()
    {
        if (!team1.checkTeam())
        {
            Debug.Log("Team 2 won !");
            Application.Quit();
        }
        else if (!team2.checkTeam())
        {
            Debug.Log("Team 1 won !");
            Application.Quit();
        }
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
