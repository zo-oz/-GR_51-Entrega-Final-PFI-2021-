using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Clase estática común a todas las escenas
    public static GameManager instance;
    public bool startGame;
    public string randomGameId;
    public int demo;
    public int difficulty;
    public string uniqueId;


    private void Awake()
    {
        uniqueId = SystemInfo.deviceUniqueIdentifier;
        //Singleton
        if (instance == null)
        {
            instance = GetComponent<GameManager>();
            instance.startGame = false;
            difficulty = 0;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
