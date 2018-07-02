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
	List<Spell> boxSpells;

	void Awake () {
		entity = null;
		nickname = gameObject.transform.Find("Nickname").gameObject.GetComponent<Text>();
		textCaracts = new Dictionary<Characteristic, Text>() {
			{ Characteristic.CurrentLife, transform.Find("Life_content").gameObject.GetComponent<Text>() },
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
		boxSpells = new List<Spell>();
	}

	void Update() {
		if (entity != null)
			UpdateDetailbox();
	}

	public void UpdateDetailbox() {
		Dictionary<Characteristic, float> stats = entity.character.Stats;
		Dictionary<ItemType, Item> items = entity.character.Items;

		nickname.text = entity.character.Nickname;
		textCaracts[Characteristic.CurrentLife].text = stats[Characteristic.CurrentLife] + " / " + stats[Characteristic.MaxLife];
		textCaracts[Characteristic.CurrentAP].text = stats[Characteristic.CurrentAP] + " / " + stats[Characteristic.MaxAP];
		textCaracts[Characteristic.CurrentMP].text = stats[Characteristic.CurrentMP] + " / " + stats[Characteristic.MaxMP];
		textCaracts[Characteristic.ContactDamage].text = (stats[Characteristic.ContactDamage] * 100) + "% - " + (stats[Characteristic.ContactResistance] * 100) + "%";
		textCaracts[Characteristic.DistantDamage].text = (stats[Characteristic.DistantDamage] * 100) + "% - " + (stats[Characteristic.DistantResistance] * 100) + "%";
		textCaracts[Characteristic.PhysicalDamage].text = (stats[Characteristic.PhysicalDamage] * 100) + "% - " + (stats[Characteristic.PhysicalResistance] * 100) + "%";
		textCaracts[Characteristic.MagicDamage].text = (stats[Characteristic.MagicDamage] * 100) + "% - " + (stats[Characteristic.MagicResistance] * 100) + "%";
		textCaracts[Characteristic.Speed].text = string.Format("{0}", stats[Characteristic.Speed]);

		if (items[ItemType.Necklace] != null) { boxItems[ItemType.Necklace].SetItem(items[ItemType.Necklace]); } 
		if (items[ItemType.Bracelet] != null) { boxItems[ItemType.Bracelet].SetItem(items[ItemType.Bracelet]); }
		if (items[ItemType.Ring] != null) { boxItems[ItemType.Ring].SetItem(items[ItemType.Ring]); }
		if (items[ItemType.Weapon] != null) { boxItems[ItemType.Weapon].SetItem(items[ItemType.Weapon]); }
	}

	public void SetEntity (EntityBehaviour entity) {
		this.entity = entity;

		UpdateDetailbox();
	}
}

