using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public GameObject selectedEntity;
    public List<GameObject> entities;

    TimelineBehaviour timelineBehaviour;
    TurnManager turnManager;
    CustomGrid grid;

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
        selectedEntity = null;
        entities = new List<GameObject>();
    }

    void Start()
    {
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
        timelineBehaviour = GameObject.Find("Timeline").GetComponent<TimelineBehaviour>();

        entities = new List<GameObject> {
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Entity"), new Vector2(1, 1)) as GameObject,
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Entity"), new Vector2(2, 1)) as GameObject,
            EntityBehaviour.LoadEntity(grid, Resources.Load("Prefabs/Entity"), new Vector2(2, 2)) as GameObject
        };
		entities[0].GetComponent<EntityBehaviour>().character = new Character("Toto");
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
		instance.selectedEntity.GetComponent<EntityBehaviour>().Rotate(rot);
	}

    public static Object GetPrefabFromId(int id)
    {
        if (id == 1)
            return Resources.Load("Prefabs/Ground");
        if (id == 2)
            return Resources.Load("Prefabs/Wall");
        return null;
    }
}
