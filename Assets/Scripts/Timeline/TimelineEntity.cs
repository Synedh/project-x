using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineEntity : MonoBehaviour {

    CharacterBehaviour characterBehaviour;
    EntityBehaviour entityBehaviour;

	// Use this for initialization
	void Start ()
    {
        characterBehaviour = null;
        entityBehaviour = null;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void SetEntity(GameObject entity)
    {
        characterBehaviour = entity.GetComponent<CharacterBehaviour>();
        entityBehaviour = entity.GetComponent<EntityBehaviour>();
        entityBehaviour.timelineEntity = this;
        GetComponentInChildren<Text>().text = characterBehaviour.nickname;
    }

    public void SetColor(Color color)
    {
        GetComponentInChildren<Text>().color = color;
    }
}
