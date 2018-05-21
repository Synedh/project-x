using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CustomGrid {

    public int cellSize, width, height;
    int[,] tilemap;
    Cell[,] grid;

    public CustomGrid(int[,] tilemap, int cellSize)
    {
        this.width = tilemap.GetLength(0);
        this.height = tilemap.GetLength(1);
        this.cellSize = cellSize;
        this.tilemap = tilemap;

        grid = new Cell[this.width, this.height];
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                grid[i, j] = new Cell(i * cellSize, j * cellSize, GetPrefabFromId(tilemap[i, j]));
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

    static UnityEngine.Object GetPrefabFromId(int id)
    {
        if (id == 1)
            return Resources.Load("Prefabs/Plane");
        if (id == 2)
            return Resources.Load("Prefabs/Wall");
        return null;
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
