using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineEntity : MonoBehaviour {

    Text timelineEntityText;

	// Use this for initialization
	void Start () {
        timelineEntityText = GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void setText(string text)
    {
        timelineEntityText.text = text;
    }
}
