using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDetailBoxItem : MonoBehaviour {
    public GameObject itemBoxPrefab;

	Item item;
	GameObject itemBox;

    void Start() {
        // /!\ Called after SetSpell() /!\
	}

	void Update() {
	}

	public void SetItem(Item item) {
		this.item = item;
		GetComponentInChildren<Text>().text = item.name;
	}

	public void OnEnter() {
		if (item != null) {
            itemBox = Instantiate(itemBoxPrefab, GameObject.Find("ToolBoxesContainer").transform) as GameObject;
			itemBox.GetComponent<ItemBoxBehaviour>().SetItem(item);
		}
	}

	public void OnExit() {
		DestroyImmediate(itemBox);
	}
}

