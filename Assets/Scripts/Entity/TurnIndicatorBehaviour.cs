using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnIndicatorBehaviour : MonoBehaviour {

    EntityBehaviour currentEntity;

    public float rotationSpeed = 10f;
    public float speedPosition = 0.03f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        currentEntity = GameManager.instance.currentEntityBehaviour;
        if (currentEntity.character.team != null)
            GetComponent<MeshRenderer>().material = currentEntity.character.team.colorMaterial;

        if (transform.position.y <= 2 || transform.position.y >= 3)
            speedPosition = -speedPosition;

        transform.position = new Vector3(
            currentEntity.transform.position.x,
            transform.position.y + speedPosition,
            currentEntity.transform.position.z
        );

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + Time.deltaTime * rotationSpeed,
            transform.eulerAngles.z
        );
	}
}
