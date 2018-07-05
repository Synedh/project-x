using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class CustomGrid: MonoBehaviour {

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
        return drawnGrid[x, y];
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
                    if (!needView || LineOfSight(new Vector2(x, y), new Vector2(i, j)))
                        reachableCells.Add(new Vector2(i, j));
                    else
                        unreachableCells.Add(new Vector2(i, j));

                }
            }
        }
        return new List<Vector2>[] { reachableCells, unreachableCells };
    }

    bool LineOfSight(Vector2 u, Vector2 v)
    {
        bool[,] viewGrid = GetViewGrid();
        // Calcul de l'équation de droite y = a * x + b
        // Source : https://fr.wikipedia.org/wiki/%C3%89quation_de_droite#Par_r%C3%A9solution_d'un_syst%C3%A8me_d'%C3%A9quations

        float a = (v.y - u.y) / (v.x - u.x);
        float b = u.y - a * u.x;

        // On prend le côté le plus éloigné entre abscisses et ordonnées. 
        // Pour chaque point on récupère les coordonnées ([x, a * x + b] ou [(y - b) / a, y]).
        // Puis on vérifie si le point n'est pas bloquant

        if (v.x - u.x == 0) { // Cas spécial même ligne
            for (int y = (int)Math.Min(u.y, v.y); y < (int)Math.Max(u.y, v.y); ++y)
            {
                Vector2 cell = new Vector2(u.x, y);
                if (!viewGrid[(int)cell.x, (int)cell.y])
                    return false;
            }
        }
        else if (v.y - u.y == 0) { // Cas spécial même colonne
            for (int x = (int)Math.Min(u.x, v.x); x < (int)Math.Max(u.x, v.x); ++x)
            {
                Vector2 cell = new Vector2(x, u.y);
                if (!viewGrid[(int)cell.x, (int)cell.y])
                    return false;
            }
        } else if (v.x - u.x > v.y - u.y) { // Abscisses
            for (int x = (int)Math.Min(u.x, v.x); x <= (int)Math.Max(u.x, v.x); ++x)
            {
                Vector2 cell = new Vector2(x, (float)Math.Round(a * x + b));
                if (!viewGrid[(int)cell.x, (int)cell.y])
                    return false;
            }
        } else { // Ordonnées
            for (int y = (int)Math.Min(u.y, v.y); y <= (int)Math.Max(u.y, v.y); ++y)
            {
                Vector2 cell = new Vector2((float)Math.Round((y - b) / a), y);
                if (!viewGrid[(int)cell.x, (int)cell.y])
                    return false;
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

    public void ColorCells(List<Vector2> cells, Color color)
    {
        foreach (Vector2 cell in cells) {
            if (GetCellObject((int)cell.x, (int)cell.y) != null && GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().isWalkable)
                GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().colorCell(color);
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
