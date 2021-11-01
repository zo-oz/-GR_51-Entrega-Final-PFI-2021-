using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleGem : MonoBehaviour
{
    public BonusType bonus { get; set; }
    public int column { get; set; }
    public int row { get; set; }
    public string type { get; set; }


    public PuzzleGem()
    {
        bonus = BonusType.None;
    }

    // Reviso si coincide con el tipo
    public bool IsSameType(PuzzleGem thisGem)
    {
        if (thisGem == null || !(thisGem is PuzzleGem))
            throw new ArgumentException("GemMismatch");
        return string.Compare(this.type, (thisGem as PuzzleGem).type) == 0;
    }

    /// Constructor de una gema
    public void Assign(string type, int row, int column)
    {

        if (string.IsNullOrEmpty(type))
            throw new ArgumentException("type");
        this.column = column;
        this.row = row;
        this.type = type;
    }

    //Intercambio de piezas
    public static void SwapColumnRow(PuzzleGem a, PuzzleGem b)
    {
        int temp = a.row;
        a.row = b.row;
        b.row = temp;
        temp = a.column;
        a.column = b.column;
        b.column = temp;
    }
}

public class NewGemsInfo
{
    private List<GameObject> newGems { get; set; }
    public int maxDistance { get; set; }
    
    //Devuelve listado de gemas
    public IEnumerable<GameObject> NewGems
    {
        get
        {
            return newGems.Distinct();
        }
    }

    //Agrego gema al listado
    public void AddGem(GameObject gem)
    {
        if (!newGems.Contains(gem))
            newGems.Add(gem);
    }

    public NewGemsInfo()
    {
        newGems = new List<GameObject>();
    }
}

//Tipos de bonus
[Flags]
public enum BonusType
{
    None,
    DestroyWholeRowColumn
}


public static class BonusTypeUtilities
{
    // Verifico si contiene bonus
    public static bool hasBonusType(BonusType bt)
    {
        return (bt & BonusType.DestroyWholeRowColumn)
            == BonusType.DestroyWholeRowColumn;
    }
}

// Tipos de estado (juego)
public enum GameState
{
    None,
    SelectionStarted,
    Animating
}



