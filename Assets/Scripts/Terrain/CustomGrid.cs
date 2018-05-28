using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;  

public class CustomGrid {

    public int cellSize, width, height;
    List<GameObject> entities;
    int[,] tilemap;
    Cell[,] grid;

    public CustomGrid(int[,] tilemap, int cellSize)
    {
        this.width = tilemap.GetLength(0);
        this.height = tilemap.GetLength(1);
        this.cellSize = cellSize;
        this.tilemap = tilemap;
        entities = new List<GameObject>();

        grid = new Cell[this.width, this.height];
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                grid[i, j] = new Cell(i * cellSize, j * cellSize, GameManager.GetPrefabFromId(tilemap[i, j]));
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        return this.grid[x, y];
    }

    public Transform[,] Draw()
    {
        Transform[,] drawGrid = new Transform[this.width, this.height];

        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                drawGrid[i, j] = this.grid[i, j].Draw();
            }
        }

        return drawGrid;
    }

    public bool[,] getWalkableGrid()
    {
        bool[,] walkableGrid = new bool[this.width, this.height];

        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                try
                {
                    if (grid[i, j].cellObject.GetComponent<CellBehaviour>().isWalkable)
                        walkableGrid[i, j] = !getEntityOnCell(i, j);
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

    public bool[,] getViewGrid()
    {
        bool[,] viewGrid = new bool[this.width, this.height];

        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                try
                {
                    if (!grid[i, j].cellObject.GetComponent<CellBehaviour>().hideView)
                        viewGrid[i, j] = !getEntityOnCell(i, j);
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

    public GameObject getEntityOnCell(int x, int y)
    {
        foreach (GameObject entity in entities)
        {
            if (entity.transform.position.x == x && entity.transform.position.z == y)
                return entity;
        }
        return null;
    }

    public GameObject addEntity(GameObject entity)
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
