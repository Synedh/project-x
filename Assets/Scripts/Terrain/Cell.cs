using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

    public int id, x, y;
    public Object prefab;

    public GameObject cellObject;

    public Cell(int x, int y, Object prefab)
    {
        this.x = x;
        this.y = y;
        this.prefab = prefab;
    }

    public GameObject Draw()
    {
        GameObject grid = GameObject.Find("Grid");
        if (this.prefab) {
            cellObject = Object.Instantiate(this.prefab, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
            cellObject.transform.parent = grid.transform;
            cellObject.GetComponent<CellBehaviour>().cell = this;
            return cellObject;
        }
        return null;
    }
}
