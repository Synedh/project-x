using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityTurn {

    int turn;
    GameObject entity;

    public EntityTurn(GameObject entity)
    {
        this.entity = entity;
        turn = 0;
    }

    public void Play()
    {
        turn++;
        EntityBehaviour.SelectEntity(entity);
    }
}
