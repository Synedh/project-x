using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineEntity : MonoBehaviour {

    public CharacterBehaviour characterBehaviour;

	// Use this for initialization
	void Start ()
    {
        characterBehaviour = null;
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SetEntity(CharacterBehaviour characterBehaviour)
    {
        characterBehaviour.gameObject.GetComponent<EntityBehaviour>().timelineEntity = this;
        this.characterBehaviour = characterBehaviour;
        GetComponentInChildren<Text>().text = characterBehaviour.nickname;
    }

    public void SetColor(Color color)
    {
        GetComponentInChildren<Text>().color = color;
    }
}
