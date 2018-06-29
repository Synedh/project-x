using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour {

    float speed;
    public Material selected;
    public Material unselected;
    public Material hoverSelected;
    public Material hoverUnselected;

    BoxCollider boxCollider;
    public TimelineEntity timelineEntity;
    public Character character;
    static CustomGrid grid;

    List<Vector3> moveTargets;
	List<Vector2> MPRangeCells;
	int rotate;
    
	void Start () {
        speed = 3f;
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();

		moveTargets = new List<Vector3>();
		MPRangeCells = new List<Vector2>();
    }
	
	void Update () {
        // Move
        if (moveTargets.Count != 0)
        {
			Move(moveTargets[0]);
			if (transform.position.Equals(moveTargets[0]))
				moveTargets.RemoveAt(0);

        }
	}

	int[] Position() {
		return new int[] {
			(int)Math.Truncate(transform.position.x),
			(int)Math.Truncate(transform.position.z)
		};
	}

    public void MouseDown()
    {
		Debug.Log(character.Nickname);
    }

	public void MouseEnter()
	{
		if (GameManager.instance.selectedEntity == gameObject) {
			foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
				renderer.material = hoverSelected;
			}
		} else {
			foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
				renderer.material = hoverUnselected;
			}
		}
		int[] position = Position ();
		MPRangeCells = grid.MPRange(position[0], position[1], Convert.ToInt32(character.Stats[Characteristic.CurrentMP]));
		grid.ColorCells(MPRangeCells, new Color(0f, 1f, 0.2f, 0.5f));
	}

	public void MouseExit()
	{
		if (GameManager.instance.selectedEntity == gameObject) {
			foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
				renderer.material = selected;
			}
		} else {
			foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
				renderer.material = unselected;
			}
		}
		grid.CleanCells(MPRangeCells);
	}

    public void SetToCell(int x, int y)
    {
        transform.position = new Vector3(x, 0, y);
    }

	public void Move(Vector3 moveTarget) 
	{
		if (transform.position.x - moveTarget.x < 0 && transform.position.z - moveTarget.z == 0)
			Rotate (0);
		else if (transform.position.x - moveTarget.x == 0 && transform.position.z - moveTarget.z < 0)
			Rotate (270);
		else if (transform.position.x - moveTarget.x == 0 && transform.position.z - moveTarget.z > 0)
			Rotate (90);
		else
			Rotate (180);
		transform.position = Vector3.MoveTowards(transform.position, moveTarget, speed * Time.deltaTime);
	}

	public void Rotate(int angle)
	{	
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

    public void SetMoveTargets(CustomGrid grid, Cell target)
    {
        // Fill a targets list of point. Then this list is parsed by Update function.
        // Once a location is reached, element is remove and next one is targeted.
        // PathFinding module is used to search best way.
        bool[,] tilesmap = grid.GetWalkableGrid();

        NesScripts.Controls.PathFind.Grid pathGrid = new NesScripts.Controls.PathFind.Grid(tilesmap);
        NesScripts.Controls.PathFind.Point _from;
        if (moveTargets.Count == 0)
        {
            _from = new NesScripts.Controls.PathFind.Point((int)transform.position.x, (int)transform.position.z);
        }
        else
        {
            _from = new NesScripts.Controls.PathFind.Point((int)moveTargets[moveTargets.Count - 1].x, (int)moveTargets[moveTargets.Count - 1].z);
        }
        NesScripts.Controls.PathFind.Point _to = new NesScripts.Controls.PathFind.Point(target.x, target.y);
        List<NesScripts.Controls.PathFind.Point> path = NesScripts.Controls.PathFind.Pathfinding.FindPath(pathGrid, _from, _to, NesScripts.Controls.PathFind.Pathfinding.DistanceType.Manhattan);

        foreach (NesScripts.Controls.PathFind.Point point in path)
        {
            moveTargets.Add(new Vector3(point.x, transform.position.y, point.y));
        }
	}

    public static GameObject LoadEntity(CustomGrid grid, UnityEngine.Object prefab, Vector2 pos)
    {
        GameObject entity = Instantiate(prefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity) as GameObject;
        entity.transform.parent = GameObject.Find("Entities").transform;
        GameManager.instance.entities.Add(entity);
        GameObject.Find("Timeline").GetComponent<TimelineBehaviour>().AddTimelineEntity(entity, GameManager.instance.entities.Count - 1);
        return grid.AddEntity(entity);
    }


    public static void SelectEntity(GameObject entity)
    {
        GameManager.instance.selectedEntity = entity;
        foreach (GameObject tmpEntity in GameManager.instance.entities)
        {
            EntityBehaviour tmpEntityBehaviour = tmpEntity.GetComponent<EntityBehaviour>();
            foreach (MeshRenderer renderer in tmpEntity.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = tmpEntityBehaviour.unselected;
            }
            tmpEntityBehaviour.timelineEntity.SetColor(Color.black);
        }
        foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = entity.GetComponent<EntityBehaviour>().selected;
        }
        entity.GetComponent<EntityBehaviour>().timelineEntity.SetColor(Color.red);
    }
}
