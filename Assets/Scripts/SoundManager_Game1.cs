using UnityEngine;

public class SoundManager_Game1 : MonoBehaviour {

    public AudioClip matchAudioClip;
    private AudioSource match;
    private AudioSource breath;
    public AudioClip breathAudioClip;
    private AudioSource finish;
    public AudioClip finishAudioClip;

    void Awake()
    {
        match = AddAudio(matchAudioClip);
        breath = AddAudio(breathAudioClip);
        finish = AddAudio(finishAudioClip);
    }

    //Agrego el audioclip del match
    AudioSource AddAudio( AudioClip audioClip)
    {
        AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        return audioSource;
    }

    //Reproduce el sonido al encontrar 3 o mas coincidencias
    public void PlaySound()
    {
        match.volume = 0.1f;
        match.Play();
    }

    //Silencia al pausar al juego
    public void ToggleMute()
    {
        match.mute = !match.mute;
    }
    public void PlayFinishSound()
    {
        finish.Play();
    }

    public void PlayBreathSound()
    {
        breath.Play();
    }
}
