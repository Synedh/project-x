using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDetailBox : MonoBehaviour {

	EntityBehaviour entity;
	Text nickname;
	Dictionary<Characteristic, Text> TextCaracts;

	void Awake () {
		entity = null;
		nickname = gameObject.transform.Find("Nickname").gameObject.GetComponent<Text>();
		TextCaracts = new Dictionary<Characteristic, Text>() {
			{ Characteristic.CurrentLife, gameObject.transform.Find("Life_content").gameObject.GetComponent<Text>() },
			{ Characteristic.CurrentAP, gameObject.transform.Find("AP_content").gameObject.GetComponent<Text>() },
			{ Characteristic.CurrentMP, gameObject.transform.Find("MP_content").gameObject.GetComponent<Text>() },
			{ Characteristic.ContactDamage, gameObject.transform.Find("Contact_content").gameObject.GetComponent<Text>() },
			{ Characteristic.DistantDamage, gameObject.transform.Find("Distant_content").gameObject.GetComponent<Text>() },
			{ Characteristic.PhysicalDamage, gameObject.transform.Find("Physical_content").gameObject.GetComponent<Text>() },
			{ Characteristic.MagicDamage, gameObject.transform.Find("Magic_content").gameObject.GetComponent<Text>() },
			{ Characteristic.Speed, gameObject.transform.Find("Speed_content").gameObject.GetComponent<Text>() }
		};
	}

	void Update() {

	}

	public void UpdateDetailbox() {
		Dictionary<Characteristic, float> stats = entity.character.Stats;

		nickname.text = entity.character.Nickname;
		TextCaracts[Characteristic.CurrentLife].text = stats[Characteristic.CurrentLife] + " / " + stats[Characteristic.MaxLife];
		TextCaracts[Characteristic.CurrentAP].text = stats[Characteristic.CurrentAP] + " / " + stats[Characteristic.MaxAP];
		TextCaracts[Characteristic.CurrentMP].text = stats[Characteristic.CurrentMP] + " / " + stats[Characteristic.MaxMP];
		TextCaracts[Characteristic.ContactDamage].text = (stats[Characteristic.ContactDamage] * 100) + "% - " + (stats[Characteristic.ContactResistance] * 100) + "%";
		TextCaracts[Characteristic.DistantDamage].text = (stats[Characteristic.DistantDamage] * 100) + "% - " + (stats[Characteristic.DistantResistance] * 100) + "%";
		TextCaracts[Characteristic.PhysicalDamage].text = (stats[Characteristic.PhysicalDamage] * 100) + "% - " + (stats[Characteristic.PhysicalResistance] * 100) + "%";
		TextCaracts[Characteristic.MagicDamage].text = (stats[Characteristic.MagicDamage] * 100) + "% - " + (stats[Characteristic.MagicResistance] * 100) + "%";
		TextCaracts[Characteristic.Speed].text = string.Format("{0}", stats[Characteristic.Speed]);
	}

	public void SetEntity (EntityBehaviour entity) {
		this.entity = entity;

		UpdateDetailbox();
	}
		
}

