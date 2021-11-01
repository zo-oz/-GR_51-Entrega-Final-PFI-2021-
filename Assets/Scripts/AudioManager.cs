using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource BGM;
    public AudioClip[] bgmTracks;

    private void Awake()
    {
        if (instance == null)//Singleton
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {        
    }

    //Función: selecciona la música de fondo
    public void ChangeBGM(AudioClip music)
    {
        if (BGM.clip.name == music.name)
            return;
        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }

    public void PlayBGM0()
    {
        if (BGM.clip == bgmTracks[0])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[0];
        BGM.Play();
    }
    public void PlayBGM1()
    {
        if (BGM.clip == bgmTracks[1])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[1];
        BGM.Play();
    }
    public void PlayBGM2()
    {
        if (BGM.clip == bgmTracks[2])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[2];
        BGM.Play();
    }
    public void PlayBGM3()
    {
        if (BGM.clip == bgmTracks[3])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[3];
        BGM.Play();
    }

    public void PlayBGM4()
    {
        if (BGM.clip == bgmTracks[4])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[4];
        BGM.Play();
    }

    public void PlayBGM5()
    {
        if (BGM.clip == bgmTracks[5])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[5];
        BGM.Play();
    }

    public void PlayBGM6()
    {
        if (BGM.clip == bgmTracks[6])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[6];
        BGM.Play();
    }

    public void PlayBGM7()
    {
        if (BGM.clip == bgmTracks[7])
            return;
        BGM.Stop();
        BGM.clip = bgmTracks[7];
        BGM.Play();
    }

}
