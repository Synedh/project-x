using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBehaviour : MonoBehaviour {

    GameObject textZone;
    List<GameObject> messages;

    public static ChatBehaviour instance;
	// Use this for initialization
	void Awake () {
        textZone = transform.Find("TextZone").transform.Find("TextZone").gameObject;
        messages = new List<GameObject>();
        instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void AddMessage(GameObject message) {
        message.transform.position = new Vector3(message.transform.position.x, message.transform.position.y + textZone.GetComponent<RectTransform>().rect.height, message.transform.position.z);
        textZone.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(
            RectTransform.Axis.Vertical, 
            textZone.GetComponent<RectTransform>().rect.height + message.GetComponent<RectTransform>().rect.height
        );
        messages.Add(message);
    }

    public void WriteMessage(string message) {
        GameObject messageBox = Instantiate(Resources.Load("Prefabs/UI/Message"), textZone.transform) as GameObject;
        AddMessage(messageBox);
    }
}
