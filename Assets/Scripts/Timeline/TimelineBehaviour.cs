using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineBehaviour : MonoBehaviour
{
    public GameObject timelineEntityprefab;

    RectTransform prefabRectTransform;
    List<GameObject> entities;
    List<GameObject> timelineEntities;

    // Use this for initialization
    void Awake()
    {
        prefabRectTransform = timelineEntityprefab.GetComponent<RectTransform>();
        timelineEntities = new List<GameObject>();
        entities = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Refresh()
    {
        foreach (GameObject timelineEntity in timelineEntities)
        {
            Destroy(timelineEntity);
        }
        timelineEntities = new List<GameObject>();

        foreach (GameObject entity in entities)
        {
			GameObject timelineEntity = Instantiate(timelineEntityprefab, transform);

            timelineEntity.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                prefabRectTransform.rect.width / 2 + prefabRectTransform.rect.width * timelineEntities.Count,
                prefabRectTransform.rect.height / 2 - 10
            );
            timelineEntity.GetComponent<TimelineEntity>().SetEntity(entity);

            timelineEntities.Add(timelineEntity);
        }
    }

    public void AddTimelineEntity(GameObject entity, int position)
    {
        entities.Insert(position, entity);
    }
}
