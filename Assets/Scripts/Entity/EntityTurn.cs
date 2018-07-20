﻿using System.Collections;
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

        // Résolution d'effets
        foreach (KeyValuePair<Effect, EntityBehaviour> effect in character.effects)
        {
            effect.Key.Resolve(effect.Value, entity.GetComponent<EntityBehaviour>());
        }

        // Remove effects inflicted by him if no turn left
        foreach (GameObject entity in GameManager.instance.entities)
        {
            List<KeyValuePair<Effect, EntityBehaviour>> effects = entity.GetComponent<EntityBehaviour>().character.effects;
            for (int i = 0; i < effects.Count; ++i) {
                if (effects[i].Value == this.entity.GetComponent<EntityBehaviour>()
                    && effects[i].Key.effects.Count <= effects[i].Key.currentTurn)
                {
                    effects.RemoveAt(i--);
                }
            }
        }
        // TODO : Remove one turn to effects inflicted by him
	}
		
	public void EndTurn() {
		character.stats[Characteristic.CurrentAP] = character.stats[Characteristic.MaxAP];
		character.stats[Characteristic.CurrentMP] = character.stats[Characteristic.MaxMP];

        // Unselect entity.
        foreach (MeshRenderer renderer in entity.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.material = entity.GetComponent<EntityBehaviour>().unselected;
        }
        entity.GetComponent<EntityBehaviour>().timelineEntity.SetColor(Color.black);
	}
}
