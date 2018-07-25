using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public System.Random randomSeed;

    public int iDTeam1;
    public int iDTeam2;
    public GameObject turnCounter;

    public Team team1;
    public Team team2;
    public List<EntityBehaviour> entities;
    public EntityBehaviour currentEntityBehaviour;
    public CustomGrid grid;
    public Spell selectedSpell;
    public TimelineBehaviour timelineBehaviour;
    public TurnManager turnManager;

    public static string itemPath = "Assets/Resources/Items/";
    public static string spellPath = "Assets/Resources/Spells/";
    public static string teamPath = "Assets/Resources/Teams/";

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
        currentEntityBehaviour = null;
        entities = new List<EntityBehaviour>();
        team1 = new Team(iDTeam1);
        team2 = new Team(iDTeam2);
    }

    void Start()
    {
        randomSeed = new System.Random();
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
        timelineBehaviour = GameObject.Find("Timeline").GetComponent<TimelineBehaviour>();

        EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 5));
        EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 7));
        EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 10));
        EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 12));
        entities[0].character = team1.characters[0];
        entities[1].character = team1.characters[1];
        entities[2].character = team2.characters[0];
        entities[3].character = team2.characters[1];

        timelineBehaviour.Refresh();
        turnManager = new TurnManager(
            timelineBehaviour, 
            GameObject.Find("TurnCounter").GetComponentInChildren<Text>()
        );
        NextTurn();
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
        instance.currentEntityBehaviour.Rotate(rot);
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
