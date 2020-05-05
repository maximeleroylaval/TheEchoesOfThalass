using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sources")]
    public AudioSource engineSource;
    public AudioSource combatSource;
    public AudioSource efxSource;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource musicSource;                 //Drag a reference to the audio source which will play the music.
    public static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.

    [Header("Clips")]
    public AudioClip ambientUnderWater;
    public AudioClip effectGenericDeath;
    public AudioClip effectGenericExplode;

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
            //if not, set it to this.
            instance = this;
        //If instance already exists:
        else if (instance != this)
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public void StopMusic(AudioSource source)
    {
        source.Stop();
    }

    //Used to play single sound clips.
    public void PlaySingle(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    //Used to play music.
    public void PlayMusic(AudioSource source, AudioClip clip)
    {
        //Set the clip of our efxSource audio source to the clip passed in as a parameter.
        source.clip = clip;

        //Play the clip.
        source.Play();
    }
}