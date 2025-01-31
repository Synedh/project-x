﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellIconBehaviour: MonoBehaviour
{
    public GameObject spellBoxPrefab;

    Spell spell;
    CustomGrid grid;
    GameObject spellBox;
    List<Vector2> reachableCells;
    List<Vector2> unreachableCells;

    void Start()
    {
        grid = CustomGrid.instance;
        reachableCells = new List<Vector2>();
        unreachableCells = new List<Vector2>();
    }

    void Update()
    {
        if ((reachableCells.Count > 0 || reachableCells.Count > 0) && Input.GetMouseButtonDown(0))
        {
            UnselectSpell();
        }
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        GetComponentInChildren<Text>().text = spell.name;
    }

    public void UnselectSpell()
    {
        GameManager.instance.selectedSpell = null;
        grid.CleanCells(reachableCells);
        grid.CleanCells(unreachableCells);
        reachableCells.Clear();
        unreachableCells.Clear();
    }

    public void OnClick()
    {
        if (GameManager.instance.selectedSpell != null)
        {
            foreach (GameObject spellIcon in SpellBarBehaviour.instance.spellIcons)
            {
                spellIcon.GetComponent<SpellIconBehaviour>().UnselectSpell();
            }
        }
        GameManager.instance.selectedSpell = spell;
        EntityBehaviour entityBehavior = GameManager.instance.currentEntityBehaviour;
        int posX = 0;
        int posY = 0;

        if (entityBehavior.doMove)
        {
            posX = (int)entityBehavior.moveTargets[entityBehavior.moveTargets.Count - 1].x;
            posY = (int)entityBehavior.moveTargets[entityBehavior.moveTargets.Count - 1].y;
        }
        else if (entityBehavior.pushTargets.Count > 0)
        {
            posX = (int)entityBehavior.pushTargets[entityBehavior.pushTargets.Count - 1].x;
            posY = (int)entityBehavior.pushTargets[entityBehavior.pushTargets.Count - 1].y;
        }
        else
        {
            posX = entityBehavior.x;
            posY = entityBehavior.y;
        }

        List<Vector2>[] spellRange = grid.SpellRange(posX, posY, spell.rangeMin, spell.rangeMax, spell.rangeType, spell.needView, spell.entityRequirement);
        reachableCells = spellRange[0];
        unreachableCells = spellRange[1];
        grid.ColorCells(reachableCells, grid.reachableSpellRange);
        grid.ColorCells(unreachableCells, grid.unreachableSpellRange);
    }

    public void OnEnter()
    {
        if (spell != null)
        {
            spellBox = Instantiate(spellBoxPrefab, GameObject.Find("ToolBoxesContainer").transform) as GameObject;
            spellBox.GetComponent<SpellBoxBehaviour>().SetSpell(spell);
        }
    }

    public void OnExit()
    {
        Destroy(spellBox);
    }
}

