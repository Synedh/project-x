using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEntityBoxBehaviour : MonoBehaviour {

    Character currentCharacter;

    Text currentHP;
    Text currentAP;
    Text currentMP;

	// Use this for initialization
    void Start () {
        currentHP = transform.Find("HP/Text").GetComponent<Text>();
        currentAP = transform.Find("AP/Text").GetComponent<Text>();
        currentMP = transform.Find("MP/Text").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        currentCharacter = GameManager.instance.selectedEntity.GetComponent<EntityBehaviour>().character;
        currentHP.text = currentCharacter.stats[Characteristic.CurrentHP].ToString();
        currentAP.text = currentCharacter.stats[Characteristic.CurrentAP].ToString();
        currentMP.text = currentCharacter.stats[Characteristic.CurrentMP].ToString();
        
        transform.Find("Rotation").rotation = Quaternion.Euler(new Vector3(0, 0, 135 + CameraControl.instance.currentX));
	}
}
