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

    public Transform Draw()
    {
        if (this.prefab) {
            cellObject = Object.Instantiate(this.prefab, new Vector3(x, 0, y), Quaternion.identity) as GameObject;
            cellObject.GetComponent<CellBehaviour>().cell = this;
            return cellObject.transform;
        }
        return null;
    }
}
