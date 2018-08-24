using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentEntityBoxBehaviour : MonoBehaviour {

    Character currentCharacter;
    public Text nextButtonText;
    public Text currentHP;
    public Text currentAP;
    public Text currentMP;

    // Use this for initialization
    void Start () {
    }
    
    // Update is called once per frame
    void Update () {
        if (TurnManager.instance.currentEntityBehaviour)
        {
            currentCharacter = TurnManager.instance.currentEntityBehaviour.character;
            currentHP.text = 
                currentCharacter.stats[Characteristic.CurrentHP].ToString();
            currentAP.text =
                currentCharacter.stats[Characteristic.CurrentAP].ToString();
            currentMP.text =
                currentCharacter.stats[Characteristic.CurrentMP].ToString();
        
            transform.Find("Rotation").rotation = Quaternion.Euler(
                new Vector3(0, 0, 135 + CameraManager.instance.currentX)
            );
        }
    }

    void LateUpdate() {
        // currentTimebank.text = GameManager.instance.turnManager.currentTimebank.ToString();
    }
}
