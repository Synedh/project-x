using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager: MonoBehaviour
{
    public TimelineBehaviour timelineBehaviour;
    public Text turnCounter;
    public RectTransform timebankFullBar;
    public RectTransform timebankEmptyBar;
    public Text nextButtonText;

    int turn;
    int entityTurn;

    public EntityBehaviour currentEntityBehaviour;

    public static TurnManager instance;

    void Start()
    {
        turn = 0;   
        entityTurn = 0;
        turnCounter.text = "1"; 
        instance = this;
    }

    void Update() {
        if (currentEntityBehaviour == null
            || currentEntityBehaviour.entityTurn.currentTimebank < 1)
        {
            Next();
            timebankFullBar.offsetMin = new Vector2(0, timebankFullBar.offsetMin.y);
        }
        else
        {
            currentEntityBehaviour.entityTurn.currentTimebank -= Time.deltaTime;
            if (currentEntityBehaviour.entityTurn.currentTimebank < 11)
            {
                nextButtonText.text = ((int)currentEntityBehaviour.entityTurn.currentTimebank).ToString();
            }
            timebankFullBar.offsetMin = new Vector2(
                timebankEmptyBar.rect.width * (1 - ((currentEntityBehaviour.entityTurn.currentTimebank - 1) / (currentEntityBehaviour.entityTurn.maxTimebank - 1))),
                timebankFullBar.offsetMin.y
            );
        }
    }

    public void Next()
    {
        if (currentEntityBehaviour != null) // Si il ne s'agit pas du premier tour de match, finir le tour précédent.
            currentEntityBehaviour.entityTurn.EndTurn();
        if (entityTurn < timelineBehaviour.entities.Count)
        {
            currentEntityBehaviour = timelineBehaviour.entities[entityTurn++];
        }
        else // Nouveau tour de jeu
        {
            entityTurn = 1;
            turn++;
            turnCounter.text = (turn + 1).ToString();
            currentEntityBehaviour = timelineBehaviour.entities[0];
        }
        nextButtonText.text = "NEXT";
        currentEntityBehaviour.entityTurn.BeginTurn();
    }
}
