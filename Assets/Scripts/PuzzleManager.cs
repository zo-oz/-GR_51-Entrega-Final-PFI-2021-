using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PuzzleManager : MonoBehaviour
{
    public TextMesh scoreText;
    public bool showDebugInfo = false;
    public PuzzleArrays gems;
    private int score;
    //cantidad de puntos entre breaks y cambio de imagen
    private int scorechg;

    public readonly Vector2 boardPoint = new Vector2(-2.37f, -4.27f);
    public readonly Vector2 shapeSize = new Vector2(0.7f, 0.7f);

    private GameState state = GameState.None;
    private GameObject hitGo = null;
    private Vector2[] spawnPositions;

    //listado de piezas, bonus y animación de explosiones
    public GameObject[] gemPrefabs;
    public GameObject[] explosionPrefabs;
    public GameObject[] bonusPrefabs;
    private int difficulty;

    private IEnumerator potentialMatchesCoroutine;
    private IEnumerator animatePotentialCoroutine;

    IEnumerable<GameObject> potentialMatches;
    public SoundManager_Game1 soundManager;
    public BackgroundManager backgroundManager;

    void Awake()
    {
        //Ajustes de dificultad
        difficulty = GameManager.instance.difficulty;
        if (difficulty != 1)
        {
            GameObject.Find("TimerText").SetActive(false);
            if (GameObject.Find("Popup") != null)
                GameObject.Find("Popup").SetActive(false);
        }
        if (difficulty == -1) 
        {
            GameObject.Find("ScoreText").gameObject.SetActive(false);
            Game1DefaultValues.moveAnimationMinLength = 0.2f;
        }
        if (difficulty == 1) 
        {
            Game1DefaultValues.moveAnimationMinLength = 0.09f;
        }
        if (difficulty == 0)
        {
            Game1DefaultValues.moveAnimationMinLength = 0.3f;
        }
    }

    // Iniciar tablero
    void Start()
    {
        SetUpPrefabs();
        SetUpBoard();
        CheckForMatches();
    }

    //Iniciar sprites y bonus sprites
    private void SetUpPrefabs()
    {
        foreach (var item in gemPrefabs)
        {
            item.GetComponent<PuzzleGem>().type = item.name;
        }
        //el bonus se asigna al color de gema según su nombre
        foreach (var item in bonusPrefabs)
        {
            item.GetComponent<PuzzleGem>().type = gemPrefabs.
                Where(x => x.GetComponent<PuzzleGem>().type.Contains(item.name.Split('_')[1].Trim())).Single().name;
        }
    }

    public void SetUpBoard()
    {
        InitializeVariables();
        if (gems != null)
            clearBoard();
        gems = new PuzzleArrays();
        spawnPositions = new Vector2[Game1DefaultValues.columns];
        for (int row = 0; row < Game1DefaultValues.rows; row++)
        {
            for (int column = 0; column < Game1DefaultValues.columns; column++)
            {

                GameObject newGem = getRandomGem();
                //chequeo dos previos (horizontal)
                while (column >= 2 && gems[row, column - 1].GetComponent<PuzzleGem>()
                    .IsSameType(newGem.GetComponent<PuzzleGem>())
                    && gems[row, column - 2].GetComponent<PuzzleGem>().IsSameType(newGem.GetComponent<PuzzleGem>()))
                {
                    newGem = getRandomGem();
                }

                //chequeo dos previos (vertical)
                while (row >= 2 && gems[row - 1, column].GetComponent<PuzzleGem>()
                    .IsSameType(newGem.GetComponent<PuzzleGem>())
                    && gems[row - 2, column].GetComponent<PuzzleGem>().IsSameType(newGem.GetComponent<PuzzleGem>()))
                {
                    newGem = getRandomGem();
                }
                generateNewGem(row, column, newGem);
            }
        }

        SetupSpawnPositions();
    }

    private void generateNewGem(int row, int column, GameObject newGem)
    {
        GameObject go = Instantiate(newGem,
            boardPoint + new Vector2(column * shapeSize.x, row * shapeSize.y), Quaternion.identity)
            as GameObject;

        go.transform.parent = GameObject.Find("BoardCanvas") .transform;
        go.GetComponent<PuzzleGem>().Assign(newGem.GetComponent<PuzzleGem>().type, row, column);
        gems[row, column] = go;
    }

    private void SetupSpawnPositions()
    {
        //defino spawn positions de las nuevas piezas
        for (int column = 0; column < Game1DefaultValues.columns; column++)
        {
            spawnPositions[column] = boardPoint
                + new Vector2(column * shapeSize.x, Game1DefaultValues.rows * shapeSize.y);
        }
    }

    //limpiar tablero
    private void clearBoard()
    {
        for (int row = 0; row < Game1DefaultValues.rows; row++)
        {
            for (int column = 0; column < Game1DefaultValues.columns; column++)
            {
                Destroy(gems[row, column]);
            }
        }
    }

    void Update()
    {
        if (state == GameState.None)
        {
            //usuario: click o touch
            if (Input.GetMouseButtonDown(0))
            {
                //indico dirección
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null) //click = sprite
                {
                    hitGo = hit.collider.gameObject;
                    state = GameState.SelectionStarted;
                }
                
            }
        }
        else if (state == GameState.SelectionStarted)
        {
            if (Input.GetMouseButton(0))
            {                
                var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hitGo != hit.collider.gameObject)
                {
                    StopCheckForPotentialMatches();
                    if (!PuzzleHelper.areGemsTogether(hitGo.GetComponent<PuzzleGem>(),
                        hit.collider.gameObject.GetComponent<PuzzleGem>()))
                    {
                        state = GameState.None;
                    }
                    else
                    {
                        state = GameState.Animating;
                        FixSortingLayer(hitGo, hit.collider.gameObject);
                        StartCoroutine(FindMatchesAndCollapse(hit));
                    }
                }
            }
        }
    }

    // Modifica sorting layers para una mejor visualización
    private void FixSortingLayer(GameObject hitGo, GameObject hitGo2)
    {
        SpriteRenderer sp1 = hitGo.GetComponent<SpriteRenderer>();
        SpriteRenderer sp2 = hitGo2.GetComponent<SpriteRenderer>();
        if (sp1.sortingOrder <= sp2.sortingOrder)
        {
            sp1.sortingOrder = 1;
            sp2.sortingOrder = 0;
        }
    }

    //Si Encuentra matches limpia y genera nuevas piezas
    private IEnumerator FindMatchesAndCollapse(RaycastHit2D hit2)
    {
        var hitGo2 = hit2.collider.gameObject;
        gems.Swap(hitGo, hitGo2);

        hitGo.transform.positionTo(Game1DefaultValues.animLength, hitGo2.transform.position);
        hitGo2.transform.positionTo(Game1DefaultValues.animLength, hitGo.transform.position);
        yield return new WaitForSeconds(Game1DefaultValues.animLength);

        var hitGomatchesInfo = gems.GetMatches(hitGo);
        var hitGo2matchesInfo = gems.GetMatches(hitGo2);

        var totalMatches = hitGomatchesInfo.foundMatches
            .Union(hitGo2matchesInfo.foundMatches).Distinct();

        if (totalMatches.Count() < Game1DefaultValues.minMatches)
        {
            hitGo.transform.positionTo(Game1DefaultValues.animLength, hitGo2.transform.position);
            hitGo2.transform.positionTo(Game1DefaultValues.animLength, hitGo.transform.position);
            yield return new WaitForSeconds(Game1DefaultValues.animLength);

            gems.UndoSwap();
        }
        bool addBonus = totalMatches.Count() >= Game1DefaultValues.minMatchesForBonus &&
            !BonusTypeUtilities.hasBonusType(hitGomatchesInfo.bonus) &&
            !BonusTypeUtilities.hasBonusType(hitGo2matchesInfo.bonus);

        PuzzleGem hitGoCache = null;
        if (addBonus)
        {
            var sameTypeGo = hitGomatchesInfo.foundMatches.Count() > 0 ? hitGo : hitGo2;
            hitGoCache = sameTypeGo.GetComponent<PuzzleGem>();
        }

        int timesRun = 1;
        while (totalMatches.Count() >= Game1DefaultValues.minMatches)
        {
            //incrementa puntaje según match
            IncreaseScore((totalMatches.Count() - 2) * Game1DefaultValues.match3Score);

            if (timesRun >= 2)
                IncreaseScore(Game1DefaultValues.subsequentMatchesScore);
            //sonido de match
            soundManager.PlaySound();

            foreach (var item in totalMatches)
            {
                gems.Remove(item);
                RemoveFromScene(item);
            }

            if (addBonus)
                CreateBonus(hitGoCache);

            addBonus = false;

            var columns = totalMatches.Select(go => go.GetComponent<PuzzleGem>().column).Distinct();

            //Genera nuevas gemas al limpiar resultados
            var fallingGemsInfo = gems.Collapse(columns);
            var newGemInfo = setUpNewGem(columns);
            int maxDistance = Mathf.Max(fallingGemsInfo.maxDistance, newGemInfo.maxDistance);
            MoveAndAnimate(newGemInfo.NewGems, maxDistance);
            MoveAndAnimate(fallingGemsInfo.NewGems, maxDistance);

            //Espera la animación de caida
            yield return new WaitForSeconds(Game1DefaultValues.moveAnimationMinLength * maxDistance);

            //busca nuevos matches
            totalMatches = gems.GetMatches(fallingGemsInfo.NewGems).
                Union(gems.GetMatches(newGemInfo.NewGems)).Distinct();
            timesRun++;
        }
        state = GameState.None;
        CheckForMatches();
    }

    //Genera un nuevo bonus
    private void CreateBonus(PuzzleGem hitGoCache)
    {
        GameObject Bonus = Instantiate(GetBonusFromType(hitGoCache.type), boardPoint
            + new Vector2(hitGoCache.column * shapeSize.x,
                hitGoCache.row * shapeSize.y), Quaternion.identity)
            as GameObject;
        Bonus.transform.parent = GameObject.Find("BoardCanvas").transform;
        gems[hitGoCache.row, hitGoCache.column] = Bonus;
        var BonusShape = Bonus.GetComponent<PuzzleGem>();
        BonusShape.Assign(hitGoCache.type, hitGoCache.row, hitGoCache.column);
        BonusShape.bonus |= BonusType.DestroyWholeRowColumn;
    }

    // Las nuevas piezas caen en columnas libres
    private NewGemsInfo setUpNewGem(IEnumerable<int> emptyColumns)
    {
        NewGemsInfo ng = new NewGemsInfo();
        foreach (int column in emptyColumns)
        {
            var emptyItems = gems.GetEmptyItemsOnColumn(column);
            foreach (var item in emptyItems)
            {
                var go = getRandomGem();
                GameObject newGem = Instantiate(go, spawnPositions[column], Quaternion.identity)
                    as GameObject;

                newGem.transform.parent = GameObject.Find("BoardCanvas").transform;
                newGem.GetComponent<PuzzleGem>().Assign(go.GetComponent<PuzzleGem>().type, item.row, item.column);

                if (Game1DefaultValues.rows - item.row > ng.maxDistance)
                    ng.maxDistance = Game1DefaultValues.rows - item.row;
                gems[item.row, item.column] = newGem;
                ng.AddGem(newGem);
            }
        }
        return ng;
    }
    
    //Animacion + Movimiento
    private void MoveAndAnimate(IEnumerable<GameObject> movedGameObjects, int distance)
    {
        foreach (var item in movedGameObjects)
        {
            item.transform.positionTo(Game1DefaultValues.moveAnimationMinLength * distance, boardPoint +
                new Vector2(item.GetComponent<PuzzleGem>().column * shapeSize.x, item.GetComponent<PuzzleGem>().row * shapeSize.y));
        }
    }

    //Genera una explosion y elimino el item del juego
    private void RemoveFromScene(GameObject item)
    {
        GameObject explosion = GetRandomExplosion();
        var newExplosion = Instantiate(explosion, item.transform.position, Quaternion.identity) as GameObject;
        Destroy(newExplosion, Game1DefaultValues.explosionLength);
        Destroy(item);
    }
    
    //Nueva gema (color aleatorio)
    private GameObject getRandomGem()
    {
        return gemPrefabs[Random.Range(0, gemPrefabs.Length)];
    }

    //Inicializo puntaje
    private void InitializeVariables()
    {
        score = 0;
        scorechg = 0;
        ShowScore();
    }

    //Incremento el puntaje
    private void IncreaseScore(int amount)
    {
        score += amount;
        scorechg += amount;
        ShowScore();
        if (scorechg >= 2000)
        {
            StartCoroutine(PauseAndChangeImg());
            scorechg = 0;
        }

    }

    //Coroutine: cambio de imagen y pauso el juego
    IEnumerator PauseAndChangeImg()
    {
        soundManager.ToggleMute();
        backgroundManager.ChangeImage();
        //cullingMask esconde el tablero 
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Default");
        soundManager.PlayBreathSound();
        yield return new WaitForSecondsRealtime(5f);
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Default");
        soundManager.ToggleMute();
    }

    private void ShowScore()
    {
        scoreText.text = "Puntaje: " + score.ToString();
    }

    //Genero una explosion (random)
    private GameObject GetRandomExplosion()
    {
        return explosionPrefabs[Random.Range(0, explosionPrefabs.Length)];
    }
    
    //Tipo de bonus
    private GameObject GetBonusFromType(string type)
    {
        string color = type.Split('_')[1].Trim();
        foreach (var item in bonusPrefabs)
        {
            if (item.GetComponent<PuzzleGem>().type.Contains(color))
                return item;
        }
        throw new System.Exception("BonusTypeError");
    }

    //Coroutine: busco posibles combinaciones
    private void CheckForMatches()
    {
        StopCheckForPotentialMatches();
        potentialMatchesCoroutine = CheckPotentialMatches();
        StartCoroutine(potentialMatchesCoroutine);
    }

    //Deja de buscar combinaciones
    private void StopCheckForPotentialMatches()
    {
        if (animatePotentialCoroutine != null)
            StopCoroutine(animatePotentialCoroutine);
        if (potentialMatchesCoroutine != null)
            StopCoroutine(potentialMatchesCoroutine);
        ResetOpacityOnPotentialMatches();
    }

    // Reset opacidad en pistas
    private void ResetOpacityOnPotentialMatches()
    {
        if (potentialMatches != null)
            foreach (var item in potentialMatches)
            {
                if (item == null) break;

                Color c = item.GetComponent<SpriteRenderer>().color;
                c.a = 1.0f;
                item.GetComponent<SpriteRenderer>().color = c;
            }
    }

    // Encuentra matches como pista a x segundos
    private IEnumerator CheckPotentialMatches()
    {
        yield return new WaitForSeconds(Game1DefaultValues.waitForHints);
        potentialMatches = PuzzleHelper.GetPotentialMatches(gems);
        if (potentialMatches != null)
        {
            while (true && difficulty!=1)//No muestro pistas si la dificultad es alta
            {
                animatePotentialCoroutine = PuzzleHelper.AnimatePotentialMatches(potentialMatches);
                StartCoroutine(animatePotentialCoroutine);
                yield return new WaitForSeconds(Game1DefaultValues.waitForHints);
            }
        }
    }
}
