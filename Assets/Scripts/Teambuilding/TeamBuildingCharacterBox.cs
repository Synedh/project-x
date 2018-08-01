using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamBuildingCharacterBox : MonoBehaviour {

    public GameObject characterName;
    public GameObject characterImage;
    public GameObject necklace;
    public GameObject bracelet;
    public GameObject ring;
    public GameObject weapon;

    public GameObject spellContainer;
    public GameObject spellBoxPrefab;

    Character character;
    List<TeamBuildingSpellBox> spellBoxes;

    // Use this for initialization
    void Start () {
        spellBoxes = new List<TeamBuildingSpellBox>();
        characterName.GetComponentInChildren<Text>().text = character.nickname;
        SetItem(necklace, character.items[ItemType.Necklace]);
        SetItem(bracelet, character.items[ItemType.Bracelet]);
        SetItem(ring, character.items[ItemType.Ring]);
        SetItem(weapon, character.items[ItemType.Weapon]);
        foreach (Spell spell in character.spells)
        {
            AddSpell(spell);
        }
    }
    
    // Update is called once per frame
    void Update () {
        
    }

    public void SetCharacter(Character character) {
        this.character = character;
    }

    public void SetItem(GameObject itemObject, Item item)
    {
        itemObject.GetComponent<TeamBuildingItemBox>().SetItem(item);
    }

    public void AddSpell(Spell spell) {
        GameObject spellObject = Instantiate(spellBoxPrefab, spellContainer.transform) as GameObject;
        TeamBuildingSpellBox spellBox = spellObject.GetComponent<TeamBuildingSpellBox>();
        spellBox.SetSpell(spell);
    }
}
