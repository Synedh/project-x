using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBehaviour : MonoBehaviour {

    public bool isWalkable;
    public bool hideView;

    public Cell cell;

	// Use this for initialization
	void Start () {
        if (!isWalkable) // Correction hauteur block temporaire
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.25f, this.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnMouseDown()
    {
        GameManager.instance.character.GetComponent<CharacterBehaviour>().Move(GameManager.instance.grid, cell);
    }
}
