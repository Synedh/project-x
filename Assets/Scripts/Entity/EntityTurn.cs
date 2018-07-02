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

    public void Play()
    {
        turn++;
		BeginTurn();

        EntityBehaviour.SelectEntity(entity);
    }

	void BeginTurn() {
		// TODO : Use one effect.
	}
		
	public void EndTurn() {
		character.Stats[Characteristic.CurrentAP] = character.Stats[Characteristic.MaxAP];
		character.Stats[Characteristic.CurrentMP] = character.Stats[Characteristic.MaxMP];

		// TODO : Remove one turn to effects inflicted by him
	}
}
