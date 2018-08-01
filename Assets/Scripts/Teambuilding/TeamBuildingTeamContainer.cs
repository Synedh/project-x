using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBuildingTeamContainer : MonoBehaviour {

    public GameObject teamContainer;
    public GameObject teamBoxPrefab;

    List<TeamBuildingTeamBox> teamBoxes;

	// Use this for initialization
    void Start () {
        teamBoxes = new List<TeamBuildingTeamBox>();
        LoadTeams();
	}
	
	// Update is called once per frame
	void Update () {
    }

    public void reloadTeams() {
        if (Team.getTeams().Count != teamBoxes.Count)
        {
            foreach (TeamBuildingTeamBox teamBox in teamBoxes)
            {
                Destroy(teamBox.gameObject);
            }
            teamBoxes.Clear();
            LoadTeams();
        }
    }

    void LoadTeams() {
        foreach (Team team in Team.getTeams())
        {
            GameObject teamObject = Instantiate(teamBoxPrefab, teamContainer.transform) as GameObject;
            TeamBuildingTeamBox teamBox = teamObject.GetComponent<TeamBuildingTeamBox>();
            teamBoxes.Add(teamBox);
            teamBox.SetTeam(team);
        }
    }
}
