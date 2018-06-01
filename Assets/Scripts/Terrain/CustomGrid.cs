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

    public Cell GetCell(int x, int y)
    {
        return grid[x, y];
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

    public GameObject GetEntityOnCell(int x, int y)
    {
        foreach (GameObject entity in entities)
        {
            if (entity.transform.position.x == x && entity.transform.position.z == y)
                return entity;
        }
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
