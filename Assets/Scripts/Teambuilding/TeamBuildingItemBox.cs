using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamBuildingItemBox : MonoBehaviour {
    public GameObject itemBoxPrefab;

    Item item;
    GameObject itemBox;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public void SetItem(Item item) {
        if (item != null && item.name != "Poke")
        {
            this.item = item;
            GetComponentInChildren<Image>().color = new Color(1f, 1f, 1f, 1f);
            // GetComponentInChildren<Text>().text = item.name;
        }
        else
        {
        }
    }

    public void OnEnter() {
        if (item != null) {
            itemBox = Instantiate(
                itemBoxPrefab,
                GameObject.Find("ToolBoxesContainer").transform
            ) as GameObject;
            itemBox.GetComponent<ItemBoxBehaviour>().SetItem(item);
        }
    }

    public void OnExit() {
        Destroy(itemBox);
    }
}
