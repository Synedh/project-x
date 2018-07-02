using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBoxBehaviour : MonoBehaviour {

	Item item;

	void Start() {
	}

	void Update() {
	}

	public void SetItem(Item item) {
		this.item = item;
		transform.Find("ItemName").GetComponent<Text>().text = item.Name;
	}
}


