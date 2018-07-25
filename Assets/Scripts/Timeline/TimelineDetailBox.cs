using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDetailBox : MonoBehaviour {

    public GameObject hPContent;
    public GameObject aPContent;
    public GameObject mPContent;
    public GameObject contactContent;
    public GameObject distantContent;
    public GameObject physicalContent;
    public GameObject magicalContent;
    public GameObject speedContent;

    public GameObject itemNecklace;
    public GameObject itemBracelet;
    public GameObject itemRing;
    public GameObject itemWeapon;

    EntityBehaviour entityBehaviour;
    Text nickname;
    Dictionary<Characteristic, Text> textCaracts;
    Dictionary<ItemType, TimelineDetailBoxItem> boxItems;
    List<GameObject> boxSpells;
    List<GameObject> boxEffects;

    Vector2 oldMousePosition;

    void Awake () {
        nickname = gameObject.transform.Find("Nickname").gameObject.GetComponent<Text>();
        textCaracts = new Dictionary<Characteristic, Text>() {
            { Characteristic.CurrentHP, hPContent.GetComponent<Text>() },
            { Characteristic.CurrentAP, aPContent.GetComponent<Text>() },
            { Characteristic.CurrentMP, mPContent.GetComponent<Text>() },
            { Characteristic.ContactDamage, contactContent.GetComponent<Text>() },
            { Characteristic.DistantDamage, distantContent.GetComponent<Text>() },
            { Characteristic.PhysicalDamage, physicalContent.GetComponent<Text>() },
            { Characteristic.MagicDamage, magicalContent.GetComponent<Text>() },
            { Characteristic.Speed, speedContent.GetComponent<Text>() }
        };
        boxItems = new Dictionary<ItemType, TimelineDetailBoxItem> () {
            { ItemType.Necklace, itemNecklace.GetComponent<TimelineDetailBoxItem>() },
            { ItemType.Bracelet, itemBracelet.GetComponent<TimelineDetailBoxItem>() },
            { ItemType.Ring, itemRing.GetComponent<TimelineDetailBoxItem>() },
            { ItemType.Weapon, itemWeapon.GetComponent<TimelineDetailBoxItem>() }
        };
        boxSpells = new List<GameObject>();
        boxEffects = new List<GameObject>();
    }

    void Update() {
        if (entityBehaviour != null)
        {
            UpdateStats();
            if (boxEffects.Count != entityBehaviour.character.effects.Count)
                UpdateEffects();
        }
    }

    public void UpdateStats() {
        Dictionary<Characteristic, float> stats = entityBehaviour.character.stats;

        textCaracts[Characteristic.CurrentHP].text = stats[Characteristic.CurrentHP] + " / " + stats[Characteristic.MaxHP];
        textCaracts[Characteristic.CurrentAP].text = stats[Characteristic.CurrentAP] + " / " + stats[Characteristic.MaxAP];
        textCaracts[Characteristic.CurrentMP].text = stats[Characteristic.CurrentMP] + " / " + stats[Characteristic.MaxMP];
        textCaracts[Characteristic.ContactDamage].text = (stats[Characteristic.ContactDamage] * 100) + "% - " + (stats[Characteristic.ContactResistance] * 100) + "%";
        textCaracts[Characteristic.DistantDamage].text = (stats[Characteristic.DistantDamage] * 100) + "% - " + (stats[Characteristic.DistantResistance] * 100) + "%";
        textCaracts[Characteristic.PhysicalDamage].text = (stats[Characteristic.PhysicalDamage] * 100) + "% - " + (stats[Characteristic.PhysicalResistance] * 100) + "%";
        textCaracts[Characteristic.MagicDamage].text = (stats[Characteristic.MagicDamage] * 100) + "% - " + (stats[Characteristic.MagicResistance] * 100) + "%";
        textCaracts[Characteristic.Speed].text = string.Format("{0}", stats[Characteristic.Speed]);
    }

    void UpdateEffects()
    {
        foreach (GameObject effectBox in boxEffects)
            DestroyImmediate(effectBox);
        boxEffects.Clear();

        List<KeyValuePair<Effect,EntityBehaviour>> effects = entityBehaviour.character.effects;
        for (int i = 0; i < effects.Count; ++i) 
        {
            GameObject effectBox = Instantiate(
                Resources.Load("Prefabs/UI/DetailEffect"),
                transform.Find("Effects")
            ) as GameObject;
            effectBox.transform.SetSiblingIndex(0);
            effectBox.transform.position = new Vector3(
                effectBox.transform.position.x + (i % 4) * 60,
                effectBox.transform.position.y - (int)(i / 4) * 60,
                effectBox.transform.position.z
            );
            effectBox.GetComponent<TimelineDetailBoxEffect>().SetEffect(effects[i]);
            boxEffects.Add(effectBox);
        }
    }

    void SetItems() 
    {
        Dictionary<ItemType, Item> items = entityBehaviour.character.items;
        boxItems[ItemType.Necklace].SetItem(items[ItemType.Necklace]);
        boxItems[ItemType.Bracelet].SetItem(items[ItemType.Bracelet]);
        boxItems[ItemType.Ring].SetItem(items[ItemType.Ring]);
        boxItems[ItemType.Weapon].SetItem(items[ItemType.Weapon]);
    }

    void SetSpells() 
    {
        List<Spell> spells = entityBehaviour.character.spells;
        if (spells != null)
        {
            for (int i = 0; i < spells.Count; ++i)
            {
                GameObject spellBox = Instantiate(
                    Resources.Load("Prefabs/UI/DetailSpell"),
                    transform.Find("Spells")
                ) as GameObject;
                spellBox.transform.position = new Vector3(
                    spellBox.transform.position.x,
                    spellBox.transform.position.y - i * 60,
                    spellBox.transform.position.z
                );
                spellBox.GetComponent<TimelineDetailBoxSpell>().SetSpell(spells[i]);
                boxSpells.Add(spellBox);
            }
        }
    }

    public void OnClick() {
        Destroy(gameObject);
    }

    public void OnDrag() {
        if (oldMousePosition != new Vector2(0, 0))
        {
            transform.position = new Vector3(
                transform.position.x - oldMousePosition.x + Input.mousePosition.x, 
                transform.position.y - oldMousePosition.y + Input.mousePosition.y,
                transform.position.z
            );
        }
        oldMousePosition = Input.mousePosition;
    }

    public void SetEntity (EntityBehaviour entityBehaviour) {
        this.entityBehaviour = entityBehaviour;

        nickname.text = entityBehaviour.character.nickname.ToUpper();
        UpdateStats();
        SetItems();
        SetSpells();
    }
}

