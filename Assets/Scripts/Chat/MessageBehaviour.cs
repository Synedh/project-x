using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MessageType {
    Say,
    Wisp,
    Group,
    Combat,
    Information,
    Warning,
    Danger
}

public class MessageBehaviour : MonoBehaviour {

    Text text;

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetContent(string content) {
        text.text = content;
    }
}
