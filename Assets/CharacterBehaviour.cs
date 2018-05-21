using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Move(CustomGrid customGrid, Cell cell)
    {
        bool[,] tilesmap = new bool[customGrid.width, customGrid.height];
        for (int i = 0; i < customGrid.width; i++)
        {
            for (int j = 0; j < customGrid.height; j++)
            {
                try
                {
                    tilesmap[i, j] = customGrid.GetCell(i, j).cellObject.GetComponent<CellBehaviour>().isWalkable;
                }
                catch (NullReferenceException e)
                {
                    tilesmap[i, j] = false;
                }
            }
        }

        NesScripts.Controls.PathFind.Grid grid = new NesScripts.Controls.PathFind.Grid(tilesmap);
        NesScripts.Controls.PathFind.Point _from = new NesScripts.Controls.PathFind.Point((int) transform.position.x, (int) transform.position.z);
        NesScripts.Controls.PathFind.Point _to = new NesScripts.Controls.PathFind.Point(cell.x, cell.y);

        List<NesScripts.Controls.PathFind.Point> path = NesScripts.Controls.PathFind.Pathfinding.FindPath(grid, _from, _to, NesScripts.Controls.PathFind.Pathfinding.DistanceType.Manhattan);

        foreach (NesScripts.Controls.PathFind.Point point in path)
        {
            Debug.Log(point.x + " - " + point.y);
        }
    }

    public void SetToCell(int x, int y)
    {
        this.transform.position = new Vector3(x, 0, y);
    }
}
