using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject canvas;
    public float minDelay = 0.9f;
    public float maxDelay = 2.9f;
    public GameObject[] bubbles;
    private int difficulty;
    void Start()
    {
        difficulty = GameManager.instance.difficulty;
        //Ajusto el juego a la dificultad
        if (difficulty == -1)
        {
            GameObject.Find("ScoreText").gameObject.SetActive(false);
            minDelay = 0.2f;
            maxDelay = 1.9f;
        }
        if (difficulty == 1)
        {
            minDelay = 2f;
            maxDelay = 5f;
        }
        if (difficulty == 0)
        {
            minDelay = 0.9f;
            maxDelay = 2.9f;
        }
        StartCoroutine(SpawnBubbles());
    }

    //Genero burbujas aleatoriamente
    IEnumerator SpawnBubbles()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            float spawnX = Random.Range(-8f, 8f);
            Vector3 spawnOrigin = new Vector3(spawnX, -6, -1);
            GameObject bubblePrefab = bubbles[Random.Range(0, bubbles.Length)];
            GameObject spawnedBubble = Instantiate(bubblePrefab, spawnOrigin, new Quaternion(0,0,0,0), canvas.transform);
            Destroy(spawnedBubble, 10f);
        }
    }
}
