using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimelineDetailBoxSpell : MonoBehaviour {
    public GameObject spellBoxPrefab;

    Spell spell;
    GameObject spellBox;

    void Start() {
        // /!\ Called after SetSpell() /!\
    }

    void Update() {
    }

    public void SetSpell(Spell spell) {
        this.spell = spell;
        GetComponentInChildren<Text>().text = spell.name;
    }

    public void OnEnter() {
        if (spell != null) {
            spellBox = Instantiate(
                spellBoxPrefab,
                GameObject.Find("ToolBoxesContainer").transform
            ) as GameObject;
            spellBox.GetComponent<SpellBoxBehaviour>().SetSpell(spell);
        }
    }

    public void OnExit() {
        Destroy(spellBox);
    }
}

