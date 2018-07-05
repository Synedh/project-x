using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellIconBehaviour: MonoBehaviour {
    public GameObject spellBoxPrefab;

    Spell spell;
    CustomGrid grid;
    GameObject spellBox;
    List<Vector2> reachableCells;
    List<Vector2> unreachableCells;

    void Start() {
        grid = GameManager.instance.grid;
        reachableCells = new List<Vector2>();
        unreachableCells = new List<Vector2>();
    }

    void Update() {
        if ((reachableCells.Count > 0 || reachableCells.Count > 0) && Input.GetMouseButtonDown(0))
        {
            UnselectSpell();
        }
    }

    public void SetSpell(Spell spell) {
        this.spell = spell;
        GetComponentInChildren<Text>().text = spell.name;
    }

    void UnselectSpell() {
        grid.CleanCells(reachableCells);
        grid.CleanCells(unreachableCells);
        reachableCells.Clear();
        unreachableCells.Clear();
    }
        
    public void OnClick() {
        EntityBehaviour entityBehavior = GameManager.instance.currentEntity.GetComponent<EntityBehaviour>();
        reachableCells = grid.SpellRange(entityBehavior.x, entityBehavior.y, spell.rangeMin, spell.rangeMax)[0];
        unreachableCells = grid.SpellRange(entityBehavior.x, entityBehavior.y, spell.rangeMin, spell.rangeMax)[1];
        grid.ColorCells(reachableCells, new Color(0, 0, 1, 1));
        grid.ColorCells(unreachableCells, Color.cyan);
    }

    public void OnEnter() {
        if (spell != null) {
            spellBox = Instantiate(spellBoxPrefab, GameObject.Find("ToolBoxesContainer").transform) as GameObject;
            spellBox.GetComponent<SpellBoxBehaviour>().SetSpell(spell);
        }
    }

    public void OnExit() {
        DestroyImmediate(spellBox);
    }
}

