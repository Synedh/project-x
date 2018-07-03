using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimelineDetailBoxSpell : MonoBehaviour {

	Spell _spell;
	GameObject spellBox;

	void Start() {
        // /!\ Called after SetSpell() /!\
	}

	void Update() {
	}

    public void SetSpell(Spell spell) {
        _spell = spell;
        GetComponentInChildren<Text>().text = spell.name;
	}

	public void OnEnter() {
        if (_spell != null) {
            spellBox = Instantiate(Resources.Load("Prefabs/UI/SpellBox"), GameObject.Find("ToolBoxes").transform) as GameObject;
            spellBox.transform.position = Input.mousePosition;
            spellBox.GetComponent<SpellBoxBehaviour>().SetSpell(_spell);
		}
	}

	public void OnExit() {
        DestroyImmediate(spellBox);
	}
}

