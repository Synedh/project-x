using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBoxBehaviour : MonoBehaviour
{
    public GameObject spellNameObject;
    public GameObject priceObject;
    public GameObject descriptionObject;
    public GameObject rangeObject;
    public GameObject effectBoxObject;
    public GameObject boxTextLinePrefab;

    void Awake()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        Vector2 anchors = new Vector2(
            (int)(Input.mousePosition.x * 2 / Camera.main.pixelWidth),
            (int)(Input.mousePosition.y * 2 / Camera.main.pixelHeight)
        );
        rectTransform.anchorMin = anchors;
        rectTransform.anchorMax = anchors;
        rectTransform.pivot = anchors;
        transform.position = Input.mousePosition;
    }

    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void SetSpell(Spell spell)
    {
        spellNameObject.GetComponent<Text>().text = spell.name.ToUpperInvariant();
        priceObject.GetComponent<Text>().text = spell.price + " G";
        descriptionObject.GetComponent<Text>().text = spell.description;
        rangeObject.GetComponent<Text>().text =
            "PO " + spell.rangeMin + " - " + spell.rangeMax;
        for (int i = 0; i < spell.effects.Count; ++i)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                GetComponent<RectTransform>().sizeDelta.y + 18
            );
            GameObject effectText = Instantiate(
                boxTextLinePrefab,
                effectBoxObject.transform
            ) as GameObject;
            effectText.transform.position = new Vector3(
                effectText.transform.position.x,
                effectText.transform.position.y - i * 18,
                effectText.transform.position.z
            );
            effectText.GetComponent<Text>().text = spell.effects[i].description;
        }
    }
}
