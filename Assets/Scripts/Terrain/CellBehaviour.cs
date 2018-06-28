using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellBehaviour : MonoBehaviour {

    public bool isWalkable;
    public bool hideView;

    public Cell cell;
    CustomGrid grid;

	// Use this for initialization
	void Start () {
        if (!isWalkable) // Correction hauteur block temporaire
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.25f, this.transform.position.z);
        grid = GameObject.Find("Grid").GetComponent<CustomGrid>();
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

    void OnMouseOver()
	{
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			GameObject entity = IsThereAnEntity ();
			if (entity && GameManager.instance.selectedEntity == entity) {
				foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>()) {
					renderer.material = entity.GetComponent<EntityBehaviour> ().hoverSelected;
				}
			} else if (entity) {
				foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>()) {
					renderer.material = entity.GetComponent<EntityBehaviour> ().hoverUnselected;
				}
			}
		} else {
			OnMouseExit ();
		}
    }

    private void OnMouseExit()
	{
		GameObject entity = IsThereAnEntity ();
		if (entity && GameManager.instance.selectedEntity == entity) {
			foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>()) {
				renderer.material = entity.GetComponent<EntityBehaviour> ().selected;
			}
		} else if (entity) {
			foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>()) {
				renderer.material = entity.GetComponent<EntityBehaviour> ().unselected;
			}
		}
    }

    void OnMouseDown()
    {
		if (!EventSystem.current.IsPointerOverGameObject ()) {
			GameObject entity = IsThereAnEntity ();
			if (entity)
				Debug.Log (entity);
			else
				GameManager.instance.selectedEntity.GetComponent<EntityBehaviour> ().SetMoveTargets (grid, cell);
		}
    }
}
