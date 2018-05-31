using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTurn {

    int turn;
    GameObject entity;

    public EntityTurn(GameObject entity)
    {
        this.entity = entity;
    }

    public void Play()
    {
        EntityBehaviour.selectEntity(entity);
    }
}
