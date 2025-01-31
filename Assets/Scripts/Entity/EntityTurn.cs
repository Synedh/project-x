﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTurn {

    int turn;
    EntityBehaviour entityBehaviour;
    Character character;

    public float maxTimebank;
    public float currentTimebank;

    public EntityTurn(EntityBehaviour entityBehaviour)
    {
        this.entityBehaviour = entityBehaviour;
        turn = 0;
        currentTimebank = 0;
    }

    public void BeginTurn()
    {
        // Select entity.
        character = entityBehaviour.character;
        GameManager.instance.currentEntityBehaviour = entityBehaviour;
        CameraManager.instance.LookAt(entityBehaviour.transform.position);
        turn++;
        maxTimebank = 
            20
            + character.stats[Characteristic.CurrentAP]
            + character.stats[Characteristic.CurrentMP];
        currentTimebank = maxTimebank;
        

        entityBehaviour.timelineEntity.SetColor(Color.red);


        // Résolution d'effets
        foreach (KeyValuePair<Effect, EntityBehaviour> effect in character.effects)
        {
            effect.Key.Resolve(effect.Value, entityBehaviour);
        }

        // Remove effects inflicted by him if no turn left
        foreach (EntityBehaviour entity in EntityManager.entities)
        {
            List<KeyValuePair<Effect, EntityBehaviour>> effects = entity.character.effects;

            for (int i = 0; i < effects.Count; ++i)
            {
                if (effects[i].Value == entityBehaviour
                    && effects[i].Key.effects.Count <= effects[i].Key.currentTurn)
                {
                    if (effects[i].Key.type == EffectType.Charac)
                    {
                        character.stats[effects[i].Key.effects[0].charac] -= 
                            effects[i].Key.effects[0].valueMin;
                    }
                    effects.RemoveAt(i--);
                }
            }
        }

        if (!entityBehaviour.isAlive)
            TurnManager.instance.Next();
    }
        
    public void EndTurn()
    {
        character = entityBehaviour.character;
        character.stats[Characteristic.CurrentAP] = character.stats[Characteristic.MaxAP];
        character.stats[Characteristic.CurrentMP] = character.stats[Characteristic.MaxMP];

        // Unselect entity.
        entityBehaviour.timelineEntity.SetColor(Color.black);
    }
}
