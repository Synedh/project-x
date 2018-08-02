    using System.IO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Newtonsoft.Json;


    public struct TeamBuilder {
        public string name;
        public int imageid;
        public List<CharacterBuilder> characters;
    }

    public class Team
    {
        string _name;
        Sprite _image;
        List<Character> _characters;
        readonly Material _colorMaterial;

        public Team(string name, Sprite image, List<Character> characters = null, Material colorMaterial = null)
        {
            if (characters == null)
                _characters = new List<Character>();
            else
                _characters = characters;

            _name = name;
            _image = image;
            _colorMaterial = colorMaterial;
        }

        public Team(int teamId = -1, string teamPath = "", Material colorMaterial = null)
        {
            TeamBuilder teamBuilder;
            if (teamId != -1)
                teamPath = GameManager.teamPath + teamId.ToString() + ".json";
            using (StreamReader r = new StreamReader(teamPath))
                teamBuilder = JsonConvert.DeserializeObject<TeamBuilder>(r.ReadToEnd());

            _name = teamBuilder.name;
            _image = null;
            _characters = new List<Character>();
            _colorMaterial = colorMaterial;

            foreach (CharacterBuilder characterBuilder in teamBuilder.characters)
            {
                Character character = Character.CharacterLoader(characterBuilder);
                character.team = this;
                _characters.Add(character);
            }
        }

        public static List<Team> getTeams() {
            List<Team> teams = new List<Team>();
            foreach (string file in Directory.GetFiles(GameManager.teamPath, "*.json"))
            {
                teams.Add(new Team(teamPath: file));
            }
            return teams;
        }

        public void Add(Character character)
        {
            _characters.Add(character);
            character.team = this;
        }

        public bool Remove(Character character)
        {
            character.team = null;
            return _characters.Remove(character);
        }

        public bool checkTeam() {
            foreach (Character character in _characters) {
                if (character.stats[Characteristic.CurrentHP] > 0)
                    return true;
            }
            return false;
        }

        public string name
        {
            get
            {
                return this._name;
            }
        }

        public Sprite image
        {
            get
            {
                return this._image;
            }
        }

        public List<Character> characters
        {
            get
            {
                return this._characters;
            }
        }

        public Material colorMaterial
        {
            get
            {
                return this._colorMaterial;
            }
        }
    }

