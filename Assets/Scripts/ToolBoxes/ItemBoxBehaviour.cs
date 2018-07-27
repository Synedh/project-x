using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxBehaviour : MonoBehaviour
{
    public GameObject itemNameObject;
    public GameObject priceObject;
    public GameObject rangeObject;
    public GameObject descriptionObject;
    public GameObject effectBoxObject;
    public GameObject statsBoxObject;
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

    public void SetItem(Item item)
    {
        itemNameObject.GetComponent<Text>().text = item.name.ToUpperInvariant();
        priceObject.GetComponent<Text>().text = item.price + " G";
        descriptionObject.GetComponent<Text>().text = item.description;

        if (item.spell != null)
        {
            rangeObject.GetComponent<Text>().text =
                "PO " + item.spell.rangeMin + " - " + item.spell.rangeMax;
            statsBoxObject.transform.position = new Vector3(
                statsBoxObject.transform.position.x,
                statsBoxObject.transform.position.y - 18,
                statsBoxObject.transform.position.z
            );

            for (int i = 0; i < item.spell.effects.Count; ++i)
            {
                GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    GetComponent<RectTransform>().sizeDelta.y + 18
                );
                statsBoxObject.transform.position = new Vector3(
                    statsBoxObject.transform.position.x,
                    statsBoxObject.transform.position.y - i * 18,
                    statsBoxObject.transform.position.z
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
                effectText.GetComponent<Text>().text = item.spell.effects[i].description;
            }
        }
        else
        {
            rangeObject.GetComponent<Text>().text = "";
        }

        for (int i = 0; i < item.stats.Count; ++i)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                GetComponent<RectTransform>().sizeDelta.y + 18
            );
            GameObject statText = Instantiate(
                boxTextLinePrefab,
                statsBoxObject.transform
            ) as GameObject;
            statText.transform.position = new Vector3(
                statText.transform.position.x,
                statText.transform.position.y - i * 18,
                statText.transform.position.z
            );
            if (item.stats[i].Value >= 1)
                statText.GetComponent<Text>().text = (
                    item.stats[i].Value
                    + " "
                    + System.Text.RegularExpressions.Regex.Replace(
                        item.stats[i].Key.ToString(),
                        "([A-Z][a-z]+)([A-Z][a-z]+|[A-Z]{2})", "$1 $2"
                    )
                );
            else
                statText.GetComponent<Text>().text = (
                    item.stats[i].Value * 100
                    + "% "
                    + System.Text.RegularExpressions.Regex.Replace(
                        item.stats[i].Key.ToString(),
                        "([A-Z][a-z]+)([A-Z][a-z]+|[A-Z]{2})", "$1 $2"
                    )
                );
        }
    }
}


