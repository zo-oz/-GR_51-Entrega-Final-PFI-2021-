using System.Collections;
using UnityEngine;

public class MagnetSpawner : MonoBehaviour
{
    [SerializeField]
    private  GameObject canvas;
    [SerializeField]
    private float minDelay;
    [SerializeField]
    private float maxDelay;
    [SerializeField]
    private GameObject magnet;
    private int difficulty;

    void Start()
    {
        difficulty = GameManager.instance.difficulty;
        //Ajusto el juego a la dificultad
        if (difficulty != 1)
        {
            GameObject.Find("TimerText").SetActive(false);
            if (GameObject.Find("Popup") != null)
                GameObject.Find("Popup").SetActive(false);
        }
        if (difficulty == -1)
        {
            minDelay = 15f;
            maxDelay = 25f;
        }
        if (difficulty == 1)
        {
            minDelay = 7f;
            maxDelay = 12f;
        }
        if (difficulty == 0)
        {
            minDelay = 9f;
            maxDelay = 20f;
        }
        StartCoroutine(SpawnMagnets());
    }

    //Generacion aleatoria de obstaculos (Magnets)
    IEnumerator SpawnMagnets()
    {
        while (true)
        {
            int side = Random.Range(0,2);
            if (side==0)
            {
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);
                float spawnY = Random.Range(-3.5f, 3.5f);
                GameObject spawnedMagnet = Instantiate(magnet, new Vector2(-9, spawnY), Quaternion.identity, canvas.transform);
            }
            else 
            {
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);
                float spawnY = Random.Range(-3.5f, 3.5f);
                GameObject spawnedMagnet = Instantiate(magnet, new Vector2(9, spawnY), Quaternion.identity, canvas.transform);
            }
        }
    }
}
