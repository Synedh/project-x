using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager {

    int turn;
    int entityTurn;
    TimelineBehaviour timelineBehaviour;
    Text turnCounter;

    public TurnManager(TimelineBehaviour timelineBehaviour, Text turnCounter)
    {
        turn = 0;
        entityTurn = 0;
        this.timelineBehaviour = timelineBehaviour;
        this.turnCounter = turnCounter;
        turnCounter.text = "1";
    }

    public void Next()
    {
        if (entityTurn != 0) // Si il ne s'agit pas du premier tour de match, finir le tour précédent.
            timelineBehaviour.entities[entityTurn - 1].entityTurn.EndTurn();
        if (entityTurn < timelineBehaviour.entities.Count)
        {
            timelineBehaviour.entities[entityTurn++].entityTurn.BeginTurn();
        }
        else // Nouveau tour de jeu
        {
            timelineBehaviour.entities[0].entityTurn.BeginTurn();
            entityTurn = 1;
            turn++;
            turnCounter.text = (turn + 1).ToString();
        }
    }
}
