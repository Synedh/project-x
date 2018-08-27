using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public System.Random randomSeed;

    public int IDTeam1;
    public int IDTeam2;
    public Material colorMaterial1;
    public Material colorMaterial2;

    public GameObject turnCounter;
    public GameObject timeLine;
    public GameObject TeamContainer;

    public EntityBehaviour currentEntityBehaviour;
    public Spell selectedSpell;

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
        EntityManager.LoadTeams(IDTeam1, IDTeam2);
    }

    void Start()
    {
        randomSeed = new System.Random();

        EntityManager.LoadEntities();
    }

    void Update()
    {
        
    }

    public static void Stop() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
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
