using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {

    public float speed;
    List<Vector3> targets;

	// Use this for initialization
	void Start () {
        speed = 3f;
        targets = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
        if (targets.Count != 0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targets[0], step);
            if (transform.position.Equals(targets[0]))
                targets.RemoveAt(0);

        }
	}

    public void Move(CustomGrid customGrid, Cell cell)
    {
        bool[,] tilesmap = customGrid.getWalkableGrid();

        NesScripts.Controls.PathFind.Grid grid = new NesScripts.Controls.PathFind.Grid(tilesmap);
        NesScripts.Controls.PathFind.Point _from;
        if (targets.Count == 0)
        {
            _from = new NesScripts.Controls.PathFind.Point((int)transform.position.x, (int)transform.position.z);
        }
        else
        {
            _from = new NesScripts.Controls.PathFind.Point((int)targets[targets.Count - 1].x, (int)targets[targets.Count - 1].z);
        }
        NesScripts.Controls.PathFind.Point _to = new NesScripts.Controls.PathFind.Point(cell.x, cell.y);

        List<NesScripts.Controls.PathFind.Point> path = NesScripts.Controls.PathFind.Pathfinding.FindPath(grid, _from, _to, NesScripts.Controls.PathFind.Pathfinding.DistanceType.Manhattan);

        foreach (NesScripts.Controls.PathFind.Point point in path)
        {
            targets.Add(new Vector3(point.x, transform.position.y, point.y));
        }
    }

    public void SetToCell(int x, int y)
    {
        this.transform.position = new Vector3(x, 0, y);
    }
}
