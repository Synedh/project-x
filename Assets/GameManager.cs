using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Object characterPrefab;
    public TextAsset tileMapFile;

    public CustomGrid grid;
    public GameObject character;

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
        character = loadEntity(characterPrefab, new Vector2(1, 1), Quaternion.identity);
        GameObject entity = loadEntity(Resources.Load("Prefabs/Entity"), new Vector2(2, 1), Quaternion.identity);
        print(entity.transform.position);
    }

    private void Update()
    {

    }

    public GameObject loadEntity(Object prefab, Vector2 pos, Quaternion rot)
    {
        GameObject entity = Instantiate(prefab, new Vector3(pos.x, 0, pos.y), rot) as GameObject;
        return grid.addEntity(entity);
    }

    public static Object GetPrefabFromId(int id)
    {
        if (id == 1)
            return Resources.Load("Prefabs/Plane");
        if (id == 2)
            return Resources.Load("Prefabs/Wall");
        return null;
    }
}
