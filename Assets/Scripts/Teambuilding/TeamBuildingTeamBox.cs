using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamBuildingTeamBox : MonoBehaviour {

    public GameObject teamName;
    public GameObject teamPrice;
    public GameObject teamImage;

    public GameObject characterContainer;
    public GameObject characterBoxPrefab;

    Team team;
    List<TeamBuildingCharacterBox> characterBoxes;

	// Use this for initialization
	void Start () {
        characterBoxes = new List<TeamBuildingCharacterBox>();
        teamName.GetComponent<Text>().text = team.name;

        int price = 0;
        foreach (Character character in team.characters)
        {
            foreach (KeyValuePair<ItemType, Item> item in character.items)
            {
                if (item.Value != null)
                    price += item.Value.price;
            }
            foreach (Spell spell in character.spells)
            {
                price += spell.price;
            }
        }
        teamPrice.GetComponent<Text>().text = price + " / 500";

        foreach (Character character in team.characters)
        {
            AddCharacter(character);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public void SetTeam(Team team){
        this.team = team;
    }

    public void AddCharacter(Character character) {
        GameObject characterObject = Instantiate(characterBoxPrefab, characterContainer.transform) as GameObject;
        TeamBuildingCharacterBox characterBox = characterObject.GetComponent<TeamBuildingCharacterBox>();
        characterBox.SetCharacter(character);
    }
}
