using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class CustomGrid: MonoBehaviour {

    public Material defaultColor;
    public Material hoverMP;
    public Material pathMP;
    public Material reachableSpellRange;
    public Material unreachableSpellRange;
    public Material aoeSpellRange;

    public TextAsset tileMapFile;
    public int width, height;
    
    int[,] tilemap;
    List<GameObject> entities;
    Cell[,] grid;
    GameObject[,] drawnGrid;

    void Awake()
    {
        LoadGrid();
    }

    private void Start()
    {
        drawnGrid = Draw(); 
    }

    void Update()
    {
        
    }

    public void LoadGrid()
    {
        tilemap = ReadTileMap(tileMapFile);
        width = tilemap.GetLength(0);
        height = tilemap.GetLength(1);
        entities = new List<GameObject>();

        grid = new Cell[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                grid[i, j] = new Cell(i, j, GameManager.GetPrefabFromId(tilemap[i, j]));
            }
        }
    }

    public GameObject[,] Draw()
    {
        GameObject[,] drawGrid = new GameObject[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                drawGrid[i, j] = grid[i, j].Draw();
            }
        }

        return drawGrid;
	}

    public GameObject GetCellObject(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
            return drawnGrid[x, y];
        else
            return null;
    }

	public GameObject AddEntity(GameObject entity)
	{
		foreach (GameObject tmpEntity in entities)
		{
			if (tmpEntity.transform.position == entity.transform.position)
				return null;
		}
		entities.Add(entity);
		return entity;
	}

	public GameObject GetEntityOnCell(int x, int y)
	{
		foreach (GameObject entity in entities)
		{
			if (entity.transform.position.x == x && entity.transform.position.z == y)
				return entity;
		}
		return null;
	}

	public List<Vector2> MPRange(int x, int y, int range)
	{
		List<Vector2> cells = new List<Vector2>();

		for (int i = x - range; i <= x + range; ++i) {
			for (int j = y - range; j <= y + range; ++j) {
				try {
					int reachable = IsReachable(new Vector2(x, y), new Vector2(i, j));
					if (reachable > 0 && reachable <= range) {
						cells.Add(new Vector2(i, j));
					}
				}
				catch (ArgumentOutOfRangeException) { }
				catch (IndexOutOfRangeException) { }
			}
		}

		return cells;
    }

    public bool[,] GetWalkableGrid()
    {
        bool[,] walkableGrid = new bool[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (grid[i, j].cellObject.GetComponent<CellBehaviour>().isWalkable)
                        walkableGrid[i, j] = !GetEntityOnCell(i, j);
                    else
                        walkableGrid[i, j] = false;
                }
                catch (NullReferenceException)
                {
                    walkableGrid[i, j] = false;
                }
            }
        }

        return walkableGrid;
    }

    public List<Vector2>[] SpellRange(int x, int y, int rangeMin, int rangeMax, bool needView = true) {
        List<Vector2> reachableCells = new List<Vector2>();
        List<Vector2> unreachableCells = new List<Vector2>();

        for (int i = x - rangeMax; i <= x + rangeMax; ++i) {
            for (int j = y - rangeMax; j <= y + rangeMax; ++j) {
                if (Math.Abs(j - y) + Math.Abs(i - x) <= rangeMax &&
                    Math.Abs(j - y) + Math.Abs(i - x) >= rangeMin &&
                    i >= 0 && i < height && j >= 0 && j < width)
                {
                    if (!needView || Visibility(new Vector2(x, y), new Vector2(i, j)))
                        reachableCells.Add(new Vector2(i, j));
                    else
                        unreachableCells.Add(new Vector2(i, j));

                }
            }
        }
        return new List<Vector2>[] { reachableCells, unreachableCells };
    }

    public bool Visibility(Vector2 u, Vector3 v){
        int dx = (int)Mathf.Abs(v.x - u.x);
        int dy = (int)Mathf.Abs(v.y - u.y);
        int x = (int)u.x;
        int y = (int)u.y;
        int n = dx + dy - 1;
        int x_inc = (v.x > u.x ? 1 : -1);
        int y_inc = (v.y > u.y ? 1 : -1);
        int error = dx - dy;
        dx *= 2;
        dy *= 2;
        bool[,] viewGrid = GetViewGrid();

        while (n > 0)
        {
            if (!viewGrid[y,x] && !u.Equals(new Vector2(x, y)))
            {
                return false;
            }

            else {
                if (error > 0)
                {
                    x += x_inc;
                    error -= dy;
                }

                else if (error < 0)
                {
                    y += y_inc;
                    error += dx;
                }

                else {
                    x += x_inc;
                    error -= dy;
                    y += y_inc;
                    error += dx;
                    n--;
                }
                n--;
            }
        }
        return true;
    }

    public bool[,] GetViewGrid()
    {
        bool[,] viewGrid = new bool[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                try
                {
                    if (!grid[i, j].cellObject.GetComponent<CellBehaviour>().hideView)
                        viewGrid[i, j] = !GetEntityOnCell(i, j);
                    else
                        viewGrid[i, j] = false;
                }
                catch (NullReferenceException)
                {
                    viewGrid[i, j] = true;
                }
            }
        }

        return viewGrid;
    }

	public int IsReachable(Vector2 start, Vector2 target)
	{
		List<NesScripts.Controls.PathFind.Point> path = NesScripts.Controls.PathFind.Pathfinding.FindPath(
			new NesScripts.Controls.PathFind.Grid(GetWalkableGrid()),
			new NesScripts.Controls.PathFind.Point((int)start.x, (int)start.y),
			new NesScripts.Controls.PathFind.Point((int)target.x, (int)target.y),
			NesScripts.Controls.PathFind.Pathfinding.DistanceType.Manhattan
		);

		if (path [path.Count - 1].x == target.x && path [path.Count - 1].y == target.y)
			return path.Count;
		return 0;
    }

    public void ColorCells(List<Vector2> cells, Material material)
    {
        foreach (Vector2 cell in cells) {
            if (GetCellObject((int)cell.x, (int)cell.y) != null && GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().isWalkable)
                GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().colorCell(material);
        }
    }

    public void CleanCells(List<Vector2> cells)
    {
        foreach (Vector2 cell in cells) {
            if (GetCellObject((int)cell.x, (int)cell.y) != null && GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().isWalkable)
                GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().removeColorCell();
        }
    }

    public static int[,] ReadTileMap(TextAsset tileMapFile)
    {
        string[] lines = tileMapFile.text.Split('\n');
        int[,] tileMap = new int[lines.Length, lines[0].Split(';').Length];
        for (int i = 0; i < lines.Length; i++)
        {
            string[] line = lines[i].Split(';');
            for (int j = 0; j < line.Length; j++)
            {
                tileMap[i, j] = Int32.Parse(line[j]);
            }
        }
        return tileMap;
    }
}
