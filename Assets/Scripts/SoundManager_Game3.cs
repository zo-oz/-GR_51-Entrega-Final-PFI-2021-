using UnityEngine;

public class SoundManager_Game3 : MonoBehaviour {

    public static SoundManager_Game3 instance;

    [SerializeField]
    private AudioClip bombAudioClip;
    private AudioSource bomb;
    [SerializeField]
    private AudioClip gravityClip;
    private AudioSource gravity;
    [SerializeField]
    private AudioClip jumpClip;
    private AudioSource jump;
    [SerializeField]
    private AudioClip breathAudioClip;
    private AudioSource breath;
    [SerializeField]
    private AudioClip starAudioClip;
    private AudioSource star;
    private AudioSource finish;
    [SerializeField]
    private AudioClip finishAudioClip;


    private void Awake()
    {
        //Singleton
        if (instance == null)//Singleton
        {
            instance = GetComponent<SoundManager_Game3>();
            bomb = AddAudio(bombAudioClip);
            bomb.volume = 0.6f;
            gravity = AddAudio(gravityClip);
            breath = AddAudio(breathAudioClip);
            star = AddAudio(starAudioClip);
            star.volume = 0.8f;
            jump = AddAudio(jumpClip);
            jump.volume = 0.01f;
            finish = AddAudio(finishAudioClip);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
      
    }

    //Agrego el audioclip
    AudioSource AddAudio( AudioClip audioClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        return audioSource;
    }

    //Reproduce el sonido al hacer contacto con explosivo
    public void PlayBombSound()
    {
        //bomb.volume = 0.1f;
        bomb.Play();
    }

    //Reproduce el sonido al cambiar la gravedad
    public void PlayWaveSound()
    {
        //gravity.volume = 0.1f;
        gravity.Play();
    }


    //Reproduce el sonido al cambiar la gravedad
    public void PlayJumpSound()
    {
        jump.Play();
    }

    //Reproduce el sonido al pausar
    public void PlayBreathSound()
    {
        breath.Play();
    }


    //Reproduce el sonido al tocar una fruta
    public void PlayStarSound()
    {
        star.Play();
    }


    //Silencio todos los sonidos al pausar
    public void ToggleMute()
    {
        jump.mute = !jump.mute;
        gravity.mute = !gravity.mute;
        bomb.mute = !bomb.mute;
        star.mute = !star.mute;
    }

    public void PlayFinishSound()
    {
        finish.Play();
    }

}
