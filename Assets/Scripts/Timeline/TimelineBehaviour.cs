using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineBehaviour : MonoBehaviour
{
    static List<GameObject> timelineEntities;
    static TimelineBehaviour instance;

    // Use this for initialization
    void Start()
    {
        timelineEntities = new List<GameObject>();
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void BuildTimeline(List<GameObject> entities)
    {
        foreach (GameObject entity in entities)
        {
            GameObject prefab = Resources.Load("Prefabs/TimelineEntity") as GameObject;
            GameObject timelineEntity = Instantiate(prefab);
            timelineEntity.transform.SetParent(instance.transform, false);
            RectTransform prefabRectTransform = prefab.GetComponent<RectTransform>();
            timelineEntity.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                prefabRectTransform.rect.width / 2 + prefabRectTransform.rect.width * timelineEntities.Count,
                prefabRectTransform.rect.height / 2
            );
            timelineEntity.GetComponent<TimelineEntity>().SetEntity(entity.GetComponent<CharacterBehaviour>());
            timelineEntities.Add(timelineEntity);
        }
    }
}
