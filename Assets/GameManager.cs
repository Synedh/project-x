using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    public Object characterPrefab;
    public TextAsset tileMapFile;

    public CustomGrid grid;
    public GameObject character;

    // Use this for initialization
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
        character = Instantiate(characterPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        character.GetComponent<CharacterBehaviour>().SetToCell(1, 1);
    }

    private void Update()
    {
        
    }
}
