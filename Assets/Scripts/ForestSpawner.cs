using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSpawner : MonoBehaviour
{
    public GameObject canvas;
    public float minDelay = 0.9f;
    public float maxDelay = 3f;
    public List<GameObject> fruits;
    private int difficulty;

    void Start()
    {
        //Ajusto el juego a la dificultad
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
            GameObject lemon = fruits[2];
            GameObject pear = fruits[3];
            fruits.Add(lemon);
            fruits.Add(pear);
        }
        if (difficulty == 1)
        {
            GameObject bomb = fruits[5];
            fruits.Add(bomb);
        }
        if (difficulty == 0)
        {
            minDelay = 0.9f;
            maxDelay = 3f;
        }
        StartCoroutine(SpawnFruit());
    }
    
    //Generación aleatoria de frutas + bomba
    IEnumerator SpawnFruit()
    {
        while (true)
        {
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            float spawnX = Random.Range(-8.5f, 8.5f);
            Vector3 spawnOrigin = new Vector3(spawnX, 5.5f, -2);
            GameObject fruit = fruits[Random.Range(0, fruits.Count)];
            GameObject spawnedFruit = Instantiate(fruit, spawnOrigin, new Quaternion(0, 0, 0, 0), canvas.transform);
        }
    }
}
