using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PuzzleArrays
{
    private GameObject[,] gems = new GameObject[Game1DefaultValues.rows, Game1DefaultValues.columns];

    //Indico fila y columna
    public GameObject this[int row, int column]
    {
        get
        {
            try
            {
                return gems[row, column];
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        set
        {
            gems[row, column] = value;
        }
    }

    // Intercambio dos piezas en el tablero
    public void Swap(GameObject g1, GameObject g2)
    {
        //backup por si no genera un match
        backupG1 = g1;
        backupG2 = g2;

        var g1Shape = g1.GetComponent<PuzzleGem>();
        var g2Shape = g2.GetComponent<PuzzleGem>();
        int g1Row = g1Shape.row;
        int g1Column = g1Shape.column;
        int g2Row = g2Shape.row;
        int g2Column = g2Shape.column;
        //Intercambio de piezas
        var temp = gems[g1Row, g1Column];
        gems[g1Row, g1Column] = gems[g2Row, g2Column];
        gems[g2Row, g2Column] = temp;
        PuzzleGem.SwapColumnRow(g1Shape, g2Shape);

    }
    
    //Deshace el intercambio
    public void UndoSwap()
    {
        if (backupG1 == null || backupG2 == null)
            throw new Exception("Backup is null");

        Swap(backupG1, backupG2);
    }

    private GameObject backupG1;
    private GameObject backupG2;
    
    //Devuelve matches por objeto (array)
    public IEnumerable<GameObject> GetMatches(IEnumerable<GameObject> gos)
    {
        List<GameObject> matches = new List<GameObject>();
        foreach (var go in gos)
        {
            matches.AddRange(GetMatches(go).foundMatches);
        }
        return matches.Distinct();
    }

    //Devuelve matches por objeto
    public PuzzleMatchesInfo GetMatches(GameObject go)
    {
        PuzzleMatchesInfo matchesInfo = new PuzzleMatchesInfo();

        var horizontalMatches = GetMatchesHorizontally(go);
        if (HasBonus(horizontalMatches))
        {
            horizontalMatches = GetFullRow(go);
            if (!BonusTypeUtilities.hasBonusType(matchesInfo.bonus))
                matchesInfo.bonus |= BonusType.DestroyWholeRowColumn;
        }
        matchesInfo.AddObjectRange(horizontalMatches);

        var verticalMatches = GetMatchesVertically(go);
        if (HasBonus(verticalMatches))
        {
            verticalMatches = GetFullColumn(go);
            if (!BonusTypeUtilities.hasBonusType(matchesInfo.bonus))
                matchesInfo.bonus |= BonusType.DestroyWholeRowColumn;
        }
        matchesInfo.AddObjectRange(verticalMatches);

        return matchesInfo;
    }


    private bool HasBonus(IEnumerable<GameObject> matches)
    {
        if (matches.Count() >= Game1DefaultValues.minMatches)
        {
            foreach (var go in matches)
            {
                if (BonusTypeUtilities.hasBonusType
                    (go.GetComponent<PuzzleGem>().bonus))
                    return true;
            }
        }

        return false;
    }


    private IEnumerable<GameObject> GetFullRow(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int row = go.GetComponent<PuzzleGem>().row;
        for (int column = 0; column < Game1DefaultValues.columns; column++)
        {
            matches.Add(gems[row, column]);
        }
        return matches;
    }

    private IEnumerable<GameObject> GetFullColumn(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        int column = go.GetComponent<PuzzleGem>().column;
        for (int row = 0; row < Game1DefaultValues.rows; row++)
        {
            matches.Add(gems[row, column]);
        }
        return matches;
    }

    //Busca matches horizontalmente
    private IEnumerable<GameObject> GetMatchesHorizontally(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);
        var shape = go.GetComponent<PuzzleGem>();
        if (shape.column != 0)
            for (int column = shape.column - 1; column >= 0; column--)
            {
                if (gems[shape.row, column].GetComponent<PuzzleGem>().IsSameType(shape))
                {
                    matches.Add(gems[shape.row, column]);
                }
                else
                    break;
            }

        if (shape.column != Game1DefaultValues.columns - 1)
            for (int column = shape.column + 1; column < Game1DefaultValues.columns; column++)
            {
                if (gems[shape.row, column].GetComponent<PuzzleGem>().IsSameType(shape))
                {
                    matches.Add(gems[shape.row, column]);
                }
                else
                    break;
            }

        if (matches.Count < Game1DefaultValues.minMatches)
            matches.Clear();
        return matches.Distinct();
    }

    //busca matches verticalmente
    private IEnumerable<GameObject> GetMatchesVertically(GameObject go)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(go);
        var shape = go.GetComponent<PuzzleGem>();
        if (shape.row != 0)
            for (int row = shape.row - 1; row >= 0; row--)
            {
                if (gems[row, shape.column] != null &&
                    gems[row, shape.column].GetComponent<PuzzleGem>().IsSameType(shape))
                {
                    matches.Add(gems[row, shape.column]);
                }
                else
                    break;
            }

        if (shape.row != Game1DefaultValues.rows - 1)
            for (int row = shape.row + 1; row < Game1DefaultValues.rows; row++)
            {
                if (gems[row, shape.column] != null && 
                    gems[row, shape.column].GetComponent<PuzzleGem>().IsSameType(shape))
                {
                    matches.Add(gems[row, shape.column]);
                }
                else
                    break;
            }


        if (matches.Count < Game1DefaultValues.minMatches)
            matches.Clear();

        return matches.Distinct();
    }
    public void Remove(GameObject item)
    {
        gems[item.GetComponent<PuzzleGem>().row, item.GetComponent<PuzzleGem>().column] = null;
    }

    public NewGemsInfo Collapse(IEnumerable<int> columns)
    {
        NewGemsInfo collapseInfo = new NewGemsInfo();
        foreach (var column in columns)
        {
            for (int row = 0; row < Game1DefaultValues.rows - 1; row++)
            {
                if (gems[row, column] == null)
                {
                    for (int row2 = row + 1; row2 < Game1DefaultValues.rows; row2++)
                    {
                        if (gems[row2, column] != null)
                        {
                            gems[row, column] = gems[row2, column];
                            gems[row2, column] = null;
                            if (row2 - row > collapseInfo.maxDistance) 
                                collapseInfo.maxDistance = row2 - row;
                            gems[row, column].GetComponent<PuzzleGem>().row = row;
                            gems[row, column].GetComponent<PuzzleGem>().column = column;
                            collapseInfo.AddGem(gems[row, column]);
                            break;
                        }
                    }
                }
            }
        }

        return collapseInfo;
    }
    //Busca elementos vacios
    public IEnumerable<GemInfo> GetEmptyItemsOnColumn(int column)
    {
        List<GemInfo> emptyItems = new List<GemInfo>();
        for (int row = 0; row < Game1DefaultValues.rows; row++)
        {
            if (gems[row, column] == null)
                emptyItems.Add(new GemInfo() { row = row, column = column });
        }
        return emptyItems;
    }
}
public class GemInfo
{
    public int column { get; set; }
    public int row { get; set; }
}


