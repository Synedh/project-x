using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellBehaviour : MonoBehaviour {

    public bool isWalkable;
    public bool hideView;

    public int x;
    public int y;

    public Cell cell;
	List<GameObject> cellsColor;
    CustomGrid grid;
    List<Vector2> coloredCells;

	// Use this for initialization
	void Start () {
        if (!isWalkable) // Correction hauteur block temporaire
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
        cellsColor = new List<GameObject>();
        coloredCells = new List<Vector2>();

        x = (int)transform.position.x;
        y = (int)transform.position.z;
	}
	
	// Update is called once per frame
    void Update () {

        x = (int)transform.position.x;
        y = (int)transform.position.z;		
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
            if (entity)
                entity.GetComponent<EntityBehaviour>().MouseEnter();
            
            if (GameManager.instance.selectedSpell != null)
            {
                Spell currentSpell = GameManager.instance.selectedSpell;
                EntityBehaviour currentEntityBehaviour = GameManager.instance.currentEntity.GetComponent<EntityBehaviour>();
                float range = Mathf.Abs(x - currentEntityBehaviour.x) + Mathf.Abs(y - currentEntityBehaviour.y);
                if (currentSpell.rangeMin <= range && range <= currentSpell.rangeMax
                    && grid.Visibility(new Vector2(currentEntityBehaviour.x, currentEntityBehaviour.y), new Vector2(x, y)))
                {
                    coloredCells.Add(new Vector2(x, y));
                    grid.GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().colorCell(Color.red);
                }   
			} else {
                if (!GameManager.instance.currentEntity.GetComponent<EntityBehaviour>().doMove)
                {
                    EntityBehaviour entityBehaviour = GameManager.instance.currentEntity.GetComponent<EntityBehaviour>();
                    List<Vector2> path = entityBehaviour.SetMoveTargets(grid, cell);
                    if (path.Count <= entityBehaviour.character.stats[Characteristic.CurrentMP])
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
        if (GameManager.instance.selectedSpell != null)
        {
            Spell currentSpell = GameManager.instance.selectedSpell;
            EntityBehaviour currentEntityBehaviour = GameManager.instance.currentEntity.GetComponent<EntityBehaviour>();
            float range = Mathf.Abs(x - currentEntityBehaviour.x) + Mathf.Abs(y - currentEntityBehaviour.y);
            if (currentSpell.rangeMin <= range && range <= currentSpell.rangeMax
                && grid.Visibility(new Vector2(currentEntityBehaviour.x, currentEntityBehaviour.y), new Vector2(x, y)))
            {
                coloredCells.Add(new Vector2(x, y));
                grid.GetCellObject((int)cell.x, (int)cell.y).GetComponent<CellBehaviour>().colorCell(Color.red);
            }   
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
				GameManager.instance.currentEntity.GetComponent<EntityBehaviour>().doMove = true;
		}
    }

	public void colorCell(Color color) {
        cellsColor.Add(Instantiate(Resources.Load("Prefabs/Game/CellColor"), transform) as GameObject);
        cellsColor[cellsColor.Count - 1].GetComponent<SpriteRenderer>().color = color;
	}

	public void removeColorCell() {
        DestroyImmediate(cellsColor[cellsColor.Count - 1]);
        cellsColor.RemoveAt(cellsColor.Count - 1);
	}
}
