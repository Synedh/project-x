using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBehaviour : MonoBehaviour
{

    public static ChatBehaviour instance;

    public GameObject textZone;

    List<GameObject> messages;
    Vector2 oldMousePosition;


    // Use this for initialization
    void Awake()
    {
        instance = this;
        messages = new List<GameObject>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public static void WriteMessage(string message, MessageType messageType)
    {
        GameObject messageBox = Instantiate(
                                    Resources.Load("Prefabs/UI/Message"),
                                    instance.textZone.transform
                                ) as GameObject;
        Text messageBoxText = messageBox.GetComponent<Text>();
        messageBoxText.text = String.Format("{0:[HH:mm]} ", DateTime.Now) + message;

        switch (messageType)
        {
            case MessageType.Say:
                break;
            case MessageType.Wisp:
                messageBoxText.color = Color.cyan;
                break;
            case MessageType.Group:
                messageBoxText.color = new Color(0.75f, 0.25f, 1f);
                break;
            case MessageType.Combat:
                messageBoxText.color = new Color(0f, 0.75f, 0f);
                break;
            case MessageType.Information:
                messageBoxText.color = new Color(0f, 0.5f, 1f);
                break;
            case MessageType.Warning:
                messageBoxText.color = new Color(1f, 0.5f, 0f);
                break;
            case MessageType.Danger:
                messageBoxText.color = new Color(0.75f, 0f, 0f);
                break;
        }

        instance.messages.Add(messageBox);
    }

    public void OnDrag()
    {
        if (oldMousePosition != new Vector2(0, 0))
            transform.position = new Vector3(
                transform.position.x - oldMousePosition.x + Input.mousePosition.x, 
                transform.position.y - oldMousePosition.y + Input.mousePosition.y,
                transform.position.z
            );
        oldMousePosition = Input.mousePosition;
    }
}
