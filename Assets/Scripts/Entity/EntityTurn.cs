using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTurn {

    int turn;
    GameObject entity;
	Character character;

    public EntityTurn(GameObject entity)
    {
        this.entity = entity;
		character = entity.GetComponent<EntityBehaviour>().character;
        turn = 0;
    }

	public void BeginTurn() {
        // Select entity.
        GameManager.instance.currentEntity = entity;
        CameraManager.instance.lookAt = entity.transform;
        foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = entity.GetComponent<EntityBehaviour>().selected;
        }
        entity.GetComponent<EntityBehaviour>().timelineEntity.SetColor(Color.red);

        turn++;

		// TODO : Use one effect.
	}
		
	public void EndTurn() {
		character.stats[Characteristic.CurrentAP] = character.stats[Characteristic.MaxAP];
		character.stats[Characteristic.CurrentMP] = character.stats[Characteristic.MaxMP];

		// TODO : Remove one turn to effects inflicted by him

        // Unselect entity.
        foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = entity.GetComponent<EntityBehaviour>().unselected;
        }
        entity.GetComponent<EntityBehaviour>().timelineEntity.SetColor(Color.black);
	}
}
