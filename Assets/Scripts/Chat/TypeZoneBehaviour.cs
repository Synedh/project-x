using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeZoneBehaviour : MonoBehaviour
{

    public GameObject inputFieldObject;
    public GameObject channelObject;

    InputField inputField;
    Dropdown dropdown;
    string channel;

    // Use this for initialization
    void Start()
    {
        inputField = inputFieldObject.GetComponent<InputField>();
        dropdown = channelObject.GetComponent<Dropdown>();
        channel = "say";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnSelectChannel()
    {
        channel = dropdown.options[dropdown.value].text.ToLowerInvariant();
    }

    public void OnEndEdit()
    {
        string message = inputField.text;
        if (message != "")
        {
            switch (channel)
            {
                case "say":
                    ChatBehaviour.WriteMessage(
                        "[USER] : " + message,
                        MessageType.Say
                    );
                    break;
                case "group":
                    ChatBehaviour.WriteMessage(
                        "(Group) [USER] : " + message,
                        MessageType.Group
                    );
                    break;
            }
            inputField.text = "";
        }
    }
}
