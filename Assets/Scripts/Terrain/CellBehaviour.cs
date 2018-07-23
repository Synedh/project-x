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
    List<Material> cellsColor;
    CustomGrid grid;
    List<Vector2> coloredCells;

	// Use this for initialization
	void Start () {
        if (!isWalkable) // Correction hauteur block temporaire
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
        cellsColor = new List<Material> { grid.defaultColor };
        coloredCells = new List<Vector2>();

        x = (int)transform.position.x;
        y = (int)transform.position.z;
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
            EntityBehaviour currentEntityBehaviour = GameManager.instance.currentEntity.GetComponent<EntityBehaviour>();
            if (GameManager.instance.selectedSpell != null)
            {
                if (cellsColor[ cellsColor.Count - 1] == grid.reachableSpellRange)
                {
                    foreach (Vector2 cell in currentEntityBehaviour.GetAoeOfSpell(GameManager.instance.selectedSpell, new Vector2(x, y)))
                    {
                        CellBehaviour cellBehaviour = grid.GetCellBehaviour(x + (int)cell.x, y + (int)cell.y);
                        if (cellBehaviour != null && cellBehaviour.isWalkable)
                        {
                            coloredCells.Add(new Vector2(x + (int)cell.x, y + (int)cell.y));
                            cellBehaviour.colorCell(grid.aoeSpellRange);
                        }
                    }
                }   
            } else if (entity) {
                entity.GetComponent<EntityBehaviour>().MouseEnter();
			} else {
                if (!GameManager.instance.currentEntity.GetComponent<EntityBehaviour>().doMove)
                {
                    List<Vector2> path = currentEntityBehaviour.SetMoveTargets(cell);
                    if (path.Count <= currentEntityBehaviour.character.stats[Characteristic.CurrentMP])
                    {
                        foreach (Vector2 cell in path)
                        {
                            coloredCells.Add(new Vector2(cell.x, cell.y));
                            grid.GetCellBehaviour((int)cell.x, (int)cell.y).colorCell(grid.pathMP);
                        }
                    }
                }
			}
		} else {
			OnMouseExit();
		}
    }

    void OnMouseExit()
	{
		GameObject entity = IsThereAnEntity ();
        if (entity && GameManager.instance.selectedSpell == null) {
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
            GameObject entity = IsThereAnEntity();
            EntityBehaviour currentEntityBehaviour = GameManager.instance.currentEntity.GetComponent<EntityBehaviour>();
            if (GameManager.instance.selectedSpell != null && cellsColor[cellsColor.Count - 1] == grid.aoeSpellRange)
            {
                GameManager.instance.selectedSpell.Apply(currentEntityBehaviour, new Vector2(x, y));
            }
            else if (entity)
            {
                entity.GetComponent<EntityBehaviour>().MouseDown();
            }
            else
            {
                grid.CleanCells(coloredCells);
                currentEntityBehaviour.doMove = true;
            }
		}
    }

    public void colorCell(Material material) {
        cellsColor.Add(material);

        foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = material;
        }
	}

	public void removeColorCell() {
        if (cellsColor.Count > 1)
        {
            cellsColor.RemoveAt(cellsColor.Count - 1);

            foreach (MeshRenderer renderer in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                renderer.material = cellsColor[cellsColor.Count - 1];
            }
        }
	}
}
