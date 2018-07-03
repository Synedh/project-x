using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour {

    public Material selected;
    public Material unselected;
    public Material hoverSelected;
    public Material hoverUnselected;

    public TimelineEntity timelineEntity;
    public Character character;

	public int x;
	public int y;
	public bool doMove;

	static CustomGrid grid;
    List<Vector2> moveTargets;
	List<Vector2> MPRangeCells;
	float speed;
    // Doubleclick
    bool one_click = false;
    float timer_for_double_click;
    const float delay = .5f;

    void Start () {
		GetComponent<BoxCollider>().enabled = false;

        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();

		moveTargets = new List<Vector2>();
		MPRangeCells = new List<Vector2>();

		x = (int)transform.position.x;
		y = (int)transform.position.z;

		speed = 3f;
		doMove = false;
    }
	
	void Update () {
        // Move
		if (doMove && moveTargets.Count > 0 && moveTargets.Count <= character.stats[Characteristic.CurrentMP])
			Move (moveTargets [0]);
		else if (doMove)
			doMove = false;
        x = (int)transform.position.x;
        y = (int)transform.position.z;
    }

    public void MouseDown()
    {
        // Debug.Log(character.nickname);

        if (Input.GetMouseButtonDown(0))
        {
            if (!one_click) // first click no previous clicks
            {
                one_click = true;
                timer_for_double_click = Time.time;
            }
            else if ((Time.time - timer_for_double_click) > delay)
            {
                one_click = false;
            }
            else
            {
                one_click = false; // found a double click, now reset
                CameraControl.instance.lookAt = transform;
            }
        }
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

		MPRangeCells = grid.MPRange(x, y, (int)(character.stats[Characteristic.CurrentMP]));
		grid.ColorCells(MPRangeCells, new Color(0.3f, 0.6f, 0.3f, 1f));
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

	public void Move(Vector2 moveTarget) 
	{
		if (transform.position.x - moveTarget.x < 0 && transform.position.z - moveTarget.y == 0)
			Rotate (0);
		else if (transform.position.x - moveTarget.x == 0 && transform.position.z - moveTarget.y < 0)
			Rotate (270);
		else if (transform.position.x - moveTarget.x == 0 && transform.position.z - moveTarget.y >= 0)
			Rotate (90);
		else
			Rotate (180);
		transform.position = Vector3.MoveTowards(
			transform.position, 
			new Vector3(moveTarget.x, transform.position.y, moveTarget.y), 
			speed * Time.deltaTime
		);

		if (new Vector2(transform.position.x, transform.position.z) == moveTarget) {
			moveTargets.RemoveAt(0);
            character.stats[Characteristic.CurrentMP]--;
			if (moveTargets.Count == 0)
				doMove = false;
		}
	}

	public void Rotate(int angle)
	{	
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

    public List<Vector2> SetMoveTargets(CustomGrid grid, Cell target)
    {
        // Fill a targets list of point. Then this list is parsed by Update function.
        // Once a location is reached, element is remove and next one is targeted.
        // PathFinding module is used to search best way.

        List<NesScripts.Controls.PathFind.Point> path = NesScripts.Controls.PathFind.Pathfinding.FindPath(
			new NesScripts.Controls.PathFind.Grid(grid.GetWalkableGrid()), 
			new NesScripts.Controls.PathFind.Point(x, y),
			new NesScripts.Controls.PathFind.Point(target.x, target.y), 
			NesScripts.Controls.PathFind.Pathfinding.DistanceType.Manhattan
		);

		moveTargets.Clear();
        foreach (NesScripts.Controls.PathFind.Point point in path)
            moveTargets.Add(new Vector2(point.x, point.y));
        return moveTargets;
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

        CameraControl.instance.lookAt = entity.transform;
    }
}
