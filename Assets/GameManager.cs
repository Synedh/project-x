using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public TextAsset tileMapFile;

    public CustomGrid grid;
    public GameObject selectedEntity;
    public List<GameObject> entities;

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
        grid = new CustomGrid(CustomGrid.ReadTileMap(tileMapFile), 1);
        grid.Draw();
        selectedEntity = null;
        entities = new List<GameObject>();
    }

    void Start()
    {
        EntityBehaviour.loadEntity(Resources.Load("Prefabs/Entity"), new Vector2(1, 1), Quaternion.identity);
        EntityBehaviour.loadEntity(Resources.Load("Prefabs/Entity"), new Vector2(2, 1), Quaternion.identity);
        EntityBehaviour.loadEntity(Resources.Load("Prefabs/Entity"), new Vector2(2, 2), Quaternion.identity);
    }

    void Update()
    {

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
