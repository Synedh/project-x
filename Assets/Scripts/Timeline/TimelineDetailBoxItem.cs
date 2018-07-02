using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDetailBoxItem : MonoBehaviour {

	Item item;
	GameObject itemBox;

	void Start() {
		item = null;
	}

	void Update() {
	}

	public void SetItem(Item item) {
		this.item = item;
		GetComponentInChildren<Text>().text = item.Name;
	}

	public void OnEnter() {
		if (item != null) {
			itemBox = Instantiate (Resources.Load ("Prefabs/ItemBox"), GameObject.Find("ToolBoxes").transform) as GameObject;
			itemBox.transform.position = Input.mousePosition;
			itemBox.GetComponent<ItemBoxBehaviour>().SetItem(item);
		}
	}

	public void OnExit() {
		DestroyImmediate(itemBox);
	}
}

