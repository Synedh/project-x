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

	// Use this for initialization
	void Start () {
        if (!isWalkable) // Correction hauteur block temporaire
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
		cellColor = null;
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
				if (!GameManager.instance.selectedEntity.GetComponent<EntityBehaviour>().DoMove)
					GameManager.instance.selectedEntity.GetComponent<EntityBehaviour>().SetMoveTargets(grid, cell);
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
    }

    void OnMouseDown()
    {
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			GameObject entity = IsThereAnEntity ();
			if (entity)
				entity.GetComponent<EntityBehaviour>().MouseDown();
			else
				GameManager.instance.selectedEntity.GetComponent<EntityBehaviour>().DoMove = true;
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
