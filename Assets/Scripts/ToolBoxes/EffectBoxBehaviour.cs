using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBoxBehaviour : MonoBehaviour {

    void Awake() {
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

	void Update() {
        transform.position = Input.mousePosition;
	}

    public void SetEffect(Effect effect) {
        transform.Find("EffectName").GetComponent<Text>().text = effect.name.ToUpperInvariant();
	}
}
