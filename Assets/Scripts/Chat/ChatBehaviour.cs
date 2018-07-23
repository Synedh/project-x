using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBehaviour : MonoBehaviour {

    static GameObject textZone;
    static List<GameObject> messages;

    Vector2 oldMousePosition;


	// Use this for initialization
	void Awake () {
        textZone = transform.Find("TextZone").transform.Find("TextZone").gameObject;
        messages = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void WriteMessage(string message, MessageType messageType) {
        GameObject messageBox = Instantiate(Resources.Load("Prefabs/UI/Message"), textZone.transform) as GameObject;
        messageBox.GetComponent<Text>().text = String.Format("{0:[HH:mm]} ", DateTime.Now) + message;

        switch (messageType)
        {
            case MessageType.Say:
                break;
            case MessageType.Wisp:
                messageBox.GetComponent<Text>().color = Color.cyan;
                break;
            case MessageType.Group:
                messageBox.GetComponent<Text>().color = new Color(0.75f, 0.25f, 1f);
                break;
            case MessageType.Combat:
                messageBox.GetComponent<Text>().color = new Color(0f, 0.75f, 0f);
                break;
            case MessageType.Information:
                messageBox.GetComponent<Text>().color = new Color(0f, 0.5f, 1f);
                break;
            case MessageType.Warning:
                messageBox.GetComponent<Text>().color = new Color(1f, 0.5f, 0f);
                break;
            case MessageType.Danger:
                messageBox.GetComponent<Text>().color = new Color(0.75f, 0f, 0f);
                break;
        }

        messages.Add(messageBox);
    }

    public void OnDrag() {
        if (oldMousePosition != new Vector2(0, 0))
            transform.position = new Vector3(
                transform.position.x - oldMousePosition.x + Input.mousePosition.x, 
                transform.position.y - oldMousePosition.y + Input.mousePosition.y,
                transform.position.z
            );
        oldMousePosition = Input.mousePosition;
    }
}
