using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBarBehaviour: MonoBehaviour {

    public GameObject spellIconPrefab;

    EntityBehaviour entityBehaviour;
    List<Spell> spells;
    public List<GameObject> spellIcons;

    public static SpellBarBehaviour instance;

    void Start() {
        instance = this;
        spells = new List<Spell>();
        spellIcons = new List<GameObject>();
    }

    void Update() {
        if (entityBehaviour != GameManager.instance.currentEntityBehaviour)
        {
            entityBehaviour = GameManager.instance.currentEntityBehaviour;
            spells = entityBehaviour.character.spells;

            foreach (GameObject spellIcon in spellIcons)
                Destroy(spellIcon);
            spellIcons.Clear();

            GameObject weaponIcon = Instantiate(spellIconPrefab, transform) as GameObject;
            weaponIcon.transform.position = new Vector3(
                weaponIcon.transform.position.x,
                weaponIcon.transform.position.y,
                weaponIcon.transform.position.z
            );
            weaponIcon.GetComponent<SpellIconBehaviour>().SetSpell(
                entityBehaviour.character.items[ItemType.Weapon].spell
            );
            spellIcons.Add(weaponIcon);

            if (spells != null)
            {
                for (int i = 0; i < spells.Count; ++i)
                {
                    GameObject spellIcon = Instantiate(spellIconPrefab, transform) as GameObject;
                    spellIcon.transform.position = new Vector3(
                        spellIcon.transform.position.x + 60 * (i + 1),
                        spellIcon.transform.position.y,
                        spellIcon.transform.position.z
                    );
                    spellIcon.GetComponent<SpellIconBehaviour>().SetSpell(spells[i]);
                    spellIcons.Add(spellIcon);
                }
            }
        }

        for (int i = 0; i < spellIcons.Count; ++i) // Deactivate non usable spells
        {
            if (i == 0 && entityBehaviour.character.stats[Characteristic.CurrentAP] < entityBehaviour.character.items[ItemType.Weapon].spell.cost
                || i > 0 && entityBehaviour.character.stats[Characteristic.CurrentAP] < spells[i - 1].cost)
            {
                spellIcons[i].GetComponent<Button>().interactable = false;
            }
        }
    }
}

