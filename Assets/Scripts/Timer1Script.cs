using UnityEngine;

public class Timer1Script : MonoBehaviour
{
    private float remainingTime = 600;//Default el timer son 10 minutos
    private bool isRunning = false;
    [SerializeField]
    private TextMesh timeText;
    [SerializeField]
    private GameObject popup;
    [SerializeField]
    private TextMesh finalScore;
    [SerializeField]
    private SoundManager_Game1 soundManager;


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
                finalScore.text = "SU "+GameObject.Find("ScoreText").GetComponent<TextMesh>().text;
                soundManager.PlayFinishSound();
                popup.SetActive(true);
                GameObject.Find("BoardCanvas").SetActive(false);
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
