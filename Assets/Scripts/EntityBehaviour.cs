﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehaviour : MonoBehaviour {

    float speed;
    public Material selected;
    public Material unselected;
    public Material hoverSelected;
    public Material hoverUnselected;
    BoxCollider boxCollider;

    List<Vector3> targets;
    
	void Start () {
        speed = 3f;
        targets = new List<Vector3>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
	}
	
	void Update () {
        // Move
        if (targets.Count != 0)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targets[0], step);
            if (transform.position.Equals(targets[0]))
                targets.RemoveAt(0);

        }
	}

    public void OnMouseDown()
    {
        // selectEntity(gameObject);
    }

    public void Move(CustomGrid grid, Cell target)
    {
        // Fill a targets list of point. Then this list is parsed by Update function.
        // Once a location is reached, element is remove and next one is targeted.
        // PathFinding module is used to search best way.
        bool[,] tilesmap = grid.getWalkableGrid();

        NesScripts.Controls.PathFind.Grid pathhGrid = new NesScripts.Controls.PathFind.Grid(tilesmap);
        NesScripts.Controls.PathFind.Point _from;
        if (targets.Count == 0)
        {
            _from = new NesScripts.Controls.PathFind.Point((int)transform.position.x, (int)transform.position.z);
        }
        else
        {
            _from = new NesScripts.Controls.PathFind.Point((int)targets[targets.Count - 1].x, (int)targets[targets.Count - 1].z);
        }
        NesScripts.Controls.PathFind.Point _to = new NesScripts.Controls.PathFind.Point(target.x, target.y);

        List<NesScripts.Controls.PathFind.Point> path = NesScripts.Controls.PathFind.Pathfinding.FindPath(pathhGrid, _from, _to, NesScripts.Controls.PathFind.Pathfinding.DistanceType.Manhattan);

        foreach (NesScripts.Controls.PathFind.Point point in path)
        {
            targets.Add(new Vector3(point.x, transform.position.y, point.y));
        }
    }

    public void SetToCell(int x, int y)
    {
        this.transform.position = new Vector3(x, 0, y);
    }

    public static GameObject loadEntity(UnityEngine.Object prefab, Vector2 pos, Quaternion rot)
    {
        GameObject entity = Instantiate(prefab, new Vector3(pos.x, 0, pos.y), rot) as GameObject;
        GameManager.instance.entities.Add(entity);
        return GameManager.instance.grid.addEntity(entity);
    }


    public static void selectEntity(GameObject entity)
    {
        GameManager.instance.selectedEntity = entity;
        foreach (GameObject tmpEntity in GameManager.instance.entities)
        {
            foreach (MeshRenderer renderer in tmpEntity.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = tmpEntity.GetComponent<EntityBehaviour>().unselected;
            }
        }
        foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = entity.GetComponent<EntityBehaviour>().selected;
        }
    }
}
