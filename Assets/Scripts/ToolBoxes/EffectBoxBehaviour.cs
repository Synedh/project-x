using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBoxBehaviour : MonoBehaviour
{
    public GameObject turnsObject;
    public GameObject effectNameObject;
    public GameObject descriptionObject;

    Effect effect;

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

        if (effect != null)
        {
            int turns = effect.effects.Count - effect.currentTurn;
            turnsObject.GetComponent<Text>().text =
                turns + (turns > 1 ? " turns remaining" : " turn remaining");
        }
    }

    public void SetEffect(Effect effect)
    {
        this.effect = effect;
        effectNameObject.GetComponent<Text>().text =
            effect.name.ToUpperInvariant();
        descriptionObject.GetComponent<Text>().text =
            effect.description;
    }
}
