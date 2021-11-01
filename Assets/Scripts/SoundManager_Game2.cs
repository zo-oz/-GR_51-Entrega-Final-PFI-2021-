using UnityEngine;

public class SoundManager_Game2 : MonoBehaviour {

    [SerializeField]
    private AudioClip popAudioClip;
    private AudioSource pop;
    [SerializeField]
    private AudioClip breathAudioClip;
    private AudioSource breath;
    [SerializeField]
    private AudioClip exitAudioClip;
    private AudioSource exit;
    [SerializeField]
    private AudioClip boomAudioClip;
    private AudioSource boom;
    [SerializeField]
    private AudioClip electroAudioClip;
    private AudioSource electro;
    private AudioSource finish;
    [SerializeField]
    private AudioClip finishAudioClip;

    //Inicializa los audioclips
    void Awake()
    {
        pop = AddAudio(popAudioClip);
        breath = AddAudio(breathAudioClip);
        electro = AddAudio(electroAudioClip);
        boom = AddAudio(boomAudioClip);
        boom.volume = 0.1f;
        exit = AddAudio(exitAudioClip);
        finish = AddAudio(finishAudioClip);
    }

    //Agrego el audioclip del match
    AudioSource AddAudio( AudioClip audioClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        return audioSource;
    }

    //Silencia al pausar el juego
    public void ToggleMute()
    {
        pop.mute = !pop.mute;
        exit.mute = !exit.mute;
        boom.mute = !boom.mute;
        electro.mute = !electro.mute;
    }


    public void PlayBreathSound()
    {
        breath.Play();
    }

    public void PlayBoomSound()
    {
        boom.Play();
    }

    public void PlayElectroSound()
    {
        electro.Play();
    }

    public void PlayExitSound()
    {
        exit.Play();
    }

    public void PlayPopSound()
    {
        pop.Play();
    }

    public void PlayFinishSound()
    {
        finish.Play();
    }
}
