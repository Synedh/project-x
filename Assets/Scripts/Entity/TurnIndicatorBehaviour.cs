using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnIndicatorBehaviour : MonoBehaviour {

    EntityBehaviour currentEntityBehaviour;

    public float rotationSpeed = 10f;
    public float speedPosition = 0.03f;

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        currentEntityBehaviour = TurnManager.instance.currentEntityBehaviour;
        if (currentEntityBehaviour)
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshRenderer>().material =
                currentEntityBehaviour.character.team.colorMaterial;

            if (transform.position.y <= 2 || transform.position.y >= 3)
                speedPosition = -speedPosition;

            transform.position = new Vector3(
                currentEntityBehaviour.transform.position.x,
                transform.position.y + speedPosition,
                currentEntityBehaviour.transform.position.z
            );

            transform.eulerAngles = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y + Time.deltaTime * rotationSpeed,
                transform.eulerAngles.z
            );
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
