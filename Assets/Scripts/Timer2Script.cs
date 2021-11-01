using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer2Script : MonoBehaviour
{
    private float remainingTime = 600; //Default el timer son 10 minutos
    private bool isRunning = false;
    [SerializeField]
    private TextMesh timeText;
    [SerializeField]
    private GameObject popup;
    [SerializeField]
    private TextMesh finalScore;
    [SerializeField]
    private SoundManager_Game2 soundManager;


    private void Start()
    {
        popup.SetActive(false);
        isRunning = true;
    }

    //Inicia el contador
    void Update()
    {
        if (isRunning)
        {
            if (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                DisplayTime(remainingTime);
            }
            else
            {
                //Timeout: muestro el puntaje final y popup 
                FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Player");
                FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Magnet");
                FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("UI");
                FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Default");
                finalScore.text = "SU "+GameObject.Find("ScoreText").GetComponent<TextMesh>().text;
                soundManager.PlayFinishSound();
                popup.SetActive(true);
                Time.timeScale = 0;
                //GameObject.Find("BoardCanvas").SetActive(false);
                remainingTime = 0;
                isRunning = false;
            }
        }
    }

    //Muestro el countdown en pantalla
    private void DisplayTime(float time)
    {
        time += 1;
        float min = Mathf.FloorToInt(time / 60);
        float sec = Mathf.FloorToInt(time % 60);
        timeText.text = string.Format("{0:00}:{1:00}", min, sec);
    }
}
