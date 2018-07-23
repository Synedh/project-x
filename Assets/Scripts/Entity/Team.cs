using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Team
{
    List<EntityBehaviour> _entities;

    public Team(List<EntityBehaviour> entities = null)
    {
        if (entities == null)
            _entities = new List<EntityBehaviour>();
        else
            _entities = entities;
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

    public List<EntityBehaviour> entities
    {
        get
        {
            return this._entities;
        }
    }
}

