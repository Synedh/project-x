using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Orientation {
    Front,
    Left,
    Right,
    Back
}

public class EntityBehaviour : MonoBehaviour {

    public Material selected;
    public Material unselected;
    public Material hoverSelected;
    public Material hoverUnselected;

    public TimelineEntity timelineEntity;
    public Character character;

    public int x;
    public int y;
    public int orientation;
    public bool doMove;
    public bool isAlive = true;

    static CustomGrid grid;
    public List<Vector2> moveTargets;
    public List<Vector2> pushTargets;
    public EntityTurn entityTurn;
    List<Vector2> MPRangeCells;

    const float speed = 3f;
    const float pushSpeed = 10f;

    // Doubleclick
    bool one_click = false;
    float timer_for_double_click;
    const float delay = .5f;

    void Start() {
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();

        moveTargets = new List<Vector2>();
        pushTargets = new List<Vector2>();
        MPRangeCells = new List<Vector2>();

        x = (int)transform.position.x;
        y = (int)transform.position.z;
        orientation = (int)transform.eulerAngles.y;

        entityTurn = new EntityTurn(this);
        doMove = false;
    }
    
    void Update() {
        // Move
        if (doMove && moveTargets.Count > 0 && moveTargets.Count <= character.stats[Characteristic.CurrentMP])
            Move(moveTargets[0]);
        else if (doMove)
            doMove = false;

        // Push
        if (pushTargets.Count > 0)
            Push(pushTargets[0]);

        // grid.GetCellBehaviour((int)transform.position.x, (int)transform.position.z).colorCell(team.colorMaterial);

        x = (int)transform.position.x;
        y = (int)transform.position.z;
        
    }

    public void MouseDown()
    {
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
                CameraManager.instance.lookAt = transform;
            }
        }
    }

    public void MouseEnter()
    {
        if (GameManager.instance.currentEntityBehaviour == this) {
            foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
                renderer.material = hoverSelected;
            }
        } else {
            foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>()) {
                renderer.material = hoverUnselected;
            }
        }

        MPRangeCells = grid.MPRange(x, y, (int)(character.stats[Characteristic.CurrentMP]));
        grid.ColorCells(MPRangeCells, grid.hoverMP);
    }

    public void MouseExit()
    {
        if (GameManager.instance.currentEntityBehaviour == this) {
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
        Rotate(moveTarget);
        transform.position = Vector3.MoveTowards(
            transform.position, 
            new Vector3(moveTarget.x, transform.position.y, moveTarget.y), 
            speed * Time.deltaTime
        );

        if (new Vector2(transform.position.x, transform.position.z).Equals(moveTarget)) {
            moveTargets.RemoveAt(0);
            character.stats[Characteristic.CurrentMP]--;
            if (moveTargets.Count == 0)
                doMove = false;
        }
    }

    public void Push(Vector2 pushTarget)
    {
        transform.position = Vector3.MoveTowards(
            transform.position, 
            new Vector3(pushTarget.x, transform.position.y, pushTarget.y), 
            pushSpeed * Time.deltaTime
        );

        if (new Vector2(transform.position.x, transform.position.z).Equals(pushTarget)) {
            pushTargets.RemoveAt(0);
        }
    }

    public void Rotate(Vector2 cell)
    {
        if (cell.x - x > cell.y - y && (cell.x - x) + (cell.y - y) >= 0) // Front
            Rotate(0);
        else if (cell.x - x >= cell.y - y && (cell.x - x) + (cell.y - y) < 0) // Left
            Rotate(90);
        else if (cell.x - x < cell.y - y && (cell.x - x) + (cell.y - y) <= 0) // Back
            Rotate(180);
        else if (cell.x - x <= cell.y - y && (cell.x - x) + (cell.y - y) > 0) // Right
            Rotate(270);
    }

    public void Rotate(int angle)
    {    
        transform.eulerAngles = new Vector3(0, angle, 0);
        orientation = angle;
    }

    public void Die() {
        // TODO Die
        isAlive = false;
        transform.position = new Vector3(10000 + x, transform.position.y, 10000 + y);
    }

    void GetOrientation(Vector2 u, Vector2 v) {

    }

    public List<Vector2> GetAoeOfEffect(Effect effect, Vector2 target) {
        List<Vector2> cells = new List<Vector2>();

        if (target.x - x > target.y - y && (target.x - x) + (target.y - y) >= 0) // Front
        {
            foreach (Vector2 cell in effect.cells)
                cells.Add(new Vector2(target.x + cell.x, target.y + cell.y));
        }
        else if (target.x - x >= target.y - y && (target.x - x) + (target.y - y) < 0) // Left
        {
            foreach (Vector2 cell in effect.cells)
                cells.Add(new Vector2(target.x + cell.y, target.y - cell.x));
        }
        else if (target.x - x < target.y - y && (target.x - x) + (target.y - y) <= 0) // Back
        {
            foreach (Vector2 cell in effect.cells)
                cells.Add(new Vector2(target.x - cell.x, target.y - cell.y));
        }
        else if (target.x - x <= target.y - y && (target.x - x) + (target.y - y) > 0) // Right
        {
            foreach (Vector2 cell in effect.cells)
                cells.Add(new Vector2(target.x - cell.y, target.y + cell.x));
        }
        else // Center
        {
            foreach (Vector2 cell in effect.cells)
                cells.Add(new Vector2(x + cell.x, y + cell.y));
        }
        return cells;
    }

    public List<Vector2> GetAoeOfSpell(Spell spell, Vector2 target) {
        List<Vector2> cells = new List<Vector2>();

        if (target.x - x > target.y - y && (target.x - x) + (target.y - y) >= 0) // Front
        {
            foreach (Effect effect in spell.effects)
            {
                foreach (Vector2 cell in effect.cells)
                    cells.Add(new Vector2(cell.x, cell.y));
            }
        }
        else if (target.x - x >= target.y - y && (target.x - x) + (target.y - y) < 0) // Left
        {
            foreach (Effect effect in spell.effects)
            {
                foreach (Vector2 cell in effect.cells)
                    cells.Add(new Vector2(cell.y, -cell.x));
            }
        }
        else if (target.x - x < target.y - y && (target.x - x) + (target.y - y) <= 0) // Back
        {
            foreach (Effect effect in spell.effects)
            {
                foreach (Vector2 cell in effect.cells)
                    cells.Add(new Vector2(-cell.x, -cell.y));
            }
        }
        else if (target.x - x <= target.y - y && (target.x - x) + (target.y - y) > 0) // Right
        {
            foreach (Effect effect in spell.effects)
            {
                foreach (Vector2 cell in effect.cells)
                    cells.Add(new Vector2(-cell.y, cell.x));
            }
        }
        else // Center
        {
            foreach (Effect effect in spell.effects)
            {
                foreach (Vector2 cell in effect.cells)
                    cells.Add(new Vector2(cell.x, cell.y));
            }
        }
        return cells;
    }

    public List<Vector2> SetMoveTargets(Cell target)
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

    public void SetPushTargets(Vector2 cell, int qty)
    {
        int i = qty < 0 ? -1 : 1;
        int incr = qty < 0 ? -1 : 1;
        if (cell.x - x > cell.y - y && (cell.x - x) + (cell.y - y) >= 0) // Front
        {
            while (i != qty + incr)
            {
                CellBehaviour cellBehaviour = GameManager.instance.grid.GetCellBehaviour(x - i, y);
                if (cellBehaviour == null
                    || !cellBehaviour.isWalkable
                    || GameManager.instance.grid.GetEntityOnCell(x - i, y) != null)
                {
                    break;
                }
                pushTargets.Add(new Vector2(x - i, y));
                i += qty < 0 ? -1 : 1;
            }
        }
        else if (cell.x - x >= cell.y - y && (cell.x - x) + (cell.y - y) < 0) // Left
            while (i != qty + incr)
            {
                CellBehaviour cellBehaviour = GameManager.instance.grid.GetCellBehaviour(x, y + i);
                if (cellBehaviour == null || !cellBehaviour.isWalkable
                    || GameManager.instance.grid.GetEntityOnCell(x, y + i) != null)
                {
                    break;
                }
                pushTargets.Add(new Vector2(x, y + i));
                i += qty < 0 ? -1 : 1;
            }
        else if (cell.x - x < cell.y - y && (cell.x - x) + (cell.y - y) <= 0) // Back
            while (i != qty + incr)
            {
                CellBehaviour cellBehaviour = GameManager.instance.grid.GetCellBehaviour(x + i, y);
                if (cellBehaviour == null
                    || !cellBehaviour.isWalkable
                    || GameManager.instance.grid.GetEntityOnCell(x + i, y) != null)
                {
                    break;
                }
                pushTargets.Add(new Vector2(x + i, y));
                i += qty < 0 ? -1 : 1;
            }
        else if (cell.x - x <= cell.y - y && (cell.x - x) + (cell.y - y) > 0) // Right
            while (i != qty + incr)
            {
                CellBehaviour cellBehaviour = GameManager.instance.grid.GetCellBehaviour(x, y - i);
                if (cellBehaviour == null
                    || !cellBehaviour.isWalkable
                    || GameManager.instance.grid.GetEntityOnCell(x, y - i) != null)
                {
                    break;
                }
                pushTargets.Add(new Vector2(x, y - i));
                i += qty < 0 ? -1 : 1;
            }
    }

    public static GameObject LoadEntity(CustomGrid grid, UnityEngine.Object prefab, Vector2 pos)
    {
        GameObject entity = Instantiate(prefab, new Vector3(pos.x, 0, pos.y), Quaternion.identity) as GameObject;
        entity.transform.parent = GameObject.Find("Entities").transform;
        EntityBehaviour entityBehaviour = entity.GetComponent<EntityBehaviour>();
        GameManager.instance.entities.Add(entityBehaviour);
        GameManager.instance.timelineBehaviour.AddTimelineEntity(entityBehaviour, GameManager.instance.entities.Count - 1);
        return grid.AddEntity(entity);
    }
}
