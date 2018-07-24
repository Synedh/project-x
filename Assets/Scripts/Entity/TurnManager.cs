using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager {

    int turn;
    int entityTurn;
    List<EntityTurn> entityTurns;
    Text turnCounter;

    public TurnManager(List<GameObject> entities)
    {
        entityTurns = new List<EntityTurn>();
        foreach (GameObject entity in entities)
        {
            entityTurns.Add(new EntityTurn(entity));
        }
        turn = 0;
        entityTurn = 0;
        turnCounter = GameObject.Find("TurnCounter").GetComponentInChildren<Text>();
        turnCounter.text = "1";
    }

    public void Next()
    {
        if (entityTurn != 0) // Si il ne s'agit pas du premier tour de match, finir le tour précédent.
            entityTurns[entityTurn - 1].EndTurn();
        if (entityTurn < entityTurns.Count)
        {
            entityTurns[entityTurn++].BeginTurn();
        }
        else // Nouveau tour de jeu
        {
            entityTurns[0].BeginTurn();
            entityTurn = 1;
            turn++;
            turnCounter.text = (turn + 1).ToString();
        }
    }
}
