using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTurn {

    int turn;
    Character character;
    GameObject entity;

    public EntityTurn(GameObject entity)
    {
        this.entity = entity;
        turn = 0;
    }

	public void BeginTurn()
    {
        // Select entity.
        character = entity.GetComponent<EntityBehaviour>().character;
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
            character = entity.GetComponent<EntityBehaviour>().character;
            List<KeyValuePair<Effect, EntityBehaviour>> effects = character.effects;

            for (int i = 0; i < effects.Count; ++i)
            {
                if (effects[i].Value == this.entity.GetComponent<EntityBehaviour>()
                    && effects[i].Key.effects.Count <= effects[i].Key.currentTurn)
                {
                    if (effects[i].Key.type == EffectType.Charac)
                        entity.GetComponent<EntityBehaviour>().character.stats[effects[i].Key.effects[0].charac] -= effects[i].Key.effects[0].valueMin;
                    effects.RemoveAt(i--);
                }
            }
        }
	}
		
	public void EndTurn()
    {
        character = entity.GetComponent<EntityBehaviour>().character;
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
