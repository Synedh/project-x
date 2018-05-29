using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    Vector3 middle;

	// Use this for initialization
	void Start () {
        middle = new Vector3(GameManager.instance.grid.width / 2, 0f, GameManager.instance.grid.height / 2);
        transform.position = new Vector3(GameManager.instance.grid.width, 5f, GameManager.instance.grid.height);
        transform.LookAt(middle);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            transform.position = new Vector3(GameManager.instance.grid.width, transform.position.y, GameManager.instance.grid.height);
            transform.LookAt(middle);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            transform.position = new Vector3(GameManager.instance.grid.width, transform.position.y, 0);
            transform.LookAt(middle);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            transform.position = new Vector3(0, transform.position.y, GameManager.instance.grid.height);
            transform.LookAt(middle);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            transform.LookAt(middle);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            float newY = transform.position.y + 1 * Input.GetAxis("Mouse ScrollWheel");
            if (newY < 0)
                newY = 0;
            else if (newY > 10)
                newY = 10;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            transform.LookAt(middle);
        }
    }
}
