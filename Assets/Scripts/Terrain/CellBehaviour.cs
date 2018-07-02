using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellBehaviour : MonoBehaviour {

    public bool isWalkable;
    public bool hideView;

    public Cell cell;
	GameObject cellColor;
    CustomGrid grid;
    List<Vector2> coloredCells;

	// Use this for initialization
	void Start () {
        if (!isWalkable) // Correction hauteur block temporaire
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
		cellColor = null;
        coloredCells = new List<Vector2>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject IsThereAnEntity()
    {
        foreach (GameObject entity in GameManager.instance.entities)
        {
            if (entity.transform.position == transform.position)
            {
                return entity;
            }
        }
        return null;
    }

    void OnMouseEnter()
	{
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			GameObject entity = IsThereAnEntity ();
			if (entity) {
				entity.GetComponent<EntityBehaviour> ().MouseEnter ();
			} else {
                if (!GameManager.instance.selectedEntity.GetComponent<EntityBehaviour>().doMove)
                {
                    EntityBehaviour entityBehaviour = GameManager.instance.selectedEntity.GetComponent<EntityBehaviour>();
                    List<Vector2> path = entityBehaviour.SetMoveTargets(grid, cell);
                    if (path.Count <= entityBehaviour.character.Stats[Characteristic.CurrentMP])
                    {
                        foreach (Vector2 cell in path)
                        {
                            coloredCells.Add(new Vector2(cell.x, cell.y));
                            grid.GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().colorCell(new Color(0.2f, 0.45f, 0.2f, 1f));
                        }
                    }
                }
			}
		} else {
			OnMouseExit ();
		}
    }

    void OnMouseExit()
	{
		GameObject entity = IsThereAnEntity ();
		if (entity) {
			entity.GetComponent<EntityBehaviour>().MouseExit();
		}
        if (coloredCells.Count > 0)
        {
            grid.CleanCells(coloredCells);
            coloredCells.Clear();
        }
    }

    void OnMouseDown()
    {
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			GameObject entity = IsThereAnEntity ();
			if (entity)
				entity.GetComponent<EntityBehaviour>().MouseDown();
			else
				GameManager.instance.selectedEntity.GetComponent<EntityBehaviour>().doMove = true;
		}
    }

	public void colorCell(Color color) {
		if (cellColor != null)
			removeColorCell ();
		cellColor = Instantiate(Resources.Load("Prefabs/CellColor"), transform) as GameObject;
		cellColor.GetComponent<SpriteRenderer> ().color = color;// new Color(0f, 1f, 0.2f, 0.5f);
	}

	public void removeColorCell() {
		DestroyImmediate(cellColor);
        cellColor = null;
	}
}
