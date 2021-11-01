using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PuzzleMatchesInfo
{
    private List<GameObject> matchedGems;

    // Listado de todas las concidencias en gemas
    public IEnumerable<GameObject> foundMatches
    {
        get
        {
            return matchedGems.Distinct();
        }
    }

    public void AddObject(GameObject go)
    {
        if (!matchedGems.Contains(go))
            matchedGems.Add(go);
    }

    public void AddObjectRange(IEnumerable<GameObject> gos)
    {
        foreach (var item in gos)
        {
            AddObject(item);
        }
    }

    //Información de los matches + bonus
    public PuzzleMatchesInfo()
    {
        matchedGems = new List<GameObject>();
        bonus = BonusType.None;
    }

    public BonusType bonus { get; set; }
}

