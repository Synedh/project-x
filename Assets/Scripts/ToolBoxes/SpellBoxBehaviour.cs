using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBoxBehaviour : MonoBehaviour {

	Spell _spell;

	void Start() {
	}

	void Update() {
        transform.position = Input.mousePosition;
	}

    public void SetSpell(Spell spell) {
        _spell = spell;
        transform.Find("SpellName").GetComponent<Text>().text = spell.name.ToUpperInvariant();
        transform.Find("Price").GetComponent<Text>().text = spell.price + " G";
        transform.Find("Description").GetComponent<Text>().text = spell.description;
        for (int i = 0; i < spell.effects.Count; ++i)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, GetComponent<RectTransform>().sizeDelta.y + 18);
            GameObject effectText = Instantiate(Resources.Load("Prefabs/UI/BoxTextLine"), transform.Find("EffectBox")) as GameObject;
            effectText.transform.position = new Vector3(effectText.transform.position.x, effectText.transform.position.y - i * 18, effectText.transform.position.z);;
            effectText.GetComponent<Text>().text = spell.effects[i].description;
        }
	}
}


