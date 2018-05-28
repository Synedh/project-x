using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager {

    int turn;
    int entityTurn;
    List<EntityTurn> entityTurns;

    public TurnManager(List<GameObject> entities)
    {
        entityTurns = new List<EntityTurn>();
        foreach (GameObject entity in entities)
        {
            entityTurns.Add(new EntityTurn(entity));
        }
        turn = 0;
        entityTurn = 0;
    }

    public void Next()
    {
        try
        {
            entityTurns[entityTurn].Play();
            entityTurn++;
        }
        catch (ArgumentOutOfRangeException)
        {
            entityTurns[0].Play();
            entityTurn = 1;
            turn++;
        }
    }
}
