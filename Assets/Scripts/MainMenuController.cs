using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    private bool isAburrido;
    private bool isTenso;
    private bool isTranquilo;
    private bool isEnojado;
    private bool isAnsioso;
    private bool isContento;
    private bool isTriste;
    private bool isCansado;
    public bool isGameEnding;
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    //private string url = "http://10.0.0.227:47000/apiPFI/insertFeeling";
    private string url = "https://pfi-back.herokuapp.com/apiPFI/insertFeeling";



//BackEnd: subo los datos de la partida a la base
IEnumerator Upload()
    {

        int isAburridoInt = isAburrido ? 1 : 0;
        int isTensoInt = isTenso ? 1 : 0;
        int isTranquiloInt = isTranquilo ? 1 : 0;
        int isEnojadoInt = isEnojado ? 1 : 0;
        int isAnsiosoInt = isAnsioso ? 1 : 0;
        int isContentoInt = isContento ? 1 : 0;
        int isTristeInt = isTriste ? 1 : 0;
        int isCansadoInt = isCansado ? 1 : 0;

        if(!GameManager.instance.startGame)
        {
            isGameEnding = true;
            Debug.Log("GAME RANDOM ID : " + GameManager.instance.randomGameId);
        }
        else
        {
            int charAmount = Random.Range(20, 22); //set those to the minimum and maximum length of your string
            GameManager.instance.randomGameId = "";
            for (int i = 0; i < charAmount; i++)
            {

                GameManager.instance.randomGameId += glyphs[Random.Range(0, glyphs.Length)]; 
            }
            Debug.Log("GAME RANDOM ID : " + GameManager.instance.randomGameId);
        }

        int isGameEndingInt = isGameEnding ? 1 : 0;
        Debug.Log(GameManager.instance.uniqueId);
        WWWForm form = new WWWForm();
        form.AddField("bored", isAburridoInt);
        form.AddField("tense", isTensoInt);
        form.AddField("quiet", isTranquiloInt);
        form.AddField("angry", isEnojadoInt);
        form.AddField("anxious", isAnsiosoInt);
        form.AddField("happy", isContentoInt);
        form.AddField("sad", isTristeInt);
        form.AddField("tired", isCansadoInt);
        form.AddField("gameEnding", isGameEndingInt);
        form.AddField("matchId", GameManager.instance.demo);
        form.AddField("randomGameId", GameManager.instance.randomGameId);
        form.AddField("deviceId", GameManager.instance.uniqueId);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Post Success!");
        }

    }

    //inicializo valores
    public void Awake()
    {
        isAburrido = false;
        isTenso = false;
        isTranquilo = false;
        isEnojado = false;
        isAnsioso = false;
        isContento = false;
        isTriste = false;
        isCansado = false;
    }

    public void tgAburrido() {
        isAburrido = !isAburrido;
    }

    public void tgTenso()
    {
        isTenso = !isTenso;
    }
    public void tgTranquilo()
    {
        isTranquilo = !isTranquilo;
    }
    public void tgEnojado()
    {
        isEnojado = !isEnojado;
    }
    public void tgAnsioso()
    {
        isAnsioso = !isAnsioso;
    }
    public void tgContento()
    {
        isContento = !isContento;
    }
    public void tgTriste()
    {
        isTriste = !isTriste;
    }
    public void tgCansado()
    {
        isCansado = !isCansado;
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("MenuCuestionario");
    }

    public void PlayGame1()
    {
        GameManager.instance.startGame = true;
        GameManager.instance.demo = 1;
        SceneManager.LoadScene("MenuCuestionario");
    }

    public void PlayGame2()
    {
        GameManager.instance.startGame = true;
        GameManager.instance.demo = 2;
        SceneManager.LoadScene("MenuCuestionario");
    }

    public void PlayGame3()
    {
        GameManager.instance.startGame = true;
        GameManager.instance.demo = 3;
        SceneManager.LoadScene("MenuCuestionario");
    }

    public void RestartGame()
    {
        if (Time.timeScale == 0)
            Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Completa el cuestionario y entra / sale de un juego
    public void SurveyOK()
    {
        StartCoroutine(Upload());
        if (!GameManager.instance.startGame)
            SceneManager.LoadScene("SeleccionarJuego");
        if (GameManager.instance.startGame && GameManager.instance.demo == 1)
        {
            GameManager.instance.difficulty = getDifficulty();
            SceneManager.LoadScene("Game1");
        }
        if (GameManager.instance.startGame && GameManager.instance.demo == 2)
        {
            GameManager.instance.difficulty = getDifficulty();
            SceneManager.LoadScene("Game2");
        }
        if (GameManager.instance.startGame && GameManager.instance.demo == 3)
        {
            GameManager.instance.difficulty = getDifficulty();
            SceneManager.LoadScene("Game3");
        }
    }

    //Vuelve al menú de selección de juego
    public void HomeButton()
    {
        if (!GameManager.instance.startGame)
        { 
            SceneManager.LoadScene("SeleccionarJuego");
        }
        else 
        {
            GameManager.instance.startGame = false;
            SceneManager.LoadScene("MenuCuestionario");
        }
    }

    //Derermina la dificultad según respuestas del usuario
    private int getDifficulty()
    {
        if ((isAburrido || isContento) && !isAnsioso && !isCansado && !isTenso && !isTriste && !isEnojado)
            return 1;
        else
            if ((isTenso || isAnsioso || isCansado) && !isContento && !isAburrido)
            return -1;
        else
            return 0;
    }

    //Menú de música
    public void MusicButton()
    {
        SceneManager.LoadScene("MenuMusica");
    }

    public void Exit()
    {
        SceneManager.LoadScene("SeleccionarJuego");
    }

    public void Pause()
    {
        Debug.Log("TIME " + Time.timeScale);
        if (Time.timeScale == 1)
            Time.timeScale = 0;
        else if (Time.timeScale == 0)
            Time.timeScale = 1;
    }

    public void PlayBGM0()
    {
        AudioManager.instance.PlayBGM0();
    }

    public void PlayBGM1()
    {
        AudioManager.instance.PlayBGM1();
    }

    public void PlayBGM2()
    {
        AudioManager.instance.PlayBGM2();
    }

    public void PlayBGM3()
    {
        AudioManager.instance.PlayBGM3();
    }

    public void PlayBGM4()
    {
        AudioManager.instance.PlayBGM4();
    }

    public void PlayBGM5()
    {
        AudioManager.instance.PlayBGM5();
    }

    public void PlayBGM6()
    {
        AudioManager.instance.PlayBGM6();
    }
    public void PlayBGM7()
    {
        AudioManager.instance.PlayBGM7();
    }

}

