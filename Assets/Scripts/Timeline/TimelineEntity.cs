using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineEntity : MonoBehaviour {

    public GameObject DetailPannel;

    Character character;
    EntityBehaviour entityBehaviour;
	// GameObject detailContainer;
	GameObject detailBox;

	// Use this for initialization
	void Start () {
		// /!\ Caution, called after SetEntity /!\
		// detailContainer = GameObject.Find("DetailContainer");
		detailBox = null;
    }
	
	// Update is called once per frame
	void Update () {
	}

    public void SetEntity(GameObject entity)
    {
        entityBehaviour = entity.GetComponent<EntityBehaviour>();
		character = entityBehaviour.character;
        entityBehaviour.timelineEntity = this;
        GetComponentInChildren<Text>().text = character.nickname;
    }

    public void SetColor(Color color)
    {
        GetComponentInChildren<Text>().color = color;
    }

    public void OnEnter() 
    {
        entityBehaviour.MouseEnter();
    }

    public void OnExit() 
    {
        entityBehaviour.MouseExit();
    }

	public void OnClick()
	{
		if (detailBox == null)
		{
            detailBox = Instantiate(DetailPannel, transform) as GameObject;
			detailBox.GetComponent<TimelineDetailBox>().SetEntity(entityBehaviour);
		} 
		else 
		{
			DestroyImmediate(detailBox);
		}
	}
}
