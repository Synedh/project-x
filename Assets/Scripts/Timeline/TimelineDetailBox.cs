using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDetailBox : MonoBehaviour {

	EntityBehaviour entity;
	Text nickname;
	Dictionary<Characteristic, Text> textCaracts;
	Dictionary<ItemType, TimelineDetailBoxItem> boxItems;
    List<GameObject> boxSpells;

    Vector2 oldMousePosition;

	void Awake () {
		entity = null;
		nickname = gameObject.transform.Find("Nickname").gameObject.GetComponent<Text>();
		textCaracts = new Dictionary<Characteristic, Text>() {
			{ Characteristic.CurrentHP, transform.Find("HP_content").gameObject.GetComponent<Text>() },
			{ Characteristic.CurrentAP, transform.Find("AP_content").gameObject.GetComponent<Text>() },
			{ Characteristic.CurrentMP, transform.Find("MP_content").gameObject.GetComponent<Text>() },
			{ Characteristic.ContactDamage, transform.Find("Contact_content").gameObject.GetComponent<Text>() },
			{ Characteristic.DistantDamage, transform.Find("Distant_content").gameObject.GetComponent<Text>() },
			{ Characteristic.PhysicalDamage, transform.Find("Physical_content").gameObject.GetComponent<Text>() },
			{ Characteristic.MagicDamage, transform.Find("Magic_content").gameObject.GetComponent<Text>() },
			{ Characteristic.Speed, transform.Find("Speed_content").gameObject.GetComponent<Text>() }
		};
		boxItems = new Dictionary<ItemType, TimelineDetailBoxItem> () {
			{ ItemType.Necklace, transform.Find("Items/Necklace").gameObject.GetComponent<TimelineDetailBoxItem>() },
			{ ItemType.Bracelet, transform.Find("Items/Bracelet").gameObject.GetComponent<TimelineDetailBoxItem>() },
			{ ItemType.Ring, transform.Find("Items/Ring").gameObject.GetComponent<TimelineDetailBoxItem>() },
			{ ItemType.Weapon, transform.Find("Items/Weapon").gameObject.GetComponent<TimelineDetailBoxItem>() }
		};
        boxSpells = new List<GameObject>();
	}

	void Update() {
		if (entity != null)
            UpdateStats();
	}

	public void UpdateStats() {
		Dictionary<Characteristic, float> stats = entity.character.stats;

		textCaracts[Characteristic.CurrentHP].text = stats[Characteristic.CurrentHP] + " / " + stats[Characteristic.MaxHP];
		textCaracts[Characteristic.CurrentAP].text = stats[Characteristic.CurrentAP] + " / " + stats[Characteristic.MaxAP];
		textCaracts[Characteristic.CurrentMP].text = stats[Characteristic.CurrentMP] + " / " + stats[Characteristic.MaxMP];
		textCaracts[Characteristic.ContactDamage].text = (stats[Characteristic.ContactDamage] * 100) + "% - " + (stats[Characteristic.ContactResistance] * 100) + "%";
		textCaracts[Characteristic.DistantDamage].text = (stats[Characteristic.DistantDamage] * 100) + "% - " + (stats[Characteristic.DistantResistance] * 100) + "%";
		textCaracts[Characteristic.PhysicalDamage].text = (stats[Characteristic.PhysicalDamage] * 100) + "% - " + (stats[Characteristic.PhysicalResistance] * 100) + "%";
		textCaracts[Characteristic.MagicDamage].text = (stats[Characteristic.MagicDamage] * 100) + "% - " + (stats[Characteristic.MagicResistance] * 100) + "%";
		textCaracts[Characteristic.Speed].text = string.Format("{0}", stats[Characteristic.Speed]);
	}

    void SetItems() 
    {
        Dictionary<ItemType, Item> items = entity.character.items;
        if (items[ItemType.Necklace] != null) { boxItems[ItemType.Necklace].SetItem(items[ItemType.Necklace]); } 
        if (items[ItemType.Bracelet] != null) { boxItems[ItemType.Bracelet].SetItem(items[ItemType.Bracelet]); }
        if (items[ItemType.Ring] != null) { boxItems[ItemType.Ring].SetItem(items[ItemType.Ring]); }
        if (items[ItemType.Weapon] != null) { boxItems[ItemType.Weapon].SetItem(items[ItemType.Weapon]); }
    }

    void SetSpells() 
    {
        List<Spell> spells = entity.character.spells;
        if (boxSpells != null)
        {
            for (int i = 0; i < spells.Count; ++i)
            {
                GameObject spellBox = Instantiate(Resources.Load("Prefabs/UI/DetailSpell"), transform.Find("Spells")) as GameObject;
                spellBox.transform.position = new Vector3(spellBox.transform.position.x, spellBox.transform.position.y - i * 60, spellBox.transform.position.z);
                spellBox.GetComponent<TimelineDetailBoxSpell>().SetSpell(spells[i]);
            }
        }
    }

    public void OnClick() {
        Destroy(gameObject);
    }

    public void OnDrag() {
        if (oldMousePosition != new Vector2(0, 0))
            transform.position = new Vector3(transform.position.x - oldMousePosition.x + Input.mousePosition.x, 
                                             transform.position.y - oldMousePosition.y + Input.mousePosition.y,
                                             transform.position.z);
        oldMousePosition = Input.mousePosition;
    }

	public void SetEntity (EntityBehaviour entity) {
		this.entity = entity;

        nickname.text = entity.character.nickname.ToUpper();
        UpdateStats();
        SetItems();
        SetSpells();
    }
}

