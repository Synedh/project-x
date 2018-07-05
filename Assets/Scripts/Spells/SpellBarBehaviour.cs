using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBarBehaviour: MonoBehaviour {

    public GameObject spellIconPrefab;

    GameObject currentEntity;
    List<Spell> spells;
    List<GameObject> spellIcons;

    void Start() {
        spellIcons = new List<GameObject>();
    }

    void Update() {
        if (currentEntity != GameManager.instance.currentEntity)
        {
            currentEntity = GameManager.instance.currentEntity;
            spells = currentEntity.GetComponent<EntityBehaviour>().character.spells;

            foreach (GameObject spellIcon in spellIcons)
                DestroyImmediate(spellIcon);
            spellIcons.Clear();

            if (spells != null)
            {
                for (int i = 0; i < spells.Count; ++i)
                {
                    GameObject spellIcon = Instantiate(spellIconPrefab, transform) as GameObject;
                    spellIcon.transform.position = new Vector3(spellIcon.transform.position.x + 60 * i, spellIcon.transform.position.y, spellIcon.transform.position.z);
                    spellIcon.GetComponent<SpellIconBehaviour>().SetSpell(spells[i]);
                    spellIcons.Add(spellIcon);
                }
            }
        }
    }
}

