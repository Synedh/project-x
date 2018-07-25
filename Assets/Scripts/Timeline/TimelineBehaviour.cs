using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineBehaviour : MonoBehaviour
{
    public GameObject timelineEntityPrefab;

    public List<EntityBehaviour> entities;
    RectTransform prefabRectTransform;
    List<GameObject> timelineEntities;

    // Use this for initialization
    void Awake()
    {
        prefabRectTransform = timelineEntityPrefab.GetComponent<RectTransform>();
        entities = new List<EntityBehaviour>();
        timelineEntities = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OrderBySpeed() {
        List<EntityBehaviour> orderedEntities = new List<EntityBehaviour>();
        foreach (EntityBehaviour entityBehaviour in entities)
        {
            int i = 0;
            for (; i < orderedEntities.Count; ++i)
            {
                if (orderedEntities[i].character.stats[Characteristic.Speed]
                    < entityBehaviour.character.stats[Characteristic.Speed])
                {
                    orderedEntities.Insert(i, entityBehaviour);
                    break;
                }
            }
            orderedEntities.Insert(i, entityBehaviour);
        }
        entities = orderedEntities;
    }

    public void Refresh()
    {
        foreach (GameObject timelineEntity in timelineEntities)
        {
            Destroy(timelineEntity);
        }
        timelineEntities = new List<GameObject>();

        OrderBySpeed();

        foreach (EntityBehaviour entityBehaviour in entities)
        {
            GameObject timelineEntity = Instantiate(timelineEntityPrefab, transform);

            timelineEntity.GetComponent<RectTransform>().anchoredPosition = new Vector2(
                prefabRectTransform.rect.width / 2 + prefabRectTransform.rect.width * timelineEntities.Count + 40,
                prefabRectTransform.rect.height / 2 - 10
            );
            timelineEntity.GetComponent<TimelineEntity>().SetEntity(entityBehaviour);

            timelineEntities.Add(timelineEntity);
        }
    }

    public void AddTimelineEntity(EntityBehaviour entityBehaviour, int position)
    {
        entities.Insert(position, entityBehaviour);
    }
}
