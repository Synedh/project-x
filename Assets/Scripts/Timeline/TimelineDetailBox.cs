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
			{ Characteristic.Life, gameObject.transform.Find("Life_content").gameObject.GetComponent<Text>() },
			{ Characteristic.AP, gameObject.transform.Find("AP_content").gameObject.GetComponent<Text>() },
			{ Characteristic.MP, gameObject.transform.Find("MP_content").gameObject.GetComponent<Text>() },
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
		nickname.text = entity.character.Nickname;
		TextCaracts [Characteristic.Life].text = entity.character.Stats[Characteristic.Life] + " / " + entity.character.Stats [Characteristic.MaxLife];
		TextCaracts [Characteristic.AP].text = string.Format("{0}", entity.character.Stats[Characteristic.AP]);
		TextCaracts [Characteristic.MP].text = string.Format("{0}", entity.character.Stats[Characteristic.MP]);
		TextCaracts [Characteristic.ContactDamage].text = (entity.character.Stats[Characteristic.ContactDamage] * 100) + "% - " + (entity.character.Stats[Characteristic.ContactResistance] * 100) + "%";
		TextCaracts [Characteristic.DistantDamage].text = (entity.character.Stats[Characteristic.DistantDamage] * 100) + "% - " + (entity.character.Stats[Characteristic.DistantResistance] * 100) + "%";
		TextCaracts [Characteristic.PhysicalDamage].text = (entity.character.Stats[Characteristic.PhysicalDamage] * 100) + "% - " + (entity.character.Stats[Characteristic.PhysicalResistance] * 100) + "%";
		TextCaracts [Characteristic.MagicDamage].text = (entity.character.Stats[Characteristic.MagicDamage] * 100) + "% - " + (entity.character.Stats[Characteristic.MagicResistance] * 100) + "%";
		TextCaracts [Characteristic.Speed].text = string.Format("{0}", entity.character.Stats[Characteristic.Speed]);
	}

	public void SetEntity (EntityBehaviour entity) {
		this.entity = entity;

		UpdateDetailbox();
	}
		
}

