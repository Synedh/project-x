using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TimelineDetailBoxEffect : MonoBehaviour {
    public GameObject effectBoxPrefab;

	Effect effect;
	GameObject effectBox;

	void Start() {
        // /!\ Called after SetSpell() /!\
	}

	void Update() {
	}

    public void SetEffect(KeyValuePair<Effect,EntityBehaviour> effect) {
        this.effect = effect.Key;
        GetComponentInChildren<Text>().text = effect.Key.name;
	}

	public void OnEnter() {
        if (effect != null) {
            effectBox = Instantiate(effectBoxPrefab, GameObject.Find("ToolBoxesContainer").transform) as GameObject;
            effectBox.GetComponent<EffectBoxBehaviour>().SetEffect(effect);
		}
	}

	public void OnExit() {
        Destroy(effectBox);
	}
}

