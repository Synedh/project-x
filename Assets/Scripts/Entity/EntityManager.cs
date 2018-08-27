using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour {

    public Material colorMaterial1;
    public Material colorMaterial2;

    public static Team team1;
    public static Team team2;
    public static List<EntityBehaviour> entities;
    public static EntityManager instance;

    void Awake() {
        instance = this;
        entities = new List<EntityBehaviour>();
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    void Update () {
        if (!team1.checkTeam())
        {
            Debug.Log("Team 2 won !");
            GameManager.Stop();
        }
        else if (!team2.checkTeam())
        {
            Debug.Log("Team 1 won !");
            GameManager.Stop();
        }
	}

    public static void LoadTeams(int IDTeam1, int IDTeam2) {
        team1 = new Team(IDTeam1, colorMaterial: instance.colorMaterial1);
        team2 = new Team(IDTeam2, colorMaterial: instance.colorMaterial2);
    }

    public static void LoadEntities() {
        foreach (Character character in team1.characters.Concat(team2.characters).ToList())
        {
            LoadEntity(character, CustomGrid.instance, Resources.Load("Prefabs/Game/Entity"), new Vector2(8, 5));
        }
        TimelineBehaviour.instance.Refresh();
    }

    public static void LoadEntity(Character character, CustomGrid grid, UnityEngine.Object prefab, Vector2 pos)
    {
        GameObject entity = Instantiate(
            prefab,
            new Vector3(pos.x, 0, pos.y),
            Quaternion.identity
        ) as GameObject;
        entity.transform.parent = GameObject.Find("Entities").transform;

        EntityBehaviour entityBehaviour = entity.GetComponent<EntityBehaviour>();
        entities.Add(entityBehaviour);

        entityBehaviour.colorIndicator = Instantiate(
            Resources.Load("Prefabs/Game/ColorIndicator"),
            new Vector3(pos.x, -0.2499f, pos.y),
            Quaternion.Euler(new Vector3(90f, 0f, 0f))
        ) as GameObject;
        entityBehaviour.colorIndicator.transform.parent = entity.transform;
        entityBehaviour.SetCharacter(character);
        entityBehaviour.entityTurn = new EntityTurn(entityBehaviour);
        TimelineBehaviour.instance.AddTimelineEntity(entityBehaviour, entities.Count - 1);
    }
}
