using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class Team
{
    List<EntityBehaviour> _entities;
    readonly Material _colorMaterial;

    public Team(Material colorMaterial = null, List<EntityBehaviour> entities = null)
    {
        if (entities == null)
            _entities = new List<EntityBehaviour>();
        else
            _entities = entities;
        _colorMaterial = colorMaterial;
    }

    public static List<Character> TeamLoader(int teamId)
    {
        using (StreamReader r = new StreamReader(GameManager.spellPath + teamId.ToString() + ".json"))
            return JsonConvert.DeserializeObject<List<Character>>(r.ReadToEnd());
    }

    public void Add(EntityBehaviour entity)
    {
        _entities.Add(entity);
        entity.team = this;
    }

    public bool Remove(EntityBehaviour entity)
    {
        entity.team = null;
        return _entities.Remove(entity);
    }

    public bool checkTeam() {
        foreach (EntityBehaviour entity in _entities) {
            if (entity.isAlive)
                return true;
        }
        return false;
    }

    public List<EntityBehaviour> entities
    {
        get
        {
            return this._entities;
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

