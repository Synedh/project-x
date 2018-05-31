using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript1 : MonoBehaviour
{
    static List<GameObject> timelineEntities;

    // Use this for initialization
    void Start()
    {
        timelineEntities = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void buildTimeline(List<GameObject> entities)
    {
        foreach (GameObject entity in entities)
        {
            GameObject prefab = Resources.Load("Prefabs/Nickname") as GameObject;
            timelineEntities.Add(Instantiate(prefab, new Vector3(0, 0, prefab.GetComponent<RectTransform>().rect.width * timelineEntities.Count), Quaternion.identity));
        }
    }
}
