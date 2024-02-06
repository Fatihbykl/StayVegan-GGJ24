using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioSource effectsSource;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Manager in the scene.");
        }
        instance = this;
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        effectsSource.PlayOneShot(clip, volume);
    }
}