﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxBehaviour : MonoBehaviour
{

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
        transform.Find("ItemName").GetComponent<Text>().text =
            item.name.ToUpperInvariant();
        transform.Find("Price").GetComponent<Text>().text =
            item.price + " G";
        transform.Find("Description").GetComponent<Text>().text =
            item.description;
        for (int i = 0; i < item.stats.Count; ++i)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
                RectTransform.Axis.Vertical,
                GetComponent<RectTransform>().sizeDelta.y + 18
            );
            GameObject statText = Instantiate(
                Resources.Load("Prefabs/UI/BoxTextLine"),
                transform.Find("StatsBox")
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


