using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class PuzzleHelper
{
    // Animación si encuentra matches (hint)
    public static IEnumerator AnimatePotentialMatches(IEnumerable<GameObject> potentialMatches)
    {
        for (float i = 1f; i >= 0.3f; i -= 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = i;
                item.GetComponent<SpriteRenderer>().color = c;
            }
            yield return new WaitForSeconds(Game1DefaultValues.opacityFrameDelay);
        }
        for (float i = 0.3f; i <= 1f; i += 0.1f)
        {
            foreach (var item in potentialMatches)
            {
                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = i;
                item.GetComponent<SpriteRenderer>().color = c;
            }
            yield return new WaitForSeconds(Game1DefaultValues.opacityFrameDelay);
        }
    }

    //Revisa si dos gemas son contiguas
    public static bool areGemsTogether(PuzzleGem s1, PuzzleGem s2)
    {
        return (s1.column == s2.column ||
                        s1.row == s2.row)
                        && Mathf.Abs(s1.column - s2.column) <= 1
                        && Mathf.Abs(s1.row - s2.row) <= 1;
    }

    /// Revisa si hay potenciales matches
    public static IEnumerable<GameObject> GetPotentialMatches(PuzzleArrays shapes)
    {
        List<List<GameObject>> matches = new List<List<GameObject>>();
        for (int row = 0; row < Game1DefaultValues.rows; row++)
        {
            for (int column = 0; column < Game1DefaultValues.columns; column++)
            {

                var matches1 = CheckHorizontal1(row, column, shapes);
                var matches2 = CheckHorizontal2(row, column, shapes);
                var matches3 = CheckHorizontal3(row, column, shapes);
                var matches4 = CheckVertical1(row, column, shapes);
                var matches5 = CheckVertical2(row, column, shapes);
                var matches6 = CheckVertical3(row, column, shapes);

                if (matches1 != null) matches.Add(matches1);
                if (matches2 != null) matches.Add(matches2);
                if (matches3 != null) matches.Add(matches3);
                if (matches4 != null) matches.Add(matches4);
                if (matches5 != null) matches.Add(matches5);
                if (matches6 != null) matches.Add(matches6);
                //Devuelve un potencial match (random) como pista
                if (matches.Count >= 3)
                    return matches[UnityEngine.Random.Range(0, matches.Count - 1)];
                if(row >= Game1DefaultValues.rows / 2 && matches.Count > 0 && matches.Count <=2)
                    return matches[UnityEngine.Random.Range(0, matches.Count - 1)];
            }
        }
        return null;
    }

    public static List<GameObject> CheckHorizontal1(int row, int column, PuzzleArrays shapes)
    {
        if (column <= Game1DefaultValues.columns - 2)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
                IsSameType(shapes[row, column + 1].GetComponent<PuzzleGem>()))
            {
                if (row >= 1 && column >= 1)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row - 1, column - 1].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row - 1, column - 1]
                                };
                if (row <= Game1DefaultValues.rows - 2 && column >= 1)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row + 1, column - 1].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row + 1, column - 1]
                                };
            }
        }
        return null;
    }


    public static List<GameObject> CheckHorizontal2(int row, int column, PuzzleArrays shapes)
    {
        if (column <= Game1DefaultValues.columns - 3)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
                IsSameType(shapes[row, column + 1].GetComponent<PuzzleGem>()))
            {

                if (row >= 1 && column <= Game1DefaultValues.columns - 3)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row - 1, column + 2].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row - 1, column + 2]
                                };

                if (row <= Game1DefaultValues.rows - 2 && column <= Game1DefaultValues.columns - 3)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row + 1, column + 2].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row + 1, column + 2]
                                };
            }
        }
        return null;
    }

    // Revisa en linea horizontal
    public static List<GameObject> CheckHorizontal3(int row, int column, PuzzleArrays shapes)
    {
        if (column <= Game1DefaultValues.columns - 4)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row, column + 1].GetComponent<PuzzleGem>()) &&
               shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row, column + 3].GetComponent<PuzzleGem>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row, column + 3]
                                };
            }
        }
        if (column >= 2 && column <= Game1DefaultValues.columns - 2)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row, column + 1].GetComponent<PuzzleGem>()) &&
               shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row, column - 2].GetComponent<PuzzleGem>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row, column + 1],
                                    shapes[row, column -2]
                                };
            }
        }
        return null;
    }

    //Revisa en linea vertial
    public static List<GameObject> CheckVertical1(int row, int column, PuzzleArrays shapes)
    {
        if (row <= Game1DefaultValues.rows - 2)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
                IsSameType(shapes[row + 1, column].GetComponent<PuzzleGem>()))
            {
                if (column >= 1 && row >= 1)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row - 1, column - 1].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row - 1, column -1]
                                };

                if (column <= Game1DefaultValues.columns - 2 && row >= 1)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row - 1, column + 1].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row - 1, column + 1]
                                };
            }
        }
        return null;
    }

    //Revisa en linea vertical
    public static List<GameObject> CheckVertical2(int row, int column, PuzzleArrays shapes)
    {
        if (row <= Game1DefaultValues.rows - 3)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
                IsSameType(shapes[row + 1, column].GetComponent<PuzzleGem>()))
            {
                if (column >= 1)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row + 2, column - 1].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row + 2, column -1]
                                };

                if (column <= Game1DefaultValues.columns - 2)
                    if (shapes[row, column].GetComponent<PuzzleGem>().
                    IsSameType(shapes[row + 2, column + 1].GetComponent<PuzzleGem>()))
                        return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row+1, column],
                                    shapes[row + 2, column + 1]
                                };
            }
        }
        return null;
    }

    //Revisa en linea vertical
    public static List<GameObject> CheckVertical3(int row, int column, PuzzleArrays shapes)
    {
        if (row <= Game1DefaultValues.rows - 4)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row + 1, column].GetComponent<PuzzleGem>()) &&
               shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row + 3, column].GetComponent<PuzzleGem>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row + 3, column]
                                };
            }
        }
        if (row >= 2 && row <= Game1DefaultValues.rows - 2)
        {
            if (shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row + 1, column].GetComponent<PuzzleGem>()) &&
               shapes[row, column].GetComponent<PuzzleGem>().
               IsSameType(shapes[row - 2, column].GetComponent<PuzzleGem>()))
            {
                return new List<GameObject>()
                                {
                                    shapes[row, column],
                                    shapes[row + 1, column],
                                    shapes[row - 2, column]
                                };
            }
        }
        return null;
    }


}

